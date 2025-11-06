using UnityEngine;
using System.Collections.Generic;

namespace SectorCommand.Core
{
    /// <summary>
    /// Handles drawing and managing custom flight paths for projectiles
    /// </summary>
    public class FlightPathDrawer : MonoBehaviour
    {
        [Header("Drawing Settings")]
        public LineRenderer pathRenderer;
        public float minPointDistance = 0.5f;
        public int maxPathPoints = 20;
        public Color pathColor = Color.cyan;
        public float pathWidth = 0.1f;
        
        [Header("Path Quality")]
        public float smoothingFactor = 0.3f;
        public bool autoSmooth = true;
        
        private List<Vector3> currentPath = new List<Vector3>();
        private bool isDrawing = false;
        private Vector3 startPoint;
        private Vector3 endPoint;
        
        private void Awake()
        {
            if (pathRenderer == null)
            {
                pathRenderer = gameObject.AddComponent<LineRenderer>();
            }
            
            ConfigureLineRenderer();
        }
        
        private void ConfigureLineRenderer()
        {
            pathRenderer.startWidth = pathWidth;
            pathRenderer.endWidth = pathWidth;
            pathRenderer.material = new Material(Shader.Find("Sprites/Default"));
            pathRenderer.startColor = pathColor;
            pathRenderer.endColor = pathColor;
            pathRenderer.positionCount = 0;
        }
        
        public void StartDrawing(Vector3 start, Vector3 end)
        {
            startPoint = start;
            endPoint = end;
            isDrawing = true;
            currentPath.Clear();
            currentPath.Add(start);
        }
        
        public void AddPathPoint(Vector3 point)
        {
            if (!isDrawing) return;
            
            // Check if point is far enough from last point
            if (currentPath.Count > 0)
            {
                float distance = Vector3.Distance(currentPath[currentPath.Count - 1], point);
                if (distance < minPointDistance)
                {
                    return;
                }
            }
            
            // Limit path points
            if (currentPath.Count >= maxPathPoints)
            {
                return;
            }
            
            currentPath.Add(point);
            UpdatePathVisual();
        }
        
        public List<Vector3> FinishDrawing()
        {
            if (!isDrawing)
            {
                return null;
            }
            
            isDrawing = false;
            
            // Ensure path ends at target
            if (currentPath.Count == 0)
            {
                currentPath.Add(startPoint);
            }
            
            if (currentPath[currentPath.Count - 1] != endPoint)
            {
                currentPath.Add(endPoint);
            }
            
            if (autoSmooth)
            {
                currentPath = SmoothPath(currentPath);
            }
            
            List<Vector3> finalPath = new List<Vector3>(currentPath);
            ClearPath();
            
            return finalPath;
        }
        
        public void CancelDrawing()
        {
            isDrawing = false;
            ClearPath();
        }
        
        private void ClearPath()
        {
            currentPath.Clear();
            pathRenderer.positionCount = 0;
        }
        
        private void UpdatePathVisual()
        {
            pathRenderer.positionCount = currentPath.Count;
            pathRenderer.SetPositions(currentPath.ToArray());
        }
        
        private List<Vector3> SmoothPath(List<Vector3> path)
        {
            if (path.Count <= 2)
            {
                return path;
            }
            
            List<Vector3> smoothed = new List<Vector3>();
            smoothed.Add(path[0]); // Keep start point
            
            for (int i = 1; i < path.Count - 1; i++)
            {
                Vector3 prev = path[i - 1];
                Vector3 current = path[i];
                Vector3 next = path[i + 1];
                
                Vector3 smoothedPoint = current * (1 - smoothingFactor * 2) + 
                                       prev * smoothingFactor + 
                                       next * smoothingFactor;
                
                smoothed.Add(smoothedPoint);
            }
            
            smoothed.Add(path[path.Count - 1]); // Keep end point
            
            return smoothed;
        }
        
        public float EvaluatePathQuality(List<Vector3> path)
        {
            if (path.Count < 2)
            {
                return 0f;
            }
            
            // Calculate path efficiency
            float directDistance = Vector3.Distance(path[0], path[path.Count - 1]);
            float pathDistance = 0f;
            
            for (int i = 0; i < path.Count - 1; i++)
            {
                pathDistance += Vector3.Distance(path[i], path[i + 1]);
            }
            
            float efficiency = directDistance / Mathf.Max(pathDistance, 0.1f);
            
            // Calculate smoothness (based on angle changes)
            float smoothness = CalculateSmoothness(path);
            
            // Combined quality score
            return (efficiency * 0.6f + smoothness * 0.4f);
        }
        
        private float CalculateSmoothness(List<Vector3> path)
        {
            if (path.Count < 3)
            {
                return 1f;
            }
            
            float totalAngleChange = 0f;
            
            for (int i = 1; i < path.Count - 1; i++)
            {
                Vector3 dir1 = (path[i] - path[i - 1]).normalized;
                Vector3 dir2 = (path[i + 1] - path[i]).normalized;
                
                float angle = Vector3.Angle(dir1, dir2);
                totalAngleChange += angle;
            }
            
            float avgAngleChange = totalAngleChange / (path.Count - 2);
            
            // Lower angle changes = smoother path
            return Mathf.Clamp01(1f - (avgAngleChange / 180f));
        }
    }
}
