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
        private Rectangle[,] Highlights { get; } = new Rectangle[8, 8];
        private Dictionary<Position, Move> MoveCache { get; } = [];

        private GameState GameState { get; set; }
        private Position SelectedPosition { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            GameState = new GameState(Board.Initial(), Player.White);
            DrawBoard(GameState.Board);

            SetCursor(GameState.CurrentPlayer);
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

                    var highlight = new Rectangle();
                    Highlights[row, column] = highlight;
                    HighlightGrid.Children.Add(highlight);
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

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMenuOnScreen())
            {
                return;
            }

            Point point = e.GetPosition(BoardGrid);
            Position position = ToSquarePosition(point);

            if (SelectedPosition == null)
            {
                OnFromPositionSelected(position);
            }
            else
            {
                OnToPositionSelected(position);
            }
        }

        private Position ToSquarePosition(Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;

            int row = (int)(point.Y / squareSize);
            int column = (int)(point.X / squareSize);

            return new Position(row, column);
        }

        private void OnFromPositionSelected(Position position)
        {
            IEnumerable<Move> moves = GameState.LegalMoveForPiece(position);

            if (moves.Any())
            {
                SelectedPosition = position;

                CacheMoves(moves);

                ShowHighlights();
            }
        }

        private void OnToPositionSelected(Position position)
        {
            SelectedPosition = null;

            HideHighlights();

            if (MoveCache.TryGetValue(position, out Move move))
            {
                if (move.Type == MoveType.PawnPromotion)
                {
                    HandlePromotion(move.FromPosition, move.ToPosition);
                }
                else
                {
                    HandleMove(move);
                }
            }
        }

        private void HandlePromotion(Position from, Position to)
        {
            PieceImages[to.Row, to.Column].Source = Images.GetImage(GameState.CurrentPlayer, PieceType.Pawn);
            PieceImages[to.Row, to.Column].Source = null;

            var promotionMenu = new PromotionMenu(GameState.CurrentPlayer);
            MenuContainer.Content = promotionMenu;

            promotionMenu.PieceSelected += type =>
            {
                MenuContainer.Content = null;

                Move promotionMove = new PawnPromotion(from, to, type);

                HandleMove(promotionMove);
            };
        }

        private void HandleMove(Move move)
        {
            GameState.MakeMove(move);

            DrawBoard(GameState.Board);

            SetCursor(GameState.CurrentPlayer);

            if (GameState.IsGameOver())
            {
                ShowGameOver();
            }
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            MoveCache.Clear();

            foreach (Move move in moves)
            {
                MoveCache[move.ToPosition] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position to in MoveCache.Keys)
            {
                Highlights[to.Row, to.Column].Fill = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position to in MoveCache.Keys)
            {
                Highlights[to.Row, to.Column].Fill = Brushes.Transparent;
            }
        }

        private void SetCursor(Player player)
        {
            if (player == Player.White)
            {
                Cursor = ChessCursors.WhiteCursor;
            }
            else
            {
                Cursor = ChessCursors.BlackCursor;
            }
        }

        private bool IsMenuOnScreen()
        {
            return MenuContainer.Content != null;
        }

        private void ShowGameOver()
        {
            var gameOverMenu = new GameOverMenu(GameState);
            MenuContainer.Content = gameOverMenu;

            gameOverMenu.OptionSelected += option =>
            {
                if (option == Option.Restart)
                {
                    MenuContainer.Content = null;

                    RestartGame();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };
        }

        private void RestartGame()
        {
            HideHighlights();
            MoveCache.Clear();

            GameState = new GameState(Board.Initial(), Player.White);
            DrawBoard(GameState.Board);

            SetCursor(GameState.CurrentPlayer);
        }
    }
}
