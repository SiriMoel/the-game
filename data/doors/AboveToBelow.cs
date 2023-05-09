using Godot;
using System;

public partial class AboveToBelow : Area2D
{
	public bool Entered = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("interact")) {
			Console.WriteLine("interacted");
			// telebort
		}
	}

	private void _on_body_entered(PhysicsBody2D body)
	{
		if (body.Name == "Player") {
			Entered = true;
			Console.WriteLine("entered");
		}
	}


	private void _on_body_exited(PhysicsBody2D body)
	{
		if (body.Name == "Player") {
			Entered = false;
			Console.WriteLine("exited");
		}
	}

}
