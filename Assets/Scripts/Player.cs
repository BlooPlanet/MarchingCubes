using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {

    CharacterController controller;
    float moveSpeed = 5f;
    float walkSpeed = 5f;
    float flySpeed = 11f;
    bool flytoggle = true;
    Vector3 finalVelocity;
    Vector3 gravityForce;
    float gravity = -9.8f;
    public Transform cameraT;

    public void Start() {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update() {
        UpdateMovement();
        UpdateCamera();

        RaycastHit hit;
        if (Physics.Raycast(cameraT.transform.position, cameraT.forward, out hit)) {
            if (Input.GetMouseButtonDown(0)) {
                Chunk chunk = hit.transform.GetComponent<Chunk>();
                if (chunk != null) {
                    Vector3 hitPosition = hit.point - hit.normal * 0.1f;
                    Vector3Int coord = new Vector3Int((int)hitPosition.x,(int)hitPosition.y,(int)hitPosition.z);
                    Vector3Int chunkCoord = Vector3Int.FloorToInt(chunk.transform.position);
                    Vector3Int localCubeCoord = coord - chunkCoord;
                    Debug.Log(hit.transform.name);
                    Debug.Log(localCubeCoord);
                    chunk.SetCube(localCubeCoord,1);
                    chunk.UpdateMesh();
                }
            }else if (Input.GetMouseButtonDown(1)) {
                Chunk chunk = hit.transform.GetComponent<Chunk>();
                if (chunk != null) {
                    Vector3 hitPosition = hit.point + hit.normal * 0.1f;
                    Vector3Int coord = new Vector3Int((int)hitPosition.x,(int)hitPosition.y,(int)hitPosition.z);
                    Vector3Int chunkCoord = Vector3Int.FloorToInt(chunk.transform.position);
                    Vector3Int localCubeCoord = coord - chunkCoord;
                    Debug.Log(hit.transform.name);
                    Debug.Log(localCubeCoord);
                    chunk.SetCube(localCubeCoord,-1);
                    chunk.UpdateMesh();
                }
            }
        }
    }

    public void UpdateMovement() {
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        CheckFlyToggle();
        if (flytoggle) {
            moveSpeed = flySpeed;
            gravityForce.y = 0;
            if (Input.GetKey(KeyCode.LeftShift)) {
                gravityForce.y = -flySpeed;
            }else if (Input.GetKey(KeyCode.Space)) {
                gravityForce.y = flySpeed;
            }
        }
        else {
            moveSpeed = walkSpeed;
            if (controller.isGrounded) {
                gravityForce.y = -0.5f;
                if (Input.GetKeyDown(KeyCode.Space)) {
                    gravityForce.y = 6;
                }
            }
            else {
                gravityForce.y += gravity * Time.deltaTime;
            }
        }

        finalVelocity = (moveDir * moveSpeed) + gravityForce;
        controller.Move(finalVelocity * Time.deltaTime);
    }

    float mouseRotationY = 0;
    float sensitivity = 2f;
    public void UpdateCamera() {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;
        
        transform.Rotate(Vector3.up * mouseX);
        mouseRotationY += mouseY;
        mouseRotationY = Mathf.Clamp(mouseRotationY, -90, 90);
        cameraT.eulerAngles = new Vector3(-mouseRotationY, cameraT.eulerAngles.y, 0);
    }

    float lastJumpedTime;
    public void CheckFlyToggle() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Time.time - lastJumpedTime < 0.35f) {
                flytoggle = !flytoggle;
            }

            lastJumpedTime = Time.time;
        }
    }
}