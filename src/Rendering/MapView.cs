﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMapEditor.CCEngine;
using TSMapEditor.GameMath;
using TSMapEditor.Models;
using TSMapEditor.Models.MapFormat;

namespace TSMapEditor.Rendering
{
    public class MapView : XNAControl
    {
        public MapView(WindowManager windowManager, Map map, TheaterGraphics theaterGraphics) : base(windowManager)
        {
            Map = map;
            TheaterGraphics = theaterGraphics;
        }

        public Map Map { get; }
        public TheaterGraphics TheaterGraphics { get; }

        private RenderTarget2D renderTarget;

        private bool mapInvalidated = true;
        private Point2D cameraTopLeftPoint = new Point2D(0, 0);

        private int scrollRate = 20;

        public override void Initialize()
        {
            base.Initialize();

            renderTarget = new RenderTarget2D(GraphicsDevice,
                Map.Size.X * Constants.CellSizeX,
                Map.Size.Y * Constants.CellSizeY, false, SurfaceFormat.Color,
                DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        public void DrawWholeMap()
        {
            Renderer.PushRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            for (int i = 0; i < Map.Tiles.Length; i++)
            {
                var row = Map.Tiles[i];

                for (int j = 0; j < row.Length; j++)
                {
                    Point2D drawPoint = CellMath.CellTopLeftPoint(new Point2D(i, j), Map.Size.X);
                    if (row[j] == null)
                    {
                        DrawString("n", 0, drawPoint.ToXNAVector(), Color.Red, 0.5f);
                        continue;
                    }

                    if (row[j] == null)
                        continue;

                    DrawTerrainTile(row[j]);
                }
            }

            for (int i = 0; i < Map.TerrainObjects.Count; i++)
            {
                var obj = Map.TerrainObjects[i];
                Point2D drawPoint = CellMath.CellTopLeftPoint(obj.Position, Map.Size.X);
                int index = obj.TerrainType.Index;
                var graphics = TheaterGraphics.TerrainObjectTextures[index];
                if (graphics == null || graphics.Frames.Length == 0)
                {
                    DrawString(obj.TerrainType.ININame, 1, drawPoint.ToXNAVector(), Color.Red, 0.5f);
                    continue;
                }

                var frame = graphics.Frames[0];
                var texture = frame.Texture;
                DrawTexture(texture, new Rectangle(drawPoint.X - frame.ShapeWidth / 2 + frame.OffsetX + Constants.CellSizeX / 2,
                    drawPoint.Y - frame.ShapeHeight / 2 + frame.OffsetY + Constants.CellSizeY / 2,
                    texture.Width, texture.Height), Color.White);
            }

            Renderer.PopRenderTarget();
        }

        public void DrawTerrainTile(IsoMapPack5Tile tile)
        {
            Point2D drawPoint = CellMath.CellTopLeftPoint(new Point2D(tile.X, tile.Y), Map.Size.X);
            

            if (tile.TileIndex >= TheaterGraphics.TileCount)
                return;

            TileImage tileImage = TheaterGraphics.GetTileGraphics(tile.TileIndex);
            if (tile.SubTileIndex >= tileImage.TMPImages.Length)
            {
                DrawString(tile.SubTileIndex.ToString(), 0, new Vector2(drawPoint.X, drawPoint.Y), Color.Red);
                return;
            }
                

            int a = 0;
            
            Texture2D texture = tileImage.TMPImages[tile.SubTileIndex].Texture;
            if (texture != null)
            {
                DrawTexture(texture, new Rectangle(drawPoint.X, drawPoint.Y,
                    Constants.CellSizeX, Constants.CellSizeY), Color.White);
            }

            if (a == 0)
                return;

            using (var stream = File.OpenWrite(Environment.CurrentDirectory + "/texture.png"))
            {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.IsKeyHeldDown(Microsoft.Xna.Framework.Input.Keys.Left))
                cameraTopLeftPoint += new Point2D(-scrollRate, 0);
            else if (Keyboard.IsKeyHeldDown(Microsoft.Xna.Framework.Input.Keys.Right))
                cameraTopLeftPoint += new Point2D(scrollRate, 0);

            if (Keyboard.IsKeyHeldDown(Microsoft.Xna.Framework.Input.Keys.Up))
                cameraTopLeftPoint += new Point2D(0, -scrollRate);
            else if (Keyboard.IsKeyHeldDown(Microsoft.Xna.Framework.Input.Keys.Down))
                cameraTopLeftPoint += new Point2D(0, scrollRate);

            base.Update(gameTime);
        }

        private void DrawCursorTile()
        {
            Point cursorPoint = GetCursorPoint();
            Point2D cursorMapPoint = new Point2D(cameraTopLeftPoint.X + cursorPoint.X - Constants.CellSizeX / 2,
                cameraTopLeftPoint.Y + cursorPoint.Y - Constants.CellSizeY / 2);

            DrawStringWithShadow("Cursor coord: " + cursorMapPoint.X + ", " + cursorMapPoint.Y, 0, new Vector2(0f, 0f), Color.White);

            Point2D tileCoords = CellMath.CellCoordsFromPixelCoords(cursorMapPoint, Map.Size);

            DrawStringWithShadow(tileCoords.X + ", " + tileCoords.Y, 0, new Vector2(0f, 20f), Color.White);

            if (tileCoords.X >= 1 && tileCoords.Y >= 1 && tileCoords.Y < Map.Tiles.Length && tileCoords.X < Map.Tiles[tileCoords.Y].Length)
            {
                var tile = Map.Tiles[tileCoords.Y][tileCoords.X];

                if (tile == null)
                {
                    DrawString("Null tile", 0, new Vector2(0f, 40f), Color.White);
                }
                else
                {
                    Point2D drawPoint = CellMath.CellTopLeftPoint(new Point2D(tile.X, tile.Y), Map.Size.X);

                    FillRectangle(new Rectangle(drawPoint.X - cameraTopLeftPoint.X,
                        drawPoint.Y - cameraTopLeftPoint.Y,
                        Constants.CellSizeX, Constants.CellSizeY),
                        new Color(128, 128, 128, 128));

                    TileImage tileGraphics = TheaterGraphics.GetTileGraphics(tile.TileIndex);
                    TileSet tileSet = TheaterGraphics.Theater.TileSets[tileGraphics.TileSetId];
                    DrawStringWithShadow("TileSet: " + tileSet.SetName + " (" + tileGraphics.TileSetId + ")", 0,
                        new Vector2(0f, 40f), Color.White);
                    DrawStringWithShadow("Tile ID: " + tileGraphics.TileIndex, 0, new Vector2(0f, 60f), Color.White);
                    DrawStringWithShadow("Sub-tile ID: " + tile.SubTileIndex, 0, new Vector2(0f, 80f), Color.White);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (mapInvalidated)
            {
                DrawWholeMap();
                mapInvalidated = false;
            }

            DrawTexture(renderTarget, new Rectangle(cameraTopLeftPoint.X, cameraTopLeftPoint.Y,
                Width, Height), new Rectangle(0, 0, Width, Height), Color.White);

            DrawCursorTile();

            base.Draw(gameTime);
        }
    }
}
