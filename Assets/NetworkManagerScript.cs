using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManagerScript : MonoBehaviourPunCallbacks
{
    [SerializeField] private byte MaxPlayerRoom = 4;
    string gameVersion = "1";
    public GameObject Player;
    public GameObject[] buttons;

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
        buttons[1].SetActive(false);
        float r = Random.Range(-1f, 1f);
        GameObject player = PhotonNetwork.Instantiate(Player.name, new Vector3(r, r, 0f), Quaternion.identity);
        int i = PhotonNetwork.CountOfPlayersInRooms;
        print($"i {i} i%2 {i % 2}");
        if (i % 2 == 1)
        {
            player.GetComponent<PhotonView>().RPC("SetProperties", RpcTarget.AllBuffered, false);
        }
        else
        {
            player.GetComponent<PhotonView>().RPC("SetProperties", RpcTarget.AllBuffered, true);
        }
        Destroy(gameObject);
    }
}
