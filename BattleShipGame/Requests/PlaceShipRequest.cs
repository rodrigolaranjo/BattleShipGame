using System.Drawing;

namespace BattleShipGame.Requests
{
    internal class PlaceShipRequest
    {
        internal Point BoardPosition { get; set; }

        internal ShipDirection Direction { get; set; }

        internal ShipType ShipType { get; set; }
    }

    internal enum ShipDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
