using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public bool UseSmoothMovement = true;
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100.0f;
    public LayerMask waypointLayer; // Убедитесь, что настроили слой в редакторе Unity
    public float interactionDistance = 5f; // Дистанция, на которой игрок может взаимодействовать с объектами
    public Canvas constCanvas;

    private Rigidbody rb;
    public Transform cameraTransform;

    private float xRotation = 0f;
    private Transform currentWaypoint; // Текущий waypoint

    public Material highlightMaterial; // Светящийся материал для waypoint
    private Material originalMaterial; // Исходный материал waypoint
    private Renderer currentHighlightedRenderer; // Текущий Renderer, который подсвечивается
    private GameObject currentHighlightedChild;

    private bool previousUseSmoothMovement;
    GameObject[] cachedWaypoints;
    private TMP_Text itemText; // Добавляем переменную для текста

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Прячет и блокирует курсор
        previousUseSmoothMovement = !UseSmoothMovement;
        itemText = constCanvas.GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        // Управление состоянием UseSmoothMovement с помощью клавиш 1 и 2
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSmoothMovement = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSmoothMovement = false;
        }

        if (UseSmoothMovement)
            HandleMovement();
        else
            HandleWaypointSelection();

        HandleMouseLook();

        // Обработка наведения и подсветки
        RaycastHighlight();

        // Проверка на изменение состояния UseSmoothMovement
        if (previousUseSmoothMovement != UseSmoothMovement)
        {
            UpdateWaypointVisibility();
            previousUseSmoothMovement = UseSmoothMovement; // Обновление предыдущего значения
        }
    }

    private void UpdateWaypointVisibility()
    {
        // Отключение или включение всех объектов с тегом "Waypoint" в зависимости от состояния UseSmoothMovement
        if (cachedWaypoints == null)
            cachedWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        bool shouldBeActive = !UseSmoothMovement;
        if (itemText != null)
        {
            itemText.text = UseSmoothMovement? "Изменить способ перемещения кнопка 1 или 2.\nСпособ передвижения: Плавное (WASD)" : "Изменить способ перемещения кнопка 1 или 2.\nСпособ передвижения: По точкам"; // Устанавливаем текст
        }

        foreach (GameObject waypoint in cachedWaypoints)
        {
            waypoint.SetActive(shouldBeActive);
        }
    }

    void RaycastHighlight()
    {
        Ray ray = cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // Сброс подсветки предыдущего объекта
        if (currentHighlightedRenderer != null)
        {
            currentHighlightedRenderer.material = originalMaterial;
            currentHighlightedRenderer = null;

            // Отключаем предыдущий дочерний объект и останавливаем систему частиц, если она была активна
            if (currentHighlightedChild != null)
            {
                ParticleSystem particleSystem = currentHighlightedChild.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Stop(); // Остановить систему частиц перед отключением
                }
                currentHighlightedChild.SetActive(false);
                currentHighlightedChild = null;
            }
        }

        // Подсветка нового объекта
        if (Physics.Raycast(ray, out hit, interactionDistance, waypointLayer))
        {
            if (hit.collider.CompareTag("Waypoint"))
            {
                Renderer waypointRenderer = hit.transform.GetComponent<Renderer>();
                if (waypointRenderer != null)
                {
                    // Сохраняем исходный материал и применяем светящийся
                    originalMaterial = waypointRenderer.material;
                    waypointRenderer.material = highlightMaterial;
                    currentHighlightedRenderer = waypointRenderer;

                    // Включаем дочерний объект (предполагается, что это первый дочерний элемент)
                    Transform child = hit.transform.GetChild(0); // Убедитесь, что индекс правильный
                    if (child != null)
                    {
                        child.gameObject.SetActive(true);
                        currentHighlightedChild = child.gameObject;

                        // Воспроизводим систему частиц, если она есть
                        ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                        if (particleSystem != null)
                        {
                            particleSystem.Clear(); // Запуск системы частиц
                            particleSystem.Play(); // Запуск системы частиц
                        }
                    }
                }
            }
        }
    }

    void HandleWaypointSelection()
    {
        if (Input.GetMouseButtonDown(0)) // При нажатии левой кнопки мыши
        {
            Ray ray = cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance, waypointLayer))
            {
                if (hit.collider.CompareTag("Waypoint"))
                {
                    MoveToWaypoint(hit.transform);
                }
            }
        }
    }

    void MoveToWaypoint(Transform waypoint)
    {
        // Здесь вы можете использовать transform.position = waypoint.position; для мгновенного перемещения
        // или использовать корутину/lerp для плавного перемещения:
        //StartCoroutine(SmoothMove(waypoint.position));
        transform.position = waypoint.position;
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        Vector3 movement = transform.TransformDirection(moveDirection) * speed;
        movement.y = gravity;

        rb.velocity = new Vector3(movement.x, rb.velocity.y + movement.y * Time.deltaTime, movement.z);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
