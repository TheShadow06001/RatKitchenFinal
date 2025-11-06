using UnityEngine;
using UnityEngine.UI;

public class S_TabManager : MonoBehaviour
{
    [Header("Tabs")] 
    public GameObject[] Tabs;
    public Image[] TabButtons;
    [Header("Tab Sprites and Sizes")] 
    public Sprite InactiveTabBG, ActiveTabBG;
    public Vector2 InactiveTabSize, ActiveTabSize;
    [Header("Return Button")] 
    public GameObject OptionsMenu;

    public void SwitchToTab(int TabID)
    {
        foreach (var go in Tabs) go.SetActive(false);
        Tabs[TabID].SetActive(true);

        foreach (var Img in TabButtons)
        {
            Img.sprite = InactiveTabBG;
            Img.rectTransform.sizeDelta = InactiveTabSize;
        }

        TabButtons[TabID].sprite = ActiveTabBG;
        TabButtons[TabID].rectTransform.sizeDelta = ActiveTabSize;
    }

    public void Return()
    {
        OptionsMenu.SetActive(false);
    }
}