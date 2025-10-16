using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }
    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)  Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot) weaponSlot.SetActive(true);
            else weaponSlot.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchActiveSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchActiveSlot(1);
    }

    public void PickupWeapon(GameObject PickedUpWeapon)
    {
        Camera.main.transform.Find("PlayerCanvas").Find("Crosshair").gameObject.SetActive(true);
        Camera.main.transform.Find("PlayerCanvas").Find("WeaponIcon").gameObject.SetActive(true);
        Camera.main.transform.Find("PlayerCanvas").Find("WeaponLabel").gameObject.SetActive(true);
        
        AddWeaponIntoActiveSlot(PickedUpWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject PickedUpWeapon)
    {
        DropCurrentWeapon(PickedUpWeapon);
        PickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = PickedUpWeapon.GetComponent<Weapon>();

        PickedUpWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        PickedUpWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
    }

    private void DropCurrentWeapon(GameObject PickedUpWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;

            weaponToDrop.transform.SetParent(PickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = PickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = PickedUpWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }
}
