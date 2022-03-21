using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    static public int lScore = 0;
    static public int rScore = 0;
    public bool left = false;
    public bool right = false;
    public int Hp = 100;
    public Text txt;

    private void Start()
    {
        lScore = 0;
        rScore = 0;
        txt = GameObject.Find("Score").GetComponent<Text>();
    }

    public void GetDamge(int Damage)
    {
        Hp = Hp - Damage;
        if (Hp <= 0)
        {
            GetComponent<PhotonView>().RPC("Death", RpcTarget.All);
            Invoke("Spawn", 3f);
        }
    }

    [PunRPC]
    public void Death()
    {
        if (left)
        {
            lScore++;
        }

        if (right)
        {
            rScore++;
        }

        txt.text = $"Battle Score: {lScore} / {rScore}";

        gameObject.SetActive(false);
    }

    void Spawn()
    {
        GetComponent<PhotonView>().RPC("Respawn", RpcTarget.All);
        Hp = 100;
    }


    [PunRPC]
    public void Respawn()
    {
        gameObject.SetActive(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Hp);
        }
        else
        {
            this.Hp = (int)stream.ReceiveNext();
        }
    }
}
