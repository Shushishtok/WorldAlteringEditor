using TSMapEditor.CCEngine.TileData;
using TSMapEditor.GameMath;

namespace TSMapEditor.Models
{
    /// <summary>
    /// Immutable context for a terrain-tile placement. This is kept separate from
    /// MapTile because map cells are subsequently mutated by later placements.
    /// </summary>
    public sealed class PlacedTile
    {
        public PlacedTile(TileImage tileImage, Point2D coords)
        {
            TileImage = tileImage;
            Coords = coords;
        }

        public TileImage TileImage { get; }
        public Point2D Coords { get; }
    }
}
