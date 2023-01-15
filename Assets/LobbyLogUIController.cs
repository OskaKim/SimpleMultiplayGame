using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyLogUIController : MonoBehaviour {
    public static event Action<string> OnChangeLobbyId;

    [SerializeField] private UIDocument LogUIDocument;
    [SerializeField] private int numOfLogVisualElement;
    private readonly List<Label> logLabels = new List<Label>();
    TextField lobbyIdTextField;
    Button joinLobbyButton;

    private void Awake() {
        var rootVisualElement = LogUIDocument.rootVisualElement;
        for (int i = 0; i < numOfLogVisualElement; ++i) {
            logLabels.Add(rootVisualElement.Q<Label>(name: $"Log{i}"));
        }
        lobbyIdTextField = rootVisualElement.Q<TextField>(name: "LobbyId");
        joinLobbyButton = rootVisualElement.Q<Button>(name: "JoinLobbyButton");
    }

    private void OnEnable() {
        joinLobbyButton.clicked += OnClickJoinButton;
    }

    private void OnDisable() {
        joinLobbyButton.clicked -= OnClickJoinButton;
    }

    private void OnClickJoinButton() {
        OnChangeLobbyId(lobbyIdTextField.text);
    }

    public void SetLog(int targetLogIndex, string text) {
        logLabels[targetLogIndex].text = text;
    }
}
