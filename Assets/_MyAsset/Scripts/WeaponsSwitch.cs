using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponsSwitch : MonoBehaviour
{
    public GameObject object01;
    public GameObject object02;
    public GameObject object03;
    public GameObject object04;

    void Start()
    {
        
        object01.SetActive(true);
        object02.SetActive(false);
        object03.SetActive(false);
        //object04.SetActive(false);
    }


    void Update()
    {
        if (InputManager.Instance.IsInteractItem1())
        {
            object01.SetActive(true);
            object02.SetActive(false);
            object03.SetActive(false);
            //object04.SetActive(false);
        }

        if (InputManager.Instance.IsInteractItem2())
        {
            object01.SetActive(false);
            object02.SetActive(true);
            object03.SetActive(false);
            //object04.SetActive(false);
        }

        if (InputManager.Instance.IsInteractItem3())
        {
            object01.SetActive(false);
            object02.SetActive(false);
            object03.SetActive(true);
            //object04.SetActive(false);
        }

        if (InputManager.Instance.IsInteractItem4())
        {
            object01.SetActive(false);
            object02.SetActive(false);
            object03.SetActive(false);
            //object04.SetActive(true);
        }
    }
}
