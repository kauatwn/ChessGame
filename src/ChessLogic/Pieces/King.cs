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

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }
        }

        private IEnumerable<Position> MovePositions(Position from, Board board)
        {
            foreach (Direction direction in Directions)
            {
                Position to = from + direction;

                if (!Board.IsInside(to))
                {
                    continue;
                }

                if (board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }
            }
        }
    }
}
