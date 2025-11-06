using UnityEngine;
using System.Collections.Generic;

namespace SectorCommand.Scenarios
{
    /// <summary>
    /// Defines a playable scenario with objectives and initial conditions
    /// </summary>
    [CreateAssetMenu(fileName = "New Scenario", menuName = "Sector Command/Scenario")]
    public class ScenarioData : ScriptableObject
    {
        [Header("Scenario Info")]
        public string scenarioName;
        [TextArea(3, 5)]
        public string description;
        public string briefing;
        public int difficulty = 1; // 1-5 scale
        
        [Header("Map & Setup")]
        public Data.MapData mapData;
        public int maxTurns = 10;
        
        [Header("Starting Resources")]
        public int startingEnergy = 100;
        public int startingMaterials = 50;
        public int energyPerTurn = 20;
        public int materialsPerTurn = 10;
        
        [Header("Available Projectiles")]
        public List<Projectiles.ProjectileData> availableProjectiles = new List<Projectiles.ProjectileData>();
        
        [Header("Objectives")]
        public List<Objective> objectives = new List<Objective>();
        
        [Header("Victory Conditions")]
        public VictoryCondition victoryCondition = VictoryCondition.AllObjectivesComplete;
        public int requiredObjectives = 1;
        
        [System.Serializable]
        public class Objective
        {
            public string description;
            public ObjectiveType type;
            public Vector2Int targetSector;
            public int targetValue;
            public bool isComplete;
            
            public enum ObjectiveType
            {
                DestroyTarget,
                DefendSector,
                SurviveTurns,
                AccuracyTest,
                ResourceManagement
            }
        }
        
        public enum VictoryCondition
        {
            AllObjectivesComplete,
            AnyObjectiveComplete,
            MinimumObjectives,
            SurviveAllTurns
        }
        
        public bool CheckVictory()
        {
            int completedCount = 0;
            
            foreach (var objective in objectives)
            {
                if (objective.isComplete)
                {
                    completedCount++;
                }
            }
            
            return victoryCondition switch
            {
                VictoryCondition.AllObjectivesComplete => completedCount == objectives.Count,
                VictoryCondition.AnyObjectiveComplete => completedCount > 0,
                VictoryCondition.MinimumObjectives => completedCount >= requiredObjectives,
                VictoryCondition.SurviveAllTurns => true, // Checked by turn count
                _ => false
            };
        }
    }
}
