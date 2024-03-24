using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {


    [SerializeField] private float _restartDelay = 3f;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField ] private float _mouseSensetivity = 2f;
    private MultiplayerManager _multiplayerManager;
    private bool _hold = false;

    private void Start() {
        _multiplayerManager = MultiplayerManager.Instance;
    }

    void Update() {

        if (_hold) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool isShoot = Input.GetMouseButton(0);


        bool space = Input.GetKeyDown(KeyCode.Space);
        if (space) _player.Jump();

        _player.SetInput(h, v, mouseX * _mouseSensetivity);
        _player.RotateX(-mouseY * _mouseSensetivity);

        //Присяд
        bool crouch = Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftControl) ? true : false ;
        _player.Crouch(crouch);

        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot( ref shootInfo);

        //Выбор оружия
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ChangeGun(GunType.Pistol);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ChangeGun(GunType.Avtomat);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            ChangeGun(GunType.Shotgun);
        }


        SendMove();
    }

    private void SendShoot(ref ShootInfo shootInfo) {

        shootInfo.key = _multiplayerManager.GetSessionID();
        string json = JsonUtility.ToJson(shootInfo);

        _multiplayerManager.SendMessage("shoot", json);
    }
    private void SendMove() {

        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY,out bool crouch, 
            out float rotateVY, out string gun);
        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", position.x}, 
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x}, 
            {"vY", velocity.y},
            {"vZ", velocity.z},
            {"rX", rotateX },
            {"rY", rotateY },

            //Присяд
            {"cB", crouch },

            //Поворот сглаживание
            {"rVY", rotateVY },

            //Оружие
            {"gun", gun }
           
    };
        _multiplayerManager.SendMessage("move", data);
    }

    public void Restart(string jsonRestartInfo) {

        RestartInfo info = JsonUtility.FromJson<RestartInfo>(jsonRestartInfo);
        StartCoroutine(Hold());

        _player.transform.position = new Vector3(info.x, 0, info.z);
        _player.SetInput(0, 0, 0);

        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY, out bool crouch, 
            out float rotateVY, out string gun);
        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", info.x},
            {"pY", 0},
            {"pZ", info.z},
            {"vX", 0},
            {"vY", 0},
            {"vZ", 0},
            {"rX", 0 },
            {"rY", 0 },

            //Присяд
            {"cB", crouch },

            //Поворот сглаживание
            {"rVY", 0 },

             //Оружие
            {"gun", gun }
        };
        _multiplayerManager.SendMessage("move", data);

    }

    private IEnumerator Hold() {
        _hold = true;
        yield return new WaitForSecondsRealtime(_restartDelay);
        _hold = false;

    }

    //Смена оружия
    private void ChangeGun(GunType type) {
        _player.ChangeGun(type);
    }


}

[Serializable]
public struct ShootInfo {

    public string key;
    public float dX;
    public float dY;
    public float dZ;
    public float pX;
    public float pY;
    public float pZ;

}

[Serializable]
public struct RestartInfo {

    public float x;
    public float z;
}
