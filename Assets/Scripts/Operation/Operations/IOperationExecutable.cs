using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IOperationExecutable
{
    UnityEvent CancelOperation { get; }
    void AddCancelListener(UnityAction action)
    {
        CancelOperation.AddListener(action);
    }

    void RemoveCancelListener(UnityAction action)
    {
        CancelOperation.RemoveListener(action);
    }

    // 実行時の対象処理
    void Execute(Tile tile);

    // どのOperationTypeか（識別子）
    OperationType Type { get; }

    // 実行時に回数を消費するか
    bool IsConsumable { get; }
}
