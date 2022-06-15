using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections;
using Photon.Pun;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private TMP_Text RoundText;
    private UnityAction RoundStart;
    private UnityAction RoundEnd;
    private int RoundLeft = 0;
    private int RoundRight = 0;
    private int TimeForStart = 3;
    private PhotonView photonView;
    private int MaxPlayer = 2;

    private void Start()
    {
        photonView = gameObject.GetPhotonView();
    }

    public void SetPlayer() 
    {
        if((PhotonNetwork.CountOfRooms + 1) == MaxPlayer)
        {
            RoundStart.Invoke();
        }
    }

    public void EndRound()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        photonView.RPC("RoundIsEnd", RpcTarget.All);
        while (TimeForStart > 0)
        {
            yield return new WaitForSeconds(1f);
            photonView.RPC("Timer", RpcTarget.All);
        }
        photonView.RPC("StartRound", RpcTarget.All);
        RoundStart.Invoke();
    }

    [PunRPC]
    private void RoundIsEnd()
    {
        BulletContainer.Active(false);
        TimeForStart = 3;
        RoundEnd?.Invoke();
    }

    [PunRPC]
    private void Timer()
    {
        TimeForStart--;
        TextOnScreen($"Start: {TimeForStart}");
    }

    [PunRPC]
    private void StartRound()
    {
        TextOnScreen();
    }

    public void AddListener(UnityAction action, int Group = 0)
    {
        if (Group == 0)
            RoundStart += action;
        else RoundEnd += action;
    }

    public void LeftWin()
    {
        RoundLeft++;
        TextOnScreen();
    }

    public void RightWin()
    {
        RoundRight++;
        TextOnScreen();
    }

    private void TextOnScreen() 
    {
        RoundText.text = $"Round score: {RoundLeft} / {RoundRight}";
    }

    private void TextOnScreen(string Message)
    {
        RoundText.text = Message;
    }
}
