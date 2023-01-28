using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyElementComponent : VisualElement {
    private static class ClassNames {
        public static string LobbyName = "lobby_name_label";
    }

    private Label _lobbyNameLabel;

    public LobbyElementComponent() {
        _lobbyNameLabel = new Label() { name = "LobbyNameLabel" };
        _lobbyNameLabel.AddToClassList(ClassNames.LobbyName);
        Add(_lobbyNameLabel);
    }

    public string LobbyNameLabel {
        get => _lobbyNameLabel.text;
        set => _lobbyNameLabel.text = value;
    }
}

[RequireComponent(typeof(UIDocument))]
public class LobbyTopUIController : MonoBehaviour {
    public struct LobbyElementParams {
        public string Name { get; private set; }

        public LobbyElementParams(string name) {
            Name = name;
        }
    }

    public event Action onClickCreateRoomButton;
    public event Action onClickRefreshRoomButton;
    private VisualElement rootVisualElement;
    private ScrollView scrollView;
    private Button createRoomButton;
    private Button refreshRoomButton;

    private void Awake() {
        rootVisualElement = this.GetComponent<UIDocument>().rootVisualElement;
        scrollView = rootVisualElement.Q<ScrollView>();
        createRoomButton = rootVisualElement.Q<Button>(name: "CreateRoomButton");
        refreshRoomButton = rootVisualElement.Q<Button>(name: "RefreshButton");
    }

    private void OnEnable() {
        createRoomButton.clicked += OnClickCreateRoomButton;
        refreshRoomButton.clicked += OnClickRefreshRoomButton;
    }

    private void OnDisable() {
        createRoomButton.clicked -= OnClickCreateRoomButton;
        refreshRoomButton.clicked -= OnClickRefreshRoomButton;
    }

    public void RefreshLobbyList(List<LobbyElementParams> elementParamsList) {
        scrollView.ClearClassList();

        elementParamsList.ForEach((LobbyElementParams elementParams) => {
            var lobbyElement = new LobbyElementComponent();
            scrollView.Add(lobbyElement);
            lobbyElement.LobbyNameLabel = elementParams.Name;
        });
    }

    private void OnClickCreateRoomButton() {
        onClickCreateRoomButton();
    }

    private void OnClickRefreshRoomButton() {
        onClickRefreshRoomButton();
    }
}
