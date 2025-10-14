using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;
    public AudioClip Pistol;
    public AudioClip MG5;
    public AudioSource reloadingSoundMG5;
    public AudioSource reloadingSoundPistol;

    public AudioSource emptySoundPistol;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void PlayShootingSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol:
                ShootingChannel.PlayOneShot(Pistol);
                break;
            case Weapon.WeaponModel.MG5:
                ShootingChannel.PlayOneShot(MG5);
                break;

        }
    }

    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol:
                reloadingSoundPistol.Play();
                break;
            case Weapon.WeaponModel.MG5:
                reloadingSoundMG5.Play();
                break;

        }
    }
}


