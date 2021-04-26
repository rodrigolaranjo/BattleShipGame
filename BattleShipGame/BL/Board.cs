using BattleShipGame.CustomExceptions;
using BattleShipGame.Requests;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BattleShipGame.BL
{
    internal class Board : IBoard
    {
        internal const int xLimit = 10;
        internal const int yLimit = 10;

        public Dictionary<Point, BoardPositionStatus> BoardPositions { get; set; }
        public List<IShip> Ships { get; set; }
        public bool GameOver => !Ships.Any(s => !s.IsSunk);

        internal Board()
        {
            BoardPositions = new Dictionary<Point, BoardPositionStatus>();
            Ships = new List<IShip>();
        }

        public BoardPositionStatus GetPositionStatus(Point boardPosition)
        {
            if (!IsValidPosition(boardPosition))
                return BoardPositionStatus.Invalid;

            if (BoardPositions.ContainsKey(boardPosition))
                return BoardPositions[boardPosition];

            return BoardPositionStatus.Empty;
        }

        public void PlaceShipRandomly(ShipType shipType)
        {
            if (BoardPositions.Any())
                throw new BoardException("You cannot place ships after starting the game.");

            bool result;
            Random r = new Random();
            do
            {
                PlaceShipRequest shipToPlace = new PlaceShipRequest
                {
                    Direction = (r.Next(1, 4)) switch
                        {
                            1 => ShipDirection.Left,
                            2 => ShipDirection.Right,
                            3 => ShipDirection.Up,
                            _ => ShipDirection.Down,
                        },
                    BoardPosition = new Point(r.Next(1, 11), r.Next(1, 11)),
                    ShipType = shipType
                };

                result = PlaceShip(shipToPlace);

            } while (result != true);
        }

        public void FireShot(Point shotPosition)
        {
            if (!IsValidPosition(shotPosition) || BoardPositions.ContainsKey(shotPosition))
                return;

            BoardPositionStatus shotStatus = BoardPositionStatus.Miss;

            foreach (IShip ship in Ships.Where(s => !s.IsSunk))
            {
                shotStatus = ship.FireAtShip(shotPosition);

                if (shotStatus != BoardPositionStatus.Miss)
                    break;
            }

            BoardPositions.Add(shotPosition, shotStatus);
        }

        private bool PlaceShip(PlaceShipRequest request)
        {
            Ship newShip = new Ship(request.ShipType);
            return request.Direction switch
            {
                ShipDirection.Down => PlaceShipDown(request.BoardPosition, newShip),
                ShipDirection.Up => PlaceShipUp(request.BoardPosition, newShip),
                ShipDirection.Left => PlaceShipLeft(request.BoardPosition, newShip),
                _ => PlaceShipRight(request.BoardPosition, newShip),
            };
        }

        private bool IsValidPosition(Point boardPosition)
        {
            return boardPosition.X >= 1 && boardPosition.X <= xLimit &&
            boardPosition.Y >= 1 && boardPosition.Y <= yLimit;
        }

        private bool PlaceShipRight(Point coordinate, Ship newShip)
        {
            int positionIndex = 0;
            int maxY = coordinate.Y + newShip.BoardPositions.Length;

            for (int i = coordinate.Y; i < maxY; i++)
            {
                var currentCoordinate = new Point(coordinate.X, i);

                if (!IsValidPosition(currentCoordinate) || OverlapsAnotherShip(currentCoordinate))
                    return false;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return true;
        }

        private bool PlaceShipLeft(Point coordinate, Ship newShip)
        {
            int positionIndex = 0;
            int minY = coordinate.Y - newShip.BoardPositions.Length;

            for (int i = coordinate.Y; i > minY; i--)
            {
                var currentCoordinate = new Point(coordinate.X, i);

                if (!IsValidPosition(currentCoordinate) || OverlapsAnotherShip(currentCoordinate))
                    return false;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return true;
        }

        private bool PlaceShipUp(Point coordinate, Ship newShip)
        {
            int positionIndex = 0;
            int minX = coordinate.X - newShip.BoardPositions.Length;

            for (int i = coordinate.X; i > minX; i--)
            {
                var currentCoordinate = new Point(i, coordinate.Y);

                if (!IsValidPosition(currentCoordinate) || OverlapsAnotherShip(currentCoordinate))
                    return false;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return true;
        }

        private bool PlaceShipDown(Point coordinate, Ship newShip)
        {
            int positionIndex = 0;
            int maxX = coordinate.X + newShip.BoardPositions.Length;

            for (int i = coordinate.X; i < maxX; i++)
            {
                var currentCoordinate = new Point(i, coordinate.Y);

                if (!IsValidPosition(currentCoordinate) || OverlapsAnotherShip(currentCoordinate))
                    return false;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return true;
        }

        private void AddShipToBoard(Ship newShip)
        {
            Ships.Add(newShip);
        }

        private bool OverlapsAnotherShip(Point coordinate)
        {
            foreach (Ship ship in Ships)
            {
                if (ship != null)
                {
                    if (ship.BoardPositions.Contains(coordinate))
                        return true;
                }
            }

            return false;
        }
    }
}
