using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChainable
{
    Transform transform { get; }
    ChainableState CurrentChainableState { get; set; }
}

public enum ChainableState
{
    free,
    chained,
    pulled,
    catched,
}