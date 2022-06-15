using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletImage : MonoBehaviour
{
    private static Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public static void ChargeImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
