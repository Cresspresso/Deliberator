 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2_dontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
