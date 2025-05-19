using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    // ========================
    //      GUN SETTINGS
    // ========================
    [Header("Gun Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 20f;
    [SerializeField] private float fireRate = 15f;

    [Header("Ammo")]
    [SerializeField] private int magazineSize = 10;
    [SerializeField] public int ammoCache = 20;
    private int maxAmmo;
    private int ammoNeeded;
    private float nextTimeToFire = 0f;
    private bool isReloading;
    private bool canShoot;

    [Header("Fire Mode")]
    [SerializeField] private bool semi = true;
    [SerializeField] private bool auto = false;

    // ========================
    //      EFFECTS & FX
    // ========================
    [Header("Effects")]
    [SerializeField] private GameObject fpsCam;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject impactEffectFlesh;
    [SerializeField] private GameObject impactEffectTerrain;

    [Header("Reload & Animations")]
    [SerializeField] private Animator anim;
    [SerializeField] private float reloadTime = 2f;

    // ========================
    //      RECOIL SYSTEM
    // ========================
    [Header("Recoil")]
    public Vector3 upRecoil;
    private Vector3 originalRotation;

    // ========================
    //      SWAY SETTINGS
    // ========================
    [Header("Swaying")]
    [SerializeField] private float amount = 0.02f;
    [SerializeField] private float maxAmount = 0.06f;
    [SerializeField] private float smoothAmount = 6f;
    private Vector3 initialPosition;

    // ========================
    //      CASING SYSTEM
    // ========================
    [Header("Bullet Casing")]
    [SerializeField] private GameObject bulletCasing;
    [SerializeField] private Transform casingLocation;
    private bool casingForward;
    private bool casingBackwards;

    void Start()
    {
        amount = 0.02f;
        maxAmount = 0.06f;
        smoothAmount = 6f;
        maxAmmo = magazineSize;
        semi = true;
        isReloading = false;
        canShoot = true;

        originalRotation = transform.localEulerAngles;
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (InputManager.Instance.IsShooting() && Time.time >= nextTimeToFire && semi && magazineSize > 0 && canShoot)
        {
            AddRecoil();
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        //For Auto Weapons:

        if (InputManager.Instance.IsShooting() && Time.time >= nextTimeToFire && magazineSize > 0 && auto && canShoot)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            AddRecoilAuto();
            Shoot();
        }

        //Checks For 0 Ammo:

        if (InputManager.Instance.IsShooting() && magazineSize == 0)
        {
            AudioManager.Instance.NoAmmoSound();
        }

        else if(InputManager.Instance.IsShooting() && canShoot)
        {

            StopRecoil();
        }


        //Reloading
        if (InputManager.Instance.IsReload() && ammoCache> 0 && maxAmmo > magazineSize && !isReloading)
        {
            
            StartCoroutine(ReloadTimer());
            
        }
        else if (isReloading)
        {
            GUIManager.Instance.AmmoTextUpdate("Reload");
            return;

        }

        //Doesnt Reload If Cache Is 0:
        if (InputManager.Instance.IsReload() && ammoCache == 0)
        {

            return;

        }
        GUIManager.Instance.AmmoTextUpdate(magazineSize + " / " + ammoCache);


        //Our Swaying Function Being Put To Action:
        Vector2 lookVector = InputManager.Instance.PlayerLook();
        float movementX = lookVector.x * Time.deltaTime * amount;
        float movementY = lookVector.y * Time.deltaTime * amount;
        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        //Making Sure The Sway Goes Back To Original Postion:
        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);

    }

    void Shoot()
    {
        int layerToIgnore = LayerMask.GetMask("IgnoreRaycast");
        int layerMask = ~layerToIgnore;
        RaycastHit hit;
        magazineSize--;
        ammoNeeded++;

        StartCoroutine(FlashMuzzle());
        anim.SetTrigger("Shoot");
        AudioManager.Instance.ShootPistolSound(transform.position);

        if(bulletCasing)
        {
            GameObject casing = Instantiate(bulletCasing, casingLocation.position, casingLocation.rotation);
            if (casingForward)
            {
                casing.GetComponent<Rigidbody>().AddForce(transform.forward * 250);
            }

            if (casingBackwards)
            {
                casing.GetComponent<Rigidbody>().AddForce(transform.forward * -250);
            }
            Destroy(casing, 2f);
        }
        
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask))
        {
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            GameObject impactOB;
            if (target != null)
            {
                target.TakeDamage(damage);
                impactOB = Instantiate(impactEffectFlesh, hit.point, Quaternion.LookRotation(hit.normal));
            } else
            {
                impactOB = Instantiate(impactEffectTerrain, hit.point, Quaternion.LookRotation(hit.normal));
            }
            Destroy(impactOB, 2f);
        }
     }

    IEnumerator FlashMuzzle()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.01f); // thời gian chớp, có thể chỉnh nhỏ hơn nữa
        muzzleFlash.SetActive(false);
    }
    //Adding Recoil Postion When Shot Semi Weapons:
    public void AddRecoil()
    {
        if (canShoot)
        {
            transform.localEulerAngles += upRecoil;
            StartCoroutine(StopRecoilSemi());
        }
    }

    //Adding Recoil Postion When Shot Auto Weapons:

    public void AddRecoilAuto()
    {
        if (canShoot)
        {
            transform.localEulerAngles += upRecoil;
            StartCoroutine(StopRecoilSemi());
        }
    }

    //Stopping Recoil:

    public void StopRecoil()
    {
        transform.localEulerAngles = originalRotation;
    }

    //Stopping Recoil (Fixing Bugs)

    IEnumerator StopRecoilSemi()
    {
        yield return new WaitForSeconds(.1f);
        transform.localEulerAngles = originalRotation;
    }

    //Our Reload Timer:

    IEnumerator ReloadTimer()
    {
        canShoot = false;
        isReloading = true;
        anim.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        if (ammoCache >= ammoNeeded)
        {
            ammoCache -= ammoNeeded;
        }
        else
        {
            ammoNeeded = ammoCache;
            ammoCache = 0;
        }
        magazineSize += ammoNeeded;
        ammoNeeded = maxAmmo - magazineSize;
        isReloading = false;
        canShoot = true;
        InventoryManager.Instance.RemoveAmmo(ammoCache / maxAmmo);
    }

    public void Reset()
    {
        magazineSize = 10;
        ammoCache = 0;
        ammoNeeded = 0;
    }

    void OnDisable()
    {
        GUIManager.Instance.ShowGlockUI(false);
        if (isReloading)
        {
            StopCoroutine("ReloadTimer");
            isReloading = false;
            canShoot = true;
        }
        
    }

    private void OnEnable()
    {
        GUIManager.Instance.ShowGlockUI(true);
    }
}
