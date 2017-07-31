using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour {
    [SerializeField]
    private GameObject m_ActiveCanvas;

    [SerializeField]
    private GameObject m_InactiveCanvas;

    private void Start()
    {
        GameManager.Instance.ResetGame();
        SceneLoaderSingleManager.Instance.Reset();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        GameManager.Instance.m_backgroundMusic.Play();
        SceneManager.LoadScene(1);
    }

    public void SwapCanvas()
    {
        m_ActiveCanvas.SetActive(false);
        m_InactiveCanvas.SetActive(true);

        GameObject temp = m_ActiveCanvas;
        m_ActiveCanvas = m_InactiveCanvas;
        m_InactiveCanvas = temp;
    }
}
