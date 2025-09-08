using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Seeding : IOperationExecutable
{
    public UnityEvent CancelOperation { get; } = new UnityEvent();

    private FlowerData _flowerData;

    public Seeding(FlowerData flowerData)
    {
        _flowerData = flowerData;
    }

    public OperationType Type => OperationType.Seeding;
    public bool IsConsumable => false;

    public void Execute(Tile tile)
    {
        if (tile.Seed(_flowerData))
        {
            CancelOperation?.Invoke();
        }
    }
}
