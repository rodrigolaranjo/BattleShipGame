using System.Collections.Generic;
using System.Drawing;

namespace BattleShipGame.BL
{
    internal class Ship : IShip
    {
        private readonly ShipType shipType;

        private int lifeRemaining;

        public string ShipName => shipType.ToString();

        public bool IsSunk => lifeRemaining == 0;

        internal List<Point> BoardPositions { get; set; }

        internal Ship(ShipType shipType)
        {
            this.shipType = shipType;
            lifeRemaining = (int)shipType;
            BoardPositions = new List<Point>();
        }

        BoardPositionStatus IShip.FireAtShip(Point position)
        {
            if (BoardPositions.Contains(position))
            {
                lifeRemaining--;

                return BoardPositionStatus.Hit;
            }

            return BoardPositionStatus.Miss;
        }
    }
}
