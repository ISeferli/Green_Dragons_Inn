using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera topDownCamera;
    [SerializeField] CinemachineVirtualCamera isometricCamera;

    private void OnEnable(){
        CameraSwitcher.register(isometricCamera);
        CameraSwitcher.register(topDownCamera);
        CameraSwitcher.switchCamera(isometricCamera);
    }

    private void OnDisable(){
        CameraSwitcher.unregister(isometricCamera);
        CameraSwitcher.unregister(topDownCamera);
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.C)){
            if(CameraSwitcher.isActiveCamera(topDownCamera)){
                CameraSwitcher.switchCamera(isometricCamera);
                CameraEvent.changeCameraHandler(isometricCamera);
            } else if (CameraSwitcher.isActiveCamera(isometricCamera)){
                CameraSwitcher.switchCamera(topDownCamera);
                CameraEvent.changeCameraHandler(topDownCamera);
            }
        }
    }
}
