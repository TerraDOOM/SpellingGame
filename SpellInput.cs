using Godot;
using System;

public class SpellInput : Godot.LineEdit
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

	public void _on_SpellInput_text_entered(String spell) {
		if (this.Text != "") {
			this.Text = "";
			try {
				((MainGame)GetParent()).EnteredSpell(spell);
				Console.WriteLine(spell);
			} catch (IndexOutOfRangeException) {}
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}