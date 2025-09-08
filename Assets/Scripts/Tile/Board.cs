using UnityEngine;
using ProtGardening;
using System;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour
{
    // 正六角形状に配置する際の半径
    [Range(1, 5)]
    public int Radius;
    // グリッド座標(u, v)
    // u: 中心から見た横方向のタイル数
    // v: 中心から見た縦方向のタイル数
    private int _u => 2 * (Radius - 1);
    private int _v => (Radius - 1);

    private List<Tile> _allTiles = new List<Tile>();

    public void Initialize(Tile tilePrefab, OperationManager operationManager)
    {
        Debug.Log($"u: {_u}, v: {_v}");
        // タイルの生成と初期化
        for (int y = _v; y >= -_v; y--)
        {
            for (int x = -(_u - Mathf.Abs(y)); x <= (_u - Mathf.Abs(y)); x += 2)
            {
                var tile = Instantiate(tilePrefab, new Vector2(x * Constants.TILE_INTERVAL_X, y * Constants.TILE_INTERVAL_Y), Quaternion.identity, this.transform);
                tile.Initialize(new Vector2Int(x, y), TileState.Normal);
                _allTiles.Add(tile);

                // タイルクリック時のイベントにOperationManagerのハンドラを登録
                tile.gameObject.GetComponent<TileTapHandler>().AddListener(operationManager.OnTappedHandler);
            }
        }

        // 隣接するタイルの取得
        foreach (var tile in _allTiles)
        {
            var currentCoord = tile.GetCoord();
            var adjacentTiles = new List<Tile>();

            // 6方向の隣接タイルをチェック
            foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
            {
                var offset = direction.ToVector2Int();
                if (TryGetTileAt(currentCoord + offset, out var adjacentTile))
                {
                    adjacentTiles.Add(adjacentTile);
                }
            }

            // 隣接タイルをセット
            tile.SetAdjacentTiles(adjacentTiles);
        }
    }

    public List<Tile> GetAllTiles()
    {
        return _allTiles;
    }

    // 花のグレード順にタイルを取得（花がない場合は0として扱う）
    public List<Tile> GetOrderedTilesByGrade()
    {
        return _allTiles.OrderBy(t => t.GetCurrentFlower()?.GetGrade() ?? 0).ToList();
    }

    // グリッド座標からタイルを取得
    public bool TryGetTileAt(Vector2Int coord, out Tile tile)
    {
        tile = _allTiles.FirstOrDefault(t => t.GetCoord() == coord);
        return tile != null;
    }
}
