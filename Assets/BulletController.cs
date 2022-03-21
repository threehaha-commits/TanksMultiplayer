using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletController : MonoBehaviour, IPunObservable
{
    Rigidbody2D r2d;
    Transform myTransform;
    [HideInInspector] public Vector3 Pos = Vector3.zero;
    [HideInInspector] public Quaternion Rot = Quaternion.identity;
    PhotonView photonView;
    public float LerpValue = 2f;

    private void Start()
    {
        Invoke("Death", 1.5f);
        photonView = GetComponent<PhotonView>();
        myTransform = transform;
        r2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PhotonView>().IsMine)
        {
            photonView.RPC("Death", RpcTarget.All);
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().GetDamge(100);
            photonView.RPC("Death", RpcTarget.All);
        }
    }

    [PunRPC]
    void Death()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(!photonView)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, Pos, LerpValue * Time.deltaTime);
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Rot, LerpValue * Time.deltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            Pos = (Vector3)stream.ReceiveNext();
            Rot = (Quaternion)stream.ReceiveNext();
        }
    }
}
