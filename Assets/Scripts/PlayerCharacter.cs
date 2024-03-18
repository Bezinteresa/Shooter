using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _minHeadAngle = -90f;
    [SerializeField] private float _maxHeadAngle = 90f;
    [SerializeField] private float _jumpForce = 50f;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private float _junpDelay = 0.2f;

    [SerializeField] private CharacterAnimation _characterAnimation;

    private float _inputH;
    private float _inputV;
    private float _rotateY;
    private float _currentRotateX;
    private float _jumpTime;

    private void Start() {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;

    }

    public void SetInput(float h, float v, float rotateY) {
        _inputH = h;
        _inputV = v;
        _rotateY += rotateY;
    }

    void FixedUpdate()
    {
        Move();
        RotateY();
    }

    private void Move() {

        //Vector3 direction = new Vector3(_inputH,0,_inputV).normalized;
        //transform.position += direction * Time.deltaTime * _speed;

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
        velocity.y = _rigidbody.velocity.y;
        _velocity = velocity;


        _rigidbody.velocity = _velocity;

    }

    private void RotateY() {
        _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }

    public void RotateX(float value) {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value,_minHeadAngle, _maxHeadAngle);

        _head.localEulerAngles = new Vector3(_currentRotateX,0,0);
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY) {
        position = transform.position;
        velocity = _rigidbody.velocity;
        rotateY = transform.eulerAngles.y;
        rotateX = _head.localEulerAngles.x;

    }


    public void Jump() {
        if (_checkFly.IsFly) return;
        if (Time.time - _jumpTime < _junpDelay) return;

        _jumpTime = Time.time;
        _rigidbody.AddForce(0,_jumpForce,0, ForceMode.VelocityChange);
    }

    //Присяд наверное можно сразу из контроллера
    public void Crouch(bool boo) {
        _characterAnimation.ChangeCrouchBool(boo);
    }

}
