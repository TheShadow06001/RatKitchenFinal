using UnityEngine;

public class MenuStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.CheckState<MenuState>())
        {
            GameManager.Instance.SwitchState<MenuState>();
        }

    }
}
