using Godot;
using System;

public partial class Interactable : Area2D
{
	private bool inArea = false;
	private bool animationDown = false;
	public override void _Ready()
	{
		foreach (var item in GetChildren())
		{
			if (item.Name != "Sprite2D" && item.Name != "BaseShape2D") {
				GetNode<CollisionShape2D>("BaseShape2D").Shape = ((CollisionShape2D)item).Shape;
				item.QueueFree();
			}
		}
		GetNode<Sprite2D>("Sprite2D").Visible = false;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
		if (inArea) {
			if (animationDown) {
				sprite.Position = sprite.Position with {Y = (float)Mathf.Lerp(sprite.Position.Y, -2, 0.02)};
			} else {
				sprite.Position = sprite.Position with {Y = (float)Mathf.Lerp(sprite.Position.Y, 2, 0.02)};
			}
			if (sprite.Position.Y < -1.9) {
				animationDown = false;
			}
			if (sprite.Position.Y > 1.9) {
				animationDown = true;
			}
		} else {
			sprite.Position = sprite.Position with {Y = 0};
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player") {
			GD.Print("entered");
			inArea = true;
			GetNode<Sprite2D>("Sprite2D").Visible = true;
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.Name == "Player") {
			GD.Print("exited");
			inArea = false;
			GetNode<Sprite2D>("Sprite2D").Visible = false;
		}
	}

}


