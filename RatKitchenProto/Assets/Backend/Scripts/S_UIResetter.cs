using UnityEngine;

public class S_UIResetter : MonoBehaviour
{
    public GameObject OptionsPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CheckState<PlayingState>())
        {
            OptionsPanel.SetActive(false);
        }
    }
}
