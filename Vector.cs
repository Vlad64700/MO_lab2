using System;
using System.IO;

namespace MO_lab2
{

    public class Vector
    {
        //размер вектора
        public int N { set; get; }
        public double[] Elem { set; get; }
        private const double EPS = 1E-10;

        //конструктор по умолчанию
        public Vector()
        {
        }

        //конструктор нуль-вектора по размеру n
        public Vector(int n)
        {
            N = n;
            Elem = new double[n];
        }

        //перегрузка операторов квадратные скобочки
        public double this[int index]
        {
            set
            {
                if (index >= N)
                    throw new Exception("Invalid index: out of range");
                Elem[index] = value;
            }
            get
            {
                return Elem[index];
            }
        }


        //умножение на скаляр с выделением памяти под новый вектор
        public static Vector operator *(Vector T, double Scal)
        {
            Vector RES = new Vector(T.N);

            for (int i = 0; i < T.N; i++)
            {
                RES.Elem[i] = T.Elem[i] * Scal;
            }
            return RES;
        }

        //умножение на скаляр, результат записывается в тот же вектор
        public void Dot_Scal(double Scal)
        {
            for (int i = 0; i < N; i++)
            {
                Elem[i] = Elem[i] * Scal;
            }
        }

        //скалярное произведение векторов
        public static double operator *(Vector V1, Vector V2)
        {
            if (V1.N != V2.N) throw new Exception("V1 * V2: dim(vector1) != dim(vector2)...");

            Double RES = 0.0;

            for (int i = 0; i < V1.N; i++)
            {
                RES += V1.Elem[i] * V2.Elem[i];
            }
            return RES;
        }

        //сумма векторов с выделением памяти под новый вектор
        public static Vector operator +(Vector V1, Vector V2)
        {
            if (V1.N != V2.N) throw new Exception("V1 + V2: dim(vector1) != dim(vector2)...");
            Vector RES = new Vector(V1.N);

            for (int i = 0; i < V1.N; i++)
            {
                RES.Elem[i] = V1.Elem[i] + V2.Elem[i];
            }
            return RES;
        }

        //сумма всех компонент вектора со скалятором
        public static Vector operator +(Vector V1, double x)
        {
            Vector RES = new Vector(V1.N);

            for (int i = 0; i < V1.N; i++)
            {
                RES.Elem[i] = V1.Elem[i] + x;
            }
            return RES;
        }

        //разность всех компонент вектора со скалятором
        public static Vector operator -(Vector V1, double x)
        {
            Vector RES = new Vector(V1.N);

            for (int i = 0; i < V1.N; i++)
            {
                RES.Elem[i] = V1.Elem[i] - x;
            }
            return RES;
        }

        //разность векторов с выделением памяти под новый вектор
        public static Vector operator -(Vector V1, Vector V2)
        {
            if (V1.N != V2.N) throw new Exception("V1 + V2: dim(vector1) != dim(vector2)...");
            Vector RES = new Vector(V1.N);

            for (int i = 0; i < V1.N; i++)
            {
                RES.Elem[i] = V1.Elem[i] - V2.Elem[i];
            }
            return RES;
        }

        //сумма векторов без выделения памяти под новый вектор
        public void Add(Vector V2)
        {
            if (N != V2.N) throw new Exception("V1 + V2: dim(vector1) != dim(vector2)...");

            for (int i = 0; i < N; i++)
            {
                Elem[i] += V2.Elem[i];
            }
        }

        //копирование вектора V2
        public void Copy(Vector V2)
        {
            if (N != V2.N) throw new Exception("Copy: dim(vector1) != dim(vector2)...");
            for (int i = 0; i < N; i++) Elem[i] = V2.Elem[i];
        }

        //нормальное копирование вектора 
        public Vector Copy()
        {
            var temp = new Vector(this.N);
            temp.Copy(this);
            return temp;
        }

        //норма вектора: ключевое слово this ссылается на текущий экземпляр класса
        public double Norm()
        {
            return Math.Sqrt(this * this);
        }

        //нормирование вектора
        public void Normalizing()
        {
            double norma = Norm();
            if (norma < EPS) throw new Exception("Error in Vector.Normalizing: ||V|| = 0...");
            for (int i = 0; i < N; i++) Elem[i] /= norma;
        }

        //вывод вектора на консоль
        public void Console_Write_Vector()
        {
            for (int i = 0; i < N; i++) Console.WriteLine(Elem[i]);
        }

        public double Norm2()
        {
            double res = 0;
            for (int i = 0; i < N; i++)
            {
                res += Elem[i] * Elem[i];
            }
            return Math.Sqrt(res);
        }

        public double Norm2_withoutSQRT()
        {
            double res = 0;
            for (int i = 0; i < N; i++)
            {
                res += Elem[i] * Elem[i];
            }
            return res;
        }

    }
}
