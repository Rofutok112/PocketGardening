using UnityEngine;
using System.Collections.Generic;
using ProtGardening;

public class Tile : MonoBehaviour
{
    public Flower flowerPrefab;

    private Vector2Int _coord;
    private TileState _state;
    private Flower _currentFlower;
    private List<Tile> _adjacentTiles;

    public bool IsEmpty => _currentFlower == null;
    public bool IsBloom => _currentFlower.GetFlowerState() == FlowerState.Bloom;

    public void Initialize(Vector2Int coord, TileState state)
    {
        _coord = coord;
        _state = state;
    }

    // 種・芽の状態を成長させる
    public bool Water()
    {
        if (!IsEmpty && !IsBloom)
        {
            _currentFlower.Grow();
            return true;
        }
        return false;
    }

    public void Dig()
    {
        _currentFlower = null;
    }

    // 花を売却する
    public bool Sell()
    {
        if (!IsEmpty && IsBloom)
        {
            int sellPrice = _currentFlower.GetFlowerData().SellPrice;
            // currentFlowerを売却する
            GameManager.Instance.AddMoney(sellPrice);
            Destroy(_currentFlower.gameObject);
            _currentFlower = null;
            AudioManager.I.PlaySE(SEType.Cutting);
            CoinEffect.Instance.ShowCoinWithPlus(transform.position, sellPrice);
            return true;
        }
        return false;
    }

    public bool Seed(FlowerData flowerData)
    {
        if (IsEmpty)
        {
            MakeFlower(flowerData, FlowerState.Seed);
            return true;
        }
        return false;
    }

    // 花を生成するメソッド
    public void MakeFlower(FlowerData flowerData, FlowerState flowerState)
    {
        var flower = Instantiate(flowerPrefab, transform.position, Quaternion.identity);
        flower.Initialize(flowerData, flowerState);
        SetCurrentFlower(flower);
    }

    public Vector2Int GetCoord()
    {
        return _coord;
    }

    public TileState GetState()
    {
        return _state;
    }

    public void SetState(TileState state)
    {
        _state = state;
    }

    public List<Tile> GetAdjacentTiles()
    {
        return _adjacentTiles;
    }

    public void SetAdjacentTiles(List<Tile> adjacentTiles)
    {
        _adjacentTiles = adjacentTiles;
    }

    public Flower GetCurrentFlower()
    {
        return _currentFlower;
    }

    public void SetCurrentFlower(Flower flower)
    {
        _currentFlower = flower;
    }
}