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
    private VisualElement rootVisualElement;

    private void Awake() {
        rootVisualElement = this.GetComponent<UIDocument>().rootVisualElement;
        var scrollView = rootVisualElement.Q<ScrollView>();

        // todo : 로비를 쿼리하고 결과 수 만큼만 생성
        for (int i = 0; i < 100; ++i) {
            var lobbyElement = new LobbyElementComponent();
            scrollView.Add(lobbyElement);
            lobbyElement.LobbyNameLabel = "name sample";
        }
    }
}
