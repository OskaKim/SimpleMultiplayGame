using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour {
    [SerializeField] string lobbyId;
    [SerializeField] LobbyLogUIController uiController; // todo : 삭제
    [SerializeField] LobbyTopUIController lobbyTopUIController;

    private void Start() {
        UnityServices.InitializeAsync();
        Locator.Get.Provide(new Identity(OnAuthSignIn));
    }

    private void OnEnable() {
        LobbyLogUIController.OnChangeLobbyId += OnChangeLobbyId;
    }

    private void OnDisable() {
        LobbyLogUIController.OnChangeLobbyId -= OnChangeLobbyId;
    }

    private void OnChangeLobbyId(string text) {
        lobbyId = text;
        Debug.Log($"OnChangeLobbyId : {text}");
    }

    private void OnAuthSignIn() {
        Debug.Log("OnAuthSignIn");
        RefreshLobbyListAsync();
    }

    // 로비 업데이트
    private void RefreshLobbyListAsync() {
        QueryAllLobbies((QueryResponse response) => {
            var lobbyElementParamsList = new List<LobbyTopUIController.LobbyElementParams>();
            foreach (var result in response.Results) {
                lobbyElementParamsList.Add(new LobbyTopUIController.LobbyElementParams(result.Name));
            }

            lobbyTopUIController.RefreshLobbyList(lobbyElementParamsList);
        });
    }

    // todo : 코드 정리
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            LobbyUser lobbyUser = new LobbyUser();
            lobbyUser.DisplayName = "hoge";
            CreateLobbyAsync("lobby_name", 2, false, lobbyUser, (Lobby lobby) => {
                lobbyId = lobby.Id;
                //uiController.SetLog(0, $"{lobbyId}");
                //uiController.SetLog(1, $"{lobby.Created}");
                //uiController.SetLog(2, $"{lobby.Data}");
                //uiController.SetLog(3, $"{lobby.EnvironmentId}");
                //uiController.SetLog(4, $"{lobby.HostId}");
                //uiController.SetLog(5, "success");

                // note : 방을 만들고 확인을 위해 로비 리스트에 표시함
                RefreshLobbyListAsync();
                Debug.Log("success");
            }, () => {
                uiController.SetLog(5, "failed");
                Debug.Log("failed");
            });
        }

        if (Input.GetKeyDown(KeyCode.F2)) {
            LobbyUser lobbyUser = new LobbyUser();
            lobbyUser.DisplayName = "joinedUser";
            joinLobby(lobbyId, lobbyUser, (Lobby lobby) => {
                lobbyId = lobby.Id;
                uiController.SetLog(0, $"{lobbyId}");
                uiController.SetLog(1, $"{lobby.Created}");
                uiController.SetLog(2, $"{lobby.Data}");
                uiController.SetLog(3, $"{lobby.EnvironmentId}");
                uiController.SetLog(4, $"{lobby.HostId}");
                uiController.SetLog(5, "success");
            });
        }

        if (Input.GetKeyDown(KeyCode.F3)) {
            LobbyAPIInterface.DeleteLobbyAsync(lobbyId, () => {
                uiController.SetLog(5, "success");
                Debug.Log("delete lobby finished");
            });
        }

        if (Input.GetKeyDown(KeyCode.F4)) {
            QueryAllLobbies((QueryResponse response) => {
                Debug.Log(response.ContinuationToken);
                foreach (var result in response.Results) {
                    Debug.Log(result.AvailableSlots);
                    Debug.Log(result.Upid);
                    Debug.Log(result.Name);
                }
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

    public void joinLobby(string lobbyId, LobbyUser localUser, Action<Lobby> onSuccess) {
        string uasId = AuthenticationService.Instance.PlayerId;
        LobbyAPIInterface.JoinLobbyAsync_ById(uasId, lobbyId, CreateInitialPlayerData(localUser), onSuccess);
    }

    public void QueryAllLobbies(Action<QueryResponse> onComplete) {
        LobbyAPIInterface.QueryAllLobbiesAsync(null, onComplete);
    }

    private static Dictionary<string, PlayerDataObject> CreateInitialPlayerData(LobbyUser player) {
        Dictionary<string, PlayerDataObject> data = new Dictionary<string, PlayerDataObject>();
        PlayerDataObject dataObjName = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, player.DisplayName);
        data.Add("DisplayName", dataObjName);
        return data;
    }

}
