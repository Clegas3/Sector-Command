using UnityEngine;

namespace SectorCommand.Grid
{
    /// <summary>
    /// Represents a single sector in the tactical grid
    /// </summary>
    public class GridSector : MonoBehaviour
    {
        [Header("Sector Properties")]
        public Vector2Int gridPosition;
        public bool isOccupied;
        public SectorType sectorType;
        
        [Header("Visual")]
        public Color normalColor = Color.white;
        public Color selectedColor = Color.yellow;
        public Color targetColor = Color.red;
        public Color highlightColor = Color.green;
        
        private Renderer sectorRenderer;
        private SectorState currentState;
        
        public enum SectorState
        {
            Normal,
            Selected,
            Targeted,
            Highlighted
        }
        
        public enum SectorType
        {
            Ground,
            Cover,
            Obstacle,
            HighGround
        }
        
        private void Awake()
        {
            sectorRenderer = GetComponent<Renderer>();
        }
        
        public void SetState(SectorState newState)
        {
            currentState = newState;
            UpdateVisual();
        }
        
        private void UpdateVisual()
        {
            if (sectorRenderer == null) return;
            
            Color targetColor = currentState switch
            {
                SectorState.Selected => selectedColor,
                SectorState.Targeted => this.targetColor,
                SectorState.Highlighted => highlightColor,
                _ => normalColor
            };
            
            sectorRenderer.material.color = targetColor;
        }
        
        public float GetCoverBonus()
        {
            return sectorType switch
            {
                SectorType.Cover => 0.2f,
                SectorType.HighGround => 0.15f,
                _ => 0f
            };
        }
        
        public bool IsTraversable()
        {
            return sectorType != SectorType.Obstacle;
        }
    }
}
