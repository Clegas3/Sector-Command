# Sector Command - Design Document

## Game Concept

Sector Command is a grid-based tactical strategy game that emphasizes player choice, probability management, and creative problem-solving through customizable projectile trajectories.

## Core Pillars

### 1. Strategic Depth
- Resource management (Energy and Materials)
- Multiple projectile types with distinct trade-offs
- Terrain considerations (cover, obstacles, high ground)
- Turn-based planning allows thoughtful decision-making

### 2. Tactical Execution
- Real-time resolution phase for dramatic impact
- Custom flight paths influence outcomes
- Probability-based combat with clear feedback
- Risk vs. reward in every decision

### 3. Player Expression
- Draw custom trajectories for applicable weapons
- Choose between accuracy, damage, and resource efficiency
- Multiple valid approaches to each scenario
- Replayability through different strategies

## Game Loop

```
┌─────────────────────────────────────┐
│      PLANNING PHASE                 │
│  - Select projectiles               │
│  - Choose targets                   │
│  - Draw flight paths (optional)     │
│  - Review probabilities             │
│  - Queue multiple actions           │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│      EXECUTION PHASE                │
│  - Real-time projectile launches    │
│  - Visual trajectory animations     │
│  - Impact resolution                │
│  - Damage calculation               │
└──────────────┬──────────────────────┘
               │
               ▼
┌─────────────────────────────────────┐
│      RESULTS PHASE                  │
│  - Objective progress               │
│  - Resource regeneration            │
│  - Turn advancement                 │
└──────────────┬──────────────────────┘
               │
               └──────────────────────► (Loop back to Planning)
```

## Projectile System Design

### Abstract Projectile Hierarchy

```
AbstractProjectile (base class)
├── Core mechanics: flight path, launch, impact
├── Path generation algorithms
├── Accuracy calculation
└── Path quality evaluation

Concrete Implementations:
├── BallisticProjectile
│   └── Arc trajectory, area damage, medium accuracy
├── BeamProjectile
│   └── Instant hit, high accuracy, no AOE
├── MissileProjectile
│   └── Tracking, guided path, improved hit chance
└── ArtilleryProjectile
    └── High arc, long range, large AOE, lower accuracy
```

### Customization Levels

1. **Fixed Path** (Beams, basic missiles)
   - Predictable, reliable
   - Higher base accuracy
   - Lower resource costs

2. **Custom Path Allowed** (Artillery, advanced ballistics)
   - Player-drawn trajectories
   - Path quality affects accuracy (+/- 20%)
   - Higher skill ceiling
   - Potential for creative solutions

## Probability System

### Accuracy Formula

```
Base Accuracy = Projectile.baseAccuracy

Distance Modifier = 1.0 - (actual_distance / max_range)
  - At 0% of range: 100% modifier
  - At 50% of range: 50% modifier
  - At 100% of range: 0% modifier

Path Quality Bonus = (if custom path allowed)
  - Efficiency: directDistance / pathDistance
  - Smoothness: angle consistency
  - Combined: (efficiency * 0.6 + smoothness * 0.4)
  - Max bonus: +20%

Cover Penalty = target.GetCoverBonus()
  - Cover: +20% defense
  - High Ground: +15% defense

Final Hit Chance = Base × DistanceMod + PathBonus - CoverPenalty
```

### Visual Feedback

Hit probability displayed as:
- **80-100%**: Green (excellent)
- **60-79%**: Yellow (good)
- **40-59%**: Orange (risky)
- **0-39%**: Red (poor)

## Resource Economy

### Starting Resources (configurable per scenario)
- Energy: 100 (typical starting value)
- Materials: 50 (typical starting value)

### Per-Turn Regeneration
- Energy: +20-30 per turn
- Materials: +10-15 per turn

### Projectile Costs (examples)
- Beam: 25 Energy, 5 Materials (precision tool)
- Missile: 15 Energy, 10 Materials (balanced)
- Ballistic: 20 Energy, 15 Materials (versatile)
- Artillery: 30 Energy, 20 Materials (heavy hitter)

### Strategic Implications
- Can't spam expensive weapons
- Must balance damage output with sustainability
- Encourages mixing projectile types
- Resource management = key skill

## Terrain System

### Sector Types

1. **Ground** (default)
   - No bonuses or penalties
   - Fully traversable

2. **Cover**
   - +20% defensive bonus
   - Still traversable
   - Strategic positioning

3. **High Ground**
   - +15% defensive bonus
   - +10% accuracy when firing from
   - Tactical advantage

4. **Obstacle**
   - Blocks line of sight
   - Not traversable
   - Creates tactical challenges

## Scenario Design

### Scenario Components

1. **Map**: Defines grid size and terrain layout
2. **Resources**: Starting amounts and regeneration rates
3. **Turn Limit**: Maximum turns to complete objectives
4. **Available Projectiles**: Which weapons can be used
5. **Objectives**: Win/loss conditions
6. **Difficulty**: Scaling factor (1-5)

### Objective Types

- **Destroy Target**: Hit specific sector(s)
- **Survive Turns**: Last X turns
- **Accuracy Test**: Maintain Y% hit rate
- **Resource Management**: Finish with Z resources
- **Defend Sector**: Keep sector undamaged

### Victory Conditions

- All Objectives Complete
- Any Objective Complete
- Minimum Objectives (X out of Y)
- Survive All Turns

## UI/UX Design Principles

### Clarity First
- Always show hit probabilities before committing
- Display resource costs prominently
- Color-code critical information
- Clear phase indicators

### Minimal Friction
- Intuitive controls (click to select)
- Easy to undo/cancel actions
- Quick path drawing
- Keyboard shortcuts for efficiency

### Informative Feedback
- Show why accuracy is high/low
- Explain resource costs
- Visualize trajectories in real-time
- Clear success/failure indicators

## Balancing Philosophy

### Data-Driven Approach
- All values in ScriptableObjects
- No hard-coded balance numbers in scripts
- Easy iteration without recompilation

### Balance Targets

**Projectile Balance**:
- No "always best" option
- Each type has situational advantages
- Cost reflects power level
- Risk/reward trade-offs

**Scenario Balance**:
- Difficulty 1: Tutorial, impossible to fail
- Difficulty 2-3: Requires basic strategy
- Difficulty 4-5: Demands optimization
- All difficulties completable with different approaches

### Testing Metrics
- Average resources remaining at victory
- Most/least used projectile types
- Average accuracy per weapon
- Turn efficiency (complete sooner = better)

## Technical Architecture

### Key Design Patterns

1. **Singleton Managers**
   - GridManager, GameManager, ScenarioManager
   - Global access where needed
   - Clear ownership of systems

2. **ScriptableObject Data**
   - Separation of data and logic
   - Easy to version control
   - Designer-friendly

3. **Inheritance Hierarchy**
   - AbstractProjectile base
   - Shared behavior in base
   - Specialized behavior in derived
   - Open/closed principle

4. **Component-Based**
   - Unity component model
   - Modular, testable
   - Easy to extend

### Extensibility Points

- New projectile types: Inherit from AbstractProjectile
- New terrain types: Add to SectorType enum
- New objectives: Add to Objective.ObjectiveType
- New UI elements: Extend existing UI scripts
- New scenarios: Create ScriptableObject assets

## Future Enhancements (Post-Prototype)

### Short Term
- Sound effects and music
- Particle effects for impacts
- Better visual feedback for paths
- Tutorial scenario with instructions
- Multiple save slots

### Medium Term
- Unit deployment system
- Destructible terrain
- Projectile modifications/upgrades
- Campaign with story
- Achievement system

### Long Term
- Multiplayer (asynchronous turns)
- Map editor
- Steam Workshop integration
- Mobile version
- Spectator/replay mode with commentary

## Success Metrics (for prototype)

✓ Can select sectors and plan actions
✓ Projectiles launch and resolve correctly
✓ Accuracy probabilities display accurately
✓ Resources tracked correctly
✓ Custom paths influence outcomes
✓ Scenarios load and objectives track
✓ All core systems integrated and functional
✓ Code is modular and extensible

---

This design document serves as the blueprint for the Sector Command prototype, ensuring all systems work together to create a cohesive tactical experience.
