using BattleShipGame.Requests;
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
        public void TestGeneralShipPlacement(string shipsCsv)
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
        public void TestGeneralShotPositions(string shotsCsv)
        {
            foreach (string shot in shotsCsv.Split(';'))
            {
                int x = char.ToUpper(shot.Substring(0, 1).ToCharArray()[0]) - 64;
                int y = int.TryParse(shot.Substring(1), out int yy) ? yy : -1;

                board.FireShot(new Point(x, y));

                Assert.IsTrue(board.BoardPositions.Keys.Any(p => p.X.Equals(x) && p.Y.Equals(y)));
            }
        }

        [TestCase(1, 1, 5, 1, true)]
        [TestCase(1, 1, 5, 0, false)]
        [TestCase(1, 4, 4, 0, true)]
        [TestCase(5, 8, 4, 1, false)]
        [TestCase(10, 10, 1, 1, true)]
        [TestCase(10, 11, 1, 1, false)]
        public void TestShipPlacement(int x, int y, int shipType, int shipDirection, bool shouldSucceed)
        {
            PlaceShipRequest shipRequest = GetNewRequest(x, y, shipType, shipDirection);

            Assert.AreEqual(shouldSucceed, ((BL.Board)board).PlaceShip(shipRequest));
        }

        [TestCase(1, 1, 5, 1)]
        [TestCase(1, 4, 4, 0)]
        [TestCase(10, 10, 1, 1)]
        public void TestIsSunk(int x, int y, int shipType, int shipDirection)
        {
            PlaceShipRequest shipRequest = GetNewRequest(x, y, shipType, shipDirection);

            Assert.IsTrue(((BL.Board)board).PlaceShip(shipRequest));

            foreach (Point shipPosition in ((BL.Ship)board.Ships.First()).BoardPositions)
                board.FireShot(shipPosition);

            Assert.IsTrue(board.Ships.First().IsSunk);
        }

        [TestCase(6, 4, 1, 1)]
        [TestCase(5, 7, 4, 5)]
        [TestCase(10, 10, 5, 3)]
        public void TestShipPlacementAfterStart(int shotX, int shotY, int shipType1, int shipType2)
        {
            board.PlaceShipRandomly((ShipType)shipType1);

            board.FireShot(new Point(shotX, shotY));

            Assert.Throws<CustomExceptions.BoardException>(() => board.PlaceShipRandomly((ShipType)shipType2));
        }

        private static PlaceShipRequest GetNewRequest(int x, int y, int shipType, int shipDirection)
        {
            return new PlaceShipRequest()
            {
                BoardPosition = new Point(x, y),
                ShipType = (ShipType)shipType,
                Direction = (ShipDirection)shipDirection
            };
        }
    }
}