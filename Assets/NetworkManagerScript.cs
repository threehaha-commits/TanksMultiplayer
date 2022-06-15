using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManagerScript : MonoBehaviourPunCallbacks
{
    private byte MaxPlayerRoom = 4;
    private string gameVersion = "1";
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Transform[] spawnPoints = new Transform[2];
    [SerializeField] private RoundManager roundManager;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayerRoom });
    }

    public override void OnJoinedRoom()
    {
        buttons[0].SetActive(false);
        GameObject player = PhotonNetwork.Instantiate(Player.name, Vector2.zero, new Quaternion(0, 0, 0, 0));
        PhotonView playerPView = player.GetPhotonView();
        int i = PhotonNetwork.CountOfPlayersInRooms;
        i = i % 2;
        playerPView.RPC("SetPlayerSprite", RpcTarget.AllBuffered, (i + 1));
        playerPView.RPC("SetPlayerProperties", RpcTarget.AllBuffered, i);
        Vector2 spawnPoint = spawnPoints[i].position;
        player.transform.position = spawnPoint;
        roundManager.SetPlayer();
        Destroy(gameObject);
    }
}
