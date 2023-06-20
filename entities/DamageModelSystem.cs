namespace DamageModelSystem {
    public enum DamageType {
        Null,
        Melee,
        Status,
        Projectile,
        Magic,
        Crush,
        True,
    }

    namespace StatusEffects {
        public enum StatusEffect {
            Fire,
        }
        namespace Effects {}
        public static class Data {
            public static System.Collections.Generic.Dictionary<StatusEffect, string> ScenePaths = new System.Collections.Generic.Dictionary<StatusEffect, string>() {
                {StatusEffect.Fire, "res://data/status_effects/Fire.tscn"},
            };
        }
    }

    public record DamageResult(bool Success, bool BlockedByIFrames, bool DidKill, DamageType DamageType, float Amount, float OldHealth, float NewHealth);
    public record HealResult(float OldHealth, float NewHealth, float Amount);
}

