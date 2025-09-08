using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule_ButtonController : MonoBehaviour
{
    [SerializeField] GameObject beforePanel;
    [SerializeField] GameObject afterPanel;

    public void OnClickRightButton()
    {
        Debug.Log("Right button clicked");
        AudioManager.I.PlaySE(SEType.ButtonClick);
        this.gameObject.SetActive(false);
        afterPanel.SetActive(true);
    }
    public void OnClickLeftButton()
    {
        Debug.Log("Left button clicked");
        AudioManager.I.PlaySE(SEType.ButtonClick);
        beforePanel.SetActive(true);
    }
}
