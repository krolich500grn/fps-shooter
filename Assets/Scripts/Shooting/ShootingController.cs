using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootingController : MonoBehaviour
{
    public Animator animator;
    public Transform firePoint;
    public float initialAnimationDelay = 1.17f;
    public float fireRate = 0.1f;
    public float fireRange = 10f;
    private float nextFireTime = 0f;
    public bool isAuto = false;
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 1.5f;
    private bool isReloading = false;


    private bool CanShootAuto => Input.GetButton("Fire1") && Time.time >= nextFireTime;
    private bool CanShoot => Input.GetButtonDown("Fire1") && Time.time >= nextFireTime;
    public ParticleSystem muzzleFlash;
    public ParticleSystem bloodEffect;
    public int damagePerShot = 10;


    [Header("Sound Effects")]
    public AudioSource soundAudioSource;
    public AudioClip shootingSoundClip;
    public AudioClip reloadSoundClip;
    public Text currentAmmoText;

    void Start()
    {
        currentAmmo = maxAmmo;
    }


    void Update()
    {
        currentAmmoText.text = currentAmmo.ToString();
        if (isReloading) 
        {
            return;
        }

        if (isAuto)
        {
            HandleShooting(CanShootAuto);
        }
        else 
        {
            HandleShooting(CanShoot);
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            Reload();
        }
    }

    private void HandleShooting(bool isAbleToShoot)
    {
        if (isAbleToShoot)
        {
            Shoot();
        }
        else
        {
            animator.SetBool("Shoot", false);
        }
    }

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            RaycastHit hit;
            if (CanRaycast(out hit))
            {
                Debug.Log(hit.transform.name);

                ZombieAI zombieAI = hit.collider.GetComponent<ZombieAI>();
                if (zombieAI != null)
                { 
                  zombieAI.TakeDamage(damagePerShot);
                  ParticleSystem blood = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                  Destroy(blood.gameObject, blood.main.duration);
                }

                WayPointZombieAI wayPointZombieAI = hit.collider.GetComponent<WayPointZombieAI>();
                if (wayPointZombieAI != null)
                { 
                  wayPointZombieAI.TakeDamage(damagePerShot);
                  ParticleSystem blood = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                  Destroy(blood.gameObject, blood.main.duration);
                }
            }

            muzzleFlash.Play();
            animator.SetBool("Shoot", true);
            soundAudioSource.PlayOneShot(shootingSoundClip);
            currentAmmo--;


            nextFireTime = Time.time + 1f / fireRate;

            if (currentAmmo == 0)
            {
               Reload();
            }
        }
        else 
        {
            Reload();
        }
    }

    private bool CanRaycast(out RaycastHit hit)
    {
        return Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange);
    }

    private void Reload()
    {
        if (!isReloading && currentAmmo < maxAmmo)
        {
            animator.SetTrigger("Reload");
            isReloading = true;
            soundAudioSource.PlayOneShot(reloadSoundClip);
            Invoke("FinishReloading", reloadTime);
        }
    }

    private void FinishReloading() 
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        animator.ResetTrigger("Reload");
    }
}
