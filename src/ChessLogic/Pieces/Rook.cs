namespace ChessLogic
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;
        public override Player Color { get; }

        private static Direction[] Directions { get; } = 
        [
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West
        ];

        public Rook(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new Rook(Color)
            {
                HasMoved = HasMoved
            };

            return copy;
        }
    }
}
