
namespace ChessLogic
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        protected IEnumerable<Position> FindPositionsInUniqueDirection(Position from, Board board, Direction direction)
        {
            for (Position position = from + direction; Board.IsInside(position); position += direction)
            {
                if (board.IsEmpty(position))
                {
                    yield return position;
                    continue;
                }

                Piece piece = board[position];

                if (piece.Color != Color)
                {
                    yield return position;
                }

                yield break;
            }
        }

        protected IEnumerable<Position> FindPositionsInMultipleDirections(Position from, Board board, Direction[] directions)
        {
            return directions.SelectMany(direction => FindPositionsInUniqueDirection(from, board, direction));
        }

        public virtual bool CanCaptureOpponentKing(Position from, Board board)
        {
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPosition];

                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
