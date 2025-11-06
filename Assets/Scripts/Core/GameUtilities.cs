using UnityEngine;

namespace SectorCommand.Core
{
    /// <summary>
    /// Utility functions for common calculations and operations
    /// </summary>
    public static class GameUtilities
    {
        /// <summary>
        /// Calculate Manhattan distance between two grid positions
        /// </summary>
        public static int ManhattanDistance(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
        
        /// <summary>
        /// Calculate Euclidean distance between two grid positions
        /// </summary>
        public static float EuclideanDistance(Vector2Int a, Vector2Int b)
        {
            int dx = a.x - b.x;
            int dy = a.y - b.y;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }
        
        /// <summary>
        /// Convert grid position to world position
        /// </summary>
        public static Vector3 GridToWorld(Vector2Int gridPos, float sectorSize)
        {
            return new Vector3(gridPos.x * sectorSize, 0, gridPos.y * sectorSize);
        }
        
        /// <summary>
        /// Convert world position to grid position
        /// </summary>
        public static Vector2Int WorldToGrid(Vector3 worldPos, float sectorSize)
        {
            return new Vector2Int(
                Mathf.RoundToInt(worldPos.x / sectorSize),
                Mathf.RoundToInt(worldPos.z / sectorSize)
            );
        }
        
        /// <summary>
        /// Check if grid position is valid within bounds
        /// </summary>
        public static bool IsValidGridPosition(Vector2Int pos, int width, int height)
        {
            return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
        }
        
        /// <summary>
        /// Clamp value with visual feedback
        /// </summary>
        public static float ClampWithFeedback(float value, float min, float max, string warningMessage = null)
        {
            if (value < min || value > max)
            {
                if (!string.IsNullOrEmpty(warningMessage))
                {
                    Debug.LogWarning(warningMessage + $" Value {value} clamped to [{min}, {max}]");
                }
            }
            return Mathf.Clamp(value, min, max);
        }
        
        /// <summary>
        /// Format percentage for display
        /// </summary>
        public static string FormatPercentage(float value, int decimals = 1)
        {
            return (value * 100f).ToString($"F{decimals}") + "%";
        }
        
        /// <summary>
        /// Get color based on percentage value
        /// </summary>
        public static Color GetPercentageColor(float percentage)
        {
            if (percentage >= 0.8f) return Color.green;
            if (percentage >= 0.6f) return Color.yellow;
            if (percentage >= 0.4f) return new Color(1f, 0.5f, 0f); // Orange
            return Color.red;
        }
        
        /// <summary>
        /// Linear interpolation with ease in/out
        /// </summary>
        public static float SmoothLerp(float a, float b, float t)
        {
            t = Mathf.Clamp01(t);
            t = t * t * (3f - 2f * t); // Smoothstep
            return Mathf.Lerp(a, b, t);
        }
        
        /// <summary>
        /// Check if there's line of sight between two grid positions
        /// </summary>
        public static bool HasLineOfSight(Vector2Int from, Vector2Int to, Grid.GridManager gridManager)
        {
            if (gridManager == null) return true;
            
            // Use Bresenham's line algorithm
            int dx = Mathf.Abs(to.x - from.x);
            int dy = Mathf.Abs(to.y - from.y);
            int sx = from.x < to.x ? 1 : -1;
            int sy = from.y < to.y ? 1 : -1;
            int err = dx - dy;
            
            Vector2Int current = from;
            
            while (current != to)
            {
                Grid.GridSector sector = gridManager.GetSector(current);
                if (sector != null && !sector.IsTraversable())
                {
                    return false; // Blocked by obstacle
                }
                
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    current.x += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    current.y += sy;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Get all sectors in a straight line between two points
        /// </summary>
        public static System.Collections.Generic.List<Vector2Int> GetLine(Vector2Int from, Vector2Int to)
        {
            var line = new System.Collections.Generic.List<Vector2Int>();
            
            int dx = Mathf.Abs(to.x - from.x);
            int dy = Mathf.Abs(to.y - from.y);
            int sx = from.x < to.x ? 1 : -1;
            int sy = from.y < to.y ? 1 : -1;
            int err = dx - dy;
            
            Vector2Int current = from;
            
            while (true)
            {
                line.Add(current);
                
                if (current == to) break;
                
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    current.x += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    current.y += sy;
                }
            }
            
            return line;
        }
    }
}
