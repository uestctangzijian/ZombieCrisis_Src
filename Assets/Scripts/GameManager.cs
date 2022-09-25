using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;

    public static event Action OnPause;
    public static event Action onResume;

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            if (!isPaused)
            {
                Time.timeScale = 0.0f;
                isPaused = true;
                gameOverText.gameObject.SetActive(true);
                gameOverText.text = "Paused!";
                OnPause?.Invoke();
            } else
            {
                Time.timeScale = 1.0f;
                isPaused = false;
                gameOverText.gameObject.SetActive(false);
                gameOverText.text = "";
                onResume?.Invoke();
            }
        }
    }
}
