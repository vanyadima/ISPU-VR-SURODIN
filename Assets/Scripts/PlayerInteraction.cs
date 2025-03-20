using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 5f; // ���������, �� ������� ����� ����� ����������������� � ���������

    private IInteractable currentInteractable;

    void Update()
    {
        // ��� ������� ����� ������ ����
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            //// ���� ��� ���������� �����-���� ������ � �������� �������� ���������
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                // ����������� �������� ��������� IInteractable � �������
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    currentInteractable = interactable;
                    interactable.OnInteractKeyDown();
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && currentInteractable != null)
        {
            currentInteractable.OnInteractKeyUp();
            currentInteractable = null; // ������� ������� ������������� ������ ����� ���������� ������
        }

        if (Input.GetMouseButtonUp(1) && currentInteractable != null)
            currentInteractable.OnAddInteractKeyUp();

        if (Input.GetMouseButtonDown(1) && currentInteractable != null)
            currentInteractable.OnAddInteractKeyDown();
    }
}

public interface IInteractable
{
    void OnInteractKeyDown();
    void OnInteractKeyUp();

    void OnAddInteractKeyDown();
    void OnAddInteractKeyUp();
}
