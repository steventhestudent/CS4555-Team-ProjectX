using UnityEngine;

public class WeaponGun : MonoBehaviour, IWeapon
{
    public float damage = 10f;
    public float range = 50f;
    public ParticleSystem muzzleFlash;

    public void Fire()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Gun hit: " + hit.collider.name);

            // var health = hit.collider.GetComponent<EnemyHealth>();
            // if (health != null)
            //     health.TakeDamage(damage);
        }
    }
}