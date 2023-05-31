using Godot;
using System;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetWindow().MinSize = new Vector2I(1152, 648);
		Items.LoadItems("res://data/Items.json");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
