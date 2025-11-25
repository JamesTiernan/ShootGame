using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject weaponArm;
    [SerializeField] public GameObject target;
    [SerializeField] public int magazineSize;
    [SerializeField] public int ammoInMagazine;
    [SerializeField] public int totalAmmo;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform armAttachPoint;
    [SerializeField] private GameObject ammoDisplay;
    private playerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Text display = ammoDisplay.GetComponent<Text>();
        display.text = $"Ammo:{ammoInMagazine}/{totalAmmo}";
        if (Input.GetKeyDown(KeyCode.R))
        {
            int prevAmmo = ammoInMagazine;
            ammoInMagazine += totalAmmo;
            if (ammoInMagazine > magazineSize){ammoInMagazine = magazineSize;}
            totalAmmo -= ammoInMagazine - prevAmmo;
        }
        if (Input.GetMouseButton(1))
        {
            weaponArm.transform.position = armAttachPoint.transform.position;
            arm.transform.localScale = new Vector3(0, 1, 1);
            weaponArm.transform.localScale = new Vector3(1, 1, 1);

            aimWeapon();

            if (Input.GetMouseButtonDown(0))
            {
                bool dir = player.isFacingRight;
                if (ammoInMagazine > 0)
                {
                    ammoInMagazine -=1;
                    if (!dir)
                    {
                        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(new Vector3(0, 180, 0)));
                    }
                    else
                    {
                        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    }
                }
            }
        }
        else
        {
            arm.transform.localScale = new Vector3(1, 1, 1);
            weaponArm.transform.localScale = new Vector3(0, 1, 1);
        }
    }

    private void aimWeapon()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.transform.position = mousePosition;
    }
}
