# Unity Showcase Time App

A small Unity WebGL application for displaying, synchronizing, and manually editing time. The interface includes digital and analog clocks; time can be changed either through an edit form or by dragging the hands of the analog clock.

The project demonstrates a modular Unity architecture, asynchronous scene loading via Addressables, DI with Zenject, and integration with external HTTP time APIs.

## Demo

The project is available in the browser:

**[Open WebGL build](https://abysscion.github.io/TimeApp/)**

## Features

* digital and analog clocks;
* manual time editing;
* dragging the analog clock hands;
* UTC time synchronization via external APIs;
* fallback between multiple time sources and system UTC;
* loading screen with progress indication;
* gameplay scene loading via Addressables.

## Stack

* Unity `2022.3.54f1 LTS`;
* C#;
* WebGL / IL2CPP;
* UGUI and TextMesh Pro;
* Addressables;
* Zenject;
* UniTask;
* UnityWebRequest.

## Architecture

The project uses a Clean Architecture-inspired modular structure: domain models and interfaces are separated from the UI, services, and Unity-specific infrastructure, while the architecture is adapted to the Unity lifecycle, Zenject, and Addressables.

Main application state flow:

```text
Bootstrap -> Startup -> Loading -> Gameplay
```

`Startup` creates a loading request, `Loading` executes the `LoadingRequest`, displays progress, and transitions the application to `Gameplay` after loading is complete. The gameplay scene is loaded through Addressables using the `S_Gameplay` address, rather than as a regular scene from Build Settings.

DI is assembled through Zenject installers and scene/module contexts: `SceneContext_Gameplay`, `UIContext_Shared`, `ModuleContext_TimeMachine`, `UIContext_TimeMachine`.

The project is split into asmdef modules: `ADA_Shared`, `ADA_Shared_Domain`, `ADA_Modules_TimeMachine`, `ADA_Utilities_Editor`.

## Project Structure

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

Main Addressables groups:

* `Shared` — shared UI prefabs, TMP resources, and graphics;
* `Gameplay` — gameplay scene and scene context;
* `Module_TimeMachine` — clock module prefabs and configuration;
* `Built In Data` and `Orphans` — service Addressables groups.

## Time Synchronization

The list of time servers is defined in `TimeServerSettings`. By default, the following sources are used:

* `https://yandex.com/time/sync.json`;
* `http://worldclockapi.com/api/json/utc/now`;
* `https://timeapi.io/api/Time/current/zone?timeZone=UTC`.

If all external sources are unavailable, the application uses `DateTime.UtcNow`.

## Running the Project

1. Open the project in Unity Hub using Unity `2022.3.54f1`.
2. Make sure the `WebGL Build Support` module is installed if a WebGL build is required.
3. Open the bootstrap scene `Assets/_Project/Scenes/S_Boot.unity`.
4. Run the project in the Editor.

## User Flow

1. The application starts from the bootstrap scene and displays the loading screen.
2. After loading is complete, the clock screen opens.
3. The user sees synchronized time on the digital and analog clocks.
4. The time can be changed through the edit form or by dragging the clock hands.
5. Changes can be saved or cancelled.

## License

The project license is not specified.
