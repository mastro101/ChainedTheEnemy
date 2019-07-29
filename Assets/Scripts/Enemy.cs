using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IChainable
{
    ChainableState currentChainableState;
    public ChainableState CurrentChainableState
    {
        get { return currentChainableState; }
        set { currentChainableState = value; }
    }
}
