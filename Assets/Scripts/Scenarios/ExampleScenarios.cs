using UnityEngine;

namespace SectorCommand.Scenarios
{
    /// <summary>
    /// Example scenario configurations
    /// These would be created as ScriptableObject assets in a real Unity project
    /// </summary>
    public static class ExampleScenarios
    {
        public static void CreateExampleScenarios()
        {
            // Example 1: Basic Training
            // In Unity Editor: Right-click > Create > Sector Command > Scenario
            /*
            Scenario Name: "Basic Training"
            Description: "Learn the fundamentals of Sector Command"
            Briefing: "Welcome, Commander. Complete your training by successfully striking the designated targets. Pay attention to accuracy probabilities and resource management."
            Difficulty: 1
            Map Data: Training Grounds
            Max Turns: 5
            Starting Energy: 100
            Starting Materials: 50
            Energy Per Turn: 30
            Materials Per Turn: 15
            
            Available Projectiles:
                - Standard Missile
                - Ballistic Shell
            
            Objectives:
                1. Type: DestroyTarget, Description: "Hit target sector (5,5)", Target Sector: (5,5)
                2. Type: SurviveTurns, Description: "Complete the scenario", Target Value: 3
            
            Victory Condition: AllObjectivesComplete
            */
            
            // Example 2: Precision Strike
            /*
            Scenario Name: "Precision Strike"
            Description: "Test your accuracy with limited resources"
            Briefing: "Intel has located high-value targets. You have limited munitions - make every shot count."
            Difficulty: 3
            Map Data: Urban Conflict
            Max Turns: 3
            Starting Energy: 75
            Starting Materials: 40
            Energy Per Turn: 10
            Materials Per Turn: 5
            
            Available Projectiles:
                - Precision Laser
                - Standard Missile
            
            Objectives:
                1. Type: DestroyTarget, Description: "Eliminate primary target"
                2. Type: AccuracyTest, Description: "Maintain 80%+ accuracy", Target Value: 80
            
            Victory Condition: AllObjectivesComplete
            */
            
            // Example 3: Artillery Barrage
            /*
            Scenario Name: "Artillery Barrage"
            Description: "Devastate enemy positions with heavy ordnance"
            Briefing: "Enemy forces are entrenched. Use artillery to clear the area."
            Difficulty: 4
            Map Data: Mountain Pass
            Max Turns: 8
            Starting Energy: 150
            Starting Materials: 80
            Energy Per Turn: 25
            Materials Per Turn: 15
            
            Available Projectiles:
                - Artillery Strike
                - Ballistic Shell
                - Standard Missile
            
            Objectives:
                1. Type: DestroyTarget, Description: "Clear sector A"
                2. Type: DestroyTarget, Description: "Clear sector B"
                3. Type: ResourceManagement, Description: "Complete with 50+ resources remaining"
            
            Victory Condition: MinimumObjectives
            Required Objectives: 2
            */
        }
    }
}
