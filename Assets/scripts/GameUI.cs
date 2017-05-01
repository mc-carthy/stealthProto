using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

	[SerializeField]
    private GameObject gameLoseUI, gameWinUI;
    private bool gameIsOver;

    private void Start()
    {
        Guard.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLevel += ShowGameWinUI;
    }

    private void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }
        }
    }

    private void ShowGameWinUI()
    {
        OnGameOver(gameWinUI);
    }

    private void ShowGameLoseUI()
    {
        OnGameOver(gameLoseUI);
    }

    private void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLevel += ShowGameWinUI;
    }

}
