using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCanvasDestroyer : MonoBehaviour
{
    /*void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Destroy(gameObject);
            }
        }
    }*/

    public void DestroyCanvas()
    {
        Destroy(gameObject);
    }
}
