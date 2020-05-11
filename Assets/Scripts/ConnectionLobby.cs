using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ConnectionLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static ConnectionLobby lobby;

    public string roomName;
    public int roomSize;
    public GameObject roomListingPrefab;
    public Transform roomsPanel;

    public List<RoomInfo> roomListings;

    private bool isConnected = false;

    void Awake() {
        lobby = this;    
    }

    void Start() {
        isConnected = true;
        roomListings = new List<RoomInfo>();
    }

    public void CreateRoom() {
        if (isConnected) {
            Debug.Log("Trying to create a new Room");
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
            PhotonNetwork.CreateRoom(roomName, roomOps);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        base.OnRoomListUpdate(roomList);
        //RemoveRoomListing();

        int tempIndex;
        foreach(RoomInfo room in roomList) {
            if(roomListings != null) {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            } else {
                tempIndex = -1;
            }
            Debug.Log(room.Name);
            if(tempIndex != -1) {
                roomListings.RemoveAt(tempIndex);
                Destroy(roomsPanel.GetChild(tempIndex).gameObject);
            } else {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name) {
        return delegate (RoomInfo room) {
            return room.Name == name;
        };
    }

    void RemoveRoomListing() {
        int i = 0;
        while(roomsPanel.childCount != 0) {
            Destroy(roomsPanel.GetChild(i).gameObject);
            i++;
        }
    }

    void ListRoom(RoomInfo room) {
        if(room.IsOpen && room.IsVisible) {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.roomName = room.Name;
            tempButton.roomSize = room.MaxPlayers;
            tempButton.SetRoom();
        }
    }



    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Tried to create a new room but failed.");
        //CreateRoom();
    }

    public void OnRoomNameChanged(string nameIn) {
        roomName = nameIn;
    }
    public void OnRoomSizeChanged(string sizeIn) {
        roomSize = int.Parse(sizeIn);
    }


    public void JoinLobbyOnClick() {
        if (isConnected) {
            if (!PhotonNetwork.InLobby) {
                PhotonNetwork.JoinLobby();
            }
        }
    }

}
