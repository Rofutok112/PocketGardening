using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Watering : IOperationExecutable
{
    public UnityEvent CancelOperation { get; } = new UnityEvent();
    public OperationType Type => OperationType.Watering;
    public bool IsConsumable => false;

    public void Execute(Tile tile)
    {
        // 水やりの処理
        if (tile.Water())
        {
            CancelOperation?.Invoke();
        }
    }
}
