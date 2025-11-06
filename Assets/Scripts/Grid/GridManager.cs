using UnityEngine;
using System.Collections.Generic;

namespace SectorCommand.Grid
{
    /// <summary>
    /// Manages the tactical grid and sector selection
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Configuration")]
        public int gridWidth = 10;
        public int gridHeight = 10;
        public float sectorSize = 1.0f;
        public GameObject sectorPrefab;
        
        [Header("Selection")]
        public GridSector selectedSector;
        public List<GridSector> highlightedSectors = new List<GridSector>();
        
        private GridSector[,] sectors;
        private static GridManager instance;
        
        public static GridManager Instance => instance;
        
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
            InitializeGrid();
        }
        
        private void InitializeGrid()
        {
            sectors = new GridSector[gridWidth, gridHeight];
            
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 position = new Vector3(x * sectorSize, 0, y * sectorSize);
                    GameObject sectorObj;
                    
                    if (sectorPrefab != null)
                    {
                        sectorObj = Instantiate(sectorPrefab, position, Quaternion.identity, transform);
                    }
                    else
                    {
                        sectorObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        sectorObj.transform.position = position;
                        sectorObj.transform.localScale = Vector3.one * sectorSize * 0.1f;
                        sectorObj.transform.parent = transform;
                    }
                    
                    sectorObj.name = $"Sector_{x}_{y}";
                    
                    GridSector sector = sectorObj.GetComponent<GridSector>();
                    if (sector == null)
                    {
                        sector = sectorObj.AddComponent<GridSector>();
                    }
                    
                    sector.gridPosition = new Vector2Int(x, y);
                    sectors[x, y] = sector;
                }
            }
        }
        
        public GridSector GetSector(int x, int y)
        {
            if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
            {
                return sectors[x, y];
            }
            return null;
        }
        
        public GridSector GetSector(Vector2Int position)
        {
            return GetSector(position.x, position.y);
        }
        
        public void SelectSector(GridSector sector)
        {
            if (selectedSector != null)
            {
                selectedSector.SetState(GridSector.SectorState.Normal);
            }
            
            selectedSector = sector;
            if (selectedSector != null)
            {
                selectedSector.SetState(GridSector.SectorState.Selected);
            }
        }
        
        public void HighlightSectorsInRange(Vector2Int center, int range)
        {
            ClearHighlights();
            
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    int distance = Mathf.Abs(x) + Mathf.Abs(y);
                    if (distance <= range)
                    {
                        GridSector sector = GetSector(center.x + x, center.y + y);
                        if (sector != null && sector != selectedSector)
                        {
                            sector.SetState(GridSector.SectorState.Highlighted);
                            highlightedSectors.Add(sector);
                        }
                    }
                }
            }
        }
        
        public void ClearHighlights()
        {
            foreach (var sector in highlightedSectors)
            {
                if (sector != null && sector != selectedSector)
                {
                    sector.SetState(GridSector.SectorState.Normal);
                }
            }
            highlightedSectors.Clear();
        }
        
        public float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y);
        }
        
        public List<GridSector> GetSectorsInRadius(Vector2Int center, float radius)
        {
            List<GridSector> result = new List<GridSector>();
            int range = Mathf.CeilToInt(radius);
            
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    float distance = Mathf.Sqrt(x * x + y * y);
                    if (distance <= radius)
                    {
                        GridSector sector = GetSector(center.x + x, center.y + y);
                        if (sector != null)
                        {
                            result.Add(sector);
                        }
                    }
                }
            }
            
            return result;
        }
    }
}
