using NUnit.Framework;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public Transform roomListParent;
    public GameObject roomListItemPrefab;
    public TMP_InputField roomNameInput;


    [Header("In Room UI")]
    public TMP_Dropdown maxPlayersDropdown;


    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();


    void Start()
    {
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }


        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            int index = cachedRoomList.FindIndex(r => r.Name == roomInfo.Name);

            if (roomInfo.RemovedFromList)
            {
                if (index != -1) cachedRoomList.RemoveAt(index);
            }
            else
            {
                if (index != -1) cachedRoomList[index] = roomInfo;
                else cachedRoomList.Add(roomInfo);
            }
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
            Destroy(roomItem.gameObject);

        foreach (var room in cachedRoomList)
        {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);
            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/" + room.MaxPlayers;
            Button roomListBtn = roomItem.GetComponent<Button>();
            roomListBtn.onClick.AddListener(() => JoinRoomByName(room.Name));
        }
    }

    public void JoinRoomByName(string targetRoomName)
    {
        foreach (RoomInfo room in cachedRoomList)
        {
            if (room.Name == targetRoomName)
            {
                PhotonNetwork.JoinRoom(room.Name);
                return;
            }
        }

        Debug.LogWarning("Room not found: " + targetRoomName);
    }

    public void OnCreateRoomButtonClicked()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            string roomName = !string.IsNullOrEmpty(roomNameInput?.text)
                ? roomNameInput.text
                : "Room" + UnityEngine.Random.Range(1000, 9999);

            // Get selected number of players from dropdown (index 0 = "1", index 4 = "5")
            int maxPlayers = maxPlayersDropdown.value + 1; // value is index, so +1 to get actual number

            RoomOptions options = new RoomOptions
            {
                MaxPlayers = (byte)maxPlayers,
                IsVisible = true,
                IsOpen = true
            };

            PhotonNetwork.CreateRoom(roomName, options);
            roomNameInput.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
