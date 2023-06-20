using Godot;
using System;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetWindow().MinSize = new Vector2I(1152, 648);
		//Items.LoadItems("res://data/Items.json");
		DamageModelSystem.DamageModel damageModel = GetNode<DamageModelSystem.DamageModel>("Player/DamageModel");
		damageModel.Connect(DamageModelSystem.DamageModel.SignalName.Damaged, Callable.From((Utils.SignalWrapper<DamageModelSystem.DamageResult> r) => {
			DamageModelSystem.DamageResult result = Utils.UnwrapClass(r);
			GD.Print($"Frame {Engine.GetPhysicsFrames()}: player damaged {result.DamageType} {result.Amount} {result.OldHealth} {result.NewHealth} {result.BlockedByIFrames}");
		}));
		damageModel.Connect(DamageModelSystem.DamageModel.SignalName.Killed, Callable.From((Utils.SignalWrapper<DamageModelSystem.DamageResult> r) => {
			DamageModelSystem.DamageResult result = Utils.UnwrapClass(r);
			GD.Print($"Frame {Engine.GetPhysicsFrames()}: player died {result.DamageType} {result.Amount} {result.OldHealth}");
		}));
		damageModel.Connect(DamageModelSystem.DamageModel.SignalName.Healed, Callable.From((Utils.SignalWrapper<DamageModelSystem.HealResult> r) => {
			var result = Utils.UnwrapClass(r);
			GD.Print($"Frame {Engine.GetPhysicsFrames()}: player healed {result.Amount} {result.OldHealth} {result.NewHealth}");
		}));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}
}
