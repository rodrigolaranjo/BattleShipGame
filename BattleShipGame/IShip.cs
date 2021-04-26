using System.Drawing;

namespace BattleShipGame
{
    public interface IShip
    {
        public bool IsSunk { get; }
        public string ShipName { get; }
        internal BoardPositionStatus FireAtShip(Point position);
    }
}
