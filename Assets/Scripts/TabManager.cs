using UnityEngine;
using UnityEngine.UI;
public class TabManager : MonoBehaviour
{
    public GameObject[] tabs; // Assign your tab panels in the inspector

    public void ShowTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == index);
        }
    }
}