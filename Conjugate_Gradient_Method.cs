using System;
using System.IO;

namespace MO_lab2
{
   
    public class Conjugate_Gradient_Method
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
        private Vector x_previous; // вектор x на пердыдщей итерации
        private Vector s; // вектор S - направление посика
        private const double EPS = 1E-3;
        private const double DELTA = 1E-8;
        public enum Modify //модификация в засисмости от которой будет меняться вычисления коэфициента омега
        {
            PolakRibier_method,
            FletcherReeves_method
        }
        public int countIteration = 0;
        public int countCalculation = 0;


        //в конструктор ссылка на функцию и её градиент, а так же начальное приближение
        public Conjugate_Gradient_Method(lab2_function f, lab2_gradient g, Vector x)
        {
            this.Own_function = f;
            this.Own_GradientFunction = g;
            this.x = x.Copy();
        }

        public Vector StartSolver(Modify mod=Modify.PolakRibier_method)
        {
            countCalculation = 0;
            countIteration = 0;
            //создаём вектор S размерность совпадает с размерностью вектора начального приближения
            s = Own_GradientFunction(x);

            //инвертируем значения
            for (int i=0; i<s.N; i++)
            {
                s[i] = -s[i];
            }

            double[] Arraylymbda; // диапазон лямбд, где хранится минимальная лямбда
            double lymbda=0; 
            double omega=0;

            while (s.Norm2() > EPS && countIteration<5000)
            {
                this.countIteration+=1;
                // диапазон лямбд, где хранится минимальная лямбда 
                Arraylymbda = IntervalSearch(0);
                // пытаемся найти лямбду
                lymbda = GoldenSectionMethod(Arraylymbda[0], Arraylymbda[1]);

                x_previous = x.Copy();
                x = x + s * lymbda;

                //вычисляем коэффициент омега
                switch (mod)
                {
                    case (Modify.PolakRibier_method):
                        {
                            omega = Omega_PolakRibier_method();
                            break;
                        }
                    case (Modify.FletcherReeves_method):
                        {
                            omega = Omega_FletcherReeves_method();
                            break;
                        }
                }

                //определяем новое направление s
                s = s * omega - Own_GradientFunction(x);
            }

            return x;
        }

        public Vector StartSolver_WriteToFile(Modify mod = Modify.PolakRibier_method, string fileName = "OutConjugateGradientMethod.txt")
        {
            //поток для записи в файлик
            var file = new StreamWriter(fileName, false);
            countCalculation = 0;
            countIteration = 0;
            //создаём вектор S размерность совпадает с размерностью вектора начального приближения
            s = Own_GradientFunction(x);

            //инвертируем значения
            for (int i = 0; i < s.N; i++)
            {
                s[i] = -s[i];
            }

            double[] Arraylymbda; // диапазон лямбд, где хранится минимальная лямбда
            double lymbda = 0;
            double omega = 0;

            while (s.Norm2() > EPS && countIteration < 5000)
            {
                this.countIteration += 1;

                // диапазон лямбд, где хранится минимальная лямбда 
                Arraylymbda = IntervalSearch(0);
                // пытаемся найти лямбду
                lymbda = GoldenSectionMethod(Arraylymbda[0], Arraylymbda[1]);


                x_previous = x.Copy();
                x = x + s * lymbda;

                //угол между вектрами s и x
                var angle_s_x = Math.Acos((x[0] * s[0] + x[1] * s[1]) / (Math.Sqrt(x[0] * x[0] + x[1] * x[1]) * Math.Sqrt(s[0] * s[0] + s[1] * s[1])));
                //запись в файл
                file.Write($"{countIteration} ({x[0]:f4},{x[1]:f4}) {Own_function(x):f4} ({s[0]:f4},{s[1]:f4}) {lymbda:f4} ({x[0]-x_previous[0]:f4},{x[1] - x_previous[1]:f4},{Own_function(x)-Own_function(x_previous):f4}) {angle_s_x:f4} ({Own_GradientFunction(x)[0]:f4},{Own_GradientFunction(x)[1]:f4})\n");


                //вычисляем коэффициент омега
                switch (mod)
                {
                    case (Modify.PolakRibier_method):
                        {
                            omega = Omega_PolakRibier_method();
                            break;
                        }
                    case (Modify.FletcherReeves_method):
                        {
                            omega = Omega_FletcherReeves_method();
                            break;
                        }
                }

                //определяем новое направление s
                s = s * omega - Own_GradientFunction(x);
            }
            file.Close();
            return x;
        }

        public double Omega_PolakRibier_method()
        {
            double numerator = 0;
            double denumerator = 0;
            Vector b = Own_GradientFunction(x);
            Vector a = Own_GradientFunction(x) - Own_GradientFunction(x_previous);
            //вычисляем числитель
            for (int i=0; i<b.N; i++)
            {
                numerator += b[i] * a[i];
            }

            //вычисляем знаменатель
            b = Own_GradientFunction(x_previous);
            for (int i = 0; i < b.N; i++)
            {
                denumerator += b[i] * s[i];
            }


            return numerator/denumerator;
        }

        public double Omega_FletcherReeves_method()
        {
            double numerator = 0;
            double denumerator = 0;
            numerator = Own_GradientFunction(x).Norm2_withoutSQRT();
            denumerator = Own_GradientFunction(x_previous).Norm2_withoutSQRT();

            return numerator / denumerator;
        }

        //метод золотого сечения для поиска минимальной лямбды
        private double GoldenSectionMethod(double A, double B)
        {


            double[] x = new double[2];
            double[] a = new double[2];
            double[] b = new double[2];
            double[] funcResult = new double[2]; // для результата
            a[1] = A;
            b[1] = B;

            x[0] = (a[1] + 0.381966011d * (b[1] - a[1]));
            x[1] = (a[1] + 0.6180033989d * (b[1] - a[1]));
            funcResult[0] = Own_function(this.x + this.s * x[0]);
            funcResult[1] = Own_function(this.x + this.s * x[1]);
            countCalculation += 2;

            while (b[1] - a[1] > EPS)
            {

                if (funcResult[0] < funcResult[1])
                {
                    //сохраняем значения с предыдущих итераций
                    a[0] = a[1];
                    b[0] = b[1];

                    b[1] = x[1];
                    x[1] = x[0];
                    x[0] = (a[1] + 0.381966011d * (b[1] - a[1]));
                    //вычисляем ОДНО новое значение функции, другое перезаписываем
                    funcResult[1] = funcResult[0];
                    funcResult[0] = Own_function(this.x + this.s * x[0]);
                    countCalculation++;
                }
                else
                {
                    //сохраняем значения с предыдущих итераций
                    a[0] = a[1];
                    b[0] = b[1];

                    a[1] = x[0];
                    x[0] = x[1];
                    x[1] = (a[1] + 0.6180033989d * (b[1] - a[1]));
                    //вычисляем ОДНО новое значение функции, другое перезаписываем
                    funcResult[0] = funcResult[1];
                    funcResult[1] = Own_function(this.x + this.s * x[1]);
                    countCalculation++;
                }
                
            }
            return (a[1]+b[1])/2;
        }

        
        // интервальный поиск чтобы найти диапазон где хранится лямбда-мин
        private double[] IntervalSearch(double lymbda)
        {

            double h = 0;
            //лямбда на предыдущей итерации
            double lymbda_previous = lymbda;


            //шаг 1. определяем направление поиска.
            if (Own_function(x+this.s*lymbda) > Own_function(x + this.s * (lymbda+DELTA)))
            {
                lymbda = lymbda + DELTA;
                h = DELTA;
            }
            else if (Own_function(x + this.s * lymbda) > Own_function(x + this.s * (lymbda - DELTA)))
            {
                lymbda = lymbda - DELTA;
                h = -DELTA;
            }
            countCalculation += 2;
            //шаг 2
            h *= 2;
            lymbda_previous = lymbda;
            lymbda = lymbda + h;

            //шаги 2 и 3
            while (Own_function(x + this.s * lymbda_previous) > Own_function(x + this.s * lymbda))
            {
                countCalculation++;
                h *= 2;

                lymbda_previous = lymbda;
                lymbda = lymbda + h;
            }

            double[] result = new double[2];
            result[0] = lymbda_previous;
            result[1] = lymbda;
            return result;
        }
    }
}
