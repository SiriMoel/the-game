using DamageModelSystem.StatusEffects;
using Godot;
namespace DamageModelSystem.StatusEffects.Effects;

public partial class Fire : StatusEffect
{
    public override int RunEveryNSeconds => 1;

    public override void OnStatusApply(Entity entity, double duration)
    {
        GD.Print($"Entity {entity.Name} is on fire for {duration}s");
    }

    public override void OnStatusRemove(Entity entity, bool deathRemoval, bool forceRemoval)
    {
        GD.Print($"Entity {entity.Name} fire expired");
    }

    public override void OnTick(Entity entity)
    {
        DamageModel damageModel = GetParent<DamageModel>();
        damageModel.Damage(DamageType.Status, damageModel.MaxHealth * 0.02f);
    }
}
