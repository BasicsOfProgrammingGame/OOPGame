using UnityEngine;
using UnityEngine.SceneManagement;

public class GoNextLVL : MonoBehaviour
{
    public string LevelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(LevelName);
        }
       
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
