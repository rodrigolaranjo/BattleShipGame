using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BattleShipGame
{
    internal class ControlOutput
    {
        private static int shots = -1;

        private static List<IShip> sunkShips = new List<IShip>();

        private static string GetLetterFromNumber(int number)
        {
            return Convert.ToChar(64 + number).ToString();
        }

        internal static void DrawBoard(IBoard board)
        {
            Console.Clear();

            Console.Write("  ");
            for (int y = 1; y <= 10; y++)
            {
                Console.Write(y);
                Console.Write(" ");
            }
            Console.WriteLine();
            for (int x = 1; x <= 10; x++)
            {
                Console.Write(GetLetterFromNumber(x) + " ");
                for (int y = 1; y <= 10; y++)
                {
                    switch (board.GetPositionStatus(new Point(x, y)))
                    {
                        case BoardPositionStatus.Hit:
                            Console.Write("X");
                            break;
                        case BoardPositionStatus.Miss:
                            Console.Write(".");
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine("");
        }

        internal static void ShowFinalResult(IBoard board)
        {
            DrawBoard(board);
            ShowMessages(board);
            Console.WriteLine($"All ships sunk. You did in {board.BoardPositions.Count} hits.");
            Console.WriteLine();
            Console.WriteLine("Game over");
            Console.ReadKey();
        }

        internal static void ShowMessages(IBoard board)
        {
            string message = "";

            if (board.BoardPositions.Count == shots)
                message = "Invalid shot.";

            IShip sunkShip = board.Ships.Where(s => s.IsSunk && !sunkShips.Contains(s)).FirstOrDefault();

            if (sunkShip != null)
                message = $"{sunkShip.ShipName} sunk!";

            sunkShips = board.Ships.Where(s => s.IsSunk).ToList();
            shots = board.BoardPositions.Count;

            if (!String.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
                Console.WriteLine();
            }
        }
    }
}
