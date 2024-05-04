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

        public static Direction operator +(Direction firstDirection, Direction secondDirection)
        {
            return new Direction(firstDirection.RowDelta + secondDirection.RowDelta, secondDirection.ColumnDelta + secondDirection.ColumnDelta);
        }

        public static Direction operator *(int scalar, Direction direction)
        {
            return new Direction (scalar * direction.RowDelta, scalar * direction.ColumnDelta);
        }
    }
}
