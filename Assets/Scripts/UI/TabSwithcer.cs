using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    public Transform[] Tabs;

    public void SwitchTab(int index)
    {
        Tabs[index].SetSiblingIndex(100);
    }
}