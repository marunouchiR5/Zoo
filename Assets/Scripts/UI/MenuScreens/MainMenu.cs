using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private Button newGameButton;
    private Button quitButton;
    private UIDocument uiDocument;

    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();

        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;

            // Find the New Game button
            newGameButton = root.Q<Button>("new-game");
            if (newGameButton != null)
            {
                newGameButton.clicked += OnNewGameClicked;
            }

            // Find the Quit button
            quitButton = root.Q<Button>("quit");
            if (quitButton != null)
            {
                quitButton.clicked += OnQuitClicked;
            }
        }
    }

    private void OnNewGameClicked()
    {
        // Load the "Game" scene
        SceneManager.LoadScene("Game");
    }

    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        // If running in the Unity editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a build
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        // Unregister button click events to prevent memory leaks
        if (newGameButton != null)
        {
            newGameButton.clicked -= OnNewGameClicked;
        }

        if (quitButton != null)
        {
            quitButton.clicked -= OnQuitClicked;
        }
    }
}
