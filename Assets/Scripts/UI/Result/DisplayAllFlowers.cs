using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEditor;
using DG.Tweening;
using System.Threading;
using UnityEngine.UI;
using System.Linq;

public class DisplayAllFlowers : MonoBehaviour
{
    private List<Sprite> _flowerSprites;
    [SerializeField] private GameObject _gridPrefab;
    [SerializeField] private Sprite testsp;
    [SerializeField] private double _timeSpan;

    public async UniTask StartDisplay(CancellationTokenSource cancellationTokenSource)
    {
        // GameManagerから咲いた花のデータを取得してBloomスプライトのリストに変換
        _flowerSprites = GameManager.Instance.GetBloomedFlowers()
            .Select(flowerData => flowerData.Sprite.Bloom)
            .ToList();

        GameObject grid = Instantiate(_gridPrefab, transform);

        // 実際に咲いた花の数を使用（テスト用の27ではなく）
        int count = _flowerSprites.Count;
        List<GameObject> sprites = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject imageObj = new GameObject("FlowerSprite_" + i);
            imageObj.transform.SetParent(grid.transform);
            Image image = imageObj.AddComponent<Image>();
            
            // 実際の花のスプライトを使用
            image.sprite = _flowerSprites[i];

            // ぽこっと出るアニメーション（スケール0→1.2→1）
            AudioManager.I.PlaySE(SEType.FlowerAppear);
            imageObj.transform.localScale = Vector3.zero;
            imageObj.transform.DOScale(1.2f, 0.15f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => imageObj.transform.DOScale(1f, 0.1f).SetEase(Ease.OutQuad));

            sprites.Add(imageObj);

            Handheld.Vibrate();

            try
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(_timeSpan), cancellationToken: cancellationTokenSource.Token);
            }
            catch (System.OperationCanceledException)
            {
                // キャンセルされたら残りを一気に表示
                for (int j = i + 1; j < count; j++)
                {
                    GameObject instantImageObj = new GameObject("FlowerSprite_" + j);
                    instantImageObj.transform.SetParent(grid.transform);
                    Image instantImage = instantImageObj.AddComponent<Image>();
                    
                    // 実際の花のスプライトを使用
                    instantImage.sprite = _flowerSprites[j];

                    instantImageObj.transform.localScale = Vector3.zero;
                    instantImageObj.transform.DOScale(1.2f, 0.15f)
                        .SetEase(Ease.OutBack)
                        .OnComplete(() => instantImageObj.transform.DOScale(1f, 0.1f).SetEase(Ease.OutQuad));

                    sprites.Add(instantImageObj);
                }

                Handheld.Vibrate();
                break;
            }
        }
    }

    public CancellationTokenSource _cts;

    public void StartDisplayWithCancel()
    {
        _cts = new CancellationTokenSource();
        StartDisplay(_cts).Forget();
    }

    public void CancelWaiting()
    {
        if (_cts != null)
        {
            _cts.Cancel();
        }
    }

    /// <summary>
    /// 使用済みの花のスプライトをセットする
    /// </summary>
    /// <param name="spr"></param>
    public void SetFlowerSprites(List<Sprite> spr)
    {
        _flowerSprites = spr;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DisplayAllFlowers))]
public class DisplayAllFlowersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DisplayAllFlowers displayAllFlowers = (DisplayAllFlowers)target;

        if (GUILayout.Button("Start Display"))
        {
            displayAllFlowers.StartDisplayWithCancel();
        }

        if (GUILayout.Button("Cancel Waiting"))
        {
            displayAllFlowers.CancelWaiting();
        }
    }
}
#endif
