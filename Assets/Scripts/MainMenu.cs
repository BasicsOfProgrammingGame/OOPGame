using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("2 level"); // Укажи имя сцены с самой игрой
    }

    public void ExitGame()
    {
        Debug.Log("Игра завершена!");
        Application.Quit(); // Работает только в сборке, не в редакторе
    }
}
