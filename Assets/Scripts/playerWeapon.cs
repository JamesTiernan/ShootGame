using UnityEngine;

public class playerWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject weaponArm;
    [SerializeField] public GameObject target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform armAttachPoint;
    private playerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            weaponArm.transform.position = armAttachPoint.transform.position;
            arm.transform.localScale = new Vector3(0, 1, 1);
            weaponArm.transform.localScale = new Vector3(1, 1, 1);

            aimWeapon();

            if (Input.GetMouseButtonDown(0))
            {
                bool dir = player.isFacingRight;
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
