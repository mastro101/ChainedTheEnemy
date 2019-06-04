using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour
{
    [SerializeField] float MovementSpeed, MaxDistance;
    Vector3 TargetPoint;

    public void Shoot()
    {
        Move();
    }

    public void SetTargetPoint(Vector3 _targetPosition)
    {
        TargetPoint = _targetPosition;
    }

    public IEnumerator Move()
    {
        Vector3 startPosition = transform.position;
        float distanceFromShootPosition = 0;
        while (distanceFromShootPosition < MaxDistance)
        {
            transform.Translate(Vector3.forward, Space.Self);
            distanceFromShootPosition = Vector3.Distance(startPosition, transform.position);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }
}
