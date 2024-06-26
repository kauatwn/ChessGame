﻿namespace ChessLogic
{
    public class GameState
    {
        public Board Board { get; }
        public Player CurrentPlayer { get; private set; }
        public Result Result { get; private set; } = null;

        private int NoCaptureOrPawnMoves { get; set; } = 0;

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
            Board.SetPawnSkipPosition(CurrentPlayer, null);

            bool captureOrPawn = move.Execute(Board);

            if (captureOrPawn)
            {
                NoCaptureOrPawnMoves = 0;
            }
            else
            {
                NoCaptureOrPawnMoves++;
            }

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

        public bool IsGameOver()
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
            else if (Board.InsufficientMaterial())
            {
                Result = Result.Draw(EndReason.InsufficientMaterial);
            }
            else if (FiftyMoveRule())
            {
                Result = Result.Draw(EndReason.FiftyMoveRule);
            }
        }

        private bool FiftyMoveRule()
        {
            int fullMoves = NoCaptureOrPawnMoves / 2;

            return fullMoves == 50;
        }
    }
}
