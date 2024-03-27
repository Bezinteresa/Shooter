using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyCharacter : Character {

    private string _sessionID;

    [SerializeField] private Health _health;
    [SerializeField] private Transform _head;

    public Vector3 targetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0;

    //Для поворота
    //public Vector3 targetRotationY { get; private set; } = Vector3.zero;
    //private float _velocityYMagnitude = 0;


    //Для присяда
    [SerializeField] CharacterAnimation _characterAnimation;
    private bool _isCrouch;


    public void Init(string sessionID) {
        _sessionID = sessionID;
    }

    void Start () {
        targetPosition = transform.position;
        //targetRotationY = transform.eulerAngles;
    }

    public void SetSpeed(float value) => _speed = value;

    public void SetMaxHp(int value) {
        maxHealth = value;
        _health.SetMax(value);
        _health.SetCurrent(value);

    }

    public void RestoreHp(int newValue) {
        _health.SetCurrent(newValue);
    }

    private void Update() {

        if(_velocityMagnitude > 0.1f) {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistance);
        } else {
            transform.position = targetPosition;
        }

        //Поворот плавный возможно
        //if(_velocityYMagnitude > 0.1f) {
        //    float maxAngle = _velocityYMagnitude * Time.deltaTime;
        //    transform.localEulerAngles = Vector3.MoveTowards(transform.eulerAngles, targetRotationY, maxAngle);
        //    FindObjectOfType<Test>().SetText("if",transform.localEulerAngles.y.ToString() );
        //} else {
        //    transform.eulerAngles = targetRotationY;
        //    FindObjectOfType<Test>().SetText("else", transform.localEulerAngles.y.ToString());
        //}

    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval) {
        targetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        this._velocity = velocity;
    }

    public void ApplyDamage(int damage) {
        _health.ApplyDamage(damage);

        Dictionary<string, object> data = new Dictionary<string, object>() {
            {"id",_sessionID },
            {"value", damage }

        };
        
        MultiplayerManager.Instance.SendMessage("damage", data);
        Debug.Log("sendDamage");
    }

    public void SetRotateX(float value) {
        _head.localEulerAngles = new Vector3(value,0,0);
    }

    //Поворот
    public void SetRotateY(in Vector3 position) {
        //Новый поворот
        //targetRotationY = position + (velocity * averageInterval);
        //_velocityYMagnitude = velocity.magnitude;


        //старый поворот
        //transform.localEulerAngles = new Vector3(0, value, 0);
        transform.localEulerAngles = position;
    }


    //Присяд наверное можно сразу из контроллера
    public void SetCrouch(bool boo) {
        _characterAnimation.ChangeCrouchBool(boo);
        _isCrouch = boo;
    }

}
