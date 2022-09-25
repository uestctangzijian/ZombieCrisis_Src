using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI multiplierText;
    [SerializeField]
    private int maxMultiplier = 125;

    public float multiplierDecayTime = 3f;  
    private float multiplierDecayCountdown; 

    private int score = 0;      // 当前得分
    private int multiplier = 1; // 连杀数

    // 连杀数增加，通知外部武器升级
    public static event Action<int> OnMultiplierIncreased;

    public int CurrentMultiplier => multiplier;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString("D12");

    }

    // Update is called once per frame
    void Update()
    {
        if (multiplier == 1) return;

        multiplierDecayCountdown -= Time.deltaTime;
        if (multiplierDecayCountdown <= 0f)
        {
            multiplierDecayCountdown = 0f;
            ResetMultiplierCountdown();
            multiplier--;
            UpdateMultiplierText();
        }
    }

    public void IncreseScore(int points)
    {
        if (multiplier == maxMultiplier)
        {
            ResetMultiplierCountdown();
            score += points * multiplier;
            scoreText.text = score.ToString();
            return;
        }
        multiplier++;
        score += points * multiplier;
        scoreText.text = score.ToString("D12");
        ResetMultiplierCountdown();
        OnMultiplierIncreased?.Invoke(multiplier);
        UpdateMultiplierText();
    }

    private void UpdateMultiplierText()
    {
        multiplierText.text = string.Format("x{0}", multiplier);
    }

    // 连杀数会随时间减少，减小的速度与连杀数有关，连杀数越大，减小的速度越大
    private void ResetMultiplierCountdown()
    {
        multiplierDecayCountdown = multiplierDecayTime - ((float)(multiplier - 1) / maxMultiplier * multiplierDecayTime);
    }
}
