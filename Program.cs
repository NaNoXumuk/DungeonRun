using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonRun
{
    internal class Program
    {
        static void Main(string[] args)
        {   int x=1,y=1;
            char[,] Map = new char[30, 200];
            
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            
            Console.WindowHeight = Console.LargestWindowHeight;
            Console.WindowWidth = Console.LargestWindowWidth;
            
     
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                    Map[i,j]=' ';
                Console.WriteLine();
            }





            
                Console.SetCursorPosition(x, y);
            Console.Write('@');
            
            while (true)
            {

               

                //switch (Console.ReadKey().Key)
                //{
                //    case ConsoleKey.W: if (Map[y - 1, x] != '#') { Console.Write(' '); Console.SetCursorPosition(x, y--); } break;
                //    case ConsoleKey.A: if (Map[y, x - 1] != '#') { Console.Write(' '); Console.SetCursorPosition(x--, y); } break;
                //    case ConsoleKey.S: if (Map[y + 1, x] != '#') { Console.Write(' '); Console.SetCursorPosition(x, y++); } break;
                //    case ConsoleKey.D: if (Map[y, x + 1] != '#') { Console.Write(' '); Console.SetCursorPosition(x++, y); } break;
                //}
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.W: Console.Write(' '); Console.SetCursorPosition(x, y--);  break;
                    case ConsoleKey.A: Console.Write(' '); Console.SetCursorPosition(x--, y);  break;
                    case ConsoleKey.S: Console.Write(' '); Console.SetCursorPosition(x, y++);  break;
                    case ConsoleKey.D: Console.Write(' '); Console.SetCursorPosition(x++, y);  break;
                }
                Console.Write('@');

                //for (int i = y - 1; i < y + 1; i++)
                //    for (int j = x - 1; j < x + 1; j++)
                //        Console.Write(Map[i, j]);



                //Console.Clear();
                //    for (int i = 0; i < Map.GetLength(0); i++)
                //    {
                //        for (int j = 0; j < Map.GetLength(1); j++)
                //            Console.Write(Map[i,j]);
                //        Console.WriteLine();
                //    }



            }
        }
    }
}
