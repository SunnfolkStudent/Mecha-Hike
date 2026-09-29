using System;
using UnityEngine;

public class standControll : MonoBehaviour
{
    public GameObject parent;
    private void Awake()
    {
        transform.parent = null;
    }

    private void Update()
    {
        if (!parent.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
}
