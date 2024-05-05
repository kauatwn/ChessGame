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

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }

        private static bool CanMoveTo(Position position, Board board)
        {
            return Board.IsInside(position) && board.IsEmpty(position);
        }

        private bool CanCaptureAt(Position position, Board board)
        {
            if (!Board.IsInside(position) || board.IsEmpty(position))
            {
                return false;
            }

            return board[position].Color != Color;
        }

        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position singleForwardPosition = from + Forward;

            if (CanMoveTo(singleForwardPosition, board))
            {
                yield return new NormalMove(from, singleForwardPosition);

                Position doubleForwardPosition = singleForwardPosition + Forward;

                if (!HasMoved && CanMoveTo(doubleForwardPosition, board))
                {
                    yield return new NormalMove(from, doubleForwardPosition);
                }
            }
        }

        private IEnumerable<Move> DiagonalMoves(Position from, Board board)
        {
            foreach (Direction direction in new Direction[] { Direction.West, Direction.East })
            {
                Position to = from + Forward + direction;

                if (CanCaptureAt(to, board))
                {
                    yield return new NormalMove(from, to);
                }
            }
        }
    }
}
