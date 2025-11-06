# Sector Command - Quick Start Guide

## Installation

1. Clone this repository
2. Open the project in Unity 2020.3 LTS or newer
3. Install TextMeshPro if prompted (Window > TextMeshPro > Import TMP Essential Resources)

## Scene Setup

### Minimal Setup (5 minutes)

1. **Create Main Scene**
   - File > New Scene
   - Save as "GameScene"

2. **Add Core Managers**
   ```
   Create Empty GameObject "GridManager"
   - Add Component: Grid/GridManager
   - Set Grid Width: 10
   - Set Grid Height: 10
   - Set Sector Size: 1.0
   ```

   ```
   Create Empty GameObject "GameManager"
   - Add Component: Core/GameManager
   - Set Max Turns: 10
   - Set Starting Energy: 100
   - Set Starting Materials: 50
   ```

   ```
   Create Empty GameObject "InputController"
   - Add Component: Core/PlayerInputController
   ```

   ```
   Create Empty GameObject "ScenarioManager"
   - Add Component: Scenarios/ScenarioManager
   ```

3. **Setup Camera**
   ```
   Main Camera:
   - Position: (5, 10, -5)
   - Rotation: (45, 0, 0)
   - Clear Flags: Solid Color
   - Background: Dark color
   ```

4. **Add Lighting**
   ```
   Create > Light > Directional Light
   - Rotation: (50, -30, 0)
   ```

5. **Create Projectile Data**
   ```
   Right-click in Project > Create > Sector Command > Projectile Data
   Name: "BasicMissile"
   Configure:
   - Projectile Name: "Basic Missile"
   - Type: Missile
   - Base Damage: 25
   - Base Accuracy: 0.75
   - Area of Effect: 2.0
   - Range: 15
   - Energy Cost: 15
   - Material Cost: 10
   - Flight Speed: 5
   - Tracking: true
   ```

6. **Create Map Data**
   ```
   Right-click in Project > Create > Sector Command > Map Data
   Name: "TestMap"
   Configure:
   - Map Name: "Test Map"
   - Width: 10
   - Height: 10
   ```

7. **Create Scenario**
   ```
   Right-click in Project > Create > Sector Command > Scenario
   Name: "TestScenario"
   Configure:
   - Scenario Name: "Test Scenario"
   - Max Turns: 5
   - Starting Energy: 100
   - Starting Materials: 50
   - Drag BasicMissile into Available Projectiles list
   - Drag TestMap into Map Data field
   ```

8. **Connect References**
   ```
   In InputController:
   - Drag Main Camera to Main Camera field
   
   In ScenarioManager:
   - Drag TestScenario to Current Scenario field
   ```

9. **Press Play!**
   - Click on sectors to select
   - Left click origin, then target
   - Actions will be planned
   - Press Space to execute turn

## Adding UI (Optional - 10 more minutes)

### Simple Text Display

1. **Create Canvas**
   ```
   Right-click Hierarchy > UI > Canvas
   - Render Mode: Screen Space - Overlay
   ```

2. **Add Resource Display**
   ```
   Create > UI > Text - TextMeshPro
   Name: "ResourceText"
   - Position it top-left
   - Text: "Energy: 100 | Materials: 50"
   ```

3. **Add Turn Display**
   ```
   Create > UI > Text - TextMeshPro  
   Name: "TurnText"
   - Position it top-center
   - Text: "Turn: 1 | Phase: Planning"
   ```

4. **Add Execute Button**
   ```
   Create > UI > Button - TextMeshPro
   Name: "ExecuteButton"
   - Position it bottom-center
   - Text: "Execute Turn (Space)"
   ```

5. **Setup HUD Component**
   ```
   Add Component to Canvas: UI/GameHUD
   Drag references:
   - Energy Text → ResourceText (or create new)
   - Turn Number Text → TurnText
   - Execute Turn Button → ExecuteButton
   ```

### Complete UI Setup

Follow the same pattern to add:
- Materials text display
- Phase indicator
- Action info panel (shows selected projectile stats)
- Projectile selector buttons
- Cancel action button

## Testing Your Setup

### Test 1: Basic Grid
1. Press Play
2. Grid should appear (10x10 white/gray sectors)
3. Camera should show full grid

**Expected**: Grid visible and sectors colored

### Test 2: Selection
1. Click on a sector
2. It should turn yellow (selected)
3. Surrounding sectors should turn green (in range)

**Expected**: Visual feedback on clicks

### Test 3: Action Planning
1. Click a sector (origin)
2. Click another sector (target)
3. Check Console for "Action planned" message

**Expected**: Action logged in console

### Test 4: Turn Execution
1. Plan 1-2 actions (click origin, then target)
2. Press Space or click Execute button
3. See projectiles spawn and move

**Expected**: Projectiles animate from origin to target

## Common Issues

### "No sectors appear"
- Check GridManager is in scene
- Verify Grid Width/Height > 0
- Check Camera position can see Y=0 plane

### "Can't click sectors"
- Ensure sectors have Collider components (auto-added)
- Check Camera has Physics Raycaster (auto-added to Main Camera)
- Verify PlayerInputController has camera reference

### "Actions don't execute"
- Check GameManager is in scene
- Verify you're in Planning phase
- Ensure you have projectile data assigned

### "No projectile appears"
- Verify ProjectileData asset is created and assigned
- Check scenario has projectiles in Available Projectiles list
- Look for error messages in Console

## Next Steps

1. **Create More Projectiles**
   - Make 3-4 different types
   - Experiment with different stats
   - Add to scenario's available list

2. **Design Maps**
   - Add terrain variations
   - Place obstacles and cover
   - Test different layouts

3. **Build Scenarios**
   - Set objectives
   - Balance resources
   - Create difficulty progression

4. **Customize**
   - Adjust colors in GridSector
   - Modify path drawing settings
   - Tweak accuracy formulas

## Controls Reference

| Action | Input |
|--------|-------|
| Select Origin Sector | Left Click |
| Select Target Sector | Left Click (after origin) |
| Draw Custom Path | Hold Left Click + Drag (if enabled) |
| Cancel Selection | Right Click or Escape |
| Execute Turn | Space or Execute Button |
| Undo Last Action | Z |

## Resources

- **README.md**: Full feature documentation
- **DESIGN.md**: System architecture and design philosophy  
- **Example Scripts**: See Assets/Scripts/Data/Example*.cs for sample configurations

## Getting Help

Check these files for examples:
- `ExampleProjectiles.cs` - Projectile configurations
- `ExampleMaps.cs` - Map layouts
- `ExampleScenarios.cs` - Scenario setups

All core scripts have XML documentation comments explaining their purpose and usage.

---

**You're ready to command!** Start by playing through a simple scenario, then experiment with different projectile types and custom paths.
