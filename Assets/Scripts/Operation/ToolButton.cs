using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToolButton : MonoBehaviour
{
    [SerializeField] private OperationType operationType;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Button button;

    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            // 選択状態に応じてボタンの見た目を変更する処理をここに追加可能
            GetComponent<Image>().color = isSelected ? Color.yellow : Color.white;
        }
    }

    public void OnToolButtonTapped()
    {
        if (inventory == null) return;
        AudioManager.I.PlaySE(SEType.ButtonClick);
        inventory.UseTool(operationType);
    }

    public void UpdateInteractable(bool interactable)
    {
        if (button != null)
        {
            button.interactable = interactable;
        }
    }
}
