using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalItemInteraction : MonoBehaviour, IInteractable
{
    private Camera mainCamera;
    private GameObject selectedObject;
    private bool isObjectHeld = false;
    private bool isPressed = false;
    public float holdDistance = 5f;
    public float moveSpeed = 10f;
    public string draggableTag = "Draggable"; // Тег объектов, которые можно перемещать

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Захват объекта
        if (isPressed && !isObjectHeld)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.CompareTag(draggableTag))
                {
                    selectedObject = hit.collider.gameObject;
                    isObjectHeld = true;

                    if (selectedObject.GetComponent<Rigidbody>())
                    {
                        selectedObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
            }
        }

        // Перемещение объекта
        if (isObjectHeld)
        {
            Vector3 objPosition = mainCamera.transform.position + mainCamera.transform.forward * holdDistance;
            selectedObject.transform.position = Vector3.Lerp(selectedObject.transform.position, objPosition, moveSpeed * Time.deltaTime);
        }

        // Отпускание объекта
        if (!isPressed && isObjectHeld)
        {
            if (selectedObject.GetComponent<Rigidbody>())
            {
                selectedObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            selectedObject = null;
            isObjectHeld = false;
        }
    }

    public void OnInteractKeyDown()
    {
        isPressed = true;
    }

    public void OnInteractKeyUp()
    {
        isPressed = false;
    }

    public void OnAddInteractKeyDown()
    {
        // Дополнительные взаимодействия при нажатии
    }

    public void OnAddInteractKeyUp()
    {
        // Дополнительные взаимодействия при отпускании
    }
}
