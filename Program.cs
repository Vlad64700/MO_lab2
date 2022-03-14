using System;

namespace MO_lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            lab2_function SquereFunction = (Vector x) => {
                return 100 * Math.Pow(x[1] - x[0], 2) + Math.Pow(1 - x[0], 2);
            };

            lab2_gradient SquereGradientFunction = (Vector x) =>
            {
                var result = new Vector(x.N);
                result[0] = 202 * x[0] - 200 * x[1] - 2;
                result[1] = 200 * x[1] - 200 * x[0];
                return result;
            };

            lab2_function RosenbrocFunction = (Vector x) => {
                return 100 * Math.Pow(x[1] - x[0]*x[0], 2) + Math.Pow(1 - x[0], 2);
            };

            lab2_gradient RosenbrocGradientFunction = (Vector x) =>
            {
                var result = new Vector(x.N);
                result[0] = -2 * x[0] * (-100 * x[0] * x[0] + 100 * x[1]) - 200 * x[0] * (-x[0] * x[0] + x[1]) + 2 * x[0] - 2;
                result[1] = -200 * x[0] * x[0] + 200 * x[1];
                return result;
            };

            lab2_function IndividualFunction = (Vector x) => {
                return -1*(2*Math.Exp( -Math.Pow((x[0]-1)/2,2)-Math.Pow((x[1] - 1), 2) ) + 3 * Math.Exp(-Math.Pow((x[0] - 2) / 3, 2) - Math.Pow((x[1] - 3)/2, 2)));
            };

            lab2_gradient IndividualGradientFunction = (Vector x) =>
            {
                var result = new Vector(x.N);
                result[0] = -1*(3*(4d/9d-2*x[0]/9)*Math.Exp(-Math.Pow((x[0]/3-2d/3d),2)- Math.Pow((x[1] / 2 - 3d / 2d), 2))+ 2 * (1d / 2d - x[0] / 2) * Math.Exp(-Math.Pow((x[0] / 2 - 1d / 2d), 2) - Math.Pow((x[1] - 1), 2)));

                result[1] = -1*(3 * (3d / 2d -  x[1] / 2) * Math.Exp(-Math.Pow((x[0] / 3 - 2d / 3d), 2) - Math.Pow((x[1] / 2 - 3d / 2d), 2)) + 2 * (2 - 2*x[1]) * Math.Exp(-Math.Pow((x[0] / 2 - 1d / 2d), 2) - Math.Pow((x[1] - 1), 2)));
                return result;
            };

            var x = new Vector(2);
            x[0] = 2;
            x[1] = 3;



            var go = new Conjugate_Gradient_Method(RosenbrocFunction, RosenbrocGradientFunction, x);

            var res=go.StartSolver();
            Console.WriteLine("x: "+res[0] + " " + res[1]);
            Console.WriteLine("f(x) "+ RosenbrocFunction(res));
            Console.WriteLine("Число итераций " + go.countIteration);
            Console.WriteLine("Число вычислений " + go.countCalculation);
            Console.WriteLine("----------");

            //x[0] = 2;
            //x[1] = 3;
            //go.X=x;
            //res = go.StartSolver(Conjugate_Gradient_Method.Modify.FletcherReeves_method);
            //Console.WriteLine("x: " + res[0] + " " + res[1]);
            //Console.WriteLine("f(x) " + -IndividualFunction(res));
            //Console.WriteLine("Число итераций " + go.countIteration);
            //Console.WriteLine("Число вычислений " + go.countCalculation);
            //Console.WriteLine("----------");


            // код для методы порабол
            //Console.WriteLine("----------");
            //var go3 = new MethodParabol((double x) => { return (x - 1) * (x - 1); });
            //var res3 = go3.ParabolSearch(0, 10);
            //Console.WriteLine("x: " + res3);
            //Console.WriteLine("f(x) " + (res3 - 1) * (res3 - 1));


        }
    }
}
