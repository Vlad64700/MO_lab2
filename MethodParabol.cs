using System;
namespace MO_lab2
{
    public class MethodParabol
    {
        public lab1_function Own_function;
        public double x = 0;
        private double x_previos = 0;
        private const double EPS = 1E-7;

        public MethodParabol(lab1_function Own_function, double x=0)
        {
            this.Own_function = Own_function;
            this.x = 0;
        }

        public double ParabolSearch(double A, double B)
        {
            double x1, x2, x3;
            x1 = A;
            x3 = B;
            x2 = (A + B) / 2;

            //если не смоги попасть в неравенство
            while (Own_function(x2) > Own_function(x1) || Own_function(x2) > Own_function(x3))
                x2 = (x2 + x1) / 2;

            //вычисляем коэфициенты квадратного трехчлена
            double a0 = Own_function(x1);
            double a1 = (Own_function(x2) - Own_function(x1)) / (x2 - x1);
            double a2 = ((Own_function(x3) - Own_function(x1)) / (x3 - x1) - (Own_function(x2) - Own_function(x1)) / (x2 - x1)) / (x3 - x2);

            x = (x1 + x2 - a1 / a2) / 2;
            if (x1 <= x && x <= x2)
            {
                x3 = x2;
                x2 = x;
            }
            else
            {
                x1 = x2;
                x2 = x;
            }

            while (x - x_previos > EPS)
            {
                //вычисляем коэфициенты квадратного трехчлена
                a0 = Own_function(x1);
                a1 = (Own_function(x2) - Own_function(x1)) / (x2 - x1);
                a2 = ((Own_function(x3) - Own_function(x1)) / (x3 - x1) - (Own_function(x2) - Own_function(x1)) / (x2 - x1)) / (x3 - x2);

                x_previos = x;
                x = (x1 + x2 - a1 / a2) / 2;
                if (x1 <= x && x <= x2)
                {
                    x3 = x2;
                    x2 = x;
                }
                else
                {
                    x1 = x2;
                    x2 = x;
                }
            }
            

            return x;
        }
    }
}
