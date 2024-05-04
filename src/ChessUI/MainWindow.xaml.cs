using ChessLogic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Image[,] PieceImages { get; } = new Image[8, 8];
        private GameState GameState { get; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            GameState = new GameState(Board.Initial(), Player.White);
            DrawBoard(GameState.Board);
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    var image = new Image();
                    PieceImages[row, column] = image;
                    PieceGrid.Children.Add(image);
                }
            }
        }

        private void DrawBoard(Board board)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    Piece piece = board[row, column];

                    PieceImages[row, column].Source = Images.GetImage(piece);
                }
            }
        }
    }
}
