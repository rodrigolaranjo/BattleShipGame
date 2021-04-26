using System;

namespace BattleShipGame.CustomExceptions
{
    internal class BoardException : Exception
    {
        internal BoardException() : base() { }
        internal BoardException(string message) : base(message) { }
        internal BoardException(string message, Exception innerException) : base(message, innerException) { }
    }
}
