# Ninja Survivors

A top-down survival game built in Unity with C#. The player moves through a tile-based map while waves of enemies close in. Weapons fire automatically, and killing enemies earns experience and gold, which are spent on stat and weapon upgrades.

Developed by Kaan Akgül and Batuhan Balım during training at MANC Games (September 2024 – January 2025).

## Gameplay

- Survive as long as possible. A timer and kill counter track the run.
- Weapons fire automatically in the direction of movement.
- Enemies drop experience and gold. Each level-up pauses the game and offers either a stat upgrade or a weapon upgrade.
- **Stats:** max health, move speed, attack speed, damage, lifesteal and health regeneration.
- **Weapons:** Shuriken (the starting weapon), Axe, Katana, Ki Blast, Boomerang and Sai. Each has up to eight levels with its own progression, such as extra projectiles, more swing directions, a larger blast radius or a longer range. Upgrades beyond the first level cost gold.
- 15 enemy types, unlocked over time while the spawn rate increases.

**Controls:** WASD to move, Esc to pause, R to restart after a game over.

## Technical overview

- **Data-driven content.** Enemies (`EnemyData`) and weapons (`WeaponData`) are ScriptableObjects, so stats, rewards and prefabs are configured as assets rather than in code.
- **Weapon system.** An abstract `WeaponBehavior` handles cooldowns, damage calculation, lifesteal and level checks. Each weapon subclass implements its own `Attack` and `OnLevelUp`. `WeaponManager` owns the weapon instances, their levels and upgrade costs.
- **Enemy AI.** Enemies follow the player with 2D NavMesh pathfinding, using [NavMeshPlus](https://github.com/h8man/NavMeshPlus) to bake a NavMesh from tilemaps.
- **Spawning.** `EnemySpawner` places enemies just outside the camera view on valid NavMesh positions. It scales the spawn rate over time and unlocks new enemy types at intervals. Enemies that fall too far behind are moved back near the player instead of being destroyed.
- **Game flow and UI.** Managers for pausing, level-up screens, hover tooltips, restarting and scene changes. Music volume is saved with `PlayerPrefs`.

Scripts are in [`Assets/Scripts`](Assets/Scripts), with the weapon implementations in [`Assets/Scripts/WB Scripts`](Assets/Scripts/WB%20Scripts).

## Running the project

1. Install **Unity 2022.3.40f1** through Unity Hub. Another 2022.3 LTS version will probably work but hasn't been tested.
2. Clone the repository and add the folder in Unity Hub.
3. Open `Assets/Scenes/MainMenu.unity` and press Play.

The build includes the `MainMenu` and `GrassMap` scenes. `DesertMap` is an unfinished second map and isn't included in the build.

A WebGL build is hosted with my personal site at [kaanakgul.dev/ninjasurvivors](https://kaanakgul.dev/ninjasurvivors). It is desktop only.

## Credits

- **Kaan Akgül** and **Batuhan Balım**: development.
- [NavMeshPlus](https://github.com/h8man/NavMeshPlus) (MIT License) is included in `Assets/NavMeshComponents`.
- The Arcade Classic font is by Jakob Fischer ([pizzadude.dk](https://www.pizzadude.dk)).
- The art, audio and fonts come from third-party sources and remain under their original licenses.
