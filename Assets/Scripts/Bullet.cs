
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private int _damage;

    public void Init(Vector3 velocity, int damage = 0) {
        _damage = damage;

        _rigidbody.velocity  =  velocity;
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy() {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }

    private void Destroy() {
        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision) {

        if(collision.collider.TryGetComponent(out Damage damage)) {
           damage.SetDamage(_damage);
        }

        Destroy();

    }

}
