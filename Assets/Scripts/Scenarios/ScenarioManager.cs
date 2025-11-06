using UnityEngine;
using UnityEngine.SceneManagement;

namespace SectorCommand.Scenarios
{
    /// <summary>
    /// Manages scenario loading, progression, and completion
    /// </summary>
    public class ScenarioManager : MonoBehaviour
    {
        [Header("Current Scenario")]
        public ScenarioData currentScenario;
        
        [Header("Scenario Results")]
        public bool scenarioComplete = false;
        public bool scenarioWon = false;
        public int finalTurn = 0;
        public float finalAccuracy = 0f;
        
        private static ScenarioManager instance;
        public static ScenarioManager Instance => instance;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void LoadScenario(ScenarioData scenario)
        {
            currentScenario = scenario;
            scenarioComplete = false;
            scenarioWon = false;
            
            InitializeScenario();
        }
        
        private void InitializeScenario()
        {
            if (currentScenario == null) return;
            
            Debug.Log($"Loading scenario: {currentScenario.scenarioName}");
            Debug.Log($"Briefing: {currentScenario.briefing}");
            
            // Configure game manager
            Core.GameManager gameManager = Core.GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.playerEnergy = currentScenario.startingEnergy;
                gameManager.playerMaterials = currentScenario.startingMaterials;
                gameManager.energyPerTurn = currentScenario.energyPerTurn;
                gameManager.materialsPerTurn = currentScenario.materialsPerTurn;
                gameManager.maxTurnsPerScenario = currentScenario.maxTurns;
                gameManager.currentTurn = 0;
            }
            
            // Apply map data
            if (currentScenario.mapData != null)
            {
                Grid.GridManager gridManager = Grid.GridManager.Instance;
                if (gridManager != null)
                {
                    currentScenario.mapData.ApplyToGrid(gridManager);
                }
            }
        }
        
        public void CheckObjectives()
        {
            if (currentScenario == null || scenarioComplete) return;
            
            // Check each objective
            foreach (var objective in currentScenario.objectives)
            {
                CheckObjective(objective);
            }
            
            // Check victory
            if (currentScenario.CheckVictory())
            {
                CompleteScenario(true);
            }
        }
        
        private void CheckObjective(ScenarioData.Objective objective)
        {
            if (objective.isComplete) return;
            
            // Implement objective checking logic based on type
            switch (objective.type)
            {
                case ScenarioData.Objective.ObjectiveType.SurviveTurns:
                    if (Core.GameManager.Instance != null)
                    {
                        objective.isComplete = Core.GameManager.Instance.currentTurn >= objective.targetValue;
                    }
                    break;
                    
                // Other objective types would be implemented here
                default:
                    break;
            }
        }
        
        public void CompleteScenario(bool won)
        {
            scenarioComplete = true;
            scenarioWon = won;
            
            if (Core.GameManager.Instance != null)
            {
                finalTurn = Core.GameManager.Instance.currentTurn;
            }
            
            Debug.Log($"Scenario {(won ? "WON" : "LOST")}!");
            ShowResults();
        }
        
        private void ShowResults()
        {
            string resultText = scenarioWon ? "VICTORY!" : "DEFEAT";
            Debug.Log($"\n=== SCENARIO RESULTS ===\n{resultText}\n" +
                     $"Turns Completed: {finalTurn}/{currentScenario.maxTurns}\n" +
                     $"Objectives Completed: {GetCompletedObjectiveCount()}/{currentScenario.objectives.Count}\n" +
                     "=====================");
        }
        
        private int GetCompletedObjectiveCount()
        {
            if (currentScenario == null) return 0;
            
            int count = 0;
            foreach (var objective in currentScenario.objectives)
            {
                if (objective.isComplete) count++;
            }
            return count;
        }
        
        public void RestartScenario()
        {
            if (currentScenario != null)
            {
                LoadScenario(currentScenario);
            }
        }
        
        public void ReturnToMenu()
        {
            SceneManager.LoadScene(0); // Assumes main menu is scene 0
        }
    }
}
