using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera defaultCamera;
    public CinemachineVirtualCamera actionCamera;
    
    public void SetDefaultCamera()
    {
        defaultCamera.Priority = 20;
        actionCamera.Priority = 10;
    }
    public void SetActionCamera()
    {
        defaultCamera.Priority = 10;
        actionCamera.Priority = 20;
    }
}
