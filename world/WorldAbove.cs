using Godot;
using System;

public partial class WorldAbove : Node2D
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
		//OS.Alert("interacted");
		Player player = GetNode<Player>("/root/Main/Player");
		WorldBelow worldbelow = GetNode<WorldBelow>("/root/Main/WorldBelow");
		player.Position = worldbelow.Position;
	}
}
