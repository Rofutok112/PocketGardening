using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using TMPro;

/// <summary>
/// コインが飛ぶエフェクト
/// </summary>
public class CoinEffect : MonoBehaviour
{
    public static CoinEffect Instance { get; private set; }
    [SerializeField] private GameObject _coinPrefab;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 指定座標からコインを上にヒョイと出現させて消す＋隣に「+??」表示
    /// </summary>
    /// <param name="position">コインを出現させる座標</param>
    /// <param name="plusCount">増えたコインの数</param>
    public void ShowCoinWithPlus(Vector3 position, int plusCount)
    {
        GameObject coin = Instantiate(_coinPrefab, position, Quaternion.identity);
        TextMeshPro text = coin.GetComponentInChildren<TextMeshPro>();

        if (text != null)
        {
            text.text = $"+{plusCount}";
        }

        // コインを上に移動させて消す
        coin.transform.DOMoveY(position.y + 1f, 0.5f).OnComplete(() => Destroy(coin));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CoinEffect))]
public class CoinEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CoinEffect coinEffect = (CoinEffect)target;
        if (GUILayout.Button("Show Coin Effect"))
        {
            coinEffect.ShowCoinWithPlus(Vector3.zero, 10);
        }
    }
}
#endif
