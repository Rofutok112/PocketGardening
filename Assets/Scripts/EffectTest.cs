using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    void Start()
    {
        EffectManager.I.PlayEffect(EffectType.tmp, new Vector2(1, 3));
        print("Play Effect command sent");
    }
}
