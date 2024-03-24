
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public float _speed { get; protected set; } = 2f;
    [field: SerializeField] public int maxHealth { get; protected set; } = 10;
    public Vector3 _velocity {  get; protected set; } 


}
