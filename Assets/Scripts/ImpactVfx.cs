using UnityEngine;

public class ImpactVFX : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject impactParticlePrefab;

    void OnCollisionEnter(Collision col)
    {
        SoundManager.PLAY_IMPACT();

        if (impactParticlePrefab == null) 
        {
            return;
        }

        ContactPoint contact = col.contacts[0];

        Quaternion rot = Quaternion.LookRotation(contact.normal);

        GameObject fx = Instantiate(impactParticlePrefab, contact.point, rot);

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