using UnityEngine;
using System.Collections;

public class SpawnEnemys : MonoBehaviour
{
    [Header("��������� ������")]
    public GameObject enemyPrefab; // ������ ����� ��� ������
    public Transform spawnPoint;   // ����� ������
    public float spawnInterval = 5f;
    public int maxEnemies = 10;
    public bool spawnOnStart = true;

    [Header("������������")]
    public EnemyConfigurator configurator; // ������ �� ������������

    [Header("��������� ��������")]
    public bool randomizePosition = false;
    public Vector2 randomOffset = new Vector2(1f, 0f);

    private int currentEnemies = 0;
    private Coroutine spawnCoroutine;

    void Start()
    {
        if (spawnOnStart)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnSingleEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSingleEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("�� �������� ������ �����!", this);
            return;
        }

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;

        if (randomizePosition)
        {
            spawnPosition += new Vector3(
                Random.Range(-randomOffset.x, randomOffset.x),
                Random.Range(-randomOffset.y, randomOffset.y),
                0
            );
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemies++;

        if (configurator != null)
        {
            configurator.ConfigureEnemy(enemy);
        }
    }

    public void EnemyDestroyed()
    {
        currentEnemies--;
    }
}