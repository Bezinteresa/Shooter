using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private EnemyGun _gun;

    private List<float> _reciveTimeInterval = new List<float>() {0,0,0,0,0 };
    private float AverageInterval {
        get {

            int receiveTimeIntervalCount = _reciveTimeInterval.Count;
            float summ = 0;
            for (int i = 0; i < receiveTimeIntervalCount; i++) {
                summ += _reciveTimeInterval[i];
            }

            return summ/receiveTimeIntervalCount;
        }

    }

    private float _lastReceiveTime = 0f;
    private Player _player;
    public void Init(Player player) {
        _player = player;
        _character.SetSpeed(player.speed);
        _character.SetCrouch(player.cB);
        player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo info) {
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);

        _gun.Shoot(position,velocity);
    }

    public void Destroy() {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }


    private void SaveReceiveTime() {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _reciveTimeInterval.Add(interval);
        _reciveTimeInterval.RemoveAt(0);
    }

    internal void OnChange(List<DataChange> changes) {

        SaveReceiveTime();

        Vector3 position = _character.targetPosition;
        Vector3 velosity = _character._velocity;
        Vector3 rotationY = _character.transform.eulerAngles;
        Vector3 velocityY = new Vector3(0f, 0f, 0f);



        foreach(var dataChange in changes) {
            switch (dataChange.Field) {

                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break; 
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velosity.x = (float)dataChange.Value;
                    break; 
                case "vY":
                    velosity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velosity.z = (float)dataChange.Value;
                    break;
                case "rX":
                    _character.SetRotateX((float)dataChange.Value);
                    break;

                case "rY":
                    rotationY.y = (float)dataChange.Value;
                    break;

                    //������
                case "cB":
                    _character.SetCrouch((bool)dataChange.Value);
                    break;

                    //������� �����������
                case "rVY":
                    velocityY.y = (float)dataChange.Value;
                    break;


                default:
                    Debug.Log("���������������� ��������� ���� " +  dataChange.Field);
                    break;
            }

        }

        _character.SetMovement(position,velosity,AverageInterval);

        //�������
        _character.SetRotateY( rotationY,velocityY,AverageInterval );

    }

 
}
