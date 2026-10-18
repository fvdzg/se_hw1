# ВышКат 

> **«ВышКат»**

Консольное приложение учёта парка кикшеринга «ВышКат» НИУ ВШЭ. Реализовано в соответствии с принципами SOLID, использует DI-контейнер и покрыто юнит-тестами.

---

## Содержание

- [Запуск](#запуск)
- [Тестирование](#тестирование)
- [Структура проекта](#структура-проекта)
- [Архитектура и SOLID](#архитектура-и-solid)
- [Обязательные вопросы](#обязательные-вопросы)
---

## Запуск

### Требования

- .NET SDK 8.0 или выше

### Команды

```bash
# сборка решения
dotnet build VyshKat.sln

# запуск приложения
dotnet run --project src/VyshKat.Console
```

При старте выполняется **демо-режим** (`ConsoleDemo`) — демонстрирует приём техники, отклонение и все отчёты на заранее подготовленных данных. Затем открывается **интерактивное меню** (`InteractiveMenu`).

### Интерактивное меню

```
___menu___
0) Exit
1) Add scooter
2) Add electric bike
3) Add bicycle
4) Add helmet
5) Add dock station
6) Add charging cabinet
7) Reports
```

Пункт `7` выводит актуальные отчёты, включая предметы, добавленные через меню.

---

## Тестирование

### Запуск тестов

```bash
dotnet test VyshKat.sln
```

### Запуск с покрытием

```bash
dotnet test VyshKat.sln --collect:"XPlat Code Coverage"
```

### Результаты покрытия

| Метрика | Значение | Требование ТЗ |
|---|---|---|
| Покрытие строк (line-rate) | **79 %** | ≥ 60 % ✅ |
| Покрытие ветвлений (branch-rate) | **80 %** | — |
| Всего тестов | 16 | — |
| Сбой | 0 | — |

> **Примечание.** Классы `Program`, `ConsoleDemo`, `ConsoleUserInterface` и `InteractiveMenu` помечены `[ExcludeFromCodeCoverage]`, так как содержат только тонкую обвязку над консолью. Бизнес-логика (`Services`, `Domain`) покрыта полностью.

### Набор тестов

| Файл | Что проверяет |
|---|---|
| `ServiceCenterTests` | Правила техосмотра: префикс `HSE-`, пустой номер, превышение лимита энергии |
| `ParkManagerTests` | Приём/отклонение техники, **подмена техосмотра через DI** (stub + Moq) |
| `ReportServiceTests` | Суммарная энергия, список для новичков, инвентарь |
| `ParkRegistryTests` | Добавление в реестр, неизменяемость `GetAll()` |

---

## Структура проекта

```
VyshKat/
├── VyshKat.sln
├── src/VyshKat.Console/
│   ├── Program.cs                     # точка входа, настройка DI-контейнера
│   ├── domain/
│   │   ├── interfaces/
│   │   │   ├── IInventoryItem.cs      # инвентарный номер + имя
│   │   │   ├── IEnergyConsumer.cs     # суточный расход энергии
│   │   │   ├── IServiceCenter.cs      # абстракция техосмотра
│   │   │   └── IParkRegistry.cs       # абстракция хранилища
│   │   └── entities/
│   │       ├── Vehicle.cs             # абстрактный транспорт
│   │       ├── Scooter.cs
│   │       ├── Bicycle.cs
│   │       ├── ElectricBicycle.cs
│   │       ├── Accessory.cs           # абстрактная вещь
│   │       ├── Helmet.cs
│   │       ├── DockStation.cs
│   │       └── ChargingCabinet.cs
│   ├── services/
│   │   ├── ServiceCenter.cs           # правила техосмотра
│   │   ├── ParkRegistry.cs            # in-memory реестр
│   │   ├── ParkManager.cs             # оркестратор приёмки
│   │   └── ReportService.cs           # отчёты
│   └── ui/
│       ├── IUserInterface.cs          # абстракция ввода/вывода
│       ├── ConsoleUserInterface.cs    # реализация через Console
│       ├── ConsoleDemo.cs             # демо-режим
│       └── InteractiveMenu.cs         # интерактивное меню
└── tests/VyshKat.Tests/
    ├── StubServiceCenter.cs       # ручная подмена техосмотра
    ├── ServiceCenterTests.cs
    ├── ParkManagerTests.cs
    ├── ReportServiceTests.cs
    └── ParkRegistryTests.cs
```

---

## Архитектура и SOLID

### Слои

```
┌───────────────────────────────────────────────────┐
│  UI        InteractiveMenu · ConsoleDemo          │  ← ввод/вывод
├───────────────────────────────────────────────────┤
│  Services  ParkManager · ReportService · ...      │  ← бизнес-логика
├───────────────────────────────────────────────────┤
│  Domain    Vehicle · Accessory · интерфейсы       │  ← модель предметной области
└───────────────────────────────────────────────────┘
```

Зависимости направлены **сверху вниз**. Домен не знает о сервисах и интерфейсе.

### Доменная модель

```
IInventoryItem
├── InventoryNumber : string
└── Name : string

IEnergyConsumer
└── DailyEnergyKwh : double

Vehicle (abstract)  : IInventoryItem, IEnergyConsumer
├── BeginnerFriendliness : int        (1–10)
├── IsSuitableForBeginners : bool     (Friendliness >= 6)
├── Scooter
├── Bicycle                           (не потребляет энергию)
└── ElectricBicycle

Accessory (abstract) : IInventoryItem
├── Helmet                            (не потребляет энергию)
├── DockStation       : IEnergyConsumer
└── ChargingCabinet   : IEnergyConsumer
```

Интерфейсы `IInventoryItem` и `IEnergyConsumer` разделены. `Helmet` и `Bicycle` реализуют только первый, поэтому автоматически исключаются из расчёта энергии через `OfType<IEnergyConsumer>()` в `ReportService.TotalDailyEnergy()`.

### Правила техосмотра (`ServiceCenter`)

1. Предмет не `null`.
2. Инвентарный номер не пустой.
3. Инвентарный номер начинается с префикса `HSE-`.
4. Если предмет потребляет энергию, расход не превышает `MaxDailyEnergyKwh = 5.0` кВт·ч/сутки.

### Правило «для новичков»

Устройства с `BeginnerFriendliness >= 6` попадают в отчёт `ReportService.BeginnerFriendlyVehicles()`. Правило инкапсулировано в свойстве `Vehicle.IsSuitableForBeginners`.

### DI-контейнер (`Program.cs`)

```csharp
var services = new ServiceCollection();

services.AddSingleton<IParkRegistry, ParkRegistry>();
services.AddSingleton<IServiceCenter, ServiceCenter>();
services.AddSingleton<ParkManager>();
services.AddSingleton<ReportService>();
services.AddSingleton<IUserInterface, ConsoleUserInterface>();
services.AddSingleton<ConsoleDemo>();
services.AddSingleton<InteractiveMenu>();

var provider = services.BuildServiceProvider();
```

Все зависимости разрешаются через конструкторы. В тестах контейнер не используется — объекты собираются вручную с подменёнными зависимостями.

---

## Обязательные вопросы

### 1. Какой принцип SOLID применён наиболее явно и где?

**DIP (Dependency Inversion Principle)** — в `services/ParkManager.cs` и `services/ReportService.cs`.

`ParkManager` зависит от абстракций `IServiceCenter` и `IParkRegistry`, а не от конкретных `ServiceCenter` и `ParkRegistry`. Конкретные реализации подставляются через конструкторное внедрение и регистрируются в DI-контейнере в `Program.cs`.

Это позволяет в тестах подменять техосмотр на `StubServiceCenter` без изменения кода `ParkManager`.

Также явно применён **ISP (Interface Segregation Principle)**: интерфейсы `IInventoryItem` и `IEnergyConsumer` разделены. `Helmet` реализует только первый, `DockStation` — оба. Благодаря этому `ReportService.TotalDailyEnergy()` через `OfType<IEnergyConsumer>()` автоматически исключает шлемы и велосипеды из расчёта энергии — без единой условной проверки.

---

### 2. Какой принцип сознательно ограничен и почему это приемлемо?

**OCP ограничен в `ui/InteractiveMenu.cs`**.

Методы `AddVehicle` и `AddAccessory` содержат ветвление по типу предмета. При добавлении нового вида транспорта нужно дописать ветку в меню.

**Почему это приемлемо:**

- Доменная модель (`Vehicle`, `ParkManager`, `ReportService`, `ServiceCenter`) полностью открыта для расширения: новый наследник `Vehicle` работает без правок бизнес-логики.
- Ограничение касается только точки ввода данных — единственного места, где пользователь физически выбирает тип предмета.
- Вынесение этой логики в фабрики или стратегии ради одного пункта меню — это «декоративный» SOLID.

---

### 3. Что сломается при добавлении нового вида транспорта, сотрудника или склада запчастей?

| Расширение | Сломается? | Что произойдёт |
|---|---|---|
| Новый вид транспорта (`CargoBike`) | ❌ Нет | Создаётся `CargoBike : Vehicle`. `ParkManager`, `ReportService`, `ServiceCenter` работают без изменений — они зависят от `IInventoryItem` / `IEnergyConsumer`. Единственное изменение — новый пункт в `InteractiveMenu` |
| Сотрудник службы эксплуатации | ❌ Нет | По ТЗ сотрудники не ставятся на баланс и не реализуют `IInventoryItem`. Это отдельная сущность со своим репозиторием, не затрагивающая текущую логику парка |
| Склад запчастей | ❌ Нет | Вводится новый интерфейс `IWarehouseRegistry` и новые сущности. Существующий `IParkRegistry` и логика приёмки остаются нетронутыми |

---

### 4. Какой тест фиксирует решение техосмотра и/или правило «для новичков»? Как подменяется зависимость?

**Решение техосмотра** фиксируют тесты в `tests/VyshKat.Tests/ParkManagerTests.cs`:

| Тест | Что проверяет |
|---|---|
| `ServiceCenterAllows_ItemAddToRegistry` | При успешном техосмотре предмет попадает в реестр |
| `ServiceCenterRejects_ItemNotAddToRegistry` | При отклонении реестр остаётся пустым |
| `DelegateDecisionToServiceCenter` | Через `Moq` проверяет, что решение делегируется сервису |


**Подмена зависимости** реализована двумя способами:

1. **Ручной стаб** — `tests/VyshKat.Tests/Stubs/StubServiceCenter.cs` реализует `IServiceCenter` и принимает делегат с решением:

   ```csharp
   var stub = StubServiceCenter.AlwaysAccept();
   var manager = new ParkManager(registry, stub);
   ```

2. **Мок через Moq** — для проверки взаимодействий:

   ```csharp
   var mock = new Mock<IServiceCenter>();
   mock.Setup(s => s.Inspect(It.IsAny<IInventoryItem>())).Returns(true);
   mock.Verify(s => s.Inspect(item), Times.Once);
   ```

---

### 5. Что сделал ИИ, а что изменили вы?

**ИИ использовался** как вспомогательный инструмент:

- **Юнит-тесты**
- **Документация**: структура и формулировки в `README.md`.
- **Подсказки по коду**: примеры использования `Moq`,  разбор ошибок компиляции и запусков тестов.

---

