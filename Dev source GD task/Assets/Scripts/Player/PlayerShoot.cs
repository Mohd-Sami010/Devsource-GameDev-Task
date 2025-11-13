using UnityEngine;

public class PlayerShoot :MonoBehaviour {
    public static PlayerShoot Instance { private set; get; }

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gunTransform;

    [Header("Gun Stats")]
    [SerializeField] private float timeBetweenShots = 1f;
    [SerializeField] private int damage = 30;
    [SerializeField] private int magSize = 8;
    [SerializeField] private int totalBullets = 64;
    [SerializeField] private float reloadTime = 1f;

    [Header("Others")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashObject;

    private bool readyToShoot = true;
    private int bulletsLeftInMag;
    private bool reloading;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InputManager.Instance.OnShootButtonPressed += InputMananger_OnShootButtonPressed;
        InputManager.Instance.OnReloadButtonPressed += Reload;
        bulletsLeftInMag = magSize;
    }

    private void InputMananger_OnShootButtonPressed()
    {
        if (totalBullets > 0 && bulletsLeftInMag == 0)
        {
            Reload();
        }

        if (!readyToShoot || reloading) return;

        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bulletObj.GetComponent<Bullet>().ShootBulletPrefab(transform.localScale.x > 0 ? 1 : -1);
        muzzleFlashObject.SetActive(true);
        Invoke(nameof(DisableMuzzleFlash), 0.15f);
        readyToShoot = false;
        Invoke(nameof(ResetShoot), timeBetweenShots);
        bulletsLeftInMag--;
    }
    private void ResetShoot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        if (totalBullets == 0) return;
        reloading = true;
        Invoke(nameof(FinishReload), reloadTime);
    }
    private void FinishReload()
    {
        int bulletsToReload = magSize - bulletsLeftInMag;
        if (totalBullets > bulletsToReload)
        {
            bulletsLeftInMag = magSize;
            totalBullets -= bulletsToReload;
        }
        else
        {
            bulletsLeftInMag = totalBullets;
            totalBullets = 0;
        }

        reloading = false;
        readyToShoot = true;
    }
    private void Update()
    {
        Vector3 dir = (Camera.main.ScreenToWorldPoint(InputManager.Instance.GetAimInput()) - transform.position) * transform.localScale.x;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void DisableMuzzleFlash()
    {
        muzzleFlashObject.SetActive(false);
    }
    public int GetBulletDamage()
    {
        return damage;
    }
    public string GetAmmoText()
    {
        if (reloading)
        {
            return "Reloading...";
        }
        else
        {
            return bulletsLeftInMag + " / " + totalBullets;
        }
    }
}
