using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ProtGardening;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TileManager TileManager;
    public Board BoardPrefab;
    public Tile TilePrefab;
    public UnityEvent OnTurnEnd;
    public int Money { get; private set; } = 10;
    public int Score { get; set; } = 0;
    public int Day { get; set; } = 1;
    public int MaxDay { get; set; } = 30;
    public UnityEvent OnMoneyChanged;
    public UnityEvent OnScoreChanged;
    public UnityEvent OnGameEnd;

    // 一度咲いた花を重複せずに保存するHashSet
    private HashSet<FlowerData> _bloomedFlowers = new HashSet<FlowerData>();

    private void Start()
    {
        OnTurnEnd.AddListener(TileManager.UpdateBoard);
        TileManager.GenerateBoard(BoardPrefab, TilePrefab);
        AudioManager.I.PlayBGM(BGMType.IngameTheme);
    }

    public void TriggerOnTurnEnd()
    {
        AudioManager.I.PlaySE(SEType.DayEnd);
        OnTurnEnd?.Invoke();
        if (Day < MaxDay)
        {
            Day++;
        }
        else
        {
            // ゲーム終了処理
            Debug.Log("Game Over");
            // ここでリザルト画面に遷移するなどの処理を行う
            OnGameEnd?.Invoke();
        }
    }

    public void CalcScore(Flower flower)
    {
        FlowerGrade grade = flower.GetGrade();

        // Bloom状態の花を重複せずに保存
        if (flower.GetFlowerState() == FlowerState.Bloom)
        {
            _bloomedFlowers.Add(flower.GetFlowerData());
            Debug.Log($"新しい花が咲きました: {flower.GetFlowerData().name}");
            Debug.Log($"咲いた花の種類数: {_bloomedFlowers.Count}");
        }

        switch (grade)
        {
            case FlowerGrade.Low:
                Score += 5;
                break;
            case FlowerGrade.Mid:
                Score += 10;
                break;
            case FlowerGrade.High:
                Score += 50;
                break;
        }

        OnScoreChanged?.Invoke();
        Debug.Log($"Score: {Score}");
    }

    /// <summary>
    /// 一度咲いた花のリストを取得する（重複なし）
    /// </summary>
    /// <returns>咲いた花のFlowerDataリスト</returns>
    public List<FlowerData> GetBloomedFlowers()
    {
        return _bloomedFlowers.ToList();
    }

    /// <summary>
    /// 咲いた花の種類数を取得する
    /// </summary>
    /// <returns>咲いた花の種類数</returns>
    public int GetBloomedFlowerCount()
    {
        return _bloomedFlowers.Count;
    }

    public void AddMoney(int amount)
    {
        if (amount > 0)
        {
            Money += amount;
            OnMoneyChanged?.Invoke();
        }
        else
        {
            Debug.LogWarning("GameManager: 無効な値" + amount);
        }
    }

    public void SpendMoney(int amount)
    {
        if (amount > 0 && amount <= Money)
        {
            Money -= amount;
            OnMoneyChanged?.Invoke();
            AudioManager.I.PlaySE(SEType.Purchase);
        }
        else
        {
            Debug.LogWarning("GameManager: 無効な値" + amount);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
