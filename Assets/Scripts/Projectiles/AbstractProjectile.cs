using UnityEngine;
using System.Collections.Generic;

namespace SectorCommand.Projectiles
{
    /// <summary>
    /// Abstract base class for all projectiles with customizable flight paths
    /// </summary>
    public abstract class AbstractProjectile : MonoBehaviour
    {
        [Header("Projectile Configuration")]
        public ProjectileData data;
        
        [Header("Flight Path")]
        public List<Vector3> flightPath = new List<Vector3>();
        public float pathQuality = 1.0f;
        
        protected Vector3 targetPosition;
        protected Vector3 startPosition;
        protected float journeyProgress = 0f;
        protected bool isActive = false;
        protected int currentPathIndex = 0;
        
        public virtual void Initialize(ProjectileData projectileData, Vector3 start, Vector3 target)
        {
            data = projectileData;
            startPosition = start;
            targetPosition = target;
            transform.position = start;
            
            GenerateFlightPath();
        }
        
        protected virtual void GenerateFlightPath()
        {
            flightPath.Clear();
            flightPath.Add(startPosition);
            
            // Generate default path based on projectile type
            switch (data.type)
            {
                case ProjectileData.ProjectileType.Ballistic:
                    GenerateBallisticPath();
                    break;
                case ProjectileData.ProjectileType.Beam:
                    GenerateDirectPath();
                    break;
                case ProjectileData.ProjectileType.Missile:
                    GenerateTrackingPath();
                    break;
                case ProjectileData.ProjectileType.Artillery:
                    GenerateArtilleryPath();
                    break;
                default:
                    GenerateDirectPath();
                    break;
            }
            
            flightPath.Add(targetPosition);
        }
        
        protected virtual void GenerateBallisticPath()
        {
            int segments = 10;
            for (int i = 1; i < segments; i++)
            {
                float t = i / (float)segments;
                Vector3 position = Vector3.Lerp(startPosition, targetPosition, t);
                position.y += data.trajectoryHeight.Evaluate(t) * 5f;
                flightPath.Add(position);
            }
        }
        
        protected virtual void GenerateDirectPath()
        {
            // Beam weapons go straight
            flightPath.Add(Vector3.Lerp(startPosition, targetPosition, 0.5f));
        }
        
        protected virtual void GenerateTrackingPath()
        {
            int segments = 8;
            for (int i = 1; i < segments; i++)
            {
                float t = i / (float)segments;
                Vector3 position = Vector3.Lerp(startPosition, targetPosition, t);
                // Add slight curve for tracking
                position += Vector3.up * Mathf.Sin(t * Mathf.PI) * 2f;
                flightPath.Add(position);
            }
        }
        
        protected virtual void GenerateArtilleryPath()
        {
            int segments = 15;
            for (int i = 1; i < segments; i++)
            {
                float t = i / (float)segments;
                Vector3 position = Vector3.Lerp(startPosition, targetPosition, t);
                position.y += data.trajectoryHeight.Evaluate(t) * 10f; // Higher arc
                flightPath.Add(position);
            }
        }
        
        public virtual void SetCustomPath(List<Vector3> customPath)
        {
            if (data.allowsCustomPath && customPath.Count >= 2)
            {
                flightPath = new List<Vector3>(customPath);
                pathQuality = EvaluatePathQuality(customPath);
            }
        }
        
        protected virtual float EvaluatePathQuality(List<Vector3> path)
        {
            // Evaluate path smoothness and efficiency
            float totalDistance = 0f;
            float directDistance = Vector3.Distance(path[0], path[path.Count - 1]);
            
            for (int i = 0; i < path.Count - 1; i++)
            {
                totalDistance += Vector3.Distance(path[i], path[i + 1]);
            }
            
            float efficiency = directDistance / totalDistance;
            float smoothness = 1.0f; // Could calculate angle changes
            
            return (efficiency + smoothness) * 0.5f;
        }
        
        public virtual void Launch()
        {
            isActive = true;
            currentPathIndex = 0;
            journeyProgress = 0f;
        }
        
        protected virtual void Update()
        {
            if (!isActive || flightPath.Count < 2) return;
            
            UpdatePosition();
        }
        
        protected virtual void UpdatePosition()
        {
            if (currentPathIndex >= flightPath.Count - 1)
            {
                OnReachTarget();
                return;
            }
            
            journeyProgress += Time.deltaTime * data.flightSpeed;
            
            if (journeyProgress >= 1.0f)
            {
                currentPathIndex++;
                journeyProgress = 0f;
                
                if (currentPathIndex >= flightPath.Count - 1)
                {
                    transform.position = flightPath[flightPath.Count - 1];
                    OnReachTarget();
                    return;
                }
            }
            
            transform.position = Vector3.Lerp(
                flightPath[currentPathIndex],
                flightPath[currentPathIndex + 1],
                journeyProgress
            );
        }
        
        protected virtual void OnReachTarget()
        {
            isActive = false;
            OnImpact();
        }
        
        protected abstract void OnImpact();
        
        public virtual float CalculateHitProbability(float distance)
        {
            return data.CalculateAccuracy(distance, pathQuality);
        }
    }
}
