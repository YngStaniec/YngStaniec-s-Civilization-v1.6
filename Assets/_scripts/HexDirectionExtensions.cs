public static class HexDirectionExtensions
{
    public static readonly HexCoordinates[] Offsets =
    {
        new HexCoordinates(1,-1), // NE
        new HexCoordinates(1,0),  // E
        new HexCoordinates(0,1),  // SE
        new HexCoordinates(-1,1), // SW
        new HexCoordinates(-1,0), // W
        new HexCoordinates(0,-1)  // NW
    };

    public static HexCoordinates ToOffset(this HexDirection direction)
    {
        return Offsets[(int)direction];
    }
}