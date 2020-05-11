using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;

public class ConnectionRoom : MonoBehaviourPunCallbacks
{
    public static ConnectionRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    //Player info
    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playerInGame;

    //Delayed start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart = 10;

    public GameObject lobbyGO;
    public GameObject roomGO;
    public Transform playersPanel;
    public GameObject playerListingPrefab;
    public GameObject startButton;
    public GameObject racaButton;

    public Dropdown drop;
    public Text txt;
    [Space(10)]
    [SerializeField] private Sprite[] sprites;

    void Awake() {
        if (ConnectionRoom.room == null) {
            ConnectionRoom.room = this;
        } else {
            if (ConnectionRoom.room != this) {
                Destroy(ConnectionRoom.room.gameObject);
                ConnectionRoom.room = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable() {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable() {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void Start() {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
    }

    void Update() {
        if (MultiplayerSetting.multiplayerSettings.delayStart) {
            if (playersInRoom == 1) {
                RestartTimer();
            }
            if (!isGameLoaded) {
                if (readyToStart) {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                } else if (readyToCount) {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }

                if (timeToStart <= 0) {
                    StartGame();
                }
            }
        }
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();

        Debug.Log("We are now in a room");

        lobbyGO.SetActive(false);
        roomGO.SetActive(true);

        if (PhotonNetwork.IsMasterClient) {
            startButton.SetActive(true);
            racaButton.SetActive(false);
        }

        CleaarPlayerListings();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;

        if (MultiplayerSetting.multiplayerSettings.delayStart) {
            Debug.Log("Displayer players in room out of max player possible ("+ playersInRoom + ":" +MultiplayerSetting.multiplayerSettings.maxPlayer+")");
            if (playersInRoom > 1) {
                readyToCount = true;
            }
            if(playersInRoom == MultiplayerSetting.multiplayerSettings.maxPlayer) {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log("A new player has joined the room");
        CleaarPlayerListings();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if (MultiplayerSetting.multiplayerSettings.delayStart) {
            Debug.Log("Displayer players in room out of max player possible (" + playersInRoom + ":" + MultiplayerSetting.multiplayerSettings.maxPlayer + ")");
            if (playersInRoom > 1) {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSetting.multiplayerSettings.maxPlayer) {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

    }

    void CleaarPlayerListings() { 
        for(int i = playersPanel.childCount - 1; i>= 0; i--) {
            Destroy(playersPanel.GetChild(i).gameObject);
        }
    }

    void ListPlayers() {
        if (PhotonNetwork.InRoom) {
            foreach(Player player in PhotonNetwork.PlayerList) {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPanel);
                Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
                tempText.text = player.NickName;
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + "has left the game");

        playersInRoom--;
        CleaarPlayerListings();
        ListPlayers();
    }

    public void StartGame() {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (MultiplayerSetting.multiplayerSettings.delayStart)
            PhotonNetwork.CurrentRoom.IsOpen = false;

        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSettings.multiplayerScene);
    }

    void RestartTimer() {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;

    }

    public void Refresh() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode move) {
        currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSetting.multiplayerSettings.multiplayerScene) {
            isGameLoaded = true;
                
            RPC_CreatePlayer();
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene() {
        playerInGame++;
        if(playerInGame == PhotonNetwork.PlayerList.Length) {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer() {
        if(!PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Obj()), transform.position, Quaternion.identity, 0);
    }

    private string Obj() {
        switch (drop.value) {
            case 0:
                return "PhotonNetworkPlayerFeiticeiro";
            case 1:
                return "PhotonNetworkPlayerElfo";
            case 2:
                return "PhotonNetworkPlayerOrc";
            case 3:
                return "PhotonNetworkPlayerHumano";
            case 4:
                return "PhotonNetworkPlayerNecromante";
        }
        return null;
    }

    public void DisconnectPlayer() {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad() {
        PhotonNetwork.LeaveRoom();
        //while (PhotonNetwork.IsConnected)
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(MultiplayerSetting.multiplayerSettings.menuScene);
    }

    public override void OnLeftRoom() {
        base.OnLeftRoom();

        playerInGame--;
    }
}
