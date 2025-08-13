using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Camera_Mgr : MonoBehaviour
{
    [Header("Camera")]
    public CinemachineVirtualCamera VirtualCamera;

    [Header("CameraRoot")]
    public Transform LoadingCameraRoot;

    public static Camera_Mgr Inst = null;


    private void Awake()
    {
        Inst = this;
        ChangeTarget(LoadingCameraRoot);
        SetCameraZoom(0);
    }

    bool IsNULLVirtualCamera()
    {
        if (VirtualCamera == null)
            VirtualCamera = GameObject.Find("Virtual Camera")?.GetComponent<CinemachineVirtualCamera>();

        return VirtualCamera == null;
    }

    public void ChangeTarget(Transform input)
    {
        if (IsNULLVirtualCamera()) return;

        VirtualCamera.Follow = input;
    }

    public void SetCameraZoom(float zoom)
    {
        if (IsNULLVirtualCamera()) return;


        var thirdPersonFollow = VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        if (thirdPersonFollow != null)
        {
            thirdPersonFollow.CameraDistance = zoom;
        }

    }
}
