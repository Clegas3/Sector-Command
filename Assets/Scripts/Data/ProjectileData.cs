using UnityEngine;

namespace SectorCommand.Projectiles
{
    /// <summary>
    /// Data definition for projectile types (ScriptableObject for data-driven design)
    /// </summary>
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Sector Command/Projectile Data")]
    public class ProjectileData : ScriptableObject
    {
        [Header("Basic Properties")]
        public string projectileName;
        public string description;
        public ProjectileType type;
        
        [Header("Combat Stats")]
        public float baseDamage = 10f;
        public float baseAccuracy = 0.7f;
        public float areaOfEffect = 1f;
        public float range = 10f;
        
        [Header("Resource Costs")]
        public int energyCost = 10;
        public int materialCost = 5;
        public int turnCooldown = 1;
        
        [Header("Flight Path")]
        public float flightSpeed = 5f;
        public AnimationCurve trajectoryHeight = AnimationCurve.Linear(0, 0, 1, 1);
        public bool allowsCustomPath = false;
        
        [Header("Special Properties")]
        public bool piercing = false;
        public bool tracking = false;
        public float criticalChance = 0.1f;
        
        public enum ProjectileType
        {
            Ballistic,
            Beam,
            Missile,
            Artillery,
            Custom
        }
        
        public float CalculateAccuracy(float distance, float pathQuality)
        {
            float distancePenalty = Mathf.Clamp01(1f - (distance / range));
            float pathBonus = allowsCustomPath ? pathQuality * 0.2f : 0f;
            return Mathf.Clamp01(baseAccuracy * distancePenalty + pathBonus);
        }
        
        public int CalculateTotalCost()
        {
            return energyCost + materialCost;
        }
    }
}
