using Godot;
using System;
using Spelling;

public class MainGame : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

	public Duel duel;
	public Game game;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		duel = new Duel();
		game = new Game();
		duel.InitializeSP(2, game.gameboard);
        game.gameboard[1,1].containsCloud = true;
        game.gameboard[1,1].containedCloud = new Spell("So fukrak'te ya");
        game.gameboard[1,1].containsPatch = true;
        game.gameboard[1,1].containedPath = new Spell("So fukrak'te ya");
        ((GridViewport)GetNode("ViewportContainer/GridViewport")).UpdateTileset(game.gameboard);
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
		// TODO: FIX
		// FIXME: fix
		duel.player.currentSpell.ApplyEffects(duel, duel.player, duel.players[1]);
		UpdateStats();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
