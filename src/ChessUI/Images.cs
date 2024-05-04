using ChessLogic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessUI
{
    public static class Images
    {
        private static Dictionary<PieceType, ImageSource> WhiteSources { get; } = new()
        {
            { PieceType.Pawn, LoadImage("Assets/pawnW.png") },
            { PieceType.Bishop, LoadImage("Assets/bishopW.png") },
            { PieceType.Knight, LoadImage("Assets/knightW.png") },
            { PieceType.Rook, LoadImage("Assets/rookW.png") },
            { PieceType.Queen, LoadImage("Assets/queenW.png") },
            { PieceType.King, LoadImage("Assets/kingW.png") }
        };

        private static Dictionary<PieceType, ImageSource> BlackSources { get; } = new()
        {
            { PieceType.Pawn, LoadImage("Assets/pawnB.png") },
            { PieceType.Bishop, LoadImage("Assets/bishopB.png") },
            { PieceType.Knight, LoadImage("Assets/knightB.png") },
            { PieceType.Rook, LoadImage("Assets/rookB.png") },
            { PieceType.Queen, LoadImage("Assets/queenB.png") },
            { PieceType.King, LoadImage("Assets/kingB.png") }
        };

        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType type)
        {
            return color switch
            {
                Player.White => WhiteSources[type],
                Player.Black => BlackSources[type],
                _ => null
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }

            return GetImage(piece.Color, piece.Type);
        }
    }
}
