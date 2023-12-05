using Cinemachine;
using UnityEngine;

public class CameraEvent : MonoBehaviour {
    public delegate void OnCameraChangeHandler(CinemachineVirtualCamera camera);
    public static event OnCameraChangeHandler OnCameraChange;

    public static void changeCameraHandler(CinemachineVirtualCamera camera){
        OnCameraChange(camera);
    }
}
