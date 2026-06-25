# Соответствие проекта техническому заданию (ТЗ)

В этом файле приведено подробное сопоставление требований из файла [Продвинутое программирование на С#. Техническое задание на разработку игры (7).md](file:///z:/Unity/RPGGame/Продвинутое%20программирование%20на%20С#.%20Техническое%20задание%20на%20разработку%20игры%20(7).md) с конкретными классами и файлами в кодовой базе проекта.

---

## 🎮 1. Главный герой и Боевая система (Лаба 1-2)

| Требование ТЗ | Где реализовано в коде | Статус |
| :--- | :--- | :---: |
| **Управление WASD + Shift (Бег)** | [StatePlayerMove.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Player/StatePlayerMove.cs#L20-L28) (считывание `moveAction` и `shiftAction` из View). | 🟢 Реализовано |
| **Управление камерой мышью** | Скрипт [CameraCollision.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/CameraCollision.cs) и обработка мыши в стейте передвижения. | 🟢 Реализовано |
| **Атака (ЛКМ - физ / ПКМ - маг)** | [PlayerView.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerView.cs#L62-L63) ловит инпуты мыши и шлет события в `PlayerController`, который переключает состояния в `StatePlayerMeleeAttack` и `StatePlayerRangeAttack`. | 🟢 Реализовано |
| **Анимации игрока** | Управляются стейтами через `Animator.CrossFadeInFixedTime` в [StatePlayerMove.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Player/StatePlayerMove.cs#L13), [StatePlayerMeleeAttack.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Player/StatePlayerMeleeAttack.cs), и др. | 🟢 Реализовано |
| **Характеристики HP, физ/маг урон** | [PlayerModel.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerModel.cs) хранит здоровье и модификаторы, а View отображает это. | 🟢 Реализовано |
| **Полоса HP над персонажами** | Скрипт [HealthBar.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/HUD/HealthBar.cs) (динамически обновляет UI-слайдер над головами мобов). | 🟢 Реализовано |
| **Ближний и Дальний урон** | [MeleeWeapon.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Combat/Weapons/MeleeWeapon.cs) (ближний бой по площади) и [RangeWeapon.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Combat/Weapons/RangeWeapon.cs) + [Shell.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Combat/Weapons/Shell.cs) (снаряды для магии). | 🟢 Реализовано |
| **Инверсия зависимостей (HP/урон)** | Вызовы идут через интерфейсы `IHealth`, `IHittable`, `ITargetable`, а здоровье вынесено в чистый класс `HealthModel`. | 🟢 Реализовано |

---

## 🧟 2. Мобы (Противники) и Локация (Лаба 1-2)

| Требование ТЗ | Где реализовано в коде | Статус |
| :--- | :--- | :---: |
| **Моб ближнего боя (Мечник)** | [EnemyMelee.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Enemies/EnemyMelee.cs). Преследует игрока и бьет мечом. | 🟢 Реализовано |
| **Моб дальнего боя (Маг)** | [EnemyRange.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Enemies/EnemyRange.cs). Держит дистанцию и стреляет заклинаниями. | 🟢 Реализовано |
| **Локация и препятствия** | 3D-сцена с травой, деревьями (префаб `Tree.prefab`), границами и `Directional Light`. | 🟢 Реализовано |
| **Размещение мобов на старте** | [EnemySpawner.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Core/EnemySpawner.cs) спавнит заданное количество мобов на NavMesh при старте игры. | 🟢 Реализовано |
| **Интерфейс HUD и Game Over** | Полоса HP героя, [MagicCooldownUI.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/HUD/MagicCooldownUI.cs) для магии и Canvas поражения с кнопкой Restart (управляется `GameController`). | 🟢 Реализовано |

---

## 🏛️ 3. Архитектура, Меню и DI (Лаба 3)

| Требование ТЗ | Где реализовано в коде | Статус |
| :--- | :--- | :---: |
| **Главное меню (Играть / Настройки)** | Реализовано по MVC: `MainMenuModel`, `MainMenuView` и `MainMenuController`. Настройки громкости работают через `IAudioService`. | 🟢 Реализовано |
| **Игровое меню (ESC)** | [PauseMenuView.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/Menus/Pause/PauseMenuView.cs) и [PauseMenuController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/Menus/Pause/PauseMenuController.cs) (выход в меню, Сохранить, Загрузить). | 🟢 Реализовано |
| **Паттерн Bootstrapper** | Класс [Bootstrapper.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Architecture/EntryPoints/Bootstrapper.cs) инициализирует глобальные настройки звука и грузит меню. | 🟢 Реализовано |
| **Использование DI-фреймворка** | **VContainer** настроен в [GameLifetimeScope.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Architecture/EntryPoints/GameLifetimeScope.cs). Регистрирует синглтоны сервисов и инжектит зависимости. | 🟢 Реализовано *(с доп. баллом)* |
| **Разделение MonoBehaviour** | Логика стейтов, моделей персонажей (`PlayerModel`, `HealthModel`), контроллеров, интерактора и репозиториев написана на чистом C#. | 🟢 Реализовано *(с доп. баллом)* |

---

## 💾 4. Сохранение и Загрузка (Лаба 4)

| Требование ТЗ | Где реализовано в коде | Статус |
| :--- | :--- | :---: |
| **Сохранение игрока (Позиция, HP)** | [SaveInteractor.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/SaveInteractor.cs#L17-L29). | 🟢 Реализовано |
| **Сохранение мобов (Позиции, HP)** | [SaveInteractor.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/SaveInteractor.cs#L31-L44). При загрузке лишние мобы удаляются, а сохраненные перемещаются на свои места. | 🟢 Реализовано *(с доп. баллом)* |
| **Паттерны Репозиторий и Интерактор** | `IPlayerRepository`, `IEnemyRepository`, `PlayerJsonRepository`, `EnemyJsonRepository` (запись в JSON) и `SaveInteractor`. | 🟢 Реализовано *(с доп. баллом)* |
| **Паттерн MVC для UI и Игрока** | UI Меню и Игрок (`PlayerModel`, `PlayerView`, `PlayerController`) полностью разделены по MVC. | 🟢 Реализовано *(с доп. баллом)* |

---

## 🧠 5. Логика врагов и Мирный режим (Лаба 5)

| Требование ТЗ | Где реализовано в коде | Статус |
| :--- | :--- | :---: |
| **Состояния обычных мобов (>= 4)** | Реализованы: Idle, Chase (Агрессия), Attack, Flee (Бегство при ХП < 30%), а также Hit и Dead. | 🟢 Реализовано |
| **Босс и его состояния (>= 7)** | [Boss.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Enemies/Boss.cs) имеет стейты: Idle, Chase, Attack, HeavyAttack, Teleport, Shield (блок урона), Summon (призыв). | 🟢 Реализовано *(с доп. баллом)* |
| **Ускорение босса при ХП < 50%** | Свойство `AttackSpeedMultiplier` возвращает 1.5f при ХП < 50%. В стейтах атак скорость анимации умножается на этот коэффициент. | 🟢 Реализовано *(с доп. баллом)* |
| **Мирный режим (Peaceful Mode)** | В `StateEnemyHit` обычные мобы при ударе не агрятся, а возвращаются в Idle. Босс агрится сразу при первом получении урона. | 🟢 Реализовано |

---

## ⚔️ 6. Вариативность мобов (Лаба 6)

| Требование ТЗ | Где реализовано в коде | Статус |
| :--- | :--- | :---: |
| **Виды оружия у мобов** | [EnemyWeaponFactory.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Combat/Weapons/EnemyWeaponFactory.cs) вешает `SwordWeapon` / `AxeWeapon` мечникам и `WandWeapon` / `BowWeapon` лучникам. | 🟢 Реализовано |
| **Спавнеры с фабрикой оружия** | [LocalSpawner.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Core/LocalSpawner.cs) использует фабрику оружия, внедренную через VContainer, при генерации пачек мобов. | 🟢 Реализовано |
| **Стихии Босса (Ice, Fire, Earth, Aether)** | Реализовано в `Boss.cs`. При смене стихии меняется цвет босса и оружия. На атаках срабатывают стратегии эффектов: замедление, горение, вампиризм, двойной урон. | 🟢 Реализовано |

---

## 📡 7. События и Системы (Лаба 7)

| Требование ТЗ | Где реализовано в коде | Статус |
| :--- | :--- | :---: |
| **Спавн босса после 3 убийств** | [GameController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Core/GameController.cs#L47-L50) на 3-й фраг шлет эвент `OnBossSpawnRequested`, по которому спавнится Босс. | 🟢 Реализовано |
| **Победа и музыка на 5 убийств** | [GameController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Core/GameController.cs#L51-L54) на 5-й фраг включает победную музыку и через 3 секунды загружает главное меню. | 🟢 Реализовано |
| **Система очков (Scoreboard)** | Реализовано по MVC: [ScoreboardModel.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/HUD/ScoreboardModel.cs), [ScoreboardView.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/HUD/ScoreboardView.cs) и [ScoreboardController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/HUD/ScoreboardController.cs). | 🟢 Реализовано |
