using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ConsoleApp1
{

    public class Plane
    {
        public double[] Factors;
        public double Value;
    }

    class Program
    {

        public static int N;
        public static double p;
        public static double[][] points;

        static double point_h(double[] factors) {
            double err = 0;
            foreach (var p in points)
            {
                double val = Math.Abs(factors[0] * p[0] + factors[1] * p[1] + factors[2] * p[2]) / (factors[0] * factors[0] + factors[1] * factors[1] + factors[2] * factors[2]);
                err += Math.Log(val + 1);
            }

            return err;
        }

        static double AbsoluteError(double[] factors, double[] point)
        {
            return Math.Abs(factors[0] * point[0] + factors[1] * point[1] + factors[2] * point[2] + factors[3]) /
                    Math.Pow(factors[0] * factors[0] + factors[1] * factors[1] + factors[2] * factors[2], 0.5);
        }

        static double[] get_plane_by_points(double[] p1, double[] p2, double[] p3) {
            var x1 = p1[0];
            var y1 = p1[1];
            var z1 = p1[2];
            var x2 = p2[0];
            var y2 = p2[1];
            var z2 = p2[2];
            var x3 = p3[0];
            var y3 = p3[1];
            var z3 = p3[2];
            var A = y1 * (z2 - z3) + y2 * (z3 - z1) + y3 * (z1 - z2);
            var B = z1 * (x2 - x3) + z2 * (x3 - x1) + z3 * (x1 - x2);
            var C = x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2);
            var D = x1 * (y2 * z3 - y3 * z2) + x2 * (y3 * z1 - y1 * z3) + x3 * (y1 * z2 - y2 * z1);
            D *= -1;

            return new double[] { A, B, C, D };
        }

        static bool CheckPlane(double[] factors)
        {
            int ok = 0;
            int err = 0;
            foreach( var point in points)
            {
                if (AbsoluteError(factors, point) <= p)
                {
                    ok++;
                    if (ok > N / 2.0)
                    {
                        return true;
                    }
                }
                else
                {
                    err++;
                    if (err > N / 2.0)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        static void Main(string[] args)
        {

            // Read the file and display it line by line.  
            var file = new System.IO.StreamReader("input.txt");
            p = Double.Parse(file.ReadLine(), CultureInfo.InvariantCulture);
            N = int.Parse(file.ReadLine(), CultureInfo.InvariantCulture);

            points = new double[N][];
            for( int i = 0; i < N; i ++)
            {
                var s = file.ReadLine().Split(new char[] { '\t' });
                points[i] = new double[] {
                    Double.Parse(s[0], CultureInfo.InvariantCulture),
                    Double.Parse(s[1], CultureInfo.InvariantCulture),
                    Double.Parse(s[2], CultureInfo.InvariantCulture)
                };
            }
            file.Close();
            Random rand = new Random();

            //int planes_count = Math.Min(N, 2000);
            //var planes = new List<Plane>();
            //for ( int i = 0; i < planes_count; i++)
            //{
            //    int pi1 = (int)(rand.NextDouble() * (points.Length-1));
            //    int pi2 = (int)(rand.NextDouble() * (points.Length - 1));
            //    int pi3 = (int)(rand.NextDouble() * (points.Length - 1));
            //    while (pi2 == pi1)
            //    {
            //        pi2 = (pi2 + 1) % points.Length;
            //    }
            //    while (pi3 == pi1 || pi3 == pi2)
            //    {
            //        pi3 = (pi3 + 1) % points.Length;
            //    }
            //    double val = 0;
            //    double[] factors = null;
            //    try
            //    {
            //        factors = get_plane_by_points(points[pi1], points[pi2], points[pi3]);
            //        val = point_h(factors);
            //    }
            //    catch (Exception)
            //    {
            //        continue;
            //    }
            //    planes.Add(new Plane() {
            //        Factors = factors,
            //        Value = val
            //    });
            //}
            //planes.Sort((x, y) => x.Value.CompareTo(y.Value));

            double[] factors = null;
            while (true)
            {
                int pi1 = (int)(rand.NextDouble() * 1000000) % points.Length;
                int pi2 = (int)(rand.NextDouble() * 1000000) % points.Length;
                int pi3 = (int)(rand.NextDouble() * 1000000) % points.Length;
                while (pi2 == pi1)
                {
                    pi2 = (pi2 + 1) % points.Length;
                }
                while (pi3 == pi1 || pi3 == pi2)
                {
                    pi3 = (pi3 + 1) % points.Length;
                }
                
                try
                {
                    factors = get_plane_by_points(points[pi1], points[pi2], points[pi3]);
                    if( CheckPlane(factors))
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            string res = "";
            foreach( var f in factors)
            {
                res += f.ToString("0.000000", CultureInfo.InvariantCulture) + " ";
            }

            Console.Write(res);
        }
    }
}
