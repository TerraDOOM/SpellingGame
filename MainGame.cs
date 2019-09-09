using Godot;
using System;
using Spelling;

public class MainGame : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

	public Duel duel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		duel = new Duel();
		duel.Initialize();
		UpdateStats();
    }
	
	public void UpdateStats() {
		int mana = duel.player.mana;
		int init = duel.player.initiative;
		var shields = duel.player.shields;
		((Label)GetNode("StatWindow")).UpdateStats(mana, init, shields);
	}

	public void EnteredSpell(String spell) {
		duel.player.currentSpell = new Spell(((string)spell).ToLower());
		duel.player.currentSpell.ParseWords();
		duel.player.currentSpell.ApplyEffects(duel, duel.player, duel.opponent);
		UpdateStats();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
