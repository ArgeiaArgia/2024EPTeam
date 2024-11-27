using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    private CinemachineVirtualCamera _defaultCamera;

    private void Start()
    {
        _defaultCamera = GetComponent<CinemachineVirtualCamera>();
        StartCoroutine(ZoomOut());
    }

    private IEnumerator ZoomOut()
    {
        yield return null;
        _defaultCamera.Priority = 20;
    }
}
