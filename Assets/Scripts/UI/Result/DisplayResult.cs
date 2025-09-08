using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DisplayResult : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    private DisplayAllFlowers displayAllFlowers;
    private DisplayScore displayScore;
    [SerializeField] private float waitSeconds = 1.5f;

    private void Start()
    {
        displayAllFlowers = GetComponentInChildren<DisplayAllFlowers>();
        displayScore = GetComponentInChildren<DisplayScore>();
    }

    /// <summary>
    /// ゲーム終了時イベントハンドラ
    /// </summary>
    /// <returns></returns>
    public void DisplayResultPanel()
    {
        ShowResultSequenceAsync().Forget();
    }

    public async UniTask ShowResultSequenceAsync()
    {
        resultPanel.SetActive(true);

        // 初期位置を画面上部に
        RectTransform rt = resultPanel.GetComponent<RectTransform>();
        Vector3 startPos = rt.anchoredPosition;
        rt.anchoredPosition = new Vector2(startPos.x, startPos.y + 600f);

        CanvasGroup cg = resultPanel.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = resultPanel.AddComponent<CanvasGroup>();
        }
        cg.alpha = 0f;

        // 上から落ちてきてフェードイン
        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOAnchorPos(startPos, 0.5f).SetEase(Ease.OutCubic))
           .Join(cg.DOFade(1f, 0.5f));
        await seq.AsyncWaitForCompletion();

        // 数秒待機
        await UniTask.Delay(System.TimeSpan.FromSeconds(waitSeconds));

        // DisplayAllFlowersのStartDisplayを実行し、完了まで待機
        var cts = new System.Threading.CancellationTokenSource();
        await displayAllFlowers.StartDisplay(cts);

        // スコア表示更新
        displayScore.UpdateScore();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DisplayResult))]
public class DisplayResultEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DisplayResult displayResult = (DisplayResult)target;

        if (GUILayout.Button("Show Result Sequence"))
        {
            displayResult.ShowResultSequenceAsync().Forget();
        }
    }
}
#endif
