using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game gs;

    private List<Player> players;
    [SerializeField] private List<GameObject> playersGameObject;

    [Header("Listings Player In Game")]
    [SerializeField] private GameObject playerListingPrefab;
    public List<GameObject> playerListings;
    [SerializeField] private Transform playerPanels;
    
    public GameObject playerSelected_Gameobject;
    public Player playerSelected_Player;
    public Player playerSelected_Master;

    public GameObject playerSelected_Listing;

    private void OnEnable() {
        players = new List<Player>();
        playerListings = new List<GameObject>();
        playersGameObject = new List<GameObject>();

        StartCoroutine(SetPlayerGameObject());

        foreach (Player p in PhotonNetwork.PlayerList) {
            if(!p.IsMasterClient)
                players.Add(p);
        }

        CleaarPlayerListings();
        ListPlayers();

        if (Game.gs == null) {
            Game.gs = this;
        }
    }

    IEnumerator SetPlayerGameObject() {
        yield return new WaitForSeconds(1);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player")) {
            playersGameObject.Add(g);
        }
    }

    void CleaarPlayerListings() {
        for (int i = playerPanels.childCount - 1; i >= 0; i--) {
            Destroy(playerPanels.GetChild(i).gameObject);
        }
    }

    void ListPlayers() {
        if (PhotonNetwork.InRoom) {
            int i = 0;
            foreach (Player player in PhotonNetwork.PlayerList) {
                if (!player.IsMasterClient) {
                    playerListings.Add(Instantiate(playerListingPrefab, playerPanels));
                    Text tempText = playerListings[i].transform.GetChild(0).GetChild(0).GetComponent<Text>();
                    tempText.text = player.NickName;

                    i++;
                }
            }
        }
    }

    public void DisconnectPlayer() {
        Destroy(ConnectionRoom.room.gameObject);
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad() {
        PhotonNetwork.LeaveRoom();

        CleaarPlayerListings();
        ListPlayers();

        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(MultiplayerSetting.multiplayerSettings.menuScene);
    }

    public void PlayerSelectedGameobject(string name) {
        for(int i = 0;i < players.Count;i++) {
            if(players[i] != null) {
                playerSelected_Gameobject = playersGameObject[(players.FindIndex(ByName(name)))];
            }
        }
    }
    public void PlayerSelectedPlayer(string name) {
        for (int i = 0; i < players.Count; i++) {
            if (players[i] != null) {
                playerSelected_Player = players.Find(ByName(name));
            }
        }
    }

    public void PlayerSelectedPlayerMaster(string name) {
        for (int i = 0; i < players.Count; i++) {
            if (players[i] != null) {
                if (players[i].IsMasterClient) {
                    playerSelected_Master = players.Find(ByName(name));
                }
            }
        }
    }

    public void PlayerSelectedListing(string name) {
        for (int i = 0; i < playerListings.Count; i++) {
            if (players[i] != null) {
                if (!players[i].IsMasterClient) {
                    playerSelected_Listing = playerListings.Find(ByNameListing(name));
                }
            }
        }
    }

    public void PlayerSelectedListing_Reset() {
        for (int i = 0; i < playerListings.Count; i++) {
            if (playerListings[i] != null) {
                if (playerListings[i].GetComponent<Image>().IsActive()) {
                    playerListings[i].GetComponent<Image>().enabled = false;
                }
            }
        }
    }

    public void PlayerDeselect() {
        playerSelected_Gameobject = null;
        playerSelected_Player = null;

        if (playerSelected_Listing.GetComponent<Image>().IsActive())
            playerSelected_Listing.GetComponent<Image>().enabled = false;
    }

    static System.Predicate<Player> ByName(string name) {
        return delegate (Player player) {
            return player.NickName == name;
        };
    }

    static System.Predicate<GameObject> ByNameListing(string name) {
        return delegate (GameObject player) {
            return player.transform.GetChild(0).GetChild(0).GetComponent<Text>().text == name;
        };
    }
}
