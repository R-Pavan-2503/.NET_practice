using System;

namespace ArrayDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            int[] arr = new int[] { 1, 2, 3, 4, 5 };
            Console.WriteLine("Single-Dimensional Array:");
            foreach (var item in arr)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine("\n");


            int[,] multiArr = {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 }
            };

            Console.WriteLine("Multi-Dimensional (2D) Array:");
            for (int i = 0; i < multiArr.GetLength(0); i++)
            {
                for (int j = 0; j < multiArr.GetLength(1); j++)
                {
                    Console.Write(multiArr[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();


            int[][] jaggedArr = new int[3][];
            jaggedArr[0] = new int[] { 1, 2 };
            jaggedArr[1] = new int[] { 3, 4, 5 };
            jaggedArr[2] = new int[] { 6, 7, 8, 9 };

            Console.WriteLine("Jagged Array:");
            for (int i = 0; i < jaggedArr.Length; i++)
            {
                for (int j = 0; j < jaggedArr[i].Length; j++)
                {
                    Console.Write(jaggedArr[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
