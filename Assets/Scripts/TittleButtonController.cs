using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TittleButtonController : MonoBehaviour
{
    [SerializeField]GameObject titlePanel;

    public void OnClickStartButton()
    {
        Debug.Log("Start button clicked");
        AudioManager.I.PlaySE(SEType.ButtonClick);
        titlePanel.SetActive(false);
    }
}
