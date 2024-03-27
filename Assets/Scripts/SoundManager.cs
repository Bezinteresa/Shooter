using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _headShot;


    private void OnEnable() {
        Damage._OnHeadShot += PlayHeadShot;
    }

    private void OnDisable() {
        Damage._OnHeadShot -= PlayHeadShot;
    }


    private void PlayHeadShot() {
        _headShot.Play();
    }


}
