namespace ChessLogic
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }

        private static Direction[] Directions { get; } =
        [
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West,
            Direction.NorthWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast
        ];

        public King(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new King(Color)
            {
                HasMoved = HasMoved
            };

            return copy;
        }
    }
}
