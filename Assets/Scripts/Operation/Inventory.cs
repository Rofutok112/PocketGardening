using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private OperationManager operationManager;
    [SerializeField] private ToolButton wateringButton;
    [SerializeField] private ToolButton diggingButton;
    [SerializeField] private ToolButton sellingButton;

    private readonly Dictionary<OperationType, int> _counts = new Dictionary<OperationType, int>();

    public int GetCount(OperationType type)
    {
        return _counts.TryGetValue(type, out var v) ? v : 0;
    }

    private void Start()
    {
        // 初期所持数（必要に応じて変更）
        _counts[OperationType.Watering] = GetCount(OperationType.Watering);
        _counts[OperationType.Digging] = GetCount(OperationType.Digging);
        UpdateToolButtonInteractable();
    }

    // 実行可能か（非消費型は常にtrue）
    public bool IsExecutable(IOperationExecutable operation)
    {
        if (operation == null) return false;
        return !operation.IsConsumable || GetCount(operation.Type) > 0;
    }

    // ツール選択（種類指定）
    public void UseTool(OperationType type)
    {
        var op = OperationInstances.Resolve(type);
        if (op == null)
        {
            Debug.LogError($"Operation not resolved: {type}");
            return;
        }

        if (IsExecutable(op))
        {
            switch (type)
            {
                case OperationType.Watering:
                    if (wateringButton.IsSelected)
                    {
                        CancelTool();
                        operationManager.ClearOperation();
                        return;
                    }
                    else
                    {
                        wateringButton.IsSelected = true;
                    }
                    break;
                case OperationType.Digging:
                    if (diggingButton.IsSelected)
                    {
                        CancelTool();
                        operationManager.ClearOperation();
                        return;
                    }
                    else
                    {
                        diggingButton.IsSelected = true;
                    }
                    break;
                case OperationType.Selling:
                    if (sellingButton.IsSelected)
                    {
                        CancelTool();
                        operationManager.ClearOperation();
                        return;
                    }
                    else
                    {
                        sellingButton.IsSelected = true;
                    }
                    break;
            }

            operationManager.SetOperation(op);
        }
        else
        {
            Debug.LogWarning($"Not enough count for operation: {type}");
        }
    }

    public void CancelTool()
    {
        wateringButton.IsSelected = false;
        diggingButton.IsSelected = false;
        sellingButton.IsSelected = false;
    }

    public void UpdateToolButtonInteractable()
    {
        wateringButton.UpdateInteractable(IsExecutable(OperationInstances.Watering));
        diggingButton.UpdateInteractable(IsExecutable(OperationInstances.Digging));
        sellingButton.UpdateInteractable(IsExecutable(OperationInstances.Selling));
    }

    // 使用回数を減らす
    public void DecreaseCount(OperationType type, int amount = 1)
    {
        if (!_counts.ContainsKey(type)) return;
        _counts[type] = Mathf.Max(0, _counts[type] - amount);
        UpdateToolButtonInteractable();
    }

    // 回数を増やす
    public void AddOperationCount(OperationType type, int count)
    {
        if (!_counts.ContainsKey(type)) _counts[type] = 0;
        _counts[type] += count;
        UpdateToolButtonInteractable();
    }
}
