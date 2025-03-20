using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour, IInteractable
{
    public List<GameObject> objectsToMove; // Список объектов для перемещения
    public float distanceToMove = 1.0f; // Расстояние, на которое нужно переместить объекты
    public float moveDuration = 1.0f; // Продолжительность перемещения

    private bool isMovedUp = false; // Флаг для отслеживания текущего состояния объектов
    private bool isMoving = false; // Флаг для предотвращения повторного вызова во время движения

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

    // Функция для вызова извне, чтобы переключить положение объектов
    public void ToggleObjectsPosition()
    {
        // Проверяем, идет ли в данный момент перемещение
        if (isMoving)
        {
            return; // Если идет, то ничего не делаем
        }

        isMoving = true; // Устанавливаем флаг, что началось перемещение

        // Проверяем, перемещены ли объекты вверх
        if (isMovedUp)
        {
            // Если да, то опускаем их вниз
            foreach (var obj in objectsToMove)
            {
                StartCoroutine(MoveObject(obj, obj.transform.position - new Vector3(0, distanceToMove, 0), moveDuration));
            }
        }
        else
        {
            // Если нет, то поднимаем их вверх
            foreach (var obj in objectsToMove)
            {
                StartCoroutine(MoveObject(obj, obj.transform.position + new Vector3(0, distanceToMove, 0), moveDuration));
            }
        }
        // Переключаем состояние
        isMovedUp = !isMovedUp;
    }

    // Корутина для плавного перемещения объекта
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

        obj.transform.position = targetPosition; // Убедитесь, что объект достиг конечной позиции

        // После перемещения последнего объекта сбросить флаг isMoving
        if (obj == objectsToMove[objectsToMove.Count - 1])
        {
            isMoving = false;
        }
    }
}
