using System.Collections.Generic;
using System.Drawing;

namespace BattleShipGame
{
    public interface IBoard
    {
        public List<IShip> Ships { get; }
        public Dictionary<Point, BoardPositionStatus> BoardPositions { get; }
        public bool GameOver { get; }
        public void PlaceShipRandomly(ShipType shipType);
        public void FireShot(Point shotPosition);
        public BoardPositionStatus GetPositionStatus(Point boardPosition);
    }
}
