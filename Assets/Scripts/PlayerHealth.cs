using System;
using NUnit.Compatibility;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    [SerializeField] GameObject live3;
    [SerializeField] GameObject live2;
    [SerializeField] GameObject live1;
    [SerializeField] GameObject live0;

    private void Start()
    {
        live0.SetActive(false);
        live1.SetActive(false);
        live2.SetActive(false);
        live3.SetActive(true);
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }
    }
    public void ChangeLives(int damage)
    {
        GameObject[] Lives = new GameObject[] { live0, live1, live2, live3 };

        Lives[currentHealth].SetActive(false);
        Lives[currentHealth - damage].SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("damage");
        ChangeLives(damage);
        currentHealth -= damage;


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

