namespace ChessLogic
{
    public class Castle : Move
    {
        public override MoveType Type { get; }
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        private Direction KingMoveDirection { get; }
        private Position RookFromPosition { get; }
        private Position RookToPosition { get; }

        public Castle(MoveType type, Position kingPosition)
        {
            Type = type;
            FromPosition = kingPosition;

            if (type == MoveType.CastleKingSide)
            {
                KingMoveDirection = Direction.East;

                ToPosition = new Position(kingPosition.Row, 6);

                RookFromPosition = new Position(kingPosition.Row, 7);

                RookToPosition = new Position(kingPosition.Row, 5);
            }
            else if (type == MoveType.CastleQueenSide)
            {
                KingMoveDirection = Direction.West;

                ToPosition = new Position(kingPosition.Row, 2);

                RookFromPosition = new Position(kingPosition.Row, 0);

                RookToPosition = new Position(kingPosition.Row, 3);
            }
        }

        public override void Execute(Board board)
        {
            new NormalMove(FromPosition, ToPosition).Execute(board);
            new NormalMove(RookFromPosition, RookToPosition).Execute(board);
        }

        public override bool IsLegal(Board board)
        {
            Player player = board[FromPosition].Color;

            if (board.IsInCheck(player))
            {
                return false;
            }

            var copy = board.Copy();

            Position kingPositionInCopy = FromPosition;

            for (int i = 0; i < 2; i++)
            {
                new NormalMove(kingPositionInCopy, kingPositionInCopy + KingMoveDirection).Execute(copy);

                kingPositionInCopy += KingMoveDirection;

                if (copy.IsInCheck(player))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
