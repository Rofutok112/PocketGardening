using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private int shopButtonIndex;
    [SerializeField] private Shop shop;
    [SerializeField] private UnityAction<int> onShopItemSelected;
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnShopButtonTapped);
    }

    private void OnShopButtonTapped()
    {
        shop.OnPurchaseItem(shopButtonIndex);       
    }
}
