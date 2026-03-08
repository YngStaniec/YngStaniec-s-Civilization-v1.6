public static class HexDirectionExtensions
{
    public static HexCoordinates ToOffset(this HexDirection direction, int r)
    {
        bool odd = (r & 1) == 1;

        switch (direction)
        {
            case HexDirection.NE:
                return odd ? new HexCoordinates(1, -1) : new HexCoordinates(0, -1);

            case HexDirection.E:
                return new HexCoordinates(1, 0);

            case HexDirection.SE:
                return odd ? new HexCoordinates(1, 1) : new HexCoordinates(0, 1);

            case HexDirection.SW:
                return odd ? new HexCoordinates(0, 1) : new HexCoordinates(-1, 1);

            case HexDirection.W:
                return new HexCoordinates(-1, 0);

            case HexDirection.NW:
                return odd ? new HexCoordinates(0, -1) : new HexCoordinates(-1, -1);
        }

        return new HexCoordinates(0, 0);
    }
}