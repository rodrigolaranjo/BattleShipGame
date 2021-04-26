using NUnit.Framework;
using System;
using System.Drawing;
using System.Linq;

namespace BattleShipGame.Test
{
    [TestFixture]
    public class BoardTests
    {
        private IBoard board;

        [SetUp]
        public void Setup()
        {
            board = BoardFactory.NewBoard();
        }

        [TestCase("4,2;5,1")]
        [TestCase("1,1;2,2;3,3")]
        [TestCase("2,1;2,2;2,3;4,2;5,1")]
        public void TestShipPlacement(string shipsCsv)
        {
            int hits = 0;
            Random r = new Random();

            foreach (string shipTypeAndNr in shipsCsv.Split(';'))
            {
                ShipType shipType = (ShipType)int.Parse(shipTypeAndNr.Split(',')[0]);
                int nrOfShips = int.Parse(shipTypeAndNr.Split(',')[1]);

                hits += nrOfShips * (int)shipType;

                for (int i = 0; i < nrOfShips; i++)
                    board.PlaceShipRandomly(shipType);
            }

            while (!board.GameOver)
                board.FireShot(new Point(r.Next(1, 11), r.Next(1, 11)));

            Assert.AreEqual(hits, board.BoardPositions.Where(p => p.Value == BoardPositionStatus.Hit).Count());
        }

        [TestCase("A1;B7;G10;J3;E6")]
        public void TestShotPositions(string shotsCsv)
        {
            foreach (string shot in shotsCsv.Split(';'))
            {
                int x = char.ToUpper(shot.Substring(0, 1).ToCharArray()[0]) - 64;
                int y = int.TryParse(shot.Substring(1), out int yy) ? yy : -1;

                board.FireShot(new Point(x, y));

                Assert.IsTrue(board.BoardPositions.Keys.Any(p => p.X.Equals(x) && p.Y.Equals(y)));
            }
        }
    }
}