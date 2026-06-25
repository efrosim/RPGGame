# Отчёт по реализации дополнительных баллов проекта

В этом документе собрано подробное описание выполнения всех **9 дополнительных требований (критериев на доп. баллы)** со ссылками на конкретные файлы и строки кода в проекте, а также техническое описание того, как это работает.

---

## 💎 Список дополнительных баллов (9 из 9 реализовано)

### 1. MVC для интерфейса (UI)
* **Что требует ТЗ:** Логика работы с пользовательским интерфейсом должна быть отделена от движка Unity. Контроллеры не должны быть компонентами `MonoBehaviour` (чистые C# классы).
* **Как реализовано:** 
  Интерфейс главного меню, меню паузы и таблицы очков (Scoreboard) построен по паттерну MVP/MVC.
  - **Модели (Model):** Представляют собой чистые C# данные (например, [ScoreboardModel.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/HUD/ScoreboardModel.cs) хранит счет).
  - **Представления (View):** Компоненты `MonoBehaviour` (например, [MainMenuView.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/Menus/Main/MainMenuView.cs)), которые отвечают исключительно за отображение элементов UI на экране и трансляцию кликов/ввода через события (Actions).
  - **Контроллеры (Controller):** Чистые C# классы без зависимости от Unity Engine (например, [MainMenuController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/Menus/Main/MainMenuController.cs)). Они подписываются на события из View, обновляют данные в Model и синхронизируют их обратно. Все они регистрируются в контейнере зависимостей (DI).
* **Файлы:**
  - **Главное меню:** [MainMenuModel.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/Menus/Main/MainMenuModel.cs), [MainMenuView.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/Menus/Main/MainMenuView.cs), [MainMenuController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/Menus/Main/MainMenuController.cs)
  - **Меню паузы:** [PauseMenuView.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/Menus/Pause/PauseMenuView.cs), [PauseMenuController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/Menus/Pause/PauseMenuController.cs)
  - **Счетчик очков:** [ScoreboardModel.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/HUD/ScoreboardModel.cs), [ScoreboardView.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/HUD/ScoreboardView.cs), [ScoreboardController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/UI/HUD/ScoreboardController.cs)

---

### 2. Сохранение позиции мобов
* **Что требует ТЗ:** Сохранять координаты и здоровье всех противников на сцене при сохранении игры и восстанавливать их при загрузке.
* **Как реализовано:** 
  - Во время **сохранения** интерактор [SaveInteractor.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/SaveInteractor.cs#L33-L52) сканирует все объекты типа `Enemy` на сцене, определяет их тип (`Boss`, `Melee`, `Range`), сохраняет их координаты, текущее здоровье и уникальный `Guid`. Список сериализуется в JSON через `EnemyJsonRepository`.
  - Во время **загрузки** все текущие враги на сцене уничтожаются, за исключением базовых шаблонов мобов, которые отключаются (`SetActive(false)`), чтобы избежать повреждения исходных данных спавнера. На основе сохраненных JSON-данных новые экземпляры врагов инстанцируются из префабов и позиционируются.
  - **Важная деталь:** Чтобы восстановить корректное поведение ИИ на навигационной сетке (`NavMesh`), после позиционирования вызывается метод `NavMeshAgent.Warp(spawnPos)` (строка [SaveInteractor.cs:L107](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/SaveInteractor.cs#L107)). После этого мобам восстанавливаются здоровье, ID и выдается оружие через фабрику.
* **Файлы:**
  - [SaveInteractor.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/SaveInteractor.cs#L33-L52) — сбор данных мобов и воссоздание/привязка к NavMesh.
  - [IRepositories.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/IRepositories.cs#L22-L35) — описание структуры `EnemySaveData`.

---

### 3. Сложный босс и 8 состояний (минимум 7)
* **Что требует ТЗ:** Реализовать сложного босса с использованием машины состояний (минимум 7 состояний).
* **Как реализовано:** 
  Конечный автомат босса инициализируется в [Boss.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Enemies/Boss.cs#L54-L61) и содержит ровно **8 различных состояний**, управляющих его поведением:
  1. `StateBossIdle` — ожидание игрока в зоне покоя.
  2. `StateBossChase` — погоня за игроком, если он вошел в радиус агра.
  3. `StateBossAttack` — базовая физическая атака ближнего боя.
  4. `StateBossHeavyAttack` — медленная, но разрушительная супер-атака.
  5. `StateBossTeleport` — мгновенное перемещение к игроку или от него при получении урона.
  6. `StateBossShield` — активация щита, полностью блокирующего входящий урон.
  7. `StateBossSummon` — призыв волны миньонов на помощь боссу.
  8. `StateEnemyHit` — реакция на урон (стан/прерывание действий).
* **Файлы:**
  - [BossStates.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Boss/BossStates.cs) — весь код состояний босса.
  - [Boss.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Enemies/Boss.cs) — инициализация машины состояний.

---

### 4. Продвинутый спавн мобов
* **Что требует ТЗ:** Случайные точки спавна на карте или/и редкие усиленные мобы на NavMesh.
* **Как реализовано:** 
  - Точки спавна мобов рассчитываются динамически. Спавнер берет случайную позицию в пределах заданного радиуса и сопоставляет ее с запеченной навигационной сеткой (`NavMesh.SamplePosition`), исключая застревание мобов в текстурах или препятствиях.
  - **Динамическая экипировка:** При появлении мобов на сцене [LocalSpawner.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Core/LocalSpawner.cs#L24-L45) обращается к фабрике оружия [EnemyWeaponFactory.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Combat/Weapons/EnemyWeaponFactory.cs). Фабрика случайным образом выбирает тип оружия (например, `SwordWeapon` или `WandWeapon`), динамически вешает нужный компонент на моба и копирует все параметры (префабы снарядов, зоны поражения) со стандартных префабов без медленной рефлексии.
* **Файлы:**
  - [EnemySpawner.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Core/EnemySpawner.cs#L29-L40) — выбор случайных координат на NavMesh.
  - [LocalSpawner.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Core/LocalSpawner.cs#L24-L45) — спавн пачек врагов и вызов фабрики.
  - [EnemyWeaponFactory.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Combat/Weapons/EnemyWeaponFactory.cs) — фабрика создания и настройки оружия у врагов на ходу.

---

### 5. Использование DI фреймворка (VContainer)
* **Что требует ТЗ:** Инъекция зависимостей с использованием стороннего DI-контейнера для уменьшения связанности кода.
* **Как реализовано:** 
  В проекте настроен и используется фреймворк **VContainer**.
  - Точка входа и регистрация всех зависимостей собраны в классе [GameLifetimeScope.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Architecture/EntryPoints/GameLifetimeScope.cs#L9-L34).
  - В контейнере регистрируются синглтон-сервисы (звук, сохранение), модели интерфейса, интеракторы, репозитории и фабрика оружия.
  - Внедрение зависимостей происходит через конструкторы для чистых C# классов, а для MonoBehaviour-объектов — с помощью атрибута `[Inject]` (например, в [GameplayEntryPoint.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Architecture/EntryPoints/GameplayEntryPoint.cs#L26-L32)). Это исключает использование классических синглтонов Unity (`Instance.DoSomething()`).
* **Файлы:**
  - [GameLifetimeScope.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Architecture/EntryPoints/GameLifetimeScope.cs) — контейнер зависимостей.
  - [GameplayEntryPoint.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Architecture/EntryPoints/GameplayEntryPoint.cs) — точка входа сцены, резолвящая зависимости.

---

### 6. Ограничение на MonoBehaviour
* **Что требует ТЗ:** Использовать `MonoBehaviour` строго для взаимодействия с движком Unity (рендеринг, физика, ввод, анимация). Вся логика должна быть вынесена в чистые C#-классы (POCO).
* **Как реализовано:** 
  Все ключевые архитектурные элементы не зависят от Unity:
  - Модели персонажа и UI ([PlayerModel.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerModel.cs), `ScoreboardModel`) — это чистые C# классы, содержащие только свойства, математику и C# события.
  - Логические контроллеры ([PlayerController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerController.cs), `MainMenuController`) — не наследуются от `MonoBehaviour`, создаются в DI и управляют логикой.
  - Сервисы сохранения и интеракторы ([SaveInteractor.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/SaveInteractor.cs)) — чистые C# классы.
* **Файлы:**
  - [PlayerModel.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerModel.cs) — модель игрока.
  - [PlayerController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerController.cs) — контроллер игрока.
  - [SaveInteractor.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/SaveInteractor.cs) — интерактор сохранений.

---

### 7. MVC подход в реализации Игрока
* **Что требует ТЗ:** Спроектировать архитектуру игрока на основе разделения ответственности (Model-View-Controller / MVP).
* **Как реализовано:** 
  Игрок разделен на три независимых уровня:
  - **Model:** [PlayerModel.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerModel.cs) — хранит здоровье, эффекты, скорость, кулдауны. Не знает ничего про рендеринг, физику или ввод.
  - **View:** [PlayerView.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerView.cs) — MonoBehaviour-компонент, который хранит ссылки на `Rigidbody`, `Animator` и Input System. Предоставляет контроллеру свойства ввода (`MovementInput`, `RotationInput`, `IsRunning`).
  - **Controller:** [PlayerController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerController.cs) — подписывается на события ввода от `PlayerView`, изменяет данные в `PlayerModel` и управляет сменой состояний в машине состояний игрока.
  - **Физические состояния:** Состояния движения ([StatePlayerMove.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Player/StatePlayerMove.cs)) и покоя ([StatePlayerIdle.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Player/StatePlayerIdle.cs)) считывают нормализованные свойства ввода из `PlayerView` (соблюдение Single Responsibility Principle).
* **Файлы:**
  - [PlayerModel.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerModel.cs)
  - [PlayerView.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerView.cs)
  - [PlayerController.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Player/PlayerController.cs)
  - [StatePlayerIdle.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Player/StatePlayerIdle.cs) — новое выделенное состояние покоя игрока.
  - [StatePlayerMove.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Player/StatePlayerMove.cs) — состояние бега/ходьбы.

---

### 8. Продвинутая архитектура репозиториев
* **Что требует ТЗ:** Реализовать раздельные репозитории для разных сущностей и дирижировать ими в интеракторе сохранений.
* **Как реализовано:** 
  - Написаны интерфейсы `IPlayerRepository` и `IEnemyRepository`, разделяющие ответственность за сохранение данных игрока и врагов.
  - Конкретные реализации [JsonRepositories.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/JsonRepositories.cs) сериализуют данные в JSON-строки и сохраняют их в `PlayerPrefs`. При необходимости хранилище легко заменить на файлы или БД без изменения игровой логики.
  - Интерактор [SaveInteractor.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/SaveInteractor.cs) выступает в роли дирижера (Facade / Orchestrator): собирает нужные данные со всей сцены, распределяет их по соответствующим репозиториям при сохранении и наоборот — распределяет загруженные данные по объектам сцены при загрузке.
* **Файлы:**
  - [JsonRepositories.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/JsonRepositories.cs) — реализации сохранения.
  - [IRepositories.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/IRepositories.cs) — описание данных.
  - [SaveInteractor.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/SaveSystem/SaveInteractor.cs) — интерактор-координатор сохранений.

---

### 9. Фаза ярости босса
* **Что требует ТЗ:** Увеличить скорость атак босса в 1.5 раза при падении его уровня здоровья ниже 50%.
* **Как реализовано:** 
  - В классе [Boss.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Enemies/Boss.cs#L39) реализовано свойство `AttackSpeedMultiplier`, возвращающее `1.5f`, если текущее здоровье босса меньше 50% от максимального, и `1.0f` в обычном режиме.
  - В файле состояний босса [BossStates.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Boss/BossStates.cs#L68-L72) в фазах атак (`StateBossAttack` и `StateBossHeavyAttack`) скорость воспроизведения аниматора (`animator.speed`) умножается на это свойство. Длина таймеров ожидания завершения анимации делится на этот же коэффициент, благодаря чему босс бьет чаще и быстрее. При выходе из состояний атак скорость аниматора возвращается к дефолтной.
* **Файлы:**
  - [Boss.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/Characters/Enemies/Boss.cs) — расчет множителя скорости.
  - [BossStates.cs](file:///z:/Unity/RPGGame/Assets/!Scripts/StateMachine/States/Boss/BossStates.cs#L68-L72) — применение множителя к анимациям и таймерам атак.
