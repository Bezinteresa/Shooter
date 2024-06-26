using System;
using UnityEngine;


public enum GunType {
    Pistol,
    Avtomat,
    Shotgun
}

public class PlayerGun : Gun
{

    [SerializeField] private int _damage;
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootDelay;
    private float _lastShootTime;

    [Header("������")]
    [SerializeField] private GunType _currentGun;
    [field: SerializeField] public GameObject[] _gunVisual {  get; private set; }


    public bool TryShoot(out ShootInfo info) {

        info = new ShootInfo();

        if (Time.time - _lastShootTime < _shootDelay) return false;


        Vector3 position = _bulletPoint.position;
        Vector3 velocity = _bulletPoint.forward * _bulletSpeed;

        _lastShootTime = Time.time;

        //�������� if(_currentGun ) � �������� ��������

        Instantiate( _bulletPrefab,position, _bulletPoint.rotation ).Init(velocity,_damage) ;
        shoot?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.dX = velocity.x;
        info.dY = velocity.y;
        info.dZ = velocity.z;

        return true;
    }

    public void ChangeGun(GunType gunType) {
        _currentGun = gunType;

        //����� ������� ������
        if(_currentGun == GunType.Pistol) {

        } else if(_currentGun == GunType.Shotgun) {

        } else if (_currentGun == GunType.Avtomat) {

        } 
    }


}
