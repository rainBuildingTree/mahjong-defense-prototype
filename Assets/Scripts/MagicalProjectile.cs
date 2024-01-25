using UnityEngine;

public class MagicalProjectile : MonoBehaviour {
    protected Vector2 _vectorToTarget;
    protected Vector2 _vectorToRotate;
    protected Vector2 _moveVector;
    [SerializeField] protected Transform _target;
    protected float _gravitySpeed = 10f;
    protected float _rotateSpeed = 6f;

    protected void FixedUpdate() {
        if (_target == null)
            return;
        _vectorToTarget = ((Vector2)_target.position - (Vector2)transform.position).normalized;
        _vectorToRotate = new Vector2(_vectorToTarget.y, -_vectorToTarget.x);
        _moveVector = _vectorToTarget * _gravitySpeed + _vectorToRotate * _rotateSpeed;
        _moveVector *= Time.deltaTime;
        transform.position += (Vector3)_moveVector;
    }
}
