using UnityEngine;

public class FireMeleeEffect : IBossAttackEffectStrategy {
    public void PlayEffect(Transform origin) {
        Debug.Log("🔥 Звук: Громкий огненный взмах мечом!");
    }
}
public class IceMeleeEffect : IBossAttackEffectStrategy {
    public void PlayEffect(Transform origin) {
        Debug.Log("❄️ Звук: Хруст льда при ударе вблизи!");
    }
}
public class EarthMeleeEffect : IBossAttackEffectStrategy {
    public void PlayEffect(Transform origin) {
        Debug.Log("🪨 Звук: Тяжелый каменный удар!");
    }
}
public class AetherMeleeEffect : IBossAttackEffectStrategy {
    public void PlayEffect(Transform origin) {
        Debug.Log("✨ Звук: Эфирный резонанс клинка!");
    }
}

public class FireRangeEffect : IBossAttackEffectStrategy {
    public void PlayEffect(Transform origin) {
        Debug.Log("🔥 Партиклы: Вылет КРАСНОГО огненного шара!");
    }
}
public class IceRangeEffect : IBossAttackEffectStrategy {
    public void PlayEffect(Transform origin) {
        Debug.Log("❄️ Партиклы: Вылет СИНЕЙ ледяной стрелы!");
    }
}
public class EarthRangeEffect : IBossAttackEffectStrategy {
    public void PlayEffect(Transform origin) {
        Debug.Log("🪨 Партиклы: Бросок ЗЕЛЕНОГО кислотного камня!");
    }
}
public class AetherRangeEffect : IBossAttackEffectStrategy {
    public void PlayEffect(Transform origin) {
        Debug.Log("✨ Партиклы: Вылет ФИОЛЕТОВОГО сгустка энергии!");
    }
}