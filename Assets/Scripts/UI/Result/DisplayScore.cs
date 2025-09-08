using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    
    public void UpdateScore()
    {
        scoreText.text = "Score: " + GameManager.Instance.Score.ToString();
        
        // ぽわっとするアニメーション
        scoreText.transform.localScale = Vector3.zero;
        scoreText.transform.DOScale(1.2f, 0.3f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => scoreText.transform.DOScale(1f, 0.15f).SetEase(Ease.OutQuad));
    }
}
