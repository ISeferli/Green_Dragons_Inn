using Cinemachine;
using System.Collections.Generic;

public static class CameraSwitcher {
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera activeCamera = null;

    public static void register(CinemachineVirtualCamera camera){
        cameras.Add(camera);
    }

    public static void unregister(CinemachineVirtualCamera camera){
        cameras.Remove(camera);
    }

    public static void switchCamera(CinemachineVirtualCamera camera){
        camera.Priority = 10;
        activeCamera = camera;
        foreach(CinemachineVirtualCamera c in cameras){
            if(c != camera && c.Priority != 0){
                c.Priority = 0;
            }
        }
    }

    public static bool isActiveCamera(CinemachineVirtualCamera camera){
        return camera == activeCamera;
    }
}
