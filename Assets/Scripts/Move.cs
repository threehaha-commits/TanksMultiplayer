using Photon.Pun;
using UnityEngine;

public class Move : Lerp
{
    private float Speed = 40f;
    [SerializeField] private float RotateSpeed = 2f;
    private Rigidbody2D r2d;

    private void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        myTransform.rotation = param.GetRotation();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
            IsMove();
    }

    private bool IsMove()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = param.Hit();

            if (hit.collider.tag != "Player")
            {
                Quaternion rotation = Quaternion.Lerp(myTransform.rotation, param.GetRotation(), RotateSpeed * Time.deltaTime);
                r2d.velocity = transform.up * Speed * Time.fixedDeltaTime;
                transform.rotation = rotation;
                return true;
            }
        }
        return false;
    }
}
