using UnityEngine;

public class GoalVFX : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject goalParticlePrefab;

    private bool _fired = false;

    void OnTriggerEnter(Collider other)
    {
        if (_fired) 
        {
            return;
        }

        Projectile proj = other.GetComponent<Projectile>();

        if (proj == null) 
        {
            return;
        }

        _fired = true;

        SoundManager.PLAY_GOAL();

        if (goalParticlePrefab != null)
        {
            GameObject fx = Instantiate(goalParticlePrefab, transform.position, Quaternion.identity);

            ParticleSystem ps = fx.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
            }

            else
            {
                Destroy(fx, 2f);
            }
        }
    }
}