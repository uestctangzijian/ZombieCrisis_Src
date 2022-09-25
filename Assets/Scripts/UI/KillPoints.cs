using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillPoints : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private RectTransform canvasRect;
    [SerializeField]
    private TMP_Text textPrefab;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private ScoreManager score;
    [SerializeField]
    private float minDistanceToScoreText;
    [SerializeField]
    private float textMoveSpeed;

    private List<TMP_Text> currentlyMovingTexts = new List<TMP_Text>();

    private void OnEnable()
    {
        player.onEnemyKilled += ShowKillPointText;
        StartCoroutine(MoveAllKillPointTextsToScoreText());
    }

    private void OnDisable()
    {
        player.onEnemyKilled -= ShowKillPointText;
    }

    private void ShowKillPointText(EnemyController enemy)
    {
        TMP_Text text = Instantiate(textPrefab, transform);
        text.rectTransform.anchoredPosition = WorldtoCanvasPosition(new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z + 0.5f));
        text.text = string.Concat("+", enemy.points * score.CurrentMultiplier);
        currentlyMovingTexts.Add(text);
    }

    // 世界坐标系转化为canvas坐标系
    // https://answers.unity.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
    private Vector2 WorldtoCanvasPosition(Vector3 position)
    {
        Vector2 temp = cam.WorldToViewportPoint(position);

        temp.x *= canvasRect.sizeDelta.x;
        temp.y *= canvasRect.sizeDelta.y;

        temp.x -= canvasRect.sizeDelta.x * canvasRect.pivot.x;
        temp.y -= canvasRect.sizeDelta.y * canvasRect.pivot.y;

        return temp;
    }

    private IEnumerator MoveAllKillPointTextsToScoreText()
    {
        while(true)
        {
            if (currentlyMovingTexts.Count == 0)
            {
                yield return null;
            } else
            {
                for(int i = 0; i < currentlyMovingTexts.Count; i++)
                {
                    if (Vector3.Distance(currentlyMovingTexts[i].transform.position, score.transform.position) <= minDistanceToScoreText)
                    {
                        TMP_Text text = currentlyMovingTexts[i];
                        currentlyMovingTexts.RemoveAt(i);
                        Destroy(text);
                    }
                    else
                    {
                        currentlyMovingTexts[i].transform.position = Vector3.MoveTowards(currentlyMovingTexts[i].transform.position, score.transform.position, Time.deltaTime * textMoveSpeed);
                    }
                }
                yield return null;
            }
        }
    }
}
