using UnityEngine;

public class Gun : MonoBehaviour
{
    //Damage annd range for the gun
    public float damage = 10f;  
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;


public void Shoot()
{
    muzzleFlash.Play();

    RaycastHit hit;
    if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
    {
        Debug.Log(hit.transform.name);


        Target target = hit.transform.GetComponent<Target>();
        if (target != null)
            {
                target.TakeDamage(damage);
            }
    }

        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
}
}