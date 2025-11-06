using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SectorCommand.UI
{
    /// <summary>
    /// Displays game information including resources, probabilities, and turn state
    /// </summary>
    public class GameHUD : MonoBehaviour
    {
        [Header("Resource Display")]
        public TextMeshProUGUI energyText;
        public TextMeshProUGUI materialsText;
        public TextMeshProUGUI turnNumberText;
        public TextMeshProUGUI phaseText;
        
        [Header("Action Info")]
        public GameObject actionInfoPanel;
        public TextMeshProUGUI projectileNameText;
        public TextMeshProUGUI accuracyText;
        public TextMeshProUGUI damageText;
        public TextMeshProUGUI areaText;
        public TextMeshProUGUI costText;
        
        [Header("Buttons")]
        public Button executeTurnButton;
        public Button cancelActionButton;
        public Button nextTurnButton;
        
        private Core.GameManager gameManager;
        
        private void Start()
        {
            gameManager = Core.GameManager.Instance;
            
            if (actionInfoPanel != null)
            {
                actionInfoPanel.SetActive(false);
            }
            
            SetupButtons();
        }
        
        private void SetupButtons()
        {
            if (executeTurnButton != null)
            {
                executeTurnButton.onClick.AddListener(OnExecuteTurnClicked);
            }
            
            if (cancelActionButton != null)
            {
                cancelActionButton.onClick.AddListener(OnCancelActionClicked);
            }
            
            if (nextTurnButton != null)
            {
                nextTurnButton.onClick.AddListener(OnNextTurnClicked);
                nextTurnButton.gameObject.SetActive(false);
            }
        }
        
        private void Update()
        {
            UpdateResourceDisplay();
            UpdatePhaseDisplay();
            UpdateButtons();
        }
        
        private void UpdateResourceDisplay()
        {
            if (gameManager == null) return;
            
            if (energyText != null)
            {
                energyText.text = $"Energy: {gameManager.playerEnergy}";
            }
            
            if (materialsText != null)
            {
                materialsText.text = $"Materials: {gameManager.playerMaterials}";
            }
            
            if (turnNumberText != null)
            {
                turnNumberText.text = $"Turn: {gameManager.currentTurn}/{gameManager.maxTurnsPerScenario}";
            }
        }
        
        private void UpdatePhaseDisplay()
        {
            if (gameManager == null || phaseText == null) return;
            
            phaseText.text = $"Phase: {gameManager.currentPhase}";
            
            Color phaseColor = gameManager.currentPhase switch
            {
                Core.GameManager.GamePhase.Planning => Color.cyan,
                Core.GameManager.GamePhase.Execution => Color.yellow,
                Core.GameManager.GamePhase.Results => Color.green,
                Core.GameManager.GamePhase.GameOver => Color.red,
                _ => Color.white
            };
            
            phaseText.color = phaseColor;
        }
        
        private void UpdateButtons()
        {
            if (gameManager == null) return;
            
            if (executeTurnButton != null)
            {
                bool canExecute = gameManager.currentPhase == Core.GameManager.GamePhase.Planning && 
                                 gameManager.plannedActions.Count > 0;
                executeTurnButton.interactable = canExecute;
            }
            
            if (cancelActionButton != null)
            {
                bool canCancel = gameManager.currentPhase == Core.GameManager.GamePhase.Planning && 
                                gameManager.plannedActions.Count > 0;
                cancelActionButton.interactable = canCancel;
            }
            
            if (nextTurnButton != null)
            {
                bool showNext = gameManager.currentPhase == Core.GameManager.GamePhase.Results;
                nextTurnButton.gameObject.SetActive(showNext);
            }
        }
        
        public void ShowActionInfo(Projectiles.ProjectileData projectileData, float accuracy, float distance)
        {
            if (actionInfoPanel == null) return;
            
            actionInfoPanel.SetActive(true);
            
            if (projectileNameText != null)
            {
                projectileNameText.text = projectileData.projectileName;
            }
            
            if (accuracyText != null)
            {
                float hitChance = accuracy * 100f;
                accuracyText.text = $"Accuracy: {hitChance:F1}%";
                accuracyText.color = GetAccuracyColor(accuracy);
            }
            
            if (damageText != null)
            {
                damageText.text = $"Damage: {projectileData.baseDamage}";
            }
            
            if (areaText != null)
            {
                areaText.text = $"AOE: {projectileData.areaOfEffect}";
            }
            
            if (costText != null)
            {
                int totalCost = projectileData.CalculateTotalCost();
                costText.text = $"Cost: {projectileData.energyCost}E / {projectileData.materialCost}M";
                
                bool canAfford = gameManager.CanAffordAction(projectileData);
                costText.color = canAfford ? Color.white : Color.red;
            }
        }
        
        public void HideActionInfo()
        {
            if (actionInfoPanel != null)
            {
                actionInfoPanel.SetActive(false);
            }
        }
        
        private Color GetAccuracyColor(float accuracy)
        {
            if (accuracy >= 0.8f) return Color.green;
            if (accuracy >= 0.6f) return Color.yellow;
            if (accuracy >= 0.4f) return new Color(1f, 0.5f, 0f); // Orange
            return Color.red;
        }
        
        private void OnExecuteTurnClicked()
        {
            if (gameManager != null)
            {
                gameManager.ExecuteTurn();
            }
        }
        
        private void OnCancelActionClicked()
        {
            if (gameManager != null)
            {
                gameManager.CancelLastAction();
            }
        }
        
        private void OnNextTurnClicked()
        {
            if (gameManager != null)
            {
                gameManager.StartNewTurn();
            }
        }
    }
}
