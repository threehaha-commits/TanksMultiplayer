using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TestTank : MonoBehaviour
{
    private void Update()
    {
        if (PhotonNetwork.CountOfPlayersInRooms == 1)
        {
            gameObject.SetActive(false);
        }
    }
}
