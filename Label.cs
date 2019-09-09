using Godot;
using System;
using System.Text;
using System.Collections.Generic;
using Spelling;

public class Label : Godot.Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
	
	public void UpdateStats(int mana, int initiative, Dictionary<DamageType, int> shields) {
		var builder = new StringBuilder();
		builder.Append(String.Format("Mana: {0}\n", mana));
		builder.Append(String.Format("Initiative: {0}\n", initiative));
		builder.Append("Shields:\n");
		foreach (var shieldType in shields.Keys) {
			if (shields[shieldType] != 0) {
				builder.Append(String.Format("    {0}: {1}\n", shieldType, shields[shieldType]));
			}
		}
		this.Text = builder.ToString();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
