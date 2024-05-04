namespace ChessLogic
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.Queen;
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

        public Queen(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new Queen(Color)
            {
                HasMoved = HasMoved
            };

            return copy;
        }
    }
}
