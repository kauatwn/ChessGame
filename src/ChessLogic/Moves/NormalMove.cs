namespace ChessLogic
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPosition { get; }
        public override Position ToPosition { get; }

        public NormalMove(Position from, Position to)
        {
            FromPosition = from;
            ToPosition = to;
        }

        public override void Execute(Board board)
        {
            Piece piece = board[FromPosition];
            board[ToPosition] = piece;
            board[FromPosition] = null;
            piece.HasMoved = true;
        }
    }
}
