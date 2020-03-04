﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezeRotationScript : MonoBehaviour
{

    Quaternion rotation;

    void Awake()
    {
        rotation = transform.rotation;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}