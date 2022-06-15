using Photon.Pun;
using UnityEngine;

public interface IBullet
{
    void Set(GameObject bullet);
}

public class Attack : Lerp, IBullet
{
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform BulletSpawnPosition;
    private float ReloadTime;
    private float MaxReloadTime = 0.15f;

    void Start()
    {
        ReloadTime = MaxReloadTime;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (ReloadTime > 0)
            {
                ReloadTime = ReloadTime - Time.deltaTime;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                myTransform.rotation = param.GetRotation();
                RaycastHit2D hit = param.Hit(BulletSpawnPosition);
                if (hit == false)
                    return;
                if (hit.collider.tag == "Player" && hit.collider.gameObject != gameObject)
                {
                    GameObject bullet = PhotonNetwork.Instantiate(Bullet.name, BulletSpawnPosition.position, param.GetRotation());
                    ReloadTime = MaxReloadTime;
                }
            }
        }
    }

    public void Set(GameObject Bullet)
    {
        this.Bullet = Bullet;
    }
}
