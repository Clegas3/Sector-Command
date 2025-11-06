# Sector Command

A Unity C# prototype for a grid-based tactical strategy game where players select sectors, customize abstract projectile classes, and draw stylized flight paths that influence accuracy, area of effect, and timing.

## Overview

Sector Command is a turn-based tactical game that combines strategic planning with real-time execution. Players manage resources, select projectile types, and plan custom flight paths to achieve objectives across various scenarios.

## Core Features

### 🎯 Grid-Based Tactical System
- **Sector Selection**: Interactive grid system with different terrain types (Ground, Cover, Obstacle, High Ground)
- **Visual Feedback**: Color-coded sectors showing selection states, targets, and valid ranges
- **Terrain Effects**: Cover bonuses and line-of-sight considerations

### 🚀 Projectile System
- **Abstract Base Class**: Modular projectile system with customizable behaviors
- **Multiple Types**:
  - **Ballistic**: Arc-based trajectory with area damage
  - **Beam**: Instant hit with high accuracy
  - **Missile**: Guided tracking with improved hit chance
  - **Artillery**: High-arc long-range with large AOE
- **Data-Driven Design**: ScriptableObject-based projectile definitions for easy balancing

### ✏️ Flight Path Drawing
- **Custom Trajectories**: Draw custom flight paths for applicable projectiles
- **Path Quality System**: Evaluates path efficiency and smoothness
- **Accuracy Modifiers**: Better paths improve hit probability
- **Visual Path Preview**: Real-time line rendering of planned trajectories

### 🎮 Turn-Based Planning & Real-Time Resolution
- **Planning Phase**: Queue multiple actions, manage resources
- **Execution Phase**: Watch actions resolve in real-time
- **Results Phase**: Review outcomes and prepare next turn
- **Resource Management**: Energy and materials with per-turn regeneration

### 📊 Probability & UI Systems
- **Hit Probability Display**: Clear percentage-based accuracy indicators
- **Resource Tracking**: Real-time energy and material displays
- **Cost Preview**: See action costs before committing
- **Color-Coded Feedback**: Visual indicators for accuracy quality (green=high, red=low)

### 🎪 Scenario System
- **Replayable Scenarios**: Pre-configured missions with objectives
- **Victory Conditions**: Multiple win/loss conditions
- **Difficulty Scaling**: Adjustable challenge levels
- **Objective Tracking**: Various objective types (destroy, survive, accuracy tests)

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs              # Turn management and game state
│   │   ├── FlightPathDrawer.cs         # Custom path drawing system
│   │   └── PlayerInputController.cs    # Input handling and action planning
│   ├── Grid/
│   │   ├── GridSector.cs               # Individual sector behavior
│   │   └── GridManager.cs              # Grid generation and management
│   ├── Projectiles/
│   │   ├── AbstractProjectile.cs       # Base projectile class
│   │   ├── BallisticProjectile.cs      # Ballistic implementation
│   │   ├── BeamProjectile.cs           # Beam weapon implementation
│   │   └── MissileProjectile.cs        # Guided missile implementation
│   ├── Data/
│   │   ├── ProjectileData.cs           # Projectile ScriptableObject
│   │   ├── MapData.cs                  # Map configuration data
│   │   ├── ExampleProjectiles.cs       # Sample projectile configs
│   │   └── ExampleMaps.cs              # Sample map layouts
│   ├── UI/
│   │   ├── GameHUD.cs                  # Main HUD display
│   │   └── ProjectileSelector.cs       # Projectile selection UI
│   └── Scenarios/
│       ├── ScenarioData.cs             # Scenario configuration
│       ├── ScenarioManager.cs          # Scenario loading and tracking
│       └── ExampleScenarios.cs         # Sample scenario configs
```

## Getting Started

### Prerequisites
- Unity 2020.3 LTS or newer
- TextMeshPro package (for UI text)

### Setup in Unity

1. **Create a new Unity project** or open this repository in Unity

2. **Create ScriptableObject Assets**:
   ```
   Right-click in Project window:
   - Create > Sector Command > Projectile Data (create 3-4 different types)
   - Create > Sector Command > Map Data
   - Create > Sector Command > Scenario
   ```

3. **Set up a basic scene**:
   - Create an empty GameObject, add `GridManager` component
   - Create an empty GameObject, add `GameManager` component
   - Create an empty GameObject, add `PlayerInputController` component
   - Create an empty GameObject, add `ScenarioManager` component
   - Set up a camera at appropriate position (e.g., position: 5,10,-5, rotation: 45,0,0)

4. **Configure UI** (optional but recommended):
   - Create Canvas with `GameHUD` component
   - Add TextMeshPro elements for resources, turn info, phase display
   - Add buttons for Execute Turn, Cancel Action, Next Turn
   - Create `ProjectileSelector` UI panel

5. **Assign References**:
   - Link UI elements in `GameHUD`
   - Link projectile data assets in `ProjectileSelector`
   - Set camera reference in `PlayerInputController`

### Basic Controls

- **Left Click**: Select origin sector, then target sector
- **Mouse Drag**: Draw custom flight path (for applicable projectiles)
- **Right Click**: Cancel current selection
- **Space**: Execute planned turn
- **Z**: Undo last planned action
- **Escape**: Cancel selection

## Gameplay Flow

1. **Planning Phase**:
   - Select a projectile type from available options
   - Click origin sector (your launch position)
   - Click target sector
   - Optionally draw custom flight path
   - Review accuracy probability and costs
   - Repeat to queue multiple actions

2. **Execution Phase**:
   - Press Execute Turn or Space
   - Watch projectiles launch and resolve
   - Automatic phase transition

3. **Results Phase**:
   - Review damage dealt and objectives
   - Check remaining resources
   - Automatically advance to next turn

## Data-Driven Balancing

All projectiles, maps, and scenarios use ScriptableObjects, making it easy to:
- Tweak damage, accuracy, costs without code changes
- Create new projectile types by duplicating and modifying
- Design new scenarios with different objectives and resources
- Balance gameplay through Unity Inspector

### Example Projectile Configuration

```csharp
Name: "Standard Missile"
Base Damage: 25
Base Accuracy: 0.75
Area of Effect: 2.0
Range: 15
Energy Cost: 15
Material Cost: 10
Flight Speed: 5
Allows Custom Path: false
Tracking: true
```

## Key Systems Explained

### Accuracy Calculation
```
Final Accuracy = Base Accuracy × Distance Penalty + Path Quality Bonus
- Distance Penalty: Decreases with range (0% at max range, 100% at close range)
- Path Quality Bonus: +20% max for excellent custom paths (if allowed)
```

### Resource System
- **Energy**: Used for firing projectiles, regenerates per turn
- **Materials**: Physical ammunition costs, regenerates per turn
- Each projectile has configurable costs
- Cannot fire if insufficient resources

### Path Quality Evaluation
- **Efficiency**: Direct distance / actual path distance
- **Smoothness**: Based on angle changes between path segments
- Better paths = higher accuracy bonus

## Extending the System

### Adding New Projectile Types

1. Create new class inheriting from `AbstractProjectile`
2. Override `OnImpact()` for custom behavior
3. Optionally override `GenerateFlightPath()` for unique trajectories
4. Create corresponding ProjectileData asset

### Creating Custom Scenarios

1. Create new ScenarioData ScriptableObject
2. Configure objectives, resources, turn limits
3. Link to appropriate MapData
4. Add to scenario selection menu

### Custom Objective Types

Add new objective types in `ScenarioData.Objective.ObjectiveType` enum and implement checking logic in `ScenarioManager.CheckObjective()`.

## Technical Details

- **Language**: C# 
- **Engine**: Unity (2020.3+)
- **Architecture**: Component-based with ScriptableObject data
- **Design Patterns**: Singleton managers, inheritance hierarchy, data-driven design
- **Namespaces**: Organized by feature (Core, Grid, Projectiles, UI, Scenarios, Data)

## Future Enhancement Ideas

- Multiplayer support (turn-based PvP)
- Unit system (defend/deploy units on sectors)
- Destructible terrain
- Projectile upgrades and modifications
- Campaign mode with progression
- Save/load system
- Replay system with different camera angles
- Sound effects and visual polish
- Mobile touch controls

## License

This is a prototype project for educational and demonstration purposes.

## Contributing

This is a prototype project. Feel free to fork and extend for your own purposes.

---

**Sector Command** - Strategic depth meets tactical precision.
