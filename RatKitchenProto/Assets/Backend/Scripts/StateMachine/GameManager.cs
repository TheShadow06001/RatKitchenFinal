using System.Collections.Generic;
using UnityEngine;

public class GameManager : StateMachine
{
    public static GameManager Instance;

    #region Singleton

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        SwitchState<PlayingState>();
    }

    private void Update()
    {
        UpdateStateMachine();
    }
}
