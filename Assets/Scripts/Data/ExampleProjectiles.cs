using UnityEngine;

namespace SectorCommand.Data
{
    /// <summary>
    /// Example projectile data configurations
    /// These would be created as ScriptableObject assets in a real Unity project
    /// </summary>
    public static class ExampleProjectiles
    {
        public static void CreateExampleProjectiles()
        {
            // Example 1: Standard Missile
            // In Unity Editor: Right-click > Create > Sector Command > Projectile Data
            // Then configure with these values:
            /*
            Name: "Standard Missile"
            Description: "Reliable guided missile with moderate damage and accuracy"
            Type: Missile
            Base Damage: 25
            Base Accuracy: 0.75
            Area of Effect: 2.0
            Range: 15
            Energy Cost: 15
            Material Cost: 10
            Flight Speed: 5
            Allows Custom Path: false
            Tracking: true
            Critical Chance: 0.1
            */
            
            // Example 2: Artillery Strike
            /*
            Name: "Artillery Strike"
            Description: "High-damage area attack with lower accuracy"
            Type: Artillery
            Base Damage: 50
            Base Accuracy: 0.6
            Area of Effect: 3.5
            Range: 20
            Energy Cost: 30
            Material Cost: 20
            Flight Speed: 3
            Allows Custom Path: true
            Piercing: false
            Critical Chance: 0.15
            */
            
            // Example 3: Laser Beam
            /*
            Name: "Precision Laser"
            Description: "Instant beam weapon with high accuracy, no AOE"
            Type: Beam
            Base Damage: 30
            Base Accuracy: 0.9
            Area of Effect: 0
            Range: 12
            Energy Cost: 25
            Material Cost: 5
            Flight Speed: 100 (instant)
            Allows Custom Path: false
            Tracking: false
            Critical Chance: 0.2
            */
            
            // Example 4: Ballistic Shell
            /*
            Name: "Ballistic Shell"
            Description: "Classic projectile with customizable trajectory"
            Type: Ballistic
            Base Damage: 35
            Base Accuracy: 0.7
            Area of Effect: 1.5
            Range: 18
            Energy Cost: 20
            Material Cost: 15
            Flight Speed: 6
            Allows Custom Path: true
            Piercing: false
            Critical Chance: 0.12
            */
        }
    }
}
