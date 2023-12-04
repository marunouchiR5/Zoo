using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private Button newGameButton;
    private Button quitButton;
    private Button instructionsButton;
    private VisualElement instructionsPanel;
    private UIDocument uiDocument;

    private ScrollView logScrollView; // ListView
    private Button backButton; // BackButton

    private RuleSet instructionsRuleSet;

    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();

        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;

            // Find buttons and panels
            newGameButton = root.Q<Button>("new-game");
            quitButton = root.Q<Button>("quit");
            instructionsButton = root.Q<Button>("instructions");
            instructionsPanel = root.Q<VisualElement>("LogView");

            // Find the ListView and BackButton
            logScrollView = instructionsPanel.Q<ScrollView>("log-scroll-view");
            backButton = instructionsPanel.Q<Button>("back--button");

            // Set up button click events
            if (newGameButton != null)
                newGameButton.clicked += OnNewGameClicked;
            if (quitButton != null)
                quitButton.clicked += OnQuitClicked;
            if (instructionsButton != null)
                instructionsButton.clicked += OnInstructionsClicked;
            if (backButton != null)
                backButton.clicked += OnBackClicked;
        }

        // Load the instructions RuleSet
        instructionsRuleSet = Resources.Load<RuleSet>("GameData/Instructions/Game Instruction");
        if (instructionsRuleSet == null)
        {
            Debug.LogError("Instructions RuleSet not found in Resources/GameData/Instructions/Game Instruction");
        }
    }

    private void OnNewGameClicked()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnInstructionsClicked()
    {
        if (instructionsPanel != null)
        {
            // Toggle the visibility of the instructions panel
            instructionsPanel.style.display = instructionsPanel.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        }

        // Populate the ScrollView with the instructionsRuleSet
        PopulateScrollView();
    }

    private void OnBackClicked()
    {
        // Logic to handle back button click
        instructionsPanel.style.display = DisplayStyle.None;
    }

    private void PopulateScrollView()
    {
        if (instructionsRuleSet != null)
        {
            logScrollView.Clear();

            // Add an empty element at the beginning for margin
            var topMarginElement = new VisualElement();
            topMarginElement.AddToClassList("margin-element");
            logScrollView.Add(topMarginElement);

            var setNameLabel = new Label(instructionsRuleSet.SetName);
            setNameLabel.AddToClassList("set-name");
            logScrollView.Add(setNameLabel);

            VisualElement paragraphElement = null;
            string lastSpeaker = null;
            foreach (var element in instructionsRuleSet.Elements)
            {
                if (lastSpeaker == null || element.Speaker != lastSpeaker)
                {
                    if (paragraphElement != null)
                    {
                        logScrollView.Add(paragraphElement);
                    }

                    paragraphElement = new VisualElement();
                    paragraphElement.AddToClassList("paragraph-entry");

                    var speakerLabel = new Label(element.Speaker);
                    speakerLabel.AddToClassList("speaker-label");
                    paragraphElement.Add(speakerLabel);
                    lastSpeaker = element.Speaker;
                }

                var contentLabel = new Label(element.Content);
                contentLabel.AddToClassList("content-label");
                paragraphElement.Add(contentLabel);
            }

            if (paragraphElement != null)
            {
                logScrollView.Add(paragraphElement);
            }

            // Add an empty element at the end for margin
            var bottomMarginElement = new VisualElement();
            bottomMarginElement.AddToClassList("margin-element");
            logScrollView.Add(bottomMarginElement);
        }
        else
        {
            Debug.LogError("No RuleSet provided for instructions");
        }
    }

    private void OnDestroy()
    {
        // Unregister button click events
        if (newGameButton != null)
            newGameButton.clicked -= OnNewGameClicked;
        if (quitButton != null)
            quitButton.clicked -= OnQuitClicked;
        if (instructionsButton != null)
            instructionsButton.clicked -= OnInstructionsClicked;
        if (backButton != null)
            backButton.clicked -= OnBackClicked;
    }
}
