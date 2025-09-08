using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TileTapHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Tile _tile;
    [SerializeField] private UnityEvent<Tile> _onTileTapped;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Tile tapped: {_tile.GetCoord()}");
        _onTileTapped?.Invoke(_tile);
    }

    public void AddListener(UnityAction<Tile> listener)
    {
        _onTileTapped.AddListener(listener);
    }

    public void RemoveListener(UnityAction<Tile> listener)
    {
        _onTileTapped.RemoveListener(listener);
    }
}