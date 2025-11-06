using System;
using System.IO;

// namespace fileHandling
// {
//     class Program
//     {
//         static void Main()
//         {
//             string path = @"D:\ups\try\sample.txt";

//             if (File.Exists(path))
//             {
//                 using (StreamReader reader = new StreamReader(path))
//                 {
//                     string? line;
//                     while ((line = reader.ReadLine()) != null)
//                     {
//                         Console.WriteLine(line);
//                     }
//                 }
//             }
//             else
//             {
//                 Console.WriteLine(" File not found at: " + path);
//             }
//         }
//     }
// }


using System;
using System.IO;

namespace fileHandling
{
    class Program
    {
        static void Main()
        {
            string path = @"D:\ups\try\sample.txt";

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine("Hello from C#!");
                writer.WriteLine("This text overwrites the previous content.");
                writer.WriteLine("Writing to file successfully done ");
            }

            Console.WriteLine(" File written successfully at: " + path);
        }
    }
}


// class Program
// {
//     static void Main()
//     {
//         FileStream fs = new FileStream(@"D:\ups\try\sample.txt", FileMode.OpenOrCreate);
//         StreamWriter w = new StreamWriter(fs);

//         string ans = "Y";

//         while (ans == "Y" || ans == "y")
//         {
//             Console.WriteLine("ENter the Book ID : ");
//             string Id = Console.ReadLine();
//             Console.WriteLine("ENter the Book Name : ");
//             string title = Console.ReadLine();
//             Console.WriteLine("Enter the author name : ");
//             string author = Console.ReadLine();


//             string line = ($"{Id} , {title} , {author}");
//             w.WriteLine(line);


//             Console.WriteLine("Do u want to add More (Y/N)");

//             ans = Console.ReadLine();
//         }

//         w.Close();
//     }
// }


// using System;
// using System.IO;

// class Program
// {
//     static void Main()
//     {
//         string path = @"D:\ups\try\sample.txt";

//         if (File.Exists(path))
//         {
//             using (StreamReader reader = new StreamReader(path))
//             {
//                 string? line;
//                 while ((line = reader.ReadLine()) != null)
//                 {
//                     string[] cols = line.Split(',');

//                     Console.WriteLine("Id     : " + cols[0].Trim());
//                     Console.WriteLine("Name   : " + cols[1].Trim());
//                     Console.WriteLine("Author : " + cols[2].Trim());
//                 }
//             }
//         }
//         else
//         {
//             Console.WriteLine(" File not found at: " + path);
//         }
//     }
// }
