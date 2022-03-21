using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour, IPunObservable
{
    public float Speed;
    public float RotateSpeed = 10f;
    Transform myTransform;
    Rigidbody2D r2d;
    public GameObject Bullet;
    public Transform BulletSpawnPosition;
    public float bulletSpeed;
    public float LerpValue = 2f;
    public Transform Gun;
    [HideInInspector] public Vector3 Pos = Vector3.zero;
    [HideInInspector] public Quaternion Rot = Quaternion.identity;
    PhotonView photonView;

    private void Start()
    {
        PhotonNetwork.SendRate = 40;
        PhotonNetwork.SerializationRate = 20;
        myTransform = transform;
        r2d = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        StartCoroutine(FireController());
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), -Vector2.up);

            if (hit.collider.tag != "Player")
            {
                Vector3 v2 = ray.origin - myTransform.position;
                Quaternion rotation = Quaternion.LookRotation(v2, Vector3.forward);
                Quaternion r = Quaternion.Lerp(myTransform.rotation, rotation, RotateSpeed * Time.deltaTime);
                r2d.AddForce(v2 * Speed * Time.deltaTime);
                transform.rotation = r;
            }
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, Pos, LerpValue * Time.deltaTime);
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Rot, LerpValue * Time.deltaTime);
        }
    }

    IEnumerator FireController()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (photonView.IsMine)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), -Vector2.up);

                    if (hit.collider.tag == "Player" && hit.collider.gameObject != this.gameObject)
                    {
                        Vector3 v2 = ray.origin - transform.position;
                        Quaternion rotation = Quaternion.LookRotation(v2, Vector3.forward);
                        transform.rotation = rotation;
                        yield return new WaitForSeconds(0.25f);
                        GameObject bullet = PhotonNetwork.Instantiate(Bullet.name, BulletSpawnPosition.position, rotation);
                        bullet.GetComponent<Rigidbody2D>().AddForce(v2 * (bulletSpeed / 2 ) * Time.deltaTime, ForceMode2D.Impulse);
                    }
                }
            }

            yield return null;
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
