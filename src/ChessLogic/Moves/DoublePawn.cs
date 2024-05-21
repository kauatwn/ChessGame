namespace ChessLogic
{
    public class DoublePawn : Move
    {
        public override MoveType Type => MoveType.DoublePawn;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        private Position SkippedPosition { get; }

        public DoublePawn(Position from, Position to)
        {
            FromPosition = from;
            ToPosition = to;
            SkippedPosition = new Position((from.Row + to.Row) / 2, from.Column);
        }

        public override void Execute(Board board)
        {
            Player player = board[FromPosition].Color;
            board.SetPawnSkipPosition(player, SkippedPosition);

            new NormalMove(FromPosition, ToPosition).Execute(board);
        }
    }
}
