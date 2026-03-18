public enum DamageType { Melee, Range }

// OCP: Заменяем строки на Enum для безопасной передачи событий анимации
public enum AnimationEventType { AttackEnd, DealDamage }