using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string NameScene;
    public void StartGame()
    {
        SceneManager.LoadScene(NameScene); // ����� ��� ����� � ����� �����
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
        Debug.Log("���� ���������!");
        Application.Quit(); // �������� ������ � ������, �� � ���������
    }
}
