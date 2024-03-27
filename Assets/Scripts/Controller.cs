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

    private bool _hideCursor;

    private void Start() {
        _multiplayerManager = MultiplayerManager.Instance;
        _hideCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            _hideCursor = !_hideCursor;
            Cursor.lockState = _hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (_hold) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = 0f;
        float mouseY = 0f;

        bool isShoot = false;

        if (_hideCursor) {
             mouseX = Input.GetAxis("Mouse X");
             mouseY = Input.GetAxis("Mouse Y");
             isShoot = Input.GetMouseButton(0);
        }



        bool space = Input.GetKeyDown(KeyCode.Space);
        if (space) _player.Jump();

        _player.SetInput(h, v, mouseX * _mouseSensetivity);
        _player.RotateX(-mouseY * _mouseSensetivity);

        //������
        bool crouch = Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftControl) ? true : false ;
        _player.Crouch(crouch);

        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot( ref shootInfo);

        //����� ������
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
            out string gun);
        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", position.x}, 
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x}, 
            {"vY", velocity.y},
            {"vZ", velocity.z},
            {"rX", rotateX },
            {"rY", rotateY },

            //������
            {"cB", crouch },

            //������
            {"gun", gun }
           
    };
        _multiplayerManager.SendMessage("move", data);
    }

    public void Restart(int spawnIndex) {

        _multiplayerManager._spawnPoints.GetPoint(spawnIndex, out Vector3 position, out Vector3 rotation);
        StartCoroutine(Hold());

        _player.transform.position = position;
        rotation.x = 0;
        rotation.z = 0;
        _player.transform.eulerAngles = rotation;
        _player.SetInput(0, 0, 0);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", 0},
            {"vY", 0},
            {"vZ", 0},
            {"rX", 0 },
            {"rY", rotation.y },
        };
        _multiplayerManager.SendMessage("move", data);

    }

    private IEnumerator Hold() {
        _hold = true;
        yield return new WaitForSecondsRealtime(_restartDelay);
        _hold = false;

    }

    //����� ������
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

