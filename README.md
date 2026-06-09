# Unity Showcase Time App

Небольшое Unity WebGL-приложение для отображения, синхронизации и ручного редактирования времени. В интерфейсе есть цифровые и аналоговые часы; время можно менять через форму редактирования или перетаскиванием стрелок аналоговых часов.

Проект демонстрирует модульную Unity-архитектуру, асинхронную загрузку сцены через Addressables, DI через Zenject и работу с внешними HTTP API времени.

## Demo

Проект доступен в браузере:

**[Открыть WebGL-билд](https://abysscion.github.io/TimeApp/)**

## Возможности

* цифровые и аналоговые часы;
* ручное редактирование времени;
* перетаскивание стрелок аналоговых часов;
* синхронизация UTC-времени через внешние API;
* fallback между несколькими источниками времени и системным UTC;
* загрузочный экран с прогрессом;
* загрузка gameplay-сцены через Addressables.

## Стек

* Unity `2022.3.54f1 LTS`;
* C#;
* WebGL / IL2CPP;
* UGUI и TextMesh Pro;
* Addressables;
* Zenject;
* UniTask;
* UnityWebRequest.

## Архитектура

Проект использует Clean Architecture-inspired модульную структуру: доменные модели и интерфейсы отделены от UI, сервисов и Unity-specific инфраструктуры, но архитектура адаптирована под Unity lifecycle, Zenject и Addressables.

Основной поток состояний приложения:

```text
Bootstrap -> Startup -> Loading -> Gameplay
```

`Startup` создает запрос загрузки, `Loading` выполняет `LoadingRequest`, показывает прогресс и после загрузки переводит приложение в `Gameplay`. Gameplay-сцена загружается через Addressables по адресу `S_Gameplay`, а не как обычная сцена из Build Settings.

DI собирается через Zenject installers и контексты сцены/модулей: `SceneContext_Gameplay`, `UIContext_Shared`, `ModuleContext_TimeMachine`, `UIContext_TimeMachine`.

Проект разделен на asmdef-модули: `ADA_Shared`, `ADA_Shared_Domain`, `ADA_Modules_TimeMachine`, `ADA_Utilities_Editor`.

## Структура проекта

```text
Assets/_Project
├── Scenes
│   ├── S_Boot.unity
│   └── S_Gameplay.unity
├── _Scripts
│   ├── Shared
│   │   ├── Domain
│   │   ├── Infrastructure
│   │   ├── Installers
│   │   ├── UI
│   │   └── Usecases
│   └── Modules
│       └── TimeMachine
│           ├── Configs
│           ├── Domain
│           ├── Installers
│           ├── Services
│           ├── UI
│           └── UseCases
├── Prefabs
├── Configs
├── Art
└── AddressableAssetsData
```

## Addressables

Основные группы Addressables:

* `Shared` — общие UI-префабы, TMP-ресурсы и графика;
* `Gameplay` — gameplay-сцена и контекст сцены;
* `Module_TimeMachine` — префабы и конфиг модуля часов;
* `Built In Data` и `Orphans` — служебные группы Addressables.

## Синхронизация времени

Список серверов времени задается в `TimeServerSettings`. По умолчанию используются:

* `https://yandex.com/time/sync.json`;
* `http://worldclockapi.com/api/json/utc/now`;
* `https://timeapi.io/api/Time/current/zone?timeZone=UTC`.

Если все внешние источники недоступны, приложение использует `DateTime.UtcNow`.

## Запуск проекта

1. Откройте проект в Unity Hub через Unity `2022.3.54f1`.
2. Убедитесь, что установлен модуль `WebGL Build Support`, если нужна WebGL-сборка.
3. Откройте bootstrap-сцену `Assets/_Project/Scenes/S_Boot.unity`.
4. Запустите проект в Editor.

## Пользовательский сценарий

1. Приложение стартует с bootstrap-сцены и показывает загрузочный экран.
2. После загрузки открывается экран часов.
3. Пользователь видит синхронизированное время на цифровых и аналоговых часах.
4. Время можно изменить через форму редактирования или перетаскиванием стрелок.
5. Изменения можно сохранить или отменить.

## License

Лицензия проекта не указана.
