using UnityEngine;
using ProtGardening;
using System.Linq;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public FlowerData initialFlowerData;
    public FlowerData secondFlowerData;
    public RecipeBook RecipeBook;
    public OperationManager OperationManager;

    private Board _board;

    public void GenerateBoard(Board boardPrefab, Tile tilePrefab)
    {
        _board = Instantiate(boardPrefab);
        _board.Initialize(tilePrefab, OperationManager);
        _board.gameObject.transform.position += new Vector3(0, 2.3f, 0);
        var tiles = _board.GetAllTiles();
        int r_1 = Random.Range(0, tiles.Count);
        int r_2 = Random.Range(0, tiles.Count);
        if (r_1 == r_2) r_2 = (r_2 + 1) % tiles.Count;
        tiles[r_1].MakeFlower(initialFlowerData, FlowerState.Seed);
        tiles[r_2].MakeFlower(secondFlowerData, FlowerState.Seed);
    }

    public void UpdateBoard()
    {
        Debug.Log("Update Board");
        // 花のグレード順にタイルを取得
        var orderedTiles = _board.GetOrderedTilesByGrade();
        Debug.Log($"Ordered Tiles Count: {orderedTiles.Count}");

        // 繁殖計画のプラン
        // key: 繁殖先タイル　value : 親タイル
        var reproductionPlan = new Dictionary<Tile, List<Tile>>();

        // 成長するタイルのリスト
        var growTileList = new List<Tile>();

        foreach (var tile in orderedTiles)
        {
            // 花が咲いているかチェック
            if (!tile.IsEmpty && tile.IsBloom)
            {
                Debug.Log("Add ReproductionPlan");
                // 周辺の空きタイルを取得し、ランダムに1つ選択 ( TODO : 肥沃な土地に生えている花は2つ )
                var candidates = tile.GetAdjacentTiles().Where(t => t.IsEmpty).ToList();

                // 候補がある場合、ランダムに選択して繁殖プランに追加
                while (candidates.Count > 0)
                {
                    Tile targetTile = candidates[Random.Range(0, candidates.Count)];

                    // 繁殖先タイルが未登録の場合は新規作成
                    if (!reproductionPlan.ContainsKey(targetTile))
                    {
                        reproductionPlan[targetTile] = new List<Tile> { tile };
                        break;
                    }
                    // 親が１つだけ登録されている場合、交配を試みる
                    else if (reproductionPlan[targetTile].Count == 1)
                    {
                        if (this.RecipeBook.TryHybridize(reproductionPlan[targetTile][0].GetCurrentFlower(), tile.GetCurrentFlower(), out var _))
                        {
                            // 交配成功
                            reproductionPlan[targetTile].Add(tile);
                            break;
                        }
                        else
                        {
                            // 交配失敗
                            candidates.Remove(targetTile);
                            continue;
                        }
                    }
                    else
                    {
                        // 既に親が2つ登録されている場合、別のタイルを選択
                        candidates.Remove(targetTile);
                        continue;
                    }
                }
            }
            // 花が種・芽の状態の場合は成長させるリストに追加
            else if (!tile.IsEmpty)
            {
                Debug.Log("Add GrowList");
                growTileList.Add(tile);
            }
        }

        // 最終的な植え付けアクション
        var finalPlantingActions = new Dictionary<Tile, FlowerData>();

        foreach (var plan in reproductionPlan)
        {
            Tile targetTile = plan.Key;
            var parents = plan.Value;

            if (parents.Count == 1)
            {
                finalPlantingActions[targetTile] = parents[0].GetCurrentFlower().GetFlowerData();
            }
            else if (parents.Count >= 2)
            {
                if (TryHybridizeFlowers(parents, out var hybridFlower))
                {
                    finalPlantingActions[targetTile] = hybridFlower;
                }
            }
        }

        // ここから実際に盤面を更新(のちにUniTaskやアニメーションを追加)
        foreach (var tile in orderedTiles)
        {
            // 成長アクションがある場合
            if (growTileList.Contains(tile))
            {
                tile.GetCurrentFlower().Grow();
            }

            // 植え付けアクションがある場合
            if (finalPlantingActions.ContainsKey(tile))
            {
                var flowerData = finalPlantingActions[tile];
                tile.MakeFlower(flowerData, FlowerState.Sprout);
                continue;
            }


        }
    }

    // 交配可能な組み合わせを探すメソッド
    private bool TryHybridizeFlowers(List<Tile> parents, out FlowerData hybridFlower)
    {
        for (int i = 0; i < parents.Count - 1; i++)
        {
            for (int j = i + 1; j < parents.Count; j++)
            {
                if (this.RecipeBook.TryHybridize(parents[i].GetCurrentFlower(), parents[j].GetCurrentFlower(), out hybridFlower))
                {
                    // 組み合わせが見つかったら、親から組み合わせを削除してループを抜ける
                    parents.RemoveAt(j);
                    parents.RemoveAt(i);
                    return true;
                }
            }
        }
        hybridFlower = null;
        return false;
    }


}
