﻿namespace ChessLogic
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;
        public override Player Color { get; }

        private static Direction[] Directions { get; } =
        [
            Direction.NorthWest,
            Direction.NorthEast,
            Direction.SouthWest,
            Direction.SouthEast
        ];

        public Bishop(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new Bishop(Color)
            {
                HasMoved = HasMoved
            };

            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return FindPositionsInMultipleDirections(from, board, Directions).Select(to => new NormalMove(from, to));
        }
    }
}
