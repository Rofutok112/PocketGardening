using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 画面上に表示されているテキストを管理するクラス
/// </summary>
public class TopText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Start()
    {
        SetMoneyText();
        SetDayText();
        SetScoreText();
    }

    public void SetDayText()
    {
        if (_dayText != null)
        {
            _dayText.text = "Day " + GameManager.Instance.Day.ToString();
        }
    }

    public void SetMoneyText()
    {
        if (_moneyText != null)
        {
            _moneyText.text = GameManager.Instance.Money.ToString();
        }
    }


    public void SetScoreText()
    {
        if (_scoreText != null)
        {
            _scoreText.text = GameManager.Instance.Score.ToString();
        }
    }
}
