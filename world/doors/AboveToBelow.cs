using Godot;
using System;

public partial class AboveToBelow : Area2D
{
	public bool Entered = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("interact")) {
			GD.Print("interacted");
			// telebort
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player") {
			Entered = true;
			GD.Print("entered");
			Visible = true;
		}
	}


	private void OnBodyExited(Node2D body)
	{
		if (body.Name == "Player") {
			Entered = false;
			GD.Print("exited");
			Visible = false;
		}
	}

}
