using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    List<LevelInfo> levels;

    [SerializeField]
    List<EnemySpawner> enemySpawners;

    [SerializeField]
    private int curLevel = 0;

    [SerializeField]
    private EnemyManager enemyManager;

    [SerializeField]
    private InfoText infoText;

    [SerializeField]
    private TextMeshProUGUI gameOverText;

    [SerializeField]
    private float levelBetweenTime = 3f;

    private bool isGoingToNextLevel = false;

    private bool levelFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.Play("LevelStart");
        StartCoroutine(UpdateLevelFinish());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (levelFinished && !isGoingToNextLevel)
        {
            isGoingToNextLevel = true;
            StartCoroutine(GoToNextLevel());
        }
    }

    private IEnumerator UpdateLevelFinish()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            bool flag = true;
            foreach (EnemySpawner s in enemySpawners)
            {
                if (!s.hasFinishedSpawning)
                {
                    flag = false;
                    break;
                }
            }
            levelFinished = flag && enemyManager.CurrentEnemiesCount == 0;
        }
    }

    // 下一关
    private IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(levelBetweenTime);

        curLevel++;
        if (curLevel == levels.Count)
        {
            GameFinished("Congratulations! You Finished All Levels!");
            yield return null;
        }

        LevelInfo level = levels[curLevel];
        // 普通僵尸
        for (int i = 0; i < enemySpawners.Count; i++)
        {
            enemySpawners[i].enmeyWaves.Clear();
            EnemyWave wave = new EnemyWave();
            wave.EnemyType = EnemyType.Zombie;
            wave.Amount = level.enemyAmount / enemySpawners.Count;
            wave.TimeBetweenSpawn = level.ememySpawnRate + Random.Range(-0.5f, 0.5f);
            enemySpawners[i].enmeyWaves.Add(wave);
        }

        // Boss
        for (int i = 0; i < level.bossAmount; i++)
        {
            int index = Random.Range(0, enemySpawners.Count - 1);
            EnemyWave wave = new EnemyWave();
            wave.EnemyType = EnemyType.Boss;
            wave.Amount = 1;
            wave.TimeBetweenSpawn = level.bossSpawnRate + Random.Range(-0.5f, 0.5f);
            enemySpawners[index].enmeyWaves.Add(wave);
        }

        foreach(EnemySpawner enemy in enemySpawners)
        {
            enemy.Refresh();
        }

        AudioManager.Instance.Play("LevelStart");
        infoText.AddLevelTextEntry(level.levelIndex);

        levelFinished = false;
        isGoingToNextLevel = false;
    }

    // 游戏结束
    public void GameFinished(string text)
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = text;

        Invoke("Restart", 5f);
    }

    private void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
