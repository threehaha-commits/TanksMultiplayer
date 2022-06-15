using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviour
{
    [SerializeField] private int Group;
    [SerializeField] private int Hp = 100;
    private PhotonView photonView;
    private RoundManager roundManager;

    private void Start()
    {
        photonView = gameObject.GetPhotonView();
    }

    public void SetRoundManager(RoundManager roundManager) => this.roundManager = roundManager;


    [PunRPC]
    public void SetPlayerProperties(int i)
    {
        Group = i;
        gameObject.name = gameObject.name + " " + Group;
    }

    public void GetDamge(int Damage)
    {
        Hp = Hp - Damage;
        if (Hp <= 0)
        {
            photonView?.RPC("Death", RpcTarget.All, Group);
            roundManager.EndRound();
        }
    }

    [PunRPC]
    public void Death(int Group)
    {
        gameObject.SetActive(false);
        if (Group == 1)
            roundManager.LeftWin();
        else
            roundManager.RightWin();
        Hp = 100;
    }
}
