using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("1 level"); // ����� ��� ����� � ����� �����
    }

    public void ExitGame()
    {
        Debug.Log("���� ���������!");
        Application.Quit(); // �������� ������ � ������, �� � ���������
    }
}
