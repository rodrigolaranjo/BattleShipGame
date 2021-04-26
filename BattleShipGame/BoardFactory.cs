namespace BattleShipGame
{
    public abstract class BoardFactory
    {
        public static IBoard NewBoard()
        {
            return new BL.Board();
        }
    }
}
