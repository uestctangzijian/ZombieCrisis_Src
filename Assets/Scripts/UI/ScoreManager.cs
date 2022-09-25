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

    private int score = 0;      // ��ǰ�÷�
    private int multiplier = 1; // ��ɱ��

    // ��ɱ�����ӣ�֪ͨ�ⲿ��������
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

    // ��ɱ������ʱ����٣���С���ٶ�����ɱ���йأ���ɱ��Խ�󣬼�С���ٶ�Խ��
    private void ResetMultiplierCountdown()
    {
        multiplierDecayCountdown = multiplierDecayTime - ((float)(multiplier - 1) / maxMultiplier * multiplierDecayTime);
    }
}
