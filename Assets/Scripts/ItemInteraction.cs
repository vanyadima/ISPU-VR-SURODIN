using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour,IInteractable
{
    public Camera mainCamera;
    public Transform inspectionPosition; // Позиция перед камерой для осмотра
    public GameObject descriptionUI; // UI с описанием предмета

    private GameObject selectedObject;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isInspecting = false;

    private bool isInteractionActive = false;

    void Update()
    {
        if (isInteractionActive)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Inspectable"))
                {
                    BeginInspection(hit.collider.gameObject);
                }
            }
        }

        if (isInspecting)
        {
            if (isInteractionActive)
            {
                RotateObject();
            }
            else if (!isInteractionActive)
            {
                EndInspection();
            }
        }
    }

    void BeginInspection(GameObject obj)
    {
        //selectedObject = obj;
        //originalPosition = obj.transform.position;
        //originalRotation = obj.transform.rotation;

        //obj.transform.position = inspectionPosition.position;
        //obj.transform.LookAt(mainCamera.transform);

        descriptionUI.SetActive(true); // Показываем описание
        isInspecting = true;

        // Применить размытие к фону (например, с помощью пост-эффекта)
    }

    void RotateObject()
    {
        //float rotationSpeed = 5.0f;
        //float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        //float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        //selectedObject.transform.Rotate(mainCamera.transform.up, -mouseX, Space.World);
        //selectedObject.transform.Rotate(mainCamera.transform.right, mouseY, Space.World);
    }

    void EndInspection()
    {
        //selectedObject.transform.position = originalPosition;
        //selectedObject.transform.rotation = originalRotation;
        descriptionUI.SetActive(false); // Скрываем описание
        isInspecting = false;

        // Убрать размытие фона
    }

    public void OnInteractKeyDown()
    {
        isInteractionActive = true;
    }

    public void OnInteractKeyUp()
    {
        isInteractionActive = false;
    }

    public void OnAddInteractKeyDown()
    {
        
    }

    public void OnAddInteractKeyUp()
    {
        
    }
}
