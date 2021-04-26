using System;
using System.Drawing;

namespace BattleShipGame
{
    internal class ControlInput
    {
        private static Point GetRandomEmptyPositionFromBoard(IBoard board)
        {
            Point position = new Point();
            BoardPositionStatus boardPositionStatus = BoardPositionStatus.Invalid;
            Random r = new Random();

            while (boardPositionStatus != BoardPositionStatus.Empty)
            {
                position = new Point(r.Next(1, 11), r.Next(1, 11));
                boardPositionStatus = board.GetPositionStatus(position);
            }

            return position;
        }

        private static Point GetCoordinateFromUserEntry(string userEntry)
        {
            if (userEntry.Length < 2)
                return new Point(-1, -1);

            int x = char.ToUpper(userEntry.Substring(0, 1).ToCharArray()[0]) - 64;
            int y = int.TryParse(userEntry.Substring(1), out int yy) ? yy : -1;

            return new Point(x, y);
        }

        internal static Point AskUserForShotLocation(IBoard board)
        {
            Console.Write("Enter a letter and a number (empty for random shot): ");
            string userEntry = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(userEntry))
                return GetRandomEmptyPositionFromBoard(board);

            return GetCoordinateFromUserEntry(userEntry);
        }
    }
}
