# Assignment 5: Orbital Strike

> Launch energy orbs from a gravity slingshot to destroy alien outposts across three increasingly punishing combat scenarios. Watch out for the satellite.

---

## Play It

**WebGL Build (GitHub Pages):** [INSERT GITHUB PAGES URL HERE]

**Gameplay Video:** [INSERT YOUTUBE/VIMEO URL HERE]

**GitHub Repository:** [https://github.com/acc668/IMG421-Assignment-5/tree/main]

---

## Assignment Rubric — What's Included

### Base Requirements (10 pts)

| Requirement | Implementation |
|-------------|---------------|
| Everything designed in class | Full Mission Demolition prototype preserved and extended |
| Title screen linking to all modes | Menu scene with Easy / Medium / Hard buttons; win and loss screens both return to menu |
| First difficulty mode (Easy) | **Scout Mission** — unlimited shots, aim-assist arc, slow-motion launch |
| Second difficulty mode (Medium) | **Assault Run** — 5 shots per level, random wind force with UI indicator |
| Third difficulty mode (Hard) | **Siege Protocol** — 3 shots, strong wind, moving goal target |

### Extra Credit

| Feature | Points | Implementation |
|---------|--------|---------------|
| Sound Effects | 1 pt | Launch whoosh, impact crash, goal chime, win fanfare, loss buzzer |
| Particle VFX | 1 pt | Energy burst on projectile impact; celebration burst on goal trigger |
| Music | 0.5 pt | Persistent space ambient track across all scenes via DontDestroyOnLoad |
| Stylized GitHub Page | 1 pt | This README — formatted, descriptive, organized, with credits |
| Unlisted extras | up to 0.25 pt each | See "Bonus Features" section below |

---

## Difficulty Modes — In Depth

### Scout Mission (Easy)
The introductory experience. Perfect for learning the slingshot mechanics.
- **Unlimited shots** — fire as many times as you need
- **Aim Assist** — a cyan trajectory arc previews the projectile path before launch
- **Slow Motion** — a brief slow-mo effect plays on every launch so you can watch the physics
- Satellite appears rarely (every 40–70 seconds)

### Assault Run (Medium)
The core challenge. Every shot has consequences.
- **5 shots per level** — displayed in the HUD; failing to hit the goal in time ends the mission
- **Wind Force (2–6 units)** — a random horizontal wind pushes your projectile off course; direction and strength shown by a colour-coded UI indicator that re-rolls before every shot
- Satellite appears occasionally (every 20–40 seconds)

### Siege Protocol (Hard)
Maximum difficulty. Precision required.
- **3 shots per level** — no margin for error
- **Strong Wind (6–12 units)** — dramatically harder to compensate for
- **Moving Goal** — the target oscillates back and forth; you must time your shot
- Satellite appears frequently (every 8–16 seconds)

---

## Bonus Features

These features go beyond the rubric requirements and may qualify for unlisted extra credit:

### Satellite Hazard
A pixel-art satellite randomly flies across the screen. If your projectile hits it, the game immediately ends with a special **"YOU HIT THE SATELLITE! Mission aborted by Space Command."** message. The satellite appears at difficulty-scaled frequencies — rare on Easy, frequent on Hard. Built entirely with procedural sprite generation — no external art assets.

### Procedural Space Background
- **StarField** — randomly placed stars with procedural twinkle animations
- **Planets** — randomly coloured planets with optional rings, generated at runtime using Texture2D pixel drawing
- **Shooting Stars** — periodic shooting stars fly diagonally across the screen
- All visuals generated procedurally in C# — no external sprite files

### Lunar Ground Surface
The ground uses a procedurally generated texture with Perlin noise base variation, randomised crater placement with dark interiors and light rims, and a surface highlight line — all generated at runtime via LunarGround.cs.

### Wind Sound
A looping wind ambience track plays during Medium and Hard modes and stops automatically when the win/loss screen appears.

---

## Repository Structure

```
Assets/
├── _Materials/
│   ├── Mat_Goal.mat
│   ├── Mat_Ground.mat
│   ├── Mat_Projectile.mat
│   ├── Mat_Slingshot.mat
│   └── Mat_Stone.mat
├── _PreFabs/
│   ├── Castle_0.prefab       ← Easy castle
│   ├── Castle_1.prefab       ← Medium castle
│   ├── Castle_2.prefab       ← Hard castle A
│   ├── Castle_3.prefab       ← Hard castle B
│   ├── FX_Impact.prefab      ← Impact particle effect
│   ├── FX_Goal.prefab        ← Goal particle effect
│   ├── Goal.prefab
│   ├── Projectile.prefab
│   ├── ProjectileLine.prefab
│   ├── Slab.prefab
│   └── Wall.prefab
├── Audio/
│   ├── dragon-studio-boom-crash-487664.mp3
│   ├── dragon-studio-simple-whoosh-382724.mp3
│   ├── freemusicforvideo-space-ambient-495614.mp3
│   ├── freesound_community-tada-fanfare-a-6313.mp3
│   ├── ncprime-winds-sound-effects-304060.mp3
│   ├── soundshelfstudio-ui-success-chime-513565.mp3
│   └── universfield-cartoon-fail-trumpet-278822.mp3
├── Scenes/
│   ├── Menu_scene.unity      ← Build Index 0
│   └── _Scene_0.unity        ← Build Index 1
├── Scripts/
│   ├── AimAssist.cs
│   ├── CloudCover.cs
│   ├── DifficultyManager.cs
│   ├── FollowCam.cs
│   ├── Goal.cs
│   ├── GoalVFX.cs
│   ├── ImpactVFX.cs
│   ├── LunarGround.cs
│   ├── MenuManager.cs
│   ├── MissionDemolition.cs
│   ├── MovingGoal.cs
│   ├── Projectile.cs
│   ├── ProjectileLine.cs
│   ├── RigidbodySleep.cs
│   ├── Satellite.cs
│   ├── Slab.cs
│   ├── Slingshot.cs
│   ├── SoundManager.cs
│   ├── SpaceBackground.cs
│   ├── StarField.cs
│   ├── Wall.cs
│   └── WindManager.cs
└── Textures & Sprites/
    ├── cloud1.png
    ├── cloud2.png
    ├── cloud3.png
    ├── cloud4.png
    └── cloud5.png
```

---

## How to Build Locally

1. Clone this repository
2. Open in **Unity 2022.3.x**
3. In **File → Build Settings**, confirm scene order:
   - Index 0: `Scenes/Menu_scene`
   - Index 1: `Scenes/_Scene_0`
4. Switch Platform to **WebGL**
5. Click **Build and Run**

---

## Audio Credits

All audio used under free/royalty-free licenses from Pixabay. No modifications made.

| File | Usage | Credit |
|------|-------|--------|
| `dragon-studio-boom-crash-487664.mp3` | Projectile impact SFX | Dragon Studio via Pixabay |
| `dragon-studio-simple-whoosh-382724.mp3` | Slingshot launch SFX | Dragon Studio via Pixabay |
| `freemusicforvideo-space-ambient-495614.mp3` | Background music | FreeMusicForVideo via Pixabay |
| `freesound_community-tada-fanfare-a-6313.mp3` | Win screen fanfare | Freesound Community via Pixabay |
| `ncprime-winds-sound-effects-304060.mp3` | Wind ambience — Medium/Hard | NCPrime via Pixabay |
| `soundshelfstudio-ui-success-chime-513565.mp3` | Goal achieved chime | SoundShelfStudio via Pixabay |
| `universfield-cartoon-fail-trumpet-278822.mp3` | Loss screen buzzer | Universfield via Pixabay |

---

## Built With

- **Unity 2022.3.62f3**
- **C#** — all scripts written from scratch or modified from the base prototype
- **No external art assets** — all space visuals are procedurally generated at runtime
- Base prototype from: *Introduction to Game Design, Prototyping, and Development* — Jeremy Gibson Bond

---

*Orbital Strike — NAU Game Design Assignment*
