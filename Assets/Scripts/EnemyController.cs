using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private EnemyCharacter _character;
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


    private void SaveReceiveTime() {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _reciveTimeInterval.Add(interval);
        _reciveTimeInterval.RemoveAt(0);
    }

    internal void OnChange(List<DataChange> changes) {

        SaveReceiveTime();

        Vector3 position = _character.targetPosition;
        Vector3 velosity = Vector3.zero;

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

                default:
                    Debug.Log("Необрабатывается изменение поля " +  dataChange.Field);
                    break;
            }

        }

        _character.SetMovement(position,velosity,AverageInterval);

    }

 
}
