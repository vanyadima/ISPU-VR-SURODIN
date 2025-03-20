using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPInteract : MonoBehaviour, IInteractable
{
    public Transform handTransform;
    public Canvas itemCanvas;
    public string itemName = "�������� ��������"; // ��� �������� ��� ����������� � ������

    private GameObject currentObject;
    private Rigidbody currentObjectRb;
    private bool isHolding = false;

    private bool isInteract = false;
    private bool StopisInteract = false;

    private TMP_Text itemText; // ��������� ���������� ��� ������

    private void Start()
    {
        // ���� ��������� Text � �������� ��������� �������
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
                itemText.text = itemName; // ������������� �����
            }
        }
    }

    private void StopPhysicInteract()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.SetParent(null);
        isHolding = false;
        itemCanvas.enabled = false;

        // ���� �� ������ �������� ���� ��� �������� �������:
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
