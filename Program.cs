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
            Console.ReadKey();
            while (true)
            {

                ConsoleKeyInfo WayToGo = Console.ReadKey();
                switch (WayToGo.Key)
                {
                    case ConsoleKey.W: if (Map[y - 1, x] != '#') { Map[y, x] = ' '; Console.Write(' '); Console.SetCursorPosition(x, y--); Map[y, x] = '@'; Console.Write('@'); } break;
                    case ConsoleKey.A: if (Map[y, x - 1] != '#') { Map[y, x] = ' '; Console.Write(' '); Console.SetCursorPosition(x--, y); Map[y, x] = '@'; Console.Write('@'); } break;
                    case ConsoleKey.S: if (Map[y + 1, x] != '#') { Map[y, x] = ' '; Console.Write(' '); Console.SetCursorPosition(x, y++); Map[y, x] = '@'; Console.Write('@'); } break;
                    case ConsoleKey.D: if (Map[y, x + 1] != '#') { Map[y, x] = ' '; Console.Write(' '); Console.SetCursorPosition(x++, y); Map[y, x] = '@'; Console.Write('@'); } break;
                }
            Console.Clear();
                for (int i = 0; i < Map.GetLength(0); i++)
                {
                    for (int j = 0; j < Map.GetLength(1); j++)
                        Console.Write(Map[i,j]);
                    Console.WriteLine();
                }



            }
        }
    }
}
