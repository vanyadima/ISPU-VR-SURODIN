using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour, IInteractable
{
    public List<GameObject> objectsToMove; // ������ �������� ��� �����������
    public float distanceToMove = 1.0f; // ����������, �� ������� ����� ����������� �������
    public float moveDuration = 1.0f; // ����������������� �����������

    private bool isMovedUp = false; // ���� ��� ������������ �������� ��������� ��������
    private bool isMoving = false; // ���� ��� �������������� ���������� ������ �� ����� ��������

    public void OnAddInteractKeyDown()
    {
        
    }

    public void OnAddInteractKeyUp()
    {
        
    }

    public void OnInteractKeyDown()
    {
        ToggleObjectsPosition();
    }

    public void OnInteractKeyUp()
    {
        
    }

    // ������� ��� ������ �����, ����� ����������� ��������� ��������
    public void ToggleObjectsPosition()
    {
        // ���������, ���� �� � ������ ������ �����������
        if (isMoving)
        {
            return; // ���� ����, �� ������ �� ������
        }

        isMoving = true; // ������������� ����, ��� �������� �����������

        // ���������, ���������� �� ������� �����
        if (isMovedUp)
        {
            // ���� ��, �� �������� �� ����
            foreach (var obj in objectsToMove)
            {
                StartCoroutine(MoveObject(obj, obj.transform.position - new Vector3(0, distanceToMove, 0), moveDuration));
            }
        }
        else
        {
            // ���� ���, �� ��������� �� �����
            foreach (var obj in objectsToMove)
            {
                StartCoroutine(MoveObject(obj, obj.transform.position + new Vector3(0, distanceToMove, 0), moveDuration));
            }
        }
        // ����������� ���������
        isMovedUp = !isMovedUp;
    }

    // �������� ��� �������� ����������� �������
    IEnumerator MoveObject(GameObject obj, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = obj.transform.position;

        while (time < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPosition; // ���������, ��� ������ ������ �������� �������

        // ����� ����������� ���������� ������� �������� ���� isMoving
        if (obj == objectsToMove[objectsToMove.Count - 1])
        {
            isMoving = false;
        }
    }
}
