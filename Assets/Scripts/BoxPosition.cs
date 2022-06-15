using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BoxPosition : MonoBehaviour, IPunObservable
{
    [HideInInspector] public Vector3 Pos = Vector3.zero;
    [HideInInspector] public Quaternion Rot = Quaternion.identity;
    protected PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, Pos, 10f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Rot, 10f * Time.deltaTime);
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
