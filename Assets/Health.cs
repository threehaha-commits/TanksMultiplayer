using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Health : MonoBehaviour, IPunObservable
{
    static public int lScore = 0;
    static public int rScore = 0;
    public bool left = false;
    public bool right = false;
    public int Hp = 100;
    public Text txt;
    public string txtScore = "";
    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        //if (photonView.IsMine == false) enabled = false;
        lScore = 0;
        rScore = 0;
        txt = GameObject.Find("Score").GetComponent<Text>();
    }

    [PunRPC]
    public void SetProperties(bool Left)
    {
        if (Left)
        { 
            left = true;
            gameObject.name = gameObject.name + " Left Team";
        }
        else 
        {
            right = true;
            gameObject.name = gameObject.name + " Right Team";
        }
    }

    public void GetDamge(int Damage)
    {
        Hp = Hp - Damage;
        if (Hp <= 0)
        {

            photonView.RPC("Death", RpcTarget.All);
            Invoke("Spawn", 1f);
        }
    }

    [PunRPC]
    public void Death()
    {
            if (left)
                rScore++;

            if (right)
                lScore++;

        txtScore = $"Battle Score: {lScore} / {rScore}";
        txt.text = txtScore;
            gameObject.SetActive(false);
    }

    void Spawn()
    {
        photonView.RPC("Respawn", RpcTarget.All);
    }


    [PunRPC]
    public void Respawn()
    {
        gameObject.SetActive(true);
        Hp = 100;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Hp);
            stream.SendNext(rScore);
            stream.SendNext(lScore);
            stream.SendNext(txtScore);
        }
        else
        {
            Hp = (int)stream.ReceiveNext();
            rScore = (int)stream.ReceiveNext();
            lScore = (int)stream.ReceiveNext();
            txtScore = (string)stream.ReceiveNext();
        }
    }
}
