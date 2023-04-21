using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float fovMax = 50;
    [SerializeField] private float fovMin = 20;

    private float targetFieldOfView = 50;
    void Update()
    {
         HandleCameraMovement();
         HandelCameraRotation();
         HandleCameraZoom();
         

       

       
    }

    private void HandleCameraMovement(){
        float rotateDir = 0f;

        if(Input.GetKey(KeyCode.Q)) rotateDir = -1f;
        if(Input.GetKey(KeyCode.E)) rotateDir = +1f;

        float rotateSpeed = 100f;


        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
        


    }

    private void HandelCameraRotation(){
         Vector3 inputDir = new Vector3(0, 0, 0);

        if(Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if(Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if(Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if(Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 50f;
        transform.position += inputDir * moveSpeed * Time.deltaTime;


    }

    private void HandleCameraZoom(){
        if(Input.mouseScrollDelta.y > 0){
            targetFieldOfView -= 5;

        }
        if(Input.mouseScrollDelta.y < 0){
            targetFieldOfView += 5;

        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView,fovMin, fovMax );

        float zoomSpeed = 10f;

        

        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);
        
        
    }
}
