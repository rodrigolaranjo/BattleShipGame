using System.Drawing;

namespace BattleShipGame
{
    class Program
    {
        private readonly static IBoard board = BoardFactory.NewBoard();

        static void Main(string[] args)
        {
            AddShips();

            while (!board.GameOver)
            {
                ControlOutput.DrawBoard(board);
                ControlOutput.ShowMessages(board);
                Point shotPosition = ControlInput.AskUserForShotLocation(board);
                board.FireShot(shotPosition);
            }

            ControlOutput.ShowFinalResult(board);
        }

        private static void AddShips()
        {
            board.PlaceShipRandomly(ShipType.Battleship);
            board.PlaceShipRandomly(ShipType.Destroyer);
            board.PlaceShipRandomly(ShipType.Destroyer);
        }
    }
}
