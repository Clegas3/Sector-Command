using UnityEngine;

namespace SectorCommand.Projectiles
{
    /// <summary>
    /// Beam weapon projectile - instant hit with visual effect
    /// </summary>
    public class BeamProjectile : AbstractProjectile
    {
        [Header("Beam Properties")]
        public LineRenderer beamRenderer;
        public float beamDuration = 0.5f;
        public float beamWidth = 0.1f;
        
        private float beamTimer = 0f;
        
        public override void Launch()
        {
            base.Launch();
            
            if (beamRenderer == null)
            {
                beamRenderer = gameObject.AddComponent<LineRenderer>();
            }
            
            beamRenderer.startWidth = beamWidth;
            beamRenderer.endWidth = beamWidth;
            beamRenderer.positionCount = 2;
            beamRenderer.SetPosition(0, startPosition);
            beamRenderer.SetPosition(1, targetPosition);
            
            // Instant hit for beam weapons
            OnReachTarget();
        }
        
        protected override void Update()
        {
            if (beamRenderer != null && beamRenderer.enabled)
            {
                beamTimer += Time.deltaTime;
                if (beamTimer >= beamDuration)
                {
                    beamRenderer.enabled = false;
                    Destroy(gameObject, 0.1f);
                }
            }
        }
        
        protected override void OnImpact()
        {
            // Beam deals direct damage with high accuracy
            float hitChance = CalculateHitProbability(Vector3.Distance(startPosition, targetPosition));
            bool hit = Random.value <= hitChance;
            
            if (hit)
            {
                Debug.Log($"Beam hit for {data.baseDamage} damage!");
            }
            else
            {
                Debug.Log("Beam missed!");
            }
        }
    }
}
