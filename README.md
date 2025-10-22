# Web Zombie Game 

A top-down 3D zombie survival shooter built with **Unity 6** for WebGL, featuring Synty assets, Cinemachine camera control, and clean UI design.  
Your goal: survive as long as you can against endless zombie waves.

---

## Gameplay

- **Movement:** WASD  
- **Aim & Shoot:** Mouse  
- **Objective:** Survive and rack up kills  
- **No ammo limit** — just reflexes and aim  
- **Score system**: Each zombie killed adds to your score  
- **Health bar + damage feedback**  
- **Pause menu** with Resume / Restart / Quit  

---

## Features

- **Cinemachine** top-down orthographic camera
- **Synty** low-poly 3D characters and environments  
- **Zombie AI** with chase, attack, and death states  
- **Player controller** with smooth aiming and shooting  
- **Object pooling** for performance (zombies + tracers)
- **WebGL optimized** (lightweight lighting, no blood FX)
- **Simple UI** using custom panels and buttons (TMP)

---

## Tech Stack

| Tool | Purpose |
|------|----------|
| **Unity 6** | Engine |
| **C#** | Game logic |
| **Cinemachine** | Camera follow system |
| **TextMeshPro (TMP)** | UI text |
| **NavMesh** | Zombie pathfinding |
| **GitHub** | Version control |
| **Itch.io** | Hosting platform |

---

## Build & Run

### 1. Open in Unity
- Requires **Unity 6.x (Built-in Render Pipeline)**
- Open the project folder via Unity Hub

### 2. Build for WebGL
`File → Build Settings → WebGL → Build`

Output folder:  
`/Builds/WebGL/`

### 3. Host to itch.io
Use **butler** CLI:
```bash
butler push Builds/WebGL devdamienkulash/zombie-topdown:html5
