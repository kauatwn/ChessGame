namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }

        public GameState(Board board, Player player)
        {
            Board = board;
            CurrentPlayer = player;
        }

        public IEnumerable<Move> LegalMoveForPiece(Position position)
        {
            if (Board.IsEmpty(position) || Board[position].Color != CurrentPlayer)
            {
                return [];
            }

            Piece piece = Board[position];

            IEnumerable<Move> moveCandidates = piece.GetMoves(position, Board);

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public void MakeMove(Move move)
        {
            move.Execute(Board);

            CurrentPlayer = CurrentPlayer.Opponent();
        }
    }
}
