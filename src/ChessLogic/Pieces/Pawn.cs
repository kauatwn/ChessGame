namespace ChessLogic
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }

        private Direction Forward { get; }

        public Pawn(Player color)
        {
            Color = color;

            if (color == Player.White)
            {
                Forward = Direction.North;
            }
            else if (color == Player.Black)
            {
                Forward = Direction.South;
            }
        }

        public override Piece Copy()
        {
            var copy = new Pawn(Color)
            {
                HasMoved = HasMoved
            };

            return copy;
        }
    }
}
