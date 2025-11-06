using UnityEngine;
using SectorCommand.Grid;
using System.Collections.Generic;

namespace SectorCommand.Projectiles
{
    /// <summary>
    /// Standard ballistic projectile implementation
    /// </summary>
    public class BallisticProjectile : AbstractProjectile
    {
        [Header("Impact Effects")]
        public GameObject impactEffectPrefab;
        public float explosionRadius = 2f;
        
        protected override void OnImpact()
        {
            // Calculate damage to sectors in area of effect
            Vector2Int impactGrid = WorldToGrid(targetPosition);
            List<GridSector> affectedSectors = GridManager.Instance?.GetSectorsInRadius(impactGrid, data.areaOfEffect);
            
            if (affectedSectors != null)
            {
                foreach (var sector in affectedSectors)
                {
                    ApplyDamageToSector(sector);
                }
            }
            
            // Spawn impact effect
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject, 0.1f);
        }
        
        private void ApplyDamageToSector(GridSector sector)
        {
            float distance = Vector3.Distance(transform.position, sector.transform.position);
            float damageMultiplier = 1f - (distance / data.areaOfEffect);
            float finalDamage = data.baseDamage * Mathf.Max(0, damageMultiplier);
            
            // Apply damage logic here (would interact with unit/structure systems)
            Debug.Log($"Applying {finalDamage} damage to sector {sector.gridPosition}");
        }
        
        private Vector2Int WorldToGrid(Vector3 worldPos)
        {
            if (GridManager.Instance != null)
            {
                float sectorSize = GridManager.Instance.sectorSize;
                return new Vector2Int(
                    Mathf.RoundToInt(worldPos.x / sectorSize),
                    Mathf.RoundToInt(worldPos.z / sectorSize)
                );
            }
            return Vector2Int.zero;
        }
    }
}
