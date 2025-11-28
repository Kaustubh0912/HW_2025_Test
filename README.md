# 🎮 Doofus Adventure (HW_2025_Test)

![Unity](https://img.shields.io/badge/Unity-6.0%2B-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-Scripting-blue?style=for-the-badge&logo=c-sharp)
![Status](https://img.shields.io/badge/Status-Active-success?style=for-the-badge)

> **VIT Assignment 2025 - Game Developer Role**

## 📖 Backstory
Meet our character **Doofus**, a cube that loves exploring green platforms called **Pulpits**. The catch? Pulpits don't last long and disappear within seconds. Doofus has set a challenge to walk on at least **50 Pulpits**.

---

## 🎯 Goal
Guide Doofus to walk on as many Pulpits as possible. Be cautious! If Doofus walks off the edge, he falls and the game ends.

---

## ✨ Features & Mechanics

- **🧊 Doofus**: The main character (Cube).
- **🟩 Pulpits**: Green metallic 9x9 platforms.
- **⏳ Dynamic Spawning**: 
  - Only two Pulpits can exist simultaneously.
  - A new Pulpit appears when the timer of the previous one reaches a certain threshold.
  - Pulpits last for a random time between `min_pulpit_destroy_time` and `max_pulpit_destroy_time`.
- **⚙️ JSON Configuration**: Game values (Speed, Timers) are read from `doofus_diary.json`.
- **🏆 Scoring**: Score increases as Doofus successfully walks on new Pulpits.

---

## 📋 Assignment Levels

This project implements the following levels as per the assignment criteria:

- [x] **Level 1**: Character Movement and Platform placements read from the JSON file.
- [x] **Level 2**: Score update after every successful move to a different pulpit.
- [x] **Level 3**: "Start" Screen and "Game Over" Screen.

---

## 🚀 Getting Started

### Prerequisites

- **Unity Hub**
- **Unity Editor** (Unity 6+ Recommended)

### Installation

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/Kaustubh0912/HW_2025_Test.git
    ```
2.  **Open in Unity**:
    - Launch Unity Hub.
    - Click **Add** and select the cloned folder.
    - Open the project.

3.  **Play**:
    - Open the main scene from `Assets/Scenes`.
    - Press the **Play** button in the editor.

---

## 🕹️ Controls

| Action | Key / Input |
| :--- | :--- |
| **Move Forward** | `W` / `Up Arrow` |
| **Move Backward** | `S` / `Down Arrow` |
| **Move Left** | `A` / `Left Arrow` |
| **Move Right** | `D` / `Right Arrow` |

---

## 🔧 Configuration

The game fetches its configuration from a remote JSON file. This allows for real-time balancing updates without patching the game client.

**Configurable Parameters:**
- `player_data.speed`: Movement speed of the player.
- `pulpit_data.min_pulpit_destroy_time`: Minimum time before a platform disappears.
- `pulpit_data.max_pulpit_destroy_time`: Maximum time before a platform disappears.
- `pulpit_data.pulpit_spawn_time`: Interval between new platform spawns.

---

## 📂 Project Structure

```
Assets/
├── Scripts/          # Core game logic (GameManager, PlayerController)
│   ├── Pulpit/       # Platform management logic
│   ├── PowerUps/     # Power-up system
│   └── UI/           # User Interface scripts
├── Prefabs/          # Game objects (Player, Pulpits, PowerUps)
├── Scenes/           # Game levels
└── Settings/         # Input and Project settings
```

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1.  Fork the project
2.  Create your feature branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request

---

Made with ❤️ for Hitwicket

