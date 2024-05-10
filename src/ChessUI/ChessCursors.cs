using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ChessUI
{
    public static class ChessCursors
    {
        public static Cursor WhiteCursor { get; } = LoadCursor("Assets/CursorW.cur");
        public static Cursor BlackCursor { get; } = LoadCursor("Assets/CursorB.cur");

        private static Cursor LoadCursor(string filePath)
        {
            Stream stream = Application.GetResourceStream(new Uri(filePath, UriKind.Relative)).Stream;

            return new Cursor(stream, true);
        }
    }
}
