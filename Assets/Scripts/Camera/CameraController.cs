using UnityEngine;
using Cinemachine;
using GDI.WorldInteraction;

public class CameraController : MonoBehaviour{
    public float playerCameraDistance { get; set; }
    public Transform cameraTarget;
    public GameObject spawnPosition;
    public CinemachineVirtualCamera playerCamera;
    private float zoomSpeed = 25f;

    float movementSpeed = 0.1f;
    public Vector3 newPosition;
    public Quaternion initRotation;
    private Transform followTransform;

    void OnDestroy(){
        CameraEvent.OnCameraChange -= changeCameraEvent;
        UIEventHandler.OnPlayerSelectedChanged -= changePlayerTarget;
        UIEventHandler.OnPlayerDeselectedChanged -= changeDeselectedTarget;
    }

    void Start(){
        CameraEvent.OnCameraChange += changeCameraEvent;
        UIEventHandler.OnPlayerSelectedChanged += changePlayerTarget;
        UIEventHandler.OnPlayerDeselectedChanged += changeDeselectedTarget;
        playerCameraDistance = 5f;
        transform.rotation = playerCamera.transform.rotation;
        newPosition = spawnPosition.transform.position - playerCamera.transform.forward * playerCameraDistance;
        followTransform = null;
    }

    void Update(){
        if(WorldInteraction.instance.combatMode==1 && CombatController.instance.sortedInitiative.Count>0){
            followTransform = CombatController.instance.whoseTurn().transform;
        }

        if(followTransform != null){
            cameraTarget = followTransform;
            handleMovementFollowingCharacter();
        } else {
            playerCamera.transform.position = newPosition;
            handleMovementInput();
        }
    }

    private void changePlayerTarget(GameObject target){
        followTransform = target.transform;
    }

    private void changeDeselectedTarget(){
        followTransform = null;
    }

    private void changeCameraEvent(CinemachineVirtualCamera camera){
        playerCamera = camera;
    }

    private void handleMovementFollowingCharacter(){
        Quaternion rotationChange = Quaternion.identity;

        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0){
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            playerCamera.m_Lens.FieldOfView -= scroll * zoomSpeed;
            playerCamera.m_Lens.FieldOfView = Mathf.Clamp(playerCamera.m_Lens.FieldOfView, 15, 110);
        }

        float rotationSpeed = 100f;
        float rotationAmount = Input.GetKey(KeyCode.Q) ? rotationSpeed : Input.GetKey(KeyCode.E) ? -rotationSpeed : 0f;
        playerCamera.transform.RotateAround(cameraTarget.position, Vector3.up, rotationAmount * Time.deltaTime);

        newPosition = cameraTarget.position - playerCamera.transform.forward * playerCameraDistance;
        playerCamera.transform.position = newPosition;
        playerCamera.transform.LookAt(cameraTarget);
    }


    private void handleMovementInput(){
        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0){
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            playerCamera.m_Lens.FieldOfView -= scroll * zoomSpeed;
            playerCamera.m_Lens.FieldOfView = Mathf.Clamp(playerCamera.m_Lens.FieldOfView, 15, 110);
        }

        Quaternion rotationChange = Quaternion.identity;
        if(Input.GetKey(KeyCode.D)) newPosition += (transform.right*movementSpeed);
        if(Input.GetKey(KeyCode.A)) newPosition += (transform.right*-movementSpeed);

        Vector3 forwardVector = new Vector3(0, 0, 1);
        if(playerCamera.name.Equals("IsometricCamera")){
            forwardVector = playerCamera.transform.TransformDirection(forwardVector);
            forwardVector.y = 0;
            if (Input.GetKey(KeyCode.W)) newPosition += forwardVector * movementSpeed; 
            if (Input.GetKey(KeyCode.S)) newPosition -= forwardVector * movementSpeed;
        } else {
            if (Input.GetKey(KeyCode.W)) newPosition += playerCamera.transform.up * movementSpeed; 
            if (Input.GetKey(KeyCode.S)) newPosition -= playerCamera.transform.up * movementSpeed;
        }

        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q)){
            rotationVector.y = +1f;
        }
        if (Input.GetKey(KeyCode.E)){
            rotationVector.y = -1f;
        }

        float rotationSpeed = 100f;
        playerCamera.transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }
}
