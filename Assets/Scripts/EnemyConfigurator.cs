using UnityEngine;

public class EnemyConfigurator : MonoBehaviour
{
    [Header("Настройки патрулирования")]
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float waitTime = 3f;
    public float distanceThreshold = 0.1f;
    public LayerMask obstacleLayer;
    public int damageToPlayer = 1;
    public Animator animator;
    public bool flipSprite = true;
    public bool destroyAtLastPoint = false; // Новая галочка для уничтожения

    public void ConfigureEnemy(GameObject enemy)
    {
        var patrol = enemy.GetComponent<PatrollingEnemy>();
        if (patrol == null)
        {
            Debug.LogError("У врага нет компонента PatrollingEnemy!", enemy);
            return;
        }

        // Копируем настройки
        patrol.patrolPoints = new Transform[patrolPoints.Length];
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrol.patrolPoints[i] = patrolPoints[i];
        }

        patrol.speed = speed;
        patrol.waitTime = waitTime;
        patrol.distanceThreshold = distanceThreshold;
        patrol.obstacleLayer = obstacleLayer;
        patrol.damageToPlayer = damageToPlayer;
        patrol.animator = animator;
        patrol.flipSprite = flipSprite;
        patrol.destroyAtLastPoint = destroyAtLastPoint; // Передаем настройку уничтожения
    }

    // Метод для ручной настройки существующего врага
    [ContextMenu("Настроить всех врагов")]
    [System.Obsolete]
    public void ConfigureAllEnemies()
    {
        PatrollingEnemy[] enemies = FindObjectsOfType<PatrollingEnemy>();
        foreach (var enemy in enemies)
        {
            ConfigureEnemy(enemy.gameObject);
        }
        Debug.Log($"Настроено {enemies.Length} врагов");
    }
}