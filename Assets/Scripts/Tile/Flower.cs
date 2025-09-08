using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtGardening;

public class Flower : MonoBehaviour
{
    private FlowerState _flowerState;
    private SpriteRenderer _spriteRenderer;
    private FlowerData _flowerData;
    private Vector3 _basePosition;
    private Vector3 _sproutPositionOffset = new Vector3(0, 0.06f, 0);
    private Vector3 _bloomPositionOffset = new Vector3(0, 0.25f, 0);

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _basePosition = transform.position;
    }

    private void UpdateVisuals()
    {
        switch (_flowerState)
        {
            case FlowerState.Seed:
                _spriteRenderer.sprite = _flowerData.Sprite.Seed;
                transform.position = _basePosition;
                break;
            case FlowerState.Sprout:
                _spriteRenderer.sprite = _flowerData.Sprite.Sprout;
                transform.position = _basePosition + _sproutPositionOffset;
                break;
            case FlowerState.Bloom:
                _spriteRenderer.sprite = _flowerData.Sprite.Bloom;
                transform.position = _basePosition + _bloomPositionOffset;
                break;
        }
    }

    public void Initialize(FlowerData flowerData, FlowerState flowerState)
    {
        _flowerData = flowerData;
        _flowerState = flowerState;
        UpdateVisuals();
    }

    public FlowerState GetFlowerState()
    {
        return _flowerState;
    }

    public FlowerGrade GetGrade()
    {
        return _flowerData.Grade;
    }

    public void Grow()
    {
        Debug.Log("Grow");
        // 成長処理
        if (_flowerState == FlowerState.Seed)
        {
            _flowerState = FlowerState.Sprout;
        }
        else if (_flowerState == FlowerState.Sprout)
        {
            _flowerState = FlowerState.Bloom;
            GameManager.Instance.CalcScore(this);
        }

        UpdateVisuals();
    }


    public FlowerData GetFlowerData()
    {
        return _flowerData;
    }
}
