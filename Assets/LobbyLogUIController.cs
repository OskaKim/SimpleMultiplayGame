using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LobbyLogUIController : MonoBehaviour {
    [SerializeField] private UIDocument LogUIDocument;
    [SerializeField] private int numOfLogVisualElement;
    private readonly List<Label> logLabels = new List<Label>();

    private void Start() {
        var rootVisualElement = LogUIDocument.rootVisualElement;
        for (int i = 0; i < numOfLogVisualElement; ++i) {
            logLabels.Add(rootVisualElement.Q<Label>(name: $"Log{i}"));
        }
    }

    public void SetLog(int targetLogIndex, string text) {
        logLabels[targetLogIndex].text = text;
    }
}
