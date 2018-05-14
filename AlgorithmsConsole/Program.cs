using System;
using System.Collections.Generic;

namespace AlgorithmsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var arr = new int[100];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = random.Next(1, 10000);
            }

            var result = Algorithms.DirectSelectAlgorithms(false, arr);
            result.ForEach(p =>
            {
                Console.WriteLine(p);
            });
            Console.ReadKey();
        }
    }
}
