using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    void Start()
    {
        AudioManager.I.PlayBGM(BGMType.IngameTheme);
        print("Play BGM command sent");

        AudioManager.I.PlaySE(SEType.ButtonClick);
        print("Play SE command sent");
    }
    
}
