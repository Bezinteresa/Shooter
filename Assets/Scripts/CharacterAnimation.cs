using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{

    private const string Grounded = "Grounded";
    private const string Speed = "Speed";
    
    //Присяд
    private const string Crouch = "Crouch";
    private bool _isCrouch;
    [SerializeField] private float _currentCrouch;
    private float t = 0f;
    private float lerpTime = 0.2f;
    [SerializeField] private Animator _crouchAnimator;

    [SerializeField] private Animator _animator;
    [SerializeField] private CheckFly _checkFly;
    [SerializeField] private Character _character;

    private void Update() {

        Vector3 localVelocity = _character.transform.InverseTransformVector(_character._velocity);
        float speed = localVelocity.magnitude / _character._speed;
        float sign = Mathf.Sign(localVelocity.z);
        
        _animator.SetFloat(Speed, speed * sign);
        _animator.SetBool(Grounded, _checkFly.IsFly== false);

        //Присяд вниз вверх
        if (_isCrouch==true && _crouchAnimator.GetFloat(Crouch) < 1) {
            Debug.Log("Down");
            _currentCrouch += Time.deltaTime / lerpTime;
            float val = Mathf.Lerp(0,1, _currentCrouch);
            SetCrouch(val);

        } else if (_isCrouch == false && _crouchAnimator.GetFloat(Crouch) > 0) {
            Debug.Log("UP");
            _currentCrouch -= Time.deltaTime / lerpTime;
            float val = Mathf.Lerp(0, 1, _currentCrouch);
            SetCrouch(val);

        }

    }

    //Присяд
    private void SetCrouch(float val) {
        _crouchAnimator.SetFloat(Crouch,val);

    }

    public void ChangeCrouchBool(bool boo) {
        _isCrouch = boo;
    }

}
