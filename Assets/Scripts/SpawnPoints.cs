
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    public int length { get { return _points.Length; } }

    public void GetPoint(int index, out Vector3 position, out Vector3 rotation) {
        if (index < 0 || index >= _points.Length) {
            position = Vector3.zero;
            rotation = Vector3.zero;
            return;
        }

        position = _points[index].position;
        rotation = _points[index].rotation.eulerAngles;

    }

}
