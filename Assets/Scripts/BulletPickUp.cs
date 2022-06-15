using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class BulletPickUp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject Bullet;

    private void Start()
    {
        BulletContainer.Add(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;

        GameObject player = collision.gameObject;
        PhotonView View = player.GetPhotonView();
        if (View.IsMine == false)
        {
            gameObject.SetActive(false);
            return;
        }
        IBullet bullet = player.GetComponent<IBullet>();
        bullet.Set(Bullet);
        BulletImage.ChargeImage(sprite.sprite);
        gameObject.SetActive(false);
    }
}

public class BulletContainer
{
    private static Queue<GameObject> items = new Queue<GameObject>();
    
    public static void Add(GameObject item)
    {
        items.Enqueue(item);
    }

    public static void Active(bool active)
    {
        foreach(GameObject item in items)
        {
            item?.SetActive(active);
        }
    }
}
