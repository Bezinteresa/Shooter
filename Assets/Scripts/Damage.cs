using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] int damageMultiplier = 1;

    [SerializeField] private EnemyCharacter _enemy;
    [SerializeField] private bool _headShot;

    public static event Action _OnHeadShot;

    public void SetDamage(int val) {
        _enemy.ApplyDamage(damageMultiplier + val);
        if (_headShot) _OnHeadShot?.Invoke(); 
    }

}
