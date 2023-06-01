using Godot;
using System;

public partial class Interactable : Area2D
{
	private bool inArea = false;
	private bool animationDown = false;
	public bool Enabled = true;
	[Signal] public delegate void InteractedEventHandler();
	public override void _Ready()
	{
		foreach (var item in GetChildren())
		{
			if (item.Name == "BaseShape2D") {
				item.QueueFree();
			}
			if (item.Name != "Sprite2D" && item.Name != "BaseShape2D" && item.Name != "Area2D") {
				GetNode<CollisionShape2D>("Area2D/BaseShape2D").Shape = ((CollisionShape2D)item).Shape;
				item.QueueFree();
			}
		}
		GetNode<Sprite2D>("Sprite2D").Visible = false;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Enabled) {
			return;
		}
		if (Input.IsActionPressed("interact") && inArea) {
			EmitSignal(SignalName.Interacted);
		}
		Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
		if (inArea) {
			if (animationDown) {
				sprite.Position = sprite.Position with {Y = sprite.Position.Y - 0.05f};
			} else {
				sprite.Position = sprite.Position with {Y = sprite.Position.Y + 0.05f};
			}
			if (sprite.Position.Y >= 2) {
				animationDown = true;
			} else if (sprite.Position.Y <= -2) {
				animationDown = false;
			}
		} else {
			sprite.Position = sprite.Position with {Y = 0};
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player") {
			//GD.Print("entered");
			inArea = true;
			GetNode<Sprite2D>("Sprite2D").Visible = true;
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.Name == "Player") {
			//GD.Print("exited");
			inArea = false;
			GetNode<Sprite2D>("Sprite2D").Visible = false;
		}
	}

}

