using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour
{
    [SerializeField] float MovementSpeed, MaxDistance;
    Vector3 targetPoint;

    [HideInInspector]
    public PlayerInput Player;
    [HideInInspector]
    public IChainable chainable;

    public void Shoot(PlayerInput _playerInput)
    {
        Player = _playerInput;
        Move();
    }

    public void SetTargetPoint(Vector3 _targetPosition)
    {
        targetPoint = _targetPosition.normalized;
    }

    Vector3 translateVector;
    public IEnumerator Move()
    {
        Vector3 startPosition = transform.position;
        translateVector = targetPoint * MovementSpeed * Time.deltaTime;
        float distanceFromShootPosition = 0;
        while (distanceFromShootPosition < MaxDistance)
        {
            transform.Translate(translateVector, Space.World);
            distanceFromShootPosition = Vector3.Distance(startPosition, transform.position);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }

    public IEnumerator Pull()
    {
        chainable.CurrentChainableState = ChainableState.pulled;
        while (Vector3.Distance(transform.position, Player.transform.position) > .1f)
        {
            transform.Translate(Player.transform.position.normalized * MovementSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        chainable.CurrentChainableState = ChainableState.catched;
        yield return null;
    }

    private void Update()
    {
        if (chainable != null)
        {
            switch (chainable.CurrentChainableState)
            {
                case ChainableState.free:
                    break;
                case ChainableState.chained:
                    startPull();
                    break;
                case ChainableState.pulled:
                    break;
                default:
                    break;
            }
        }
    }

    void startPull()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("sEFASRGAEHA");
            StartCoroutine(Pull());
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("PorcalatroiaputtanaPORCA");
        IChainable _chainable = other.gameObject.GetComponent<IChainable>();
        if (_chainable != null)
        {
            chainable = _chainable;
            Debug.Log(chainable.transform.name + " Chained");
            chainable.transform.SetParent(Player.transform);
            chainable.CurrentChainableState = ChainableState.chained;
            translateVector = Vector3.zero;
        }
    }
}
