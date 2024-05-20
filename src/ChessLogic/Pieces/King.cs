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

            if (CanCastleKingSide(from, board))
            {
                yield return new Castle(MoveType.CastleKingSide, from);
            }

            if (CanCastleQueenSide(from, board))
            {
                yield return new Castle(MoveType.CastleQueenSide, from);
            }
        }

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return MovePositions(from, board).Any(to =>
            {
                Piece piece = board[to];

                return piece != null && piece.Type == PieceType.King;
            });
        }

        private static bool IsUnmovedRook(Position position, Board board)
        {
            if (board.IsEmpty(position))
            {
                return false;
            }

            Piece piece = board[position];

            return piece.Type == PieceType.Rook && !piece.HasMoved;
        }

        private static bool AllEmpty(IEnumerable<Position> positions, Board board)
        {
            return positions.All(board.IsEmpty);
        }

        private bool CanCastleKingSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            var rookPosition = new Position(from.Row, 7);

            var betweenPositions = new Position[]
            {
                new(from.Row, 5),
                new(from.Row, 6)
            };

            return IsUnmovedRook(rookPosition, board) && AllEmpty(betweenPositions, board);
        }

        private bool CanCastleQueenSide(Position from, Board board)
        {
            if (HasMoved)
            {
                return false;
            }

            var rookPosition = new Position(from.Row, 0);

            var betweenPositions = new Position[]
            {
                new(from.Row, 1),
                new(from.Row, 2),
                new(from.Row, 3)
            };

            return IsUnmovedRook(rookPosition, board) && AllEmpty(betweenPositions, board);
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
