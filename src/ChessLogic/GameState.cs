namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }
        public Result Result { get; private set; } = null;

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

            CheckForGameOver();
        }

        public IEnumerable<Move> AllLegalMovesFor(Player player)
        {
            IEnumerable<Move> moveCandidates = Board.PiecePositionsFor(player).SelectMany(position =>
            {
                Piece piece = Board[position];

                return piece.GetMoves(position, Board);
            });

            return moveCandidates.Where(move => move.IsLegal(Board));
        }

        public bool IsGameOVer()
        {
            return Result != null;
        }

        private void CheckForGameOver()
        {
            if (!AllLegalMovesFor(CurrentPlayer).Any())
            {
                if (Board.IsInCheck(CurrentPlayer))
                {
                    Result = Result.Win(CurrentPlayer.Opponent());
                }
                else
                {
                    Result = Result.Draw(EndReason.Stalemate);
                }
            }
        }
    }
}
