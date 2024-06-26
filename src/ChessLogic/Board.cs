﻿namespace ChessLogic
{
    public class Board
    {
        private Piece[,] Pieces { get; } = new Piece[8, 8];

        private Dictionary<Player, Position> PawnSkipPositions { get; } = new()
        {
            { Player.White, null },
            { Player.Black, null }
        };

        public Piece this[int row, int column]
        {
            get { return Pieces[row, column]; }
            set { Pieces[row, column] = value; }
        }

        public Piece this[Position position]
        {
            get { return this[position.Row, position.Column]; }
            set { this[position.Row, position.Column] = value; }
        }

        public Position GetPawnSkipPosition(Player player)
        {
            return PawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player, Position position)
        {
            PawnSkipPositions[player] = position;
        }

        public static Board Initial()
        {
            var board = new Board();

            board.AddStartPieces();

            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
            this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            for (int column = 0; column < 8; column++)
            {
                this[1, column] = new Pawn(Player.Black);
                this[6, column] = new Pawn(Player.White);
            }
        }

        public static bool IsInside(Position position)
        {
            return position.Row >= 0 && position.Row < 8 && position.Column >= 0 && position.Column < 8;
        }

        public bool IsEmpty(Position position)
        {
            return this[position] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    var position = new Position(row, column);

                    if (!IsEmpty(position))
                    {
                        yield return position;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(position => this[position].Color == player);
        }

        public bool IsInCheck(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(position =>
            {
                Piece piece = this[position];

                return piece.CanCaptureOpponentKing(position, this);
            });
        }

        public Board Copy()
        {
            var copy = new Board();

            foreach (Position position in PiecePositions())
            {
                copy[position] = this[position].Copy();
            }

            return copy;
        }

        public Counting CountPieces()
        {
            var couting = new Counting();

            foreach (Position position in PiecePositions())
            {
                var piece = this[position];

                couting.Increment(piece.Color, piece.Type);
            }

            return couting;
        }

        public bool InsufficientMaterial()
        {
            var counting = new Counting();

            return IsKingVersusKing(counting) || IsKingAndBishopVersusKing(counting) || IsKingAndKnightVersusKing(counting) || IsKingAndBishopVersusKingAndBishop(counting);
        }

        private static bool IsKingVersusKing(Counting counting)
        {
            return counting.TotalCount == 2;
        }

        private static bool IsKingAndBishopVersusKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        private static bool IsKingAndKnightVersusKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        private bool IsKingAndBishopVersusKingAndBishop(Counting counting)
        {
            if (counting.TotalCount != 4)
            {
                return false;
            }

            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1)
            {
                return false;
            }

            Position whiteBishopPosition = FindPiece(Player.White, PieceType.Bishop);
            Position blackBishopPosition = FindPiece(Player.Black, PieceType.Bishop);

            return whiteBishopPosition.SquareColor() == blackBishopPosition.SquareColor();
        }

        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color).First(position => this[position].Type == type);
        }
    }
}
