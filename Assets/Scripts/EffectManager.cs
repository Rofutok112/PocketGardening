using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EffectType{
    tmp
}

public class EffectManager : MonoBehaviour
{
    [System.Serializable]
    struct Effect
    {
        public EffectType label;
        public GameObject effectPrefab;
    }
    [SerializeField]
    private List<Effect> effects;

    private static EffectManager instance;
    public static EffectManager I
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
        GameObject obj = new GameObject("EffectManager");
        instance = obj.AddComponent<EffectManager>();
        DontDestroyOnLoad(obj);
    }

    public void PlayEffect(EffectType effectType, Vector2 location){
        Effect effect = effects.Find(e => e.label == effectType);
        if (effect.effectPrefab != null)
        {
            Instantiate(effect.effectPrefab, new Vector3(location.x, location.y, 0), Quaternion.identity);
            Debug.Log($"Effect '{effectType}' played at {location}.");
        }
        else
        {
            Debug.LogWarning($"Effect Prefab '{effectType}' not found.");
            Debug.LogWarning($"Effect '{effectType}' not found.");
        }
    }
}
