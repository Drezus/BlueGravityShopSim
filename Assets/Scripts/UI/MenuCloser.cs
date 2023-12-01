using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCloser : MonoBehaviour
{
    public GameObject target;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            target.SetActive(false);
        }
    }
}
