using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPInteract : MonoBehaviour, IInteractable
{
    public Transform handTransform;
    public Canvas itemCanvas;
    public string itemName = "Название предмета"; // Имя предмета для отображения в тексте

    private GameObject currentObject;
    private Rigidbody currentObjectRb;
    private bool isHolding = false;

    private bool isInteract = false;
    private bool StopisInteract = false;

    private TMP_Text itemText; // Добавляем переменную для текста

    private void Start()
    {
        // Ищем компонент Text в дочерних элементах канваса
        itemText = itemCanvas.GetComponentInChildren<TMP_Text>();
        itemCanvas.enabled = false;
    }

    void Update()
    {
        //if (isInteract)
        //{
        //    if (!isHolding)
        //    {
        //        TryPickUpObject();
        //    }
        //    else
        //    {
        //        StopPhysicInteract();
        //    }
        //}
    }

    private void TryPickUpObject()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            gameObject.transform.SetParent(handTransform);
            isHolding = true;
            itemCanvas.enabled = true;
            if (itemText != null)
            {
                itemText.text = itemName; // Устанавливаем текст
            }
        }
    }

    private void StopPhysicInteract()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.SetParent(null);
        isHolding = false;
        itemCanvas.enabled = false;

        // Если вы хотите добавить силу для бросания объекта:
        //gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5f, ForceMode.Impulse);
    }

    public void OnInteractKeyDown()
    {
        isInteract = true;
        StopisInteract = true;
        TryPickUpObject();
    }

    public void OnInteractKeyUp()
    {
        isInteract = false;
        StopPhysicInteract();

    }

    public void OnAddInteractKeyDown()
    {

    }

    public void OnAddInteractKeyUp()
    {

    }
}
