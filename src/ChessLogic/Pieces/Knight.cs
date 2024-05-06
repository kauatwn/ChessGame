namespace ChessLogic
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }

        public Knight(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new Knight(Color)
            {
                HasMoved = HasMoved
            };

            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return JumpMoves(from, board).Select(to => new NormalMove(from, to));
        }

        private static IEnumerable<Position> GetPossibleJumpMoves(Position from)
        {
            foreach (Direction horizontalDirection in new Direction[] { Direction.West, Direction.East })
            {
                foreach (Direction verticalDirection in new Direction[] { Direction.North, Direction.South })
                {
                    yield return from + 2 * horizontalDirection + verticalDirection;

                    yield return from + 2 * verticalDirection + horizontalDirection;
                }
            }
        }

        private IEnumerable<Position> JumpMoves(Position from, Board board)
        {
            return GetPossibleJumpMoves(from).Where(position => Board.IsInside(position) && (board.IsEmpty(position) || board[position].Color != Color));
        }
    }
}
