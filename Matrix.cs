using System;
using System.Threading;
using System.IO;

namespace MO_lab2
{
    public class Matrix 
    {
        private const double EPS = 1E-10;

        //размер матрицы
        public int M { set; get; }
        public int N { set; get; }
        //элементы матрицы
        public double[][] Elem { set; get; }

        //конструктор по умолчанию
        public Matrix() { }

        //конструктор нуль-матрицы m X n
        public Matrix(int m, int n)
        {
            N = n; M = m;
            Elem = new double[m][];
            for (int i = 0; i < m; i++) Elem[i] = new double[n];

        }

        //перегрузка операторов квадратные скобочки
        public double this[int index, int index2]
        {
            set
            {
                if (index >= M || index2>=N)
                    throw new Exception("Invalid index: out of range");
                Elem[index][index2] = value;
            }
            get
            {
                return Elem[index][index2];
            }
        }

        //умножение на скаляр с выделением памяти под новую матрицу
        public static Matrix operator *(Matrix T, double Scal)
        {
            Matrix RES = new Matrix(T.M, T.N);

            for (int i = 0; i < T.M; i++)
            {
                for (int j = 0; j < T.N; j++)
                {
                    RES.Elem[i][j] = T.Elem[i][j] * Scal;
                }
            }
            return RES;
        }

        //умножение на скаляр, результат запишется в исходную матрицу
        public void Dot_Scal(double Scal)
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Elem[i][j] *= Scal;
                }
            }
        }

        //умножение матрицы на вектор
        public static Vector operator *(Matrix T, Vector V)
        {
            if (T.N != V.N) throw new Exception("M * V: dim(Matrix) != dim(Vector)...");

            Vector RES = new Vector(T.M);

            for (int i = 0; i < T.M; i++)
            {
                for (int j = 0; j < T.N; j++)
                {
                    RES.Elem[i] += T.Elem[i][j] * V.Elem[j];
                }
            }
            return RES;
        }

        //умножение транспонированной матрицы на вектор
        public Vector Multiplication_Trans_Matrix_Vector(Vector V)
        {
            if (M != V.N) throw new Exception("Mt * V: dim(Matrix) != dim(Vector)...");

            Vector RES = new Vector(N);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    RES.Elem[i] += Elem[j][i] * V.Elem[j];
                }
            }
            return RES;
        }

        //умножение матрицы на матрицу
        public static Matrix operator *(Matrix T1, Matrix T2)
        {
            if (T1.N != T2.M) throw new Exception("M * M: dim(Matrix1) != dim(Matrix2)...");

            Matrix RES = new Matrix(T1.M, T2.N);

            for (int i = 0; i < T1.M; i++)
            {
                for (int j = 0; j < T2.N; j++)
                {
                    for (int k = 0; k < T1.N; k++)
                    {
                        RES.Elem[i][j] += T1.Elem[i][k] * T2.Elem[k][j];
                    }
                }
            }
            return RES;
        }

        public static Matrix operator -(Matrix matrix, Matrix matrix2)
        {
            return matrix + (matrix2 * -1);
        }

        //сложение матриц с выделением памяти под новую матрицу
        public static Matrix operator +(Matrix T1, Matrix T2)
        {
            if (T1.M != T2.M || T1.N != T2.N) throw new Exception("dim(Matrix1) != dim(Matrix2)...");

            Matrix RES = new Matrix(T1.M, T2.N);

            for (int i = 0; i < T1.M; i++)
            {
                for (int j = 0; j < T2.N; j++)
                {
                    RES.Elem[i][j] = T1.Elem[i][j] + T2.Elem[i][j];
                }
            }
            return RES;
        }

        //сложение матриц без выделения памяти под новую матрицу (добавление в ту же матрицу)
        public void Add(Matrix T2)
        {
            if (M != T2.M || N != T2.N) throw new Exception("dim(Matrix1) != dim(Matrix2)...");

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < T2.N; j++)
                {
                    Elem[i][j] += T2.Elem[i][j];
                }
            }
        }

        //транспонирование матрицы
        public Matrix Transpose_Matrix()
        {
            var RES = new Matrix(N, M);

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    RES.Elem[i][j] = Elem[j][i];
                }
            }
            return RES;
        }

        //копирование матрицы A
        public void Copy(Matrix A)
        {
            if (N != A.N || M != A.M) throw new Exception("Copy: dim(matrix1) != dim(matrix2)...");
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                    Elem[i][j] = A.Elem[i][j];
        }

        //вывод матрицы на консоль
        public void Console_Write_Matrix()
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                    Console.Write(String.Format("{0, -22}", Elem[i][j].ToString("E5")));
                Console.WriteLine();
            }
        }
        //обратная матрица
        public Matrix InvertibleMatrix()
        {
            if (this.M != this.N)
                throw new Exception("Матрица не квадратная, вычислить определитель невозможно");

            var determinant = CalculateDeterminant();
            if (Math.Abs(determinant) < EPS)
                throw new Exception("Матрица вырожденная, вычислить определитель невозможно");

            var result = new Matrix(M, M);
            for(int i=0; i<this.M; i++)
                for (int j=0; j<this.M; j++)
                {
                    result[i, j] = CalculateMinor(i, j) / determinant;
                    result[i, j] *= Math.Pow(-1, i + j);
                }

            result = result.Transpose_Matrix();
            return result;
        }
        //далее куча вспомогательных функций для вычисления определителя

        private double CalculateMinor(int i, int j)
        {
            return CreateMatrixWithoutColumn(j).CreateMatrixWithoutRow(i).CalculateDeterminant();
        }

        private Matrix CreateMatrixWithoutRow(int row)
        {
            if (row < 0 || row >= this.M)
            {
                throw new Exception("invalid row index");
            }
            var result = new Matrix(this.M - 1, this.N);
            int flag = 0;
            for (int i = 0; i < this.M; i++)
                if (i == row)
                {
                    flag=1;
                    continue;
                }
                else
                {
                    for (int j = 0; j < this.N; j++)
                        result[i - flag,j] = this[i,j];
                }
            return result;
        }

        private Matrix CreateMatrixWithoutColumn(int column)
        {
            if (column < 0 || column >= this.N)
            {
                throw new ArgumentException("invalid column index");
            }
            var result = new Matrix(this.M, this.N-1);
            int flag = 0;
            for (int i = 0; i < this.M; i++)
            {
                flag = 0;
                for (int j = 0; j < this.N; j++)
                    if (j == column)
                    {
                        flag = 1;
                        continue;
                    }
                    else
                    {
                        result[i, j - flag] = this[i, j];
                    }
            }

            return result;
        }


        public double CalculateDeterminant()
        {
            if (this.M != this.N)
            {
                throw new Exception("determinant can be calculated only for square matrix");
            }
            if (this.N == 2)
            {
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            }
            if (this.N == 1)
                return this[0, 0];

            throw new Exception("Вычисляем только для квадртаных");

        }

      


    }
}
