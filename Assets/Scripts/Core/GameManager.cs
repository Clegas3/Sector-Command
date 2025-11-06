using UnityEngine;
using System.Collections.Generic;

namespace SectorCommand.Core
{
    /// <summary>
    /// Manages turn-based planning and real-time resolution phases
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Game State")]
        public GamePhase currentPhase = GamePhase.Planning;
        public int currentTurn = 1;
        public int maxTurnsPerScenario = 10;
        
        [Header("Player Resources")]
        public int playerEnergy = 100;
        public int playerMaterials = 50;
        public int energyPerTurn = 20;
        public int materialsPerTurn = 10;
        
        [Header("Actions Queue")]
        public List<PlannedAction> plannedActions = new List<PlannedAction>();
        
        private static GameManager instance;
        public static GameManager Instance => instance;
        
        public enum GamePhase
        {
            Planning,
            Execution,
            Results,
            GameOver
        }
        
        [System.Serializable]
        public class PlannedAction
        {
            public Vector2Int origin;
            public Vector2Int target;
            public Projectiles.ProjectileData projectileData;
            public List<Vector3> customPath;
            public float estimatedAccuracy;
            public int resourceCost;
        }
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            StartNewTurn();
        }
        
        public void StartNewTurn()
        {
            currentTurn++;
            currentPhase = GamePhase.Planning;
            
            // Replenish resources
            playerEnergy = Mathf.Min(playerEnergy + energyPerTurn, 200);
            playerMaterials = Mathf.Min(playerMaterials + materialsPerTurn, 100);
            
            plannedActions.Clear();
            
            Debug.Log($"Turn {currentTurn} started. Energy: {playerEnergy}, Materials: {playerMaterials}");
            
            if (currentTurn > maxTurnsPerScenario)
            {
                EndScenario();
            }
        }
        
        public bool PlanAction(PlannedAction action)
        {
            if (currentPhase != GamePhase.Planning)
            {
                Debug.LogWarning("Cannot plan actions outside of planning phase!");
                return false;
            }
            
            // Check resources
            int totalCost = action.projectileData.CalculateTotalCost();
            
            if (playerEnergy < action.projectileData.energyCost)
            {
                Debug.LogWarning("Insufficient energy!");
                return false;
            }
            
            if (playerMaterials < action.projectileData.materialCost)
            {
                Debug.LogWarning("Insufficient materials!");
                return false;
            }
            
            // Deduct resources
            playerEnergy -= action.projectileData.energyCost;
            playerMaterials -= action.projectileData.materialCost;
            
            action.resourceCost = totalCost;
            plannedActions.Add(action);
            
            Debug.Log($"Action planned: {action.projectileData.projectileName} from {action.origin} to {action.target}");
            return true;
        }
        
        public void ExecuteTurn()
        {
            if (currentPhase != GamePhase.Planning)
            {
                Debug.LogWarning("Cannot execute turn from current phase!");
                return;
            }
            
            currentPhase = GamePhase.Execution;
            Debug.Log($"Executing {plannedActions.Count} planned actions...");
            
            StartCoroutine(ExecuteActions());
        }
        
        private System.Collections.IEnumerator ExecuteActions()
        {
            foreach (var action in plannedActions)
            {
                ExecuteAction(action);
                yield return new WaitForSeconds(1.5f); // Delay between actions
            }
            
            yield return new WaitForSeconds(1f);
            ShowResults();
        }
        
        private void ExecuteAction(PlannedAction action)
        {
            // Create and launch projectile
            GameObject projectileObj = new GameObject($"Projectile_{action.projectileData.projectileName}");
            
            Projectiles.AbstractProjectile projectile = null;
            
            switch (action.projectileData.type)
            {
                case Projectiles.ProjectileData.ProjectileType.Ballistic:
                    projectile = projectileObj.AddComponent<Projectiles.BallisticProjectile>();
                    break;
                case Projectiles.ProjectileData.ProjectileType.Beam:
                    projectile = projectileObj.AddComponent<Projectiles.BeamProjectile>();
                    break;
                case Projectiles.ProjectileData.ProjectileType.Missile:
                    projectile = projectileObj.AddComponent<Projectiles.MissileProjectile>();
                    break;
                default:
                    projectile = projectileObj.AddComponent<Projectiles.BallisticProjectile>();
                    break;
            }
            
            Vector3 startPos = GridToWorld(action.origin);
            Vector3 targetPos = GridToWorld(action.target);
            
            projectile.Initialize(action.projectileData, startPos, targetPos);
            
            if (action.customPath != null && action.customPath.Count > 0)
            {
                projectile.SetCustomPath(action.customPath);
            }
            
            projectile.Launch();
        }
        
        private Vector3 GridToWorld(Vector2Int gridPos)
        {
            if (Grid.GridManager.Instance != null)
            {
                float size = Grid.GridManager.Instance.sectorSize;
                return new Vector3(gridPos.x * size, 0, gridPos.y * size);
            }
            return new Vector3(gridPos.x, 0, gridPos.y);
        }
        
        private void ShowResults()
        {
            currentPhase = GamePhase.Results;
            Debug.Log("Turn execution complete. Review results.");
            
            // Auto-advance to next turn after delay
            Invoke(nameof(StartNewTurn), 3f);
        }
        
        public void EndScenario()
        {
            currentPhase = GamePhase.GameOver;
            Debug.Log($"Scenario complete! Final turn: {currentTurn}");
        }
        
        public bool CanAffordAction(Projectiles.ProjectileData projectileData)
        {
            return playerEnergy >= projectileData.energyCost && 
                   playerMaterials >= projectileData.materialCost;
        }
        
        public void CancelLastAction()
        {
            if (plannedActions.Count > 0)
            {
                PlannedAction lastAction = plannedActions[plannedActions.Count - 1];
                
                // Refund resources
                playerEnergy += lastAction.projectileData.energyCost;
                playerMaterials += lastAction.projectileData.materialCost;
                
                plannedActions.RemoveAt(plannedActions.Count - 1);
                Debug.Log("Last action cancelled and resources refunded.");
            }
        }
    }
}
