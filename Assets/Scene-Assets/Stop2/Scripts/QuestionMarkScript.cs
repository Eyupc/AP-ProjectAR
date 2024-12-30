using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuestionMarkScript : MonoBehaviour
{
    public GameObject kebabInfo;
    public GameObject shakriyehInfo;
    public GameObject fattoushInfo;
    public GameObject kibbehInfo;

    private MenuItem selectedObj;
    public void SetInfo(MenuItem menuItem)
    {
        selectedObj = menuItem;
    }


    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    HandleTouch();
                }
            }
        }
    }

    private void HandleTouch()
    {
        Debug.Log("Selected Object: " + selectedObj);
        switch (selectedObj)
        {
            case MenuItem.Kebab:
                Instantiate(kebabInfo);
                break;
            case MenuItem.Shakriyeh:
                Instantiate(shakriyehInfo);
                break;
            case MenuItem.Fattoush:
                Instantiate(fattoushInfo);
                break;
            case MenuItem.Kibbeh:
                Instantiate(kibbehInfo);
                break;
        }
    }
}
