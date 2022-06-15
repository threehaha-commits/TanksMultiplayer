using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletController : MonoBehaviour, IPunObservable
{
    [SerializeField] private int Damage;
    private float Speed = 0.05f;
    private Rigidbody2D r2d;
    private Transform myTransform;
    private Vector3 Pos = Vector3.zero;
    
    private Quaternion Rot = Quaternion.identity;
    private PhotonView photonView;
    private readonly float LerpValue = 10;

    private void Start()
    {
        photonView = gameObject.GetPhotonView();
        myTransform = transform;
        r2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Health>()?.GetDamge(Damage);
        DeactiveBullet();
    }

    private void OnBecameInvisible()
    {
        DeactiveBullet();
    }

    private void DeactiveBullet()
    {
        if (photonView.IsMine)
        {
            photonView?.RPC("BulletOff", RpcTarget.Others);
            gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void BulletOff()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        r2d.AddForce(transform.forward * Speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
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
