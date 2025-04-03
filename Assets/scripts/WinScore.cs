using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI starText;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private TextMeshProUGUI totalText;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        starText.text = Score.score.ToString("000");
        killText.text = Enemy.killcounter.ToString("000");
        deathText.text = Death.deathcounter.ToString("000");
        totalText.text = (Score.score*10 + Enemy.killcounter*15 - Death.deathcounter*10).ToString();
    }
}
