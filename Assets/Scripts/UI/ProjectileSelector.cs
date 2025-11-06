using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace SectorCommand.UI
{
    /// <summary>
    /// Manages projectile selection UI
    /// </summary>
    public class ProjectileSelector : MonoBehaviour
    {
        [Header("Projectile Options")]
        public List<Projectiles.ProjectileData> availableProjectiles = new List<Projectiles.ProjectileData>();
        public Projectiles.ProjectileData selectedProjectile;
        
        [Header("UI Elements")]
        public Transform projectileButtonContainer;
        public GameObject projectileButtonPrefab;
        public TextMeshProUGUI descriptionText;
        
        private List<Button> projectileButtons = new List<Button>();
        
        private void Start()
        {
            GenerateProjectileButtons();
            
            if (availableProjectiles.Count > 0)
            {
                SelectProjectile(availableProjectiles[0]);
            }
        }
        
        private void GenerateProjectileButtons()
        {
            if (projectileButtonContainer == null) return;
            
            foreach (var projectileData in availableProjectiles)
            {
                GameObject buttonObj;
                
                if (projectileButtonPrefab != null)
                {
                    buttonObj = Instantiate(projectileButtonPrefab, projectileButtonContainer);
                }
                else
                {
                    buttonObj = new GameObject($"Button_{projectileData.projectileName}");
                    buttonObj.transform.SetParent(projectileButtonContainer);
                    buttonObj.AddComponent<Button>();
                }
                
                Button button = buttonObj.GetComponent<Button>();
                if (button == null)
                {
                    button = buttonObj.AddComponent<Button>();
                }
                
                // Set button text
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText == null)
                {
                    GameObject textObj = new GameObject("Text");
                    textObj.transform.SetParent(buttonObj.transform);
                    buttonText = textObj.AddComponent<TextMeshProUGUI>();
                }
                
                buttonText.text = projectileData.projectileName;
                buttonText.alignment = TextAlignmentOptions.Center;
                
                // Add click listener
                Projectiles.ProjectileData data = projectileData; // Capture for lambda
                button.onClick.AddListener(() => SelectProjectile(data));
                
                projectileButtons.Add(button);
            }
        }
        
        public void SelectProjectile(Projectiles.ProjectileData projectileData)
        {
            selectedProjectile = projectileData;
            
            if (descriptionText != null)
            {
                descriptionText.text = $"{projectileData.projectileName}\n\n{projectileData.description}\n\n" +
                    $"Damage: {projectileData.baseDamage}\n" +
                    $"Range: {projectileData.range}\n" +
                    $"AOE: {projectileData.areaOfEffect}\n" +
                    $"Accuracy: {projectileData.baseAccuracy * 100f:F0}%\n" +
                    $"Cost: {projectileData.energyCost}E / {projectileData.materialCost}M";
            }
            
            UpdateButtonHighlights();
        }
        
        private void UpdateButtonHighlights()
        {
            for (int i = 0; i < projectileButtons.Count && i < availableProjectiles.Count; i++)
            {
                bool isSelected = availableProjectiles[i] == selectedProjectile;
                ColorBlock colors = projectileButtons[i].colors;
                colors.normalColor = isSelected ? Color.green : Color.white;
                projectileButtons[i].colors = colors;
            }
        }
        
        public Projectiles.ProjectileData GetSelectedProjectile()
        {
            return selectedProjectile;
        }
    }
}
