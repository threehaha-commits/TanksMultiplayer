using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 position;
    private RoundManager roundManager;
    private PhotonView photonView;
    [SerializeField] private Sprite[] playerSprite = new Sprite[2];
    private SpriteRenderer spriteRenderer;
    private int Index = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        photonView = gameObject.GetPhotonView();
        position = transform.position;
        roundManager = FindObjectOfType<RoundManager>();
        GetComponent<Health>().SetRoundManager(roundManager);
        roundManager.AddListener(CallRPC);
        StartCoroutine(SetSprite(0));
    }

    [PunRPC]
    private void SetPlayerSprite(int i)
    {
        Index = i;
    }

    private IEnumerator SetSprite(int i)
    {
        yield return new WaitWhile(() => Index == 0);
        spriteRenderer.sprite = playerSprite[Index - 1];
    }

    private void CallRPC()
    {
        transform.position = position;
        photonView.RPC("SetPosition", RpcTarget.All);
    }

    [PunRPC]
    private void SetPosition() 
    {
        gameObject.SetActive(true);
    }
}
