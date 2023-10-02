using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ROBY.Photon
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] GameObject MainMenu;
        [SerializeField] GameObject HostMenu;
        [SerializeField] GameObject RoomMenu;
        [SerializeField] GameObject NickNameMenu;


        [SerializeField] TMP_Text ErrorField;
        [SerializeField] TMP_Text Room_nameText;
        [SerializeField] Text hostNameText;
        [SerializeField] Text NickNameText;

        [Header("Join Room Page")]
        [SerializeField] GameObject JoinMenu;
        [SerializeField] Transform RoomListParent;
        [SerializeField] GameObject RoomNameButtonPrefab;


        [Header("Room Creation Page")]
        [SerializeField] Transform PlayerlistParent;
        [SerializeField] GameObject PlayernamePrefab;
        [SerializeField] Button StartGameButton;
        // Start is called before the first frame update



        private void Start()
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                NickNameMenu.SetActive(false);
                PhotonNetwork.LeaveRoom();
            }

            StartGameButton.onClick.AddListener(onstartGameButtonClick);
        }
        public void onStartButtonClick()
        {

            if (string.IsNullOrEmpty(NickNameText.text))
            {
                ErrorField.text = "Please Enter your Name";
                return;
            }
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                ErrorField.text = "Connecting.... Please Wait...";
                PhotonNetwork.ConnectUsingSettings();
                NickNameMenu.SetActive(false);

            }
            else
            {
                ErrorField.text = "Already connected";
                MainMenu.SetActive(true);
                NickNameMenu.SetActive(false);
            }


        }




        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;   
            ErrorField.text = "Connected to Master";
        }


        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            NickNameMenu.SetActive(false);
            MainMenu.SetActive(true);
            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
            {
                PhotonNetwork.NickName = NickNameText.text;
            }

            ErrorField.text = "Connected to Server";
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            ErrorField.text = cause.ToString();
            print("Disconnected");
        }


        public override void OnRegionListReceived(RegionHandler regionHandler)
        {
            base.OnRegionListReceived(regionHandler);
            ErrorField.text = "Connecting to " + regionHandler.BestRegion.Code + " Region";
        }


        public void onHostButtonClick()
        {
            if (!string.IsNullOrEmpty(hostNameText.text))
            {
                PhotonNetwork.CreateRoom(hostNameText.text);
                HostMenu.SetActive(false);
                ErrorField.text = "Creating " + hostNameText.text + " Room";
            }
            else
            {
                ErrorField.text = "Please Enter a Room Name";
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            RoomMenu.SetActive(true);
            ErrorField.text = "Error While Room Creation... Please Try Again";


        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            ErrorField.text = "Waiting For Players....";
            RoomMenu.SetActive(true);
            Room_nameText.text = PhotonNetwork.CurrentRoom.Name;

            forPlayerListUpdate();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            RoomMenu.SetActive(false);
            MainMenu.SetActive(true);
            ErrorField.text = message + " due to Selected Room is not Active";
            foreach (Transform child in RoomListParent)
            {
                print("Rooms Deleted");
                Destroy(child.gameObject);
            }
        }

        public void onleaveRoomButtonClick()
        {
            PhotonNetwork.LeaveRoom();
            ErrorField.text = "Leaving Room";
            RoomMenu.SetActive(false);

        }
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            if (PhotonNetwork.NickName != null)
            {
                MainMenu.SetActive(true);
            }

            ErrorField.text = "";
        }




        public void onJoinButtonClick()
        {
            MainMenu.SetActive(false);
            JoinMenu.SetActive(true);
            ErrorField.text = "Fetching Available Rooms In This Server";
        }

        public void onJoinroomBackButtonClick()
        {
            MainMenu.SetActive(true);
            JoinMenu.SetActive(false);
        }


        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            print("RoomListUpdated");
            foreach (Transform child in RoomListParent)
            {
                print("Rooms Deleted");
                Destroy(child.gameObject);
            }
            foreach (RoomInfo roomInfo in roomList)
            {
                if (roomInfo.RemovedFromList)
                {
                    continue;
                }
                print("NewRoom added");
                var room = Instantiate(RoomNameButtonPrefab, RoomListParent);
                room.GetComponent<JoinRoom>().RoomNameText.text = roomInfo.Name;

                Button joinButton = room.GetComponent<Button>();
                joinButton.onClick.AddListener(() => JoinRoom(roomInfo.Name));


            }


        }

        void JoinRoom(string roomName)
        {
            print("join Room");
            JoinMenu.SetActive(false);
            PhotonNetwork.JoinRoom(roomName);
        }



        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            forPlayerListUpdate();


        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            forPlayerListUpdate();
        }


        void forPlayerListUpdate()
        {
            foreach (Transform child in PlayerlistParent)
            {
                Destroy(child.gameObject);
            }

            foreach (Player child in PhotonNetwork.PlayerList)
            {
                var Player = Instantiate(PlayernamePrefab, PlayerlistParent);
                Player.GetComponent<TMP_Text>().text = child.NickName;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                StartGameButton.gameObject.SetActive(true);
            }
            else
            {
                StartGameButton.gameObject.SetActive(false);
            }

            if (PhotonNetwork.PlayerList.Length == 2)
            {
                StartGameButton.interactable = true;
            }
            else
            {
                StartGameButton.interactable = false;

            }
        }



        public void onstartGameButtonClick()
        {
            SceneManeger.SceneManeger.Instance.onSceneChangeButtonClick(1);
        }
    }
}

