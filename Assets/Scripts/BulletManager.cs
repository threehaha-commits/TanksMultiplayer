using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject[] BulletItem = new GameObject[3];
    [SerializeField] private GameObject[] Bullet = new GameObject[3];
    [SerializeField] private RoundManager roundManager;
    private float SpawnTime = 10f;
    private bool IsSpawn = false;
    private Vector2 min;
    private Vector2 max;
    private int Index;

    private void Awake()
    {
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        if(gameObject.GetPhotonView().IsMine == false)
            Destroy(gameObject);
        this.roundManager.AddListener(StartSpawn, 0);
        this.roundManager.AddListener(EndSpawn, 1);
    }

    private void StartSpawn()
    {
        IsSpawn = true;
        StartCoroutine(Spawn());
    }

    private void EndSpawn() 
    {
        IsSpawn = false;
        StopCoroutine(Spawn());
        BulletContainer.Active(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            roundManager.EndRound();
        }
    }

    private IEnumerator Spawn()
    {
        while(IsSpawn)
        {
            int random = Random.Range(0, Bullet.Length);
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            Vector2 position = new Vector2(x, y);
            GameObject bullet = PhotonNetwork.Instantiate(BulletItem[random].name, position, Quaternion.identity);
            bullet.name = "Bullet " + Index;
            Index++;
            yield return new WaitForSeconds(SpawnTime);
        }
    }
}
