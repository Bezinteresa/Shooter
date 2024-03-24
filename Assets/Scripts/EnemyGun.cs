using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{

    [Header("Оружие")]
    [SerializeField] private GunType _currentGun;
    [field: SerializeField] public GameObject[] _gunVisual { get; private set; }

    public void Shoot(Vector3 position, Vector3 velocity) {

        Instantiate(_bulletPrefab, position, Quaternion.identity).Init(velocity);
        shoot?.Invoke();
    }

    public void ChangeGun(GunType gunType) {
        _currentGun = gunType;

        //Смена визуала оружия
        switch (_currentGun) {
            case GunType.Pistol:
                Debug.Log("Switched to Pistol");
                break;

            case GunType.Shotgun:
                Debug.Log("Switched to Shotgun");
                break;

            case GunType.Avtomat:
                Debug.Log("Switched to Avtomat");
                break;

            default:
                Debug.LogWarning("Unknown gun type: " + gunType);
                break;
        }


        //if (_currentGun == GunType.Pistol) {
        //    Debug.Log(_currentGun);
        //} else if (_currentGun == GunType.Shotgun) {
        //    Debug.Log(_currentGun);
        //} else if (_currentGun == GunType.Avtomat) {
        //    Debug.Log(_currentGun);
        //}
    }

}
