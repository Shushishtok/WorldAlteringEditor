using TSMapEditor.GameMath;
using TSMapEditor.UI;
using TSMapEditor.Models;

namespace TSMapEditor.Mutations.Classes
{
    struct OriginalCellTerrainData
    {
        public Point2D CellCoords;
        public int TileIndex;
        public byte SubTileIndex;
        public byte HeightLevel;
        public PlacedTile CurrentTile;
        public PlacedTile PreviousTile;

        public OriginalCellTerrainData(Point2D cellCoords, int tileIndex, byte subTileIndex, byte heightLevel,
            PlacedTile currentTile = null, PlacedTile previousTile = null)
        {
            CellCoords = cellCoords;
            TileIndex = tileIndex;
            SubTileIndex = subTileIndex;
            HeightLevel = heightLevel;
            CurrentTile = currentTile;
            PreviousTile = previousTile;
        }
    }
}
