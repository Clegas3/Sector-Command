using UnityEngine;

namespace SectorCommand.Data
{
    /// <summary>
    /// Data definition for map configurations (ScriptableObject for data-driven design)
    /// </summary>
    [CreateAssetMenu(fileName = "New Map", menuName = "Sector Command/Map Data")]
    public class MapData : ScriptableObject
    {
        [Header("Map Properties")]
        public string mapName;
        public string description;
        public int width = 10;
        public int height = 10;
        
        [Header("Terrain Configuration")]
        public TerrainLayout[] terrainLayout;
        
        [System.Serializable]
        public class TerrainLayout
        {
            public Vector2Int position;
            public Grid.GridSector.SectorType sectorType;
        }
        
        public void ApplyToGrid(Grid.GridManager gridManager)
        {
            if (gridManager == null || terrainLayout == null) return;
            
            foreach (var terrain in terrainLayout)
            {
                Grid.GridSector sector = gridManager.GetSector(terrain.position);
                if (sector != null)
                {
                    sector.sectorType = terrain.sectorType;
                }
            }
        }
    }
}
