using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAssaultRiffle : MonoBehaviour
{
    class Bullet
    {
        public float time; 
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    public bool isFiring;
    public float fireRate;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 300f;
    public TrailRenderer tracerEffect;
    public ParticleSystem hitEffect;
    public ParticleSystem muzzleFlash;
    public Transform raycastOrigin;
    public Transform raycastDestination;    
    public AmmoWidget ammoWidgetScript;



    private Animator gunsAnimator;

    Ray ray;
    RaycastHit hitInfo;
    private PlayerWeaponShoot playerWeaponShootScript;
    private PlayerWeaponSwitchScript playerWeaponSwitchScript;
    private Ammo ammoScript;
    private ZombieHealth zombieHealth;
    private float accumulatedTime;
    private float maxLifetime = 3f;
    private WeaponRecoil recoil;
    private bool coroutineIsExecuting;
    List<Bullet> bullets = new List<Bullet>();

    Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        return bullet;
    }  

    void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
        ammoScript = GetComponentInParent<Ammo>();
        playerWeaponShootScript = GetComponentInParent<PlayerWeaponShoot>();
        playerWeaponSwitchScript = GetComponentInParent<PlayerWeaponSwitchScript>();
        gunsAnimator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (isFiring)
        {
            ammoWidgetScript.Refresh(ammoScript.ammoCount);
        }
        if(playerWeaponShootScript.shootButtonPressed && !coroutineIsExecuting && playerWeaponSwitchScript.assaultRiffleEquiped)
        {
            StartCoroutine("shootingWithAssaultRifle");
            coroutineIsExecuting = true;
        }
        else if(!playerWeaponShootScript.shootButtonPressed && coroutineIsExecuting)
        {
            StopCoroutine("shootingWithAssaultRifle");
            StopFiring();
            coroutineIsExecuting = false;
        }
    }

    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f;
        FireBullet();
        //recoil.Reset();
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1 / fireRate;
        while(accumulatedTime >= 0.0f)
        {
        //FireBullet();
        accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet => 
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }
    
    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifetime); 
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {   
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start; 
        ray.direction = direction;
        if(Physics.Raycast(ray, out hitInfo)) 
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifetime;

            if (hitInfo.collider.gameObject.tag == "ZombieHead")
            {
                zombieHealth = hitInfo.collider.gameObject.GetComponentInParent<ZombieHealth>();
                zombieHealth.health = 0f;
            }
            if (hitInfo.collider.gameObject.tag == "ZombieBody")
            {
                zombieHealth = hitInfo.collider.gameObject.GetComponentInParent<ZombieHealth>();
                zombieHealth.health = zombieHealth.health - 10f;
            }
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    void FireBullet()
    {
        if (ammoScript.ammoCount <= 0)
        {
            return;
        }
        ammoScript.assaultRiffleAmmoCount--;
        ammoScript.ammoCount = ammoScript.assaultRiffleAmmoCount;

        muzzleFlash.Emit(1);
        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed; 
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
        recoil.GenerateRecoil();
    }

    IEnumerator shootingWithAssaultRifle()
    {
        while (true)
        {
            if(playerWeaponShootScript.shootButtonPressed)
            {
                StartFiring();
                gunsAnimator.SetTrigger("assaultRiffleRecoil");
            }
            else if (!playerWeaponShootScript.shootButtonPressed)
            {
                StopFiring();
            }
            yield return new WaitForSeconds(fireRate);
        }
    }
}
