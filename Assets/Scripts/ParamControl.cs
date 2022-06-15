using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamControl
{
    private Transform transform;

    public ParamControl(Transform transform)
    {
        this.transform = transform;
    }

    private Ray GetRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray;
    }

    public RaycastHit2D Hit(Transform transform)
    {
        int layer = 1 << 6;
        layer = ~layer;
        float RayDistance = 15f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward * RayDistance, Mathf.Infinity, layer);
        return hit;
    }

    public RaycastHit2D Hit() 
    {
        return Physics2D.Raycast(new Vector2(GetRay().origin.x, GetRay().origin.y), -Vector2.up);
    }

    public Quaternion GetRotation()
    {
        Vector3 v2 = GetRay().origin - transform.position;
        Quaternion rotation = Quaternion.LookRotation(v2, Vector3.forward);
        return rotation;
    }
}
