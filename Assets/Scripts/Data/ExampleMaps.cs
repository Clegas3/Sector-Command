using UnityEngine;

namespace SectorCommand.Data
{
    /// <summary>
    /// Example map configurations
    /// These would be created as ScriptableObject assets in a real Unity project
    /// </summary>
    public static class ExampleMaps
    {
        public static void CreateExampleMaps()
        {
            // Example 1: Training Grounds
            // In Unity Editor: Right-click > Create > Sector Command > Map Data
            /*
            Map Name: "Training Grounds"
            Description: "Open battlefield for learning the basics"
            Width: 10
            Height: 10
            Terrain Layout:
                - Position (2,2), Type: Cover
                - Position (2,3), Type: Cover
                - Position (7,7), Type: Cover
                - Position (7,8), Type: Cover
                - Position (5,5), Type: HighGround
            */
            
            // Example 2: Urban Warfare
            /*
            Map Name: "Urban Conflict"
            Description: "City streets with cover and obstacles"
            Width: 12
            Height: 12
            Terrain Layout:
                - Position (3,3), Type: Obstacle
                - Position (3,4), Type: Obstacle
                - Position (3,5), Type: Obstacle
                - Position (8,2), Type: Cover
                - Position (8,3), Type: Cover
                - Position (2,8), Type: HighGround
                - Position (9,9), Type: HighGround
            */
            
            // Example 3: Mountain Pass
            /*
            Map Name: "Mountain Pass"
            Description: "Narrow passage with high ground advantages"
            Width: 15
            Height: 8
            Terrain Layout:
                - Multiple HighGround positions along edges
                - Obstacles creating a narrow central passage
                - Cover positions near choke points
            */
        }
    }
}
