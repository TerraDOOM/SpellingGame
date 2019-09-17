using Godot;
using System;
using Spelling;

public class GridViewport : Viewport
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.

    ColorRect[,] cloudTiles;
    ColorRect[,] floorTiles;

    public override void _Ready()
    {
        cloudTiles = new ColorRect[10, 10];
        floorTiles = new ColorRect[10, 10];
        for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				var rect = new ColorRect();
                rect.Color = new Color(0, 0, 0, 0);
                this.AddChild(rect);
                rect.MarginLeft = i;
                rect.MarginRight = i + 1;
                rect.MarginTop = j;
                rect.MarginBottom = j + 1;
                cloudTiles[i, j] = rect;
                var floorRect = new ColorRect();
                floorRect.Color = new Color(0, 0, 0, 1);
                this.AddChild(floorRect);
                floorRect.MarginLeft = i;
                floorRect.MarginRight = i + 1;
                floorRect.MarginTop = j + 0.7f;
                floorRect.MarginBottom = j + 1;
                floorTiles[i, j] = floorRect;
			}
		}
    }

    public void UpdateTileset(Tile[,] newTiles) {
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
                var tile = newTiles[i,j];
                if (tile.containsCloud) {
                    var color = tile.containedCloud.GetColour();
                    cloudTiles[i, j].Color = new Color(0, 0, 1, 0.3f);
                }

                if (tile.containsPatch) {
                    var color = tile.containedPath.GetColour();
                    floorTiles[i, j].Color = new Color(color[0], color[1], color[2], 1f);
                }
            }
        }
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
