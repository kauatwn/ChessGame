namespace ChessLogic
{
    public class Counting
    {
        private Dictionary<PieceType, int> WhiteCount { get; } = [];
        private Dictionary<PieceType, int> BlackCount { get; } = [];

        public int TotalCount { get; private set; }

        public Counting()
        {
            foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            {
                WhiteCount[type] = 0;
                BlackCount[type] = 0;
            }
        }

        public void Increment(Player color, PieceType type)
        {
            if (color == Player.White)
            {
                WhiteCount[type]++;
            }
            else if (color == Player.Black)
            {
                BlackCount[type]++;
            }

            TotalCount++;
        }

        public int White(PieceType type)
        {
            return WhiteCount[type];
        }

        public int Black(PieceType type)
        {
            return BlackCount[type];
        }
    }
}
