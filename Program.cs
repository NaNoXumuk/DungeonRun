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
            
            //for (int i = 0; i < MapTest.GetLength(0); i++)
            //{
            //    for (int j = 0; j < MapTest.GetLength(1) ; j++)                
            //        Console.Write(MapTest[i, j]);                
            //Console.WriteLine();
            //}
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                    Map[i,j]='-';
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
                for (int i = y; i < y + 3; i++)
                    for (int j = x; j < x + 3; j++)
                        if (Map[i, j] == 'w' || Map[i, j] == 'a' || Map[i, j] == 's' || Map[i, j] == 'd')
                            Console.Write(Map[i, j]);
                        


            }
        }
    }
}
