using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class ConnectionMaster : MonoBehaviourPunCallbacks, ILobbyCallbacks {
    public TextMeshProUGUI statusConnection;
    public InputField inputApelido;

    private bool isconnect = false;
    [Space(10)]
    public GameObject lobby;
    public GameObject start;
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("sa");
    }

    IEnumerator RefreshText() {
        yield return new WaitForSeconds(3);
        statusConnection.text = "We are now connected to the <#00CC00>" + PhotonNetwork.CloudRegion.ToUpper() + "<#ffffff> server!";
    }

    public override void OnConnectedToMaster() {
        statusConnection.text = "We are now connected to the <#00CC00>" + PhotonNetwork.CloudRegion.ToUpper() + "<#ffffff> server!";

        PhotonNetwork.AutomaticallySyncScene = true;

        isconnect = true;
    }

    public void Connect() {
        if (isconnect) {
            if (inputApelido.text == "") {
                statusConnection.text = "<#FF0000>INSIRA UM APELIDO.";
                StartCoroutine(RefreshText());
            } else {
                PhotonNetwork.NickName = inputApelido.text;

                start.SetActive(false);
                lobby.SetActive(true);
            }
        }
    }
}
