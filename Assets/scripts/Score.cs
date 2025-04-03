using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private AudioClip audioStar;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image prizeImage0;
    [SerializeField] private Image prizeImage1;
    [SerializeField] private Image prizeImage2;
    public static int score = 0;
    public static int prize = 0;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Star"))
        {
            score++;
            _scoreText.text = score.ToString();
            _audioSource.PlayOneShot(audioStar, 0.05f);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Prize"))
        {
            switch (prize)
            {
                case 0:
                    var tempColor0 = prizeImage0.color;
                    tempColor0.a = 1f;
                    prizeImage0.color = tempColor0;
                break;
                case 1:
                    var tempColor1 = prizeImage1.color;
                    tempColor1.a = 1f;
                    prizeImage1.color = tempColor1;
                break;
                case 2:
                    var tempColor2 = prizeImage2.color;
                    tempColor2.a = 1f;
                    prizeImage2.color = tempColor2;
                break;
            }
            prize++;
            _audioSource.PlayOneShot(audioStar, 0.05f);
            Destroy(other.gameObject);
        }
    }
}
