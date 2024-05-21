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

        public override bool CanCaptureOpponentKing(Position from, Board board)
        {
            return DiagonalMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPosition];

                return piece != null && piece.Type == PieceType.King;
            });
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

        private static IEnumerable<Move> PromotionMoves(Position from, Position to)
        {
            yield return new PawnPromotion(from, to, PieceType.Knight);
            yield return new PawnPromotion(from, to, PieceType.Bishop);
            yield return new PawnPromotion(from, to, PieceType.Rook);
            yield return new PawnPromotion(from, to, PieceType.Queen);
        }

        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position singleForwardPosition = from + Forward;

            if (CanMoveTo(singleForwardPosition, board))
            {
                if (singleForwardPosition.Row == 0 || singleForwardPosition.Row == 7)
                {
                    foreach (Move promotionMove in PromotionMoves(from, singleForwardPosition))
                    {
                        yield return promotionMove;
                    }
                }
                else
                {
                    yield return new NormalMove(from, singleForwardPosition);
                }

                yield return new NormalMove(from, singleForwardPosition);

                Position doubleForwardPosition = singleForwardPosition + Forward;

                if (!HasMoved && CanMoveTo(doubleForwardPosition, board))
                {
                    yield return new DoublePawn(from, doubleForwardPosition);
                }
            }
        }

        private IEnumerable<Move> DiagonalMoves(Position from, Board board)
        {
            foreach (Direction direction in new Direction[] { Direction.West, Direction.East })
            {
                Position to = from + Forward + direction;

                if (to == board.GetPawnSkipPosition(Color.Opponent()))
                {
                    yield return new EnPassant(from, to);
                }
                else if (CanCaptureAt(to, board))
                {
                    if (to.Row == 0 || to.Row == 7)
                    {
                        foreach (Move promotionMove in PromotionMoves(from, to))
                        {
                            yield return promotionMove;
                        }
                    }
                    else
                    {
                        yield return new NormalMove(from, to);
                    }
                }
            }
        }
    }
}
