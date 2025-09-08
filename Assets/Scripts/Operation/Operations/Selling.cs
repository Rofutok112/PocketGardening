using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selling : IOperationExecutable
{
    public UnityEvent CancelOperation { get; }  = new UnityEvent();

    public OperationType Type => OperationType.Selling;
    public bool IsConsumable => false;

    public void Execute(Tile tile)
    {
        tile.Sell();
    }
}
