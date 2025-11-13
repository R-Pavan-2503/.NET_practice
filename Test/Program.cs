namespace Test
{
    class Program
    {
        static void Main()
        {
            int[] arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int i = 0;
            int j = 9;
            while (i <= j)
            {
                Console.Write($"{arr[j]} ");
                Console.Write($"{arr[i]} ");
                i += 1;
                j -= 1;
            }
        }
    }
}