using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint; 
    public float shootInterval = 2f;
    public float bulletSpeed = 5f;
    public int bulletsPerWave = 12; 
    public bool fullCircle = true;  

    private float timeSinceLastShot;


    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shootInterval)
        {
            ShootWave();
            timeSinceLastShot = 0f;
        }
    }

    private void ShootWave()
    {
        float angleStep = fullCircle ? 360f / bulletsPerWave : 180f / (bulletsPerWave - 1);
        float startAngle = fullCircle ? 0f : -90f;

        for (int i = 0; i < bulletsPerWave; i++)
        {
            float angle = startAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = dir * bulletSpeed;
        }
    }
}
