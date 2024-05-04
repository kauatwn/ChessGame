namespace ChessLogic
{
    public class Direction
    {
        public static Direction North { get; } = new Direction(-1, 0);
        public static Direction South { get; } = new Direction(1, 0);
        public static Direction East { get; } = new Direction(0, 1);
        public static Direction West { get; } = new Direction(0, -1);
        public static Direction NorthEast { get; } = North + East;
        public static Direction NorthWest { get; } = North + West;
        public static Direction SouthEast { get; } = South + East;
        public static Direction SouthWest { get; } = South + West;

        public int RowDelta { get; }
        public int ColumnDelta { get; }

        public Direction(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;
            ColumnDelta = columnDelta;
        }

        public static Direction operator +(Direction dir1, Direction dir2)
        {
            return new Direction(dir1.RowDelta + dir2.RowDelta, dir2.ColumnDelta + dir2.ColumnDelta);
        }

        public static Direction operator *(int scalar, Direction dir)
        {
            return new Direction (scalar * dir.RowDelta, scalar * dir.ColumnDelta);
        }
    }
}
