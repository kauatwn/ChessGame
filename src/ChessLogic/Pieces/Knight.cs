﻿namespace ChessLogic
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }

        public Knight(Player color)
        {
            Color = color;
        }

        public override Piece Copy()
        {
            var copy = new Knight(Color)
            {
                HasMoved = HasMoved
            };

            return copy;
        }
    }
}