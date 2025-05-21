using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string NameScene;
    public void StartGame()
    {
        SceneManager.LoadScene(NameScene); // Укажи имя сцены с самой игрой
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Debug.Log("Игра завершена!");
        Application.Quit(); // Работает только в сборке, не в редакторе
    }
}
