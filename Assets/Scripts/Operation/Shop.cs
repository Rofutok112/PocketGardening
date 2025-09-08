using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ProtGardening;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    private const int SHOP_ITEM_COUNT = 3;
    private ShopGrade _currentShopGrade = ShopGrade.Low;
    private List<ShopItem> _currentShopItems = new List<ShopItem>();
    private int _reloadPrice = 5;
    private int[] _upgradePrices = new int[2] { 100, 500 };

    [SerializeField] private List<ShopItem> _lowGradeShopItemPool = new List<ShopItem>();
    [SerializeField] private List<ShopItem> _midGradeShopItemPool = new List<ShopItem>();
    [SerializeField] private List<ShopItem> _highGradeShopItemPool = new List<ShopItem>();
    [SerializeField] private OperationManager _operationManager;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private FlowerDataBase _allFlowerData;
    [SerializeField] private Image[] _shopImage;
    [SerializeField] private TextMeshProUGUI[] _priceTexts;
    [SerializeField] private Button[] _shopButtons;
    [SerializeField] private Button _reloadButton;
    [SerializeField] private TextMeshProUGUI _reloadPriceText;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private TextMeshProUGUI _upgradePriceText;

    private void Start()
    {   
        Shuffle();
        GameManager.Instance.OnMoneyChanged.AddListener(UpdateVisuals);
        _reloadPriceText.text = _reloadPrice.ToString();
        Debug.Log("CurrentShopGrade :" + (int)_currentShopGrade);
        _upgradePriceText.text = _upgradePrices[(int)_currentShopGrade].ToString();
        UpdateVisuals();

    }

    /// <summary>
    /// GameManagerのターン終了時に呼ばれる
    /// </summary>
    public void OnTurnEnd()
    {
        Shuffle();
    }

    public void Reload()
    {
        GameManager.Instance.SpendMoney(_reloadPrice);
        Shuffle();
    }

    public void UpdateVisuals()
    {
        int money = GameManager.Instance.Money;
        for (int i = 0; i < SHOP_ITEM_COUNT; i++)
        {
            _shopButtons[i].interactable = money >= _currentShopItems[i].Price;
        }

        _reloadButton.interactable = money >= _reloadPrice;
        _upgradeButton.interactable = money >= _upgradePrices[(int)_currentShopGrade];
    }

    public void Upgrade()
    {
        GameManager.Instance.SpendMoney(_upgradePrices[(int)_currentShopGrade]);
        _currentShopGrade++;
        _upgradePriceText.text = _upgradePrices[(int)_currentShopGrade].ToString();
    }

    private void Shuffle()
    {
        switch (_currentShopGrade)
        {
            case ShopGrade.Low:
                _currentShopItems = _lowGradeShopItemPool.OrderBy(x => Random.value).Take(SHOP_ITEM_COUNT).ToList();
                break;
            case ShopGrade.Mid:
                _currentShopItems = _midGradeShopItemPool.OrderBy(x => Random.value).Take(SHOP_ITEM_COUNT).ToList();
                break;
            case ShopGrade.High:
                _currentShopItems = _highGradeShopItemPool.OrderBy(x => Random.value).Take(SHOP_ITEM_COUNT).ToList();
                break;
        }

        DisplayShowcase();
        DisplayPrice();
    }

    

    /// <summary>
    /// Shopボタンが押されたときに呼ばれる
    /// </summary>
    /// <param name="btnIndex"></param>
    public void OnPurchaseItem(int btnIndex)
    {
        ShopItem item = _currentShopItems[btnIndex];
        if (item.Price <= GameManager.Instance.Money)
        {
            GameManager.Instance.SpendMoney(item.Price);
            if (item.OperationType == OperationType.Seeding)
            {
                // Seedingアイテムの効果を適用
                FlowerData flowerData = item.FlowerData;
                _operationManager.SetOperation(new Seeding(flowerData));
            }
            else if (item.OperationType == OperationType.Watering)
            {
                _operationManager.SetOperation(OperationInstances.Watering);
                //_inventory.AddOperationCount(item.OperationType, item.EffectAmount);
            }
        }
        else
        {
            Debug.Log("Not enough money to purchase: " + _currentShopItems[btnIndex].ItemName);
        }
    }

    public void DisableUnbuyableButtons(int _)
    {
        for (int i = 0; i < SHOP_ITEM_COUNT; i++)
        {
            if (_currentShopItems[i].Price > GameManager.Instance.Money)
            {
                _shopButtons[i].interactable = false;
            }
        }
    }

    private void DisplayShowcase()
    {
        for (int i = 0; i < SHOP_ITEM_COUNT; i++)
        {
            _shopImage[i].sprite = _currentShopItems[i].ItemSprite;
        }
    }

    private void DisplayPrice()
    {
        for (int i = 0; i < SHOP_ITEM_COUNT; i++)
        {
            _priceTexts[i].text = _currentShopItems[i].Price.ToString();
        }
    }
}
