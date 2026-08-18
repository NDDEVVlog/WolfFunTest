using UnityEngine;

public class ToxicProjectile : BaseProjectile
{
    [SerializeField] private ToxicEffectSO _toxicEffectData;

    public void Initialize(Vector3 direction, float speed, float maxDistance)
    {   
        InitializeMovement(direction, speed, maxDistance, true);
    }

    protected override bool ProcessHit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out IEffectHandler effectHandler))
            {
                effectHandler.ApplyEffect(_toxicEffectData);
            }
        }
        
        return true; 
    }
}