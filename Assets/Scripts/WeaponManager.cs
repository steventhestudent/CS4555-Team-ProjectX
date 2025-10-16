using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }
    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;


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
        if (activeWeaponSlot.transform.childCount == 0) Utils.ToggleCrosshair(); // show crosshair
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

    internal void PickupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
        }

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
        if (activeWeaponSlot.transform.childCount == 0) Utils.ToggleCrosshair(); // hide crosshair
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

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.MG5:
                totalRifleAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.Pistol:
                totalPistolAmmo -= bulletsToDecrease;
                break;
        }
    }
    
    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch(thisWeaponModel)
        {
            case Weapon.WeaponModel.MG5:
                return totalRifleAmmo;

            case Weapon.WeaponModel.Pistol:
                return totalPistolAmmo;

            default:
                return 0;
        }
    }
}
