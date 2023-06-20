using Godot;
using DamageModelSystem.StatusEffects;
namespace DamageModelSystem {
	public partial class DamageModel: Node {
		private float Health;
		private bool IsDead = false;
		private ulong LastDamagedOnTick;
		private ulong LastRegenedOnTick = 0;
		private ulong IFramesUntil;
		[Signal] public delegate void StatusInflictedEventHandler(Utils.SignalWrapper<StatusEffects.Effects.StatusEffect> effect);
		[Signal] public delegate void StatusExpiredEventHandler(Utils.SignalWrapper<StatusEffects.Effects.StatusEffect> expiredEffect);
		[Signal] public delegate void DamagedEventHandler(Utils.SignalWrapper<DamageResult> result);
		[Signal] public delegate void KilledEventHandler(Utils.SignalWrapper<DamageResult> result);
		[Signal] public delegate void HealedEventHandler(Utils.SignalWrapper<HealResult> result);
		[Export] public int MaxHealth = 0;
		[Export] public bool NaturalRegen = false;
		[Export] public float NaturalRegenTick = 1;
		[Export] public float NaturalRegenDelay = 5;
		[Export] public float NaturalRegenAfter = 120;
		[Export] public int IFrames = 0;
		[Export] public float IFrameThreshold = 0;
		[Export] public Godot.Collections.Array<StatusEffect> ImmuneToSpecificEffects = new();
		public override void _Ready() {
			Health = MaxHealth;
		}

		public DamageResult Damage(DamageModelSystem.DamageType type, float amount) {
			DamageResult result;
			ulong thisTick = Engine.GetPhysicsFrames();
			if (IFramesUntil >= thisTick) {
				result = new(false, true, false, type, amount, Health, Health);
				EmitSignal(SignalName.Damaged, Utils.WrapClass(result));
				return result;
			}
			if (IsDead) {
				throw new System.InvalidOperationException("Can't damage a dead entity");
			}
			float oldHealth = Health;
			Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
			LastDamagedOnTick = thisTick;
			result = new(true, false, Health == 0, type, amount, oldHealth, Health);
			if (Health == 0) {
				IsDead = true;
				IFramesUntil = 0;
				foreach (StatusEffects.Effects.StatusEffect item in GetChildren())
				{
					item.RemoveStatus(false, true);
					item.QueueFree();
				}
				EmitSignal(SignalName.Killed, Utils.WrapClass(result));
				return result;
			}
			IFramesUntil = amount >= IFrameThreshold ? thisTick + (ulong)IFrames : thisTick;
			EmitSignal(SignalName.Damaged, Utils.WrapClass(result));
			return result;
		}

		public HealResult Heal(float amount) {
			if (IsDead) {
				throw new System.InvalidOperationException("Can't heal a dead entity");
			}
			float oldHealth = Health;
			Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
			var result = new HealResult(oldHealth, Health, amount);
			EmitSignal(SignalName.Healed, Utils.WrapClass(result));
			return result;
		}

		public DamageResult Kill() {
			if (IsDead) {
				throw new System.InvalidOperationException("Can't kill a dead entity");
			}
			IsDead = true;
			IFramesUntil = 0;
			float oldHealth = Health;
			Health = 0;
			foreach (StatusEffects.Effects.StatusEffect item in GetChildren())
			{
				item.RemoveStatus(false, true);
				item.QueueFree();
			}
			DamageResult result = new DamageResult(true, false, true, DamageType.Null, float.PositiveInfinity, oldHealth, 0);
			EmitSignal(SignalName.Killed, Utils.WrapClass(result));
			return result;
		}

		public override void _PhysicsProcess(double delta) {
			ulong thisTick = Engine.GetPhysicsFrames();
			if (IsDead) {
				SetPhysicsProcess(false);
			}
			if (NaturalRegen) {
				if (Health != MaxHealth && GetChildCount() == 0) {
					if ((LastDamagedOnTick + NaturalRegenAfter) <= thisTick && (LastRegenedOnTick + NaturalRegenDelay) <= thisTick) {
						Heal(NaturalRegenTick);
						LastRegenedOnTick = thisTick;
					}
				}
			}
		}

		public void InflictStatus(DamageModelSystem.StatusEffects.StatusEffect effect, double duration) {
			if (IsDead) {
				throw new System.InvalidOperationException("Can't apply status effects to a dead entity");
			}
			PackedScene scene = (PackedScene)ResourceLoader.Load(DamageModelSystem.StatusEffects.Data.ScenePaths[effect]);
			StatusEffects.Effects.StatusEffect effectNode = (StatusEffects.Effects.StatusEffect)scene.Instantiate();
			effectNode.StatusLength = duration;
			AddChild(effectNode);
			EmitSignal(DamageModel.SignalName.StatusExpired, Utils.WrapClass<StatusEffects.Effects.StatusEffect>(effectNode));
		}

		public void ClearStatuses() {
			foreach (StatusEffects.Effects.StatusEffect item in GetChildren())
			{
				item.RemoveStatus();
				item.QueueFree();
			}
		}
	}
}
