using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Digging : IOperationExecutable
{
    public UnityEvent CancelOperation { get; }

    public OperationType Type => OperationType.Digging;
    public bool IsConsumable => true;

    public void Execute(Tile tile)
    {

    }
}
