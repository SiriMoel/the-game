using Godot;
namespace DamageModelSystem.StatusEffects.Effects
{
	public abstract partial class StatusEffect : Node
	{
		public abstract int RunEveryNSeconds {get; }
		public double StatusLength;

		public sealed override void _Ready()
		{
			OnStatusApply(GetParent().GetParent<Entity>(), StatusLength);
			GetNode<Timer>("FullTimer").Start(StatusLength);
			GetNode<Timer>("TickTimer").Start(RunEveryNSeconds);
		}

		public abstract void OnTick(Entity entity);
		public abstract void OnStatusApply(Entity entity, double duration);
		public abstract void OnStatusRemove(Entity entity, bool deathRemoval, bool forceRemoval);

		private void OnFullTimerTimeout() => RemoveStatus(false);
		public void RemoveStatus(bool forceRemoval = true, bool deathRemoval = false)
		{
			GetNode<Timer>("TickTimer").Stop();
			OnStatusRemove(GetParent().GetParent<Entity>(), deathRemoval, forceRemoval);
			GetParent<DamageModel>().EmitSignal(DamageModel.SignalName.StatusExpired, Utils.WrapClass<StatusEffect>(this));
			this.QueueFree();
		}

		private void OnStatusTick()
		{
			OnTick(GetParent().GetParent<Entity>());
		}
	}
}
