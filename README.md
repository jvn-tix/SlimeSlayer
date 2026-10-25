## Developer & Contributions
Jevon (Game Programmer)

## About
Slime Slayer is a 2D action adventure game where players explore connected maps, fight off waves of enemies, collect coins, and take on powerful bosses. Spend your hard-earned coins at the shop to get stronger, interact with the world around you, and survive increasingly dangerous encounters. Every hit is backed by knockback, camera shake, and damage popups to make combat feel punchy and readable.

<br>

## Key Features

**Action Combat System**: Attack enemies with responsive player combat, with knockback, hit feedback, and damage popups on every strike. Enemies chase and attack, so positioning and timing matter.

**Coin Economy & Shop**: Defeat enemies to earn coins, with animated coin popups and a live coin display. Spend them in the shop to upgrade and prepare for tougher fights.

**Boss Encounters**: Face bosses with dedicated boss health bars that raise the stakes of every major fight.

**Exploration & Interaction**: Move between maps through transitions and interact with objects and characters using a flexible interaction system.

## Scene Flow
```mermaid
flowchart LR
  mm[Main Menu]
  gp[Gameplay]
  shop[Shop]
  boss[Boss Fight]
  go[Game Over]

  mm -- "Start Game" --> gp
  gp -- "Interact with Shop" --> shop
  shop -- "Close Shop" --> gp
  gp -- "Map Transition" --> gp
  gp -- "Reach Boss" --> boss
  boss -- "Defeat Boss" --> gp
  boss -- "Death" --> go
  gp -- "Death" --> go
  go -- "Retry" --> gp
  go -- "Main Menu" --> mm
```

## Layer / Module Design
```mermaid
---
config:
  theme: neutral
  look: neo
---
graph TD
    subgraph "Game Settings"
        GM[Game Manager]
        SC[Scene Controller]
        MT[Map Transition]
        AM[Audio Manager]
        BSFX[Button SFX]
        Shop[Shop Manager]
        Shake[Camera Shake]
        PAD[Particle Auto Destroy]
    end

    subgraph "Player Systems"
        PM[Player Movement]
        PA[Player Attack]
        PH[Player Health]
        ID[Interaction Detector]
    end

    subgraph "Enemy Systems"
        Spawner[Enemy Spawner]
        EM[Enemy Movement]
        EA[Enemy Attack]
        EH[Enemy Health]
        KB[Knockback]
    end

    subgraph "Interaction"
        II[IInteractable]
    end

    subgraph "UI Systems"
        MainMenu[Main Menu UI]
        GameUI[Game UI]
        HealthBar[Health Bar UI]
        EnemyBar[Enemy Health Bar UI]
        BossBar[Boss Health Bar UI]
        Coin[Coin Display / Feedback / Popup UI]
        DmgPopup[Damage Popup UI]
        ShopUI[Shop UI]
        GameOver[Game Over UI]
        Gradient[Gradient UI]
    end

    PM --> ID
    ID --> II
    II --> Shop
    II --> MT
    PA --> EH
    PA --> KB
    PA --> Shake
    PA --> AM

    Spawner --> EM
    Spawner --> EA
    EM --> EA
    EA --> PH
    EA --> KB
    KB --> PH
    KB --> EH

    EH --> EnemyBar
    EH --> BossBar
    EH --> DmgPopup
    EH --> Coin
    PH --> HealthBar
    PH --> Shake
    PH --> GM

    GM --> GameOver
    GM --> SC
    SC --> MainMenu
    MT --> SC
    Shop --> ShopUI
    Shop --> Coin
    BSFX --> AM
    Shake --> PAD

    classDef gameStyle fill:#e1f5fe,stroke:#01579b,stroke-width:2px
    classDef playerStyle fill:#e8f5e8,stroke:#1b5e20,stroke-width:2px
    classDef enemyStyle fill:#ffebee,stroke:#b71c1c,stroke-width:2px
    classDef interactStyle fill:#fff3e0,stroke:#e65100,stroke-width:2px
    classDef uiStyle fill:#f3e5f5,stroke:#4a148c,stroke-width:2px

    class GM,SC,MT,AM,BSFX,Shop,Shake,PAD gameStyle
    class PM,PA,PH,ID playerStyle
    class Spawner,EM,EA,EH,KB enemyStyle
    class II interactStyle
    class MainMenu,GameUI,HealthBar,EnemyBar,BossBar,Coin,DmgPopup,ShopUI,GameOver,Gradient uiStyle
```

## Modules and Features

The action-adventure gameplay, enemy AI, shop economy, map transitions, and UI are powered by a modular Unity C# scripting system organized into `Player`, `Enemy`, `GameSettings`, and `UI` folders.

| 📂 Name | 📁 Folder | 📋 Responsibility |
|---------|-----------|-------------------|
| **PlayerMovement** | **Player** | - Handle player movement input<br/>- Control speed and facing direction<br/>- Support animation states |
| **PlayerAttack** | **Player** | - Handle attack input<br/>- Detect and damage enemies in range<br/>- Manage attack cooldown<br/>- Trigger hit effects and sounds |
| **PlayerHealth** | **Player** | - Track player health (current/max)<br/>- Handle damage and invincibility<br/>- Trigger death and game over<br/>- Update health bar |
| **EnemyMovement** | **Enemy** | - Move enemies toward the player<br/>- Handle chase and idle behavior<br/>- Control facing direction |
| **EnemyAttack** | **Enemy** | - Attack the player when in range<br/>- Deal damage and manage cooldown<br/>- Trigger knockback on the player |
| **EnemyHealth** | **Enemy** | - Track enemy health points<br/>- Handle damage from the player<br/>- Update health bars and show damage popups<br/>- Drop coins and clean up on death |
| **GameManager** | **GameSettings** | - Manage overall game state<br/>- Handle player death and game over<br/>- Track coins and progress |
| **SceneController** | **GameSettings** | - Load and reload scenes<br/>- Handle transitions between menu and gameplay |
| **MapTransition** | **GameSettings** | - Move the player between maps<br/>- Handle spawn positions and fade transitions |
| **EnemySpawner** | **GameSettings** | - Spawn enemies in the world<br/>- Control spawn positions, timing, and limits |
| **ShopManager** | **GameSettings** | - Manage shop items and prices<br/>- Handle purchases with coins<br/>- Apply upgrades to the player |
| **InteractionDetector** | **GameSettings** | - Detect nearby interactable objects<br/>- Trigger interaction on player input |
| **IInteractable** | **GameSettings** | - Interface for interactable objects<br/>- Allow shops, doors, and NPCs to share one interaction flow |
| **Knockback** | **GameSettings** | - Push entities away on hit<br/>- Control force and duration |
| **CameraShake** | **GameSettings** | - Shake the camera on impacts<br/>- Control intensity and duration |
| **AudioManager** | **GameSettings** | - Play music and sound effects<br/>- Manage volume and audio clips |
| **ButtonSFX** | **GameSettings** | - Play sound effects on button hover and click |
| **ParticleAutoDestroy** | **GameSettings** | - Destroy particle effects after they finish<br/>- Keep the scene clean |
| **MainMenuUI** | **UI** | - Handle main menu buttons<br/>- Start game, settings, and quit |
| **GameUI** | **UI** | - Manage the in-game HUD<br/>- Coordinate UI elements during gameplay |
| **HealthBarUI** | **UI** | - Display player health<br/>- Update in real time |
| **EnemyHealthBarUI** | **UI** | - Display enemy health bars<br/>- Follow enemies in the world |
| **BossHealthBarUI** | **UI** | - Display the boss health bar<br/>- Show and hide during boss encounters |
| **CoinDisplayUI** | **UI** | - Display the current coin count |
| **CoinFeedbackUI** | **UI** | - Provide visual feedback when coins change |
| **CoinPopupUI** | **UI** | - Show floating coin popups on pickup or reward |
| **DamagePopupUI** | **UI** | - Show floating damage numbers<br/>- Animate and fade out popups |
| **ShopUI** | **UI** | - Display shop items and prices<br/>- Handle purchase buttons |
| **GameOverUI** | **UI** | - Show the game over screen<br/>- Handle retry and quit options |
| **GradientUI** | **UI** | - Apply gradient visuals to UI elements |

<br>

## Game Flow Chart
```mermaid
---
config:
  theme: redux
  look: neo
---
flowchart TD
  start([Game Start])
  start --> move{Player Input}

  move -->|WASD / Arrows| walk[Move Character]
  move -->|Attack Key| attack[Player Attack]
  move -->|Interact Key| interact{Interactable Nearby?}

  attack --> hit{Hits Enemy?}
  hit -->|Yes| dmg[Enemy Takes Damage]
  hit -->|No| move

  dmg --> fx[Knockback + Camera Shake + Damage Popup]
  fx --> dead{Enemy Health = 0?}
  dead -->|Yes| coin[Drop Coins]
  dead -->|No| move
  coin --> move

  interact -->|Shop| shop[Open Shop]
  interact -->|Door / Exit| transition[Map Transition]
  interact -->|None| move

  shop --> buy{Enough Coins?}
  buy -->|Yes| upgrade[Purchase Item]
  buy -->|No| move
  upgrade --> move
  transition --> move

  start --> spawn[Enemy Spawner]
  spawn --> chase[Enemy Moves Toward Player]
  chase --> eattack[Enemy Attacks]
  eattack --> hurt{Player Hit?}

  hurt -->|Yes| pdmg[Player Takes Damage + Knockback]
  hurt -->|No| move

  pdmg --> hp{Health > 0?}
  hp -->|Yes| move
  hp -->|No| gameover[Game Over]

  gameover --> retry{Retry?}
  retry -->|Yes| start
  retry -->|No| menu[Main Menu]

  walk --> bossTrigger{Boss Area?}
  bossTrigger -->|Yes| bossFight[Boss Battle]
  bossTrigger -->|No| move
  bossFight --> bossDead{Boss Health = 0?}
  bossDead -->|Yes| victory[Victory + Rewards]
  bossDead -->|No| bossFight
  victory --> move
```

<br>

## Play The Game
<a href="https://exaltedartix.itch.io/slime-slayer">Play Now</a>

<br>

## Installation & Setup
1. Clone this repository
2. Open the project in Unity (version used in your project recommended)
3. Open the main menu scene
4. Press Play to start testing

## Controls
- **WASD / Arrow Keys** - Movement
- **Attack Key** - Attack
- **Interact Key** - Interact (shop, doors, objects)
- **Escape** - Pause Menu

## Credits
Game developed as a Game Programming Project Individually.

- Engine : Unity
- Assets : Free assets itch.io
