using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationManager : MonoBehaviour
{
    private IOperationExecutable _currentOperation;
    [SerializeField] private Inventory _inventory;

    private void Start()
    {
        if (_inventory == null)
        {
            Debug.LogError("Inventoryが見つからない.");
        }
    }

    public void OnTappedHandler(Tile tile)
    {
        if (_inventory == null || _currentOperation == null) return;

        // インベントリに現在のオペレーションが実行できる状態にあるかを確認
        if (_inventory.IsExecutable(_currentOperation))
        {
            if (_currentOperation.IsConsumable)
            {
                _inventory.DecreaseCount(_currentOperation.Type, 1);
            }
            _currentOperation.Execute(tile);
        }
    }

    /// <summary>
    /// 実行するオペレーションをセット
    /// </summary>
    /// <param name="newOperation"></param>
    public void SetOperation(IOperationExecutable newOperation)
    {
        if (newOperation.Type != OperationType.Selling)
        {
            _inventory.CancelTool();
        }
        _currentOperation?.RemoveCancelListener(ClearOperation);
        Debug.Log("SetOperation: " + newOperation.GetType().Name);
        _currentOperation = newOperation;
        _currentOperation.AddCancelListener(ClearOperation);
    }

    /// <summary>
    /// 現在のオペレーションをクリア
    /// </summary>
    public void ClearOperation()
    {
        Debug.Log("ClearOperation");
        _currentOperation?.RemoveCancelListener(ClearOperation);
        _currentOperation = null;
    }
}

/// <summary>
/// 各オペレーションのインスタンスを保持するクラス
/// </summary>
public static class OperationInstances
{
    public static readonly IOperationExecutable Watering = new Watering();
    public static readonly IOperationExecutable Digging = new Digging();
    public static readonly IOperationExecutable Selling = new Selling();

    public static IOperationExecutable Resolve(OperationType type)
    {
        switch (type)
        {
            case OperationType.Watering: return Watering;
            case OperationType.Digging: return Digging;
            case OperationType.Selling: return Selling;
            // Seeding は FlowerData に依存するためここでは解決しない
            default: return null;
        }
    }
}
