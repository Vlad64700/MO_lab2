using System;
using System.IO;

namespace MO_lab2
{
    public class NewtonMethod
    {
        public lab2_function Own_function; // ссылка на функцию
        public lab2_gradient Own_GradientFunction; //ссылка на градиент
        private Vector x; // веткор х
        public Vector X
        {
            set
            {
                x = value.Copy();
            }
            get
            {
                return x;
            }
        }
        private Vector x_previous;
        private const double EPS = 1E-3;
        private const double DELTA = 1E-8;
        public int countIteration = 0;
        public int countCalculation = 0;

        public NewtonMethod(lab2_function Own_function, lab2_gradient Own_GradientFunction, Vector x)
        {
            this.Own_function = Own_function;
            this.Own_GradientFunction = Own_GradientFunction;
            this.X = x;

        }

        public Vector StartSolver(double lymbda = 1)
        {

            var invertibleMatrix = new Matrix(x.N, x.N); //обратная матрица
            x_previous = x;
            invertibleMatrix = diff2().InvertibleMatrix();
            x = x_previous - invertibleMatrix * Own_GradientFunction(x) * lymbda;

            while ((x - x_previous).Norm2() > EPS && countIteration<5000)
            {
                countIteration++;
                countCalculation++;
                x_previous=x;
                invertibleMatrix = diff2().InvertibleMatrix();
                x = x_previous - invertibleMatrix * Own_GradientFunction(x) * lymbda;
            }
            return x;
        }
        public Vector StartSolver_WriteToFile(double lymbda=1, string fileName = "OutNewtonMethod.txt")
        {
            //поток для записи в файл
            var file = new StreamWriter(fileName, false);

            var invertibleMatrix = new Matrix(x.N, x.N); //обратная матрица
            var d2f = new Matrix(x.N, x.N);
            x_previous = x;
            invertibleMatrix = diff2().InvertibleMatrix();
            x = x_previous - invertibleMatrix * Own_GradientFunction(x) * lymbda;

            while ((x - x_previous).Norm2() > EPS && countIteration < 5000)
            {
                countIteration++;
                countCalculation++;
                
                d2f = diff2();
                //запись в файл
                file.Write($"{countIteration} ({x[0]:f4},{x[1]:f4}) {Own_function(x):f4} {lymbda:f4} ({x[0] - x_previous[0]:f4},{x[1] - x_previous[1]:f4},{Own_function(x) - Own_function(x_previous):f4}) ({-d2f[0,0]:f4},{d2f[0, 1]:f4},{d2f[1, 0]:f4},{d2f[1, 1]:f4})\n");

                x_previous = x;
                invertibleMatrix = diff2().InvertibleMatrix();
                x = x_previous - invertibleMatrix * Own_GradientFunction(x) * lymbda;
            }
            file.Close();
            return x;
        }

        //вычисление второй производной, 
        public Matrix diff2(double step=1E-7)
        {
            countCalculation += 8;
            Vector tempVector = x.Copy();
            Vector tempVector2 = x.Copy();
            Matrix matrix = new Matrix(2, 2);

            // d2f/dxdx
            tempVector[0] += step;
            tempVector2[0] -= step;
            matrix[0, 0] = (Own_GradientFunction(tempVector)[0]-Own_GradientFunction(tempVector2)[0])/(step*2);
            tempVector = x.Copy();
            tempVector2 = x.Copy();

            // d2f/dxdy
            tempVector[1] += step;
            tempVector2[1] -= step;
            matrix[0, 1] = (Own_GradientFunction(tempVector)[0] - Own_GradientFunction(tempVector2)[0]) / (2*step);
            tempVector = x.Copy();
            tempVector2 = x.Copy();

            // d2f/dxdy
            tempVector[0] += step;
            tempVector2[0] -= step;
            matrix[1, 0] = (Own_GradientFunction(tempVector)[1] - Own_GradientFunction(tempVector2)[1]) / (2*step);
            tempVector = x.Copy();
            tempVector2 = x.Copy();

            // d2f/dydy
            tempVector[1] += step;
            tempVector2[1] -= step;
            matrix[1, 1] = (Own_GradientFunction(tempVector)[1] - Own_GradientFunction(tempVector2)[1]) / (2*step);


            return matrix;
        }
    }
}
