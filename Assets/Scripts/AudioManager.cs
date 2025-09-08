using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    IngameTheme,
    TittleTheme,
    RuleTheme
}
public enum SEType
{
    ButtonClick,  //ボタン系
    ToolButtonClick,
    TileGradeUp,  //タイルグレードアップ
    Seeding,  //植物成長系
    SeedToSprout,
    SproutToBloom,
    Watering,   //ツールの使用系
    Digging,
    Cutting,
    ShopWindow,  //ウィンドウ系
    RecipeWindow,
    Sale,  //ショップ関係
    Purchase,
    Reload,
    ShopGradeUp,
    GameEnd,
    DayEnd,
    FlowerAppear
}

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource; // 背景音楽用
    [SerializeField] private AudioSource seSource;   // 効果音用

    [System.Serializable]
    struct SE
    {
        public SEType label;
        public AudioClip clip;
    }
    [System.Serializable]
    struct BGM
    {
        public BGMType label;
        public AudioClip clip;
    }

    [SerializeField]
    private List<SE> ses;
    [SerializeField]
    private List<BGM> bgms;

    public static AudioManager instance;
    public static AudioManager I
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private static void SetupInstance()
    {
        GameObject obj = new GameObject("AudioManager");
        instance = obj.AddComponent<AudioManager>();
        DontDestroyOnLoad(obj);
    }

    public void PlayBGM(BGMType bgmType)
    {
        BGM bgm = bgms.Find(b => b.label == bgmType);
        if (bgm.clip != null)
        {
            // 既に同じクリップが再生中の場合はスキップ
            if (bgmSource.isPlaying && bgmSource.clip == bgm.clip)
            {
                Debug.Log($"BGM '{bgmType}' is already playing.");
                return;
            }

            // 別のBGMが再生中の場合は停止
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }

            bgmSource.clip = bgm.clip;
            bgmSource.loop = true;
            bgmSource.Play();
            Debug.Log($"Playing BGM: {bgmType}");
        }
        else
        {
            Debug.LogWarning($"BGM clip '{bgmType}' not found.");
        }
    }

    public void PlaySE(SEType seType)
    {
        SE se = ses.Find(s => s.label == seType);
        if (se.clip != null)
        {
            seSource.clip = se.clip;
            seSource.Play();
        }
        else
        {
            Debug.LogWarning($"SE clip '{seType}' not found.");
        }
    }
}
