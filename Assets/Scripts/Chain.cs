using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Chain : MonoBehaviour
{
    [SerializeField] float MovementSpeed, MaxDistance;
    Vector3 targetPoint;

    [HideInInspector]
    public PlayerInput Player;
    [HideInInspector]
    public IChainable chainable;

    bool taked;

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
        Tween tween = null;
        chainable.CurrentChainableState = ChainableState.pulled;
        tween = transform.DOMove(Player.transform.position, 3f);
        while (Vector3.Distance(transform.position, Player.transform.position) > 1)
        {
            //transform.Translate(Player.transform.position.normalized * MovementSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        if (tween != null)
            tween.Pause();
        chainable.CurrentChainableState = ChainableState.catched;
        MovementSpeed = 0;
        chainable.transform.SetParent(Player.transform);
        StopAllCoroutines();
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
                case ChainableState.catched:
                    realease();
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
            StartCoroutine(Pull());
        }
    }

    void realease()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            chainable.transform.SetParent(null);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        IChainable _chainable = other.gameObject.GetComponent<IChainable>();
        if (_chainable != null && !taked)
        {
            chainable = _chainable;
            taked = true;
            Debug.Log(chainable.transform.name + " Chained");
            chainable.transform.SetParent(transform);
            StopCoroutine(Move());
            chainable.CurrentChainableState = ChainableState.chained;
            translateVector = Vector3.zero;
        }
    }

    public void Go()
    {
        StartCoroutine(Move());
    }
}
