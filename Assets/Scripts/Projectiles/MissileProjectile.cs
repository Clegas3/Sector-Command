using UnityEngine;
using System.Collections.Generic;

namespace SectorCommand.Projectiles
{
    /// <summary>
    /// Guided missile with tracking capabilities
    /// </summary>
    public class MissileProjectile : AbstractProjectile
    {
        [Header("Missile Properties")]
        public GameObject explosionEffectPrefab;
        public float trackingStrength = 0.5f;
        public bool canRetarget = true;
        
        private Vector3 currentTarget;
        
        public override void Initialize(ProjectileData projectileData, Vector3 start, Vector3 target)
        {
            base.Initialize(projectileData, start, target);
            currentTarget = target;
        }
        
        protected override void UpdatePosition()
        {
            // Update tracking if enabled
            if (data.tracking && canRetarget)
            {
                UpdateTracking();
            }
            
            base.UpdatePosition();
        }
        
        private void UpdateTracking()
        {
            // Adjust flight path toward target
            if (currentPathIndex < flightPath.Count - 1)
            {
                Vector3 directionToTarget = (currentTarget - transform.position).normalized;
                Vector3 currentDirection = (flightPath[currentPathIndex + 1] - transform.position).normalized;
                
                Vector3 adjustedDirection = Vector3.Lerp(currentDirection, directionToTarget, trackingStrength * Time.deltaTime);
                flightPath[currentPathIndex + 1] = transform.position + adjustedDirection * 2f;
            }
        }
        
        protected override void OnImpact()
        {
            float distance = Vector3.Distance(startPosition, targetPosition);
            float hitChance = CalculateHitProbability(distance);
            
            // Tracking improves hit chance
            if (data.tracking)
            {
                hitChance = Mathf.Min(hitChance + 0.15f, 0.95f);
            }
            
            bool hit = Random.value <= hitChance;
            
            if (hit)
            {
                Debug.Log($"Missile struck target! Damage: {data.baseDamage}, AOE: {data.areaOfEffect}");
            }
            else
            {
                Debug.Log("Missile missed target!");
            }
            
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject, 0.1f);
        }
    }
}
