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
    Texture playerTexture;
    TextureRect[,] playerSprites;
    int scaleFactor;

    public override void _Ready()
    {
        scaleFactor = (int)(this.Size.x / 10.0);
        
        playerSprites = new TextureRect[10,10];
        playerTexture = (Texture)GD.Load("res://felix-sprite-large.png");
        cloudTiles = new ColorRect[10, 10];
        floorTiles = new ColorRect[10, 10];
        for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				var rect = new ColorRect();
                rect.Color = new Color(0, 0, 0, 0);
                this.AddChild(rect);
                rect.MarginLeft = i * scaleFactor;
                rect.MarginRight = i * scaleFactor + scaleFactor;
                rect.MarginTop = j * scaleFactor;
                rect.MarginBottom = j * scaleFactor + scaleFactor;
                cloudTiles[i, j] = rect;
                var floorRect = new ColorRect();
                floorRect.Color = new Color(0, 0, 0, 1);
                this.AddChild(floorRect);
                floorRect.MarginLeft = i * scaleFactor;
                floorRect.MarginRight = i * scaleFactor + scaleFactor;
                floorRect.MarginTop = (j * scaleFactor) + (0.7f * scaleFactor);
                floorRect.MarginBottom = (j * scaleFactor) + scaleFactor;
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

                if (tile.containsPlayer) {
                    if (playerSprites[i, j] == null) {
                        var textureRect = new TextureRect();
                        textureRect.SetStretchMode(TextureRect.StretchModeEnum.Scale);
                        textureRect.SetExpand(true);
                        textureRect.Texture = playerTexture;
                        textureRect.MarginLeft = i * scaleFactor;
                        textureRect.MarginRight = i * scaleFactor + 0.9f * scaleFactor;
                        textureRect.MarginTop = j * scaleFactor;
                        textureRect.MarginBottom = j * scaleFactor + 0.7f * scaleFactor;
                        AddChild(textureRect);
                    }
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
