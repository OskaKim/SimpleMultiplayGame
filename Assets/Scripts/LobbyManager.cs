using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour {
    [SerializeField] string lobbyId;

    private void Start() {
        UnityServices.InitializeAsync();
        Locator.Get.Provide(new Identity(OnAuthSignIn));
    }
    private void OnAuthSignIn() {
        Debug.Log("OnAuthSignIn");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            LobbyUser lobbyUser = new LobbyUser();
            lobbyUser.DisplayName = "hoge";
            CreateLobbyAsync("lobby_name", 2, false, lobbyUser, (Lobby lobby) => {
                lobbyId = lobby.Id;
                Debug.Log("success");
            }, () => {
                Debug.Log("failed");
            });
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            LobbyAPIInterface.DeleteLobbyAsync(lobbyId, ()=> {
                Debug.Log("delete lobby finished");
            });
        }
    }
    public void CreateLobbyAsync(string lobbyName, int maxPlayers, bool isPrivate, LobbyUser localUser, Action<Lobby> onSuccess, Action onFailure) {
        //if (!m_rateLimitHost.CanCall()) {
        //    onFailure?.Invoke();
        //    UnityEngine.Debug.LogWarning("Create Lobby hit the rate limit.");
        //    return;
        //}

        string uasId = AuthenticationService.Instance.PlayerId;
        LobbyAPIInterface.CreateLobbyAsync(uasId, lobbyName, maxPlayers, isPrivate, CreateInitialPlayerData(localUser), OnLobbyCreated);

        void OnLobbyCreated(Lobby response) {
            if (response == null)
                onFailure?.Invoke();
            else
                onSuccess?.Invoke(response); // The Create request automatically joins the lobby, so we need not take further action.
        }
    }
    private static Dictionary<string, PlayerDataObject> CreateInitialPlayerData(LobbyUser player) {
        Dictionary<string, PlayerDataObject> data = new Dictionary<string, PlayerDataObject>();
        PlayerDataObject dataObjName = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, player.DisplayName);
        data.Add("DisplayName", dataObjName);
        return data;
    }

}
