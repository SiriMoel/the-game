using Godot;
using System;

public partial class WorldBelow : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnWorldTeleporterInteracted()
	{
		Player player = GetNode<Player>("/root/Main/Player");
		Interactable worldabove = GetNode<Interactable>("/root/Main/WorldAbove/WorldTeleporter");
		player.Position = new Vector2(worldabove.Position.X + 20, worldabove.Position.Y );
	}

}
