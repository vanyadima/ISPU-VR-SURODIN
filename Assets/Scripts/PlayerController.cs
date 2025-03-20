using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public bool UseSmoothMovement = true;
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100.0f;
    public LayerMask waypointLayer; // ���������, ��� ��������� ���� � ��������� Unity
    public float interactionDistance = 5f; // ���������, �� ������� ����� ����� ����������������� � ���������
    public Canvas constCanvas;

    private Rigidbody rb;
    public Transform cameraTransform;

    private float xRotation = 0f;
    private Transform currentWaypoint; // ������� waypoint

    public Material highlightMaterial; // ���������� �������� ��� waypoint
    private Material originalMaterial; // �������� �������� waypoint
    private Renderer currentHighlightedRenderer; // ������� Renderer, ������� ��������������
    private GameObject currentHighlightedChild;

    private bool previousUseSmoothMovement;
    GameObject[] cachedWaypoints;
    private TMP_Text itemText; // ��������� ���������� ��� ������

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // ������ � ��������� ������
        previousUseSmoothMovement = !UseSmoothMovement;
        itemText = constCanvas.GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        // ���������� ���������� UseSmoothMovement � ������� ������ 1 � 2
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

        // ��������� ��������� � ���������
        RaycastHighlight();

        // �������� �� ��������� ��������� UseSmoothMovement
        if (previousUseSmoothMovement != UseSmoothMovement)
        {
            UpdateWaypointVisibility();
            previousUseSmoothMovement = UseSmoothMovement; // ���������� ����������� ��������
        }
    }

    private void UpdateWaypointVisibility()
    {
        // ���������� ��� ��������� ���� �������� � ����� "Waypoint" � ����������� �� ��������� UseSmoothMovement
        if (cachedWaypoints == null)
            cachedWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        bool shouldBeActive = !UseSmoothMovement;
        if (itemText != null)
        {
            itemText.text = UseSmoothMovement? "�������� ������ ����������� ������ 1 ��� 2.\n������ ������������: ������� (WASD)" : "�������� ������ ����������� ������ 1 ��� 2.\n������ ������������: �� ������"; // ������������� �����
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

        // ����� ��������� ����������� �������
        if (currentHighlightedRenderer != null)
        {
            currentHighlightedRenderer.material = originalMaterial;
            currentHighlightedRenderer = null;

            // ��������� ���������� �������� ������ � ������������� ������� ������, ���� ��� ���� �������
            if (currentHighlightedChild != null)
            {
                ParticleSystem particleSystem = currentHighlightedChild.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Stop(); // ���������� ������� ������ ����� �����������
                }
                currentHighlightedChild.SetActive(false);
                currentHighlightedChild = null;
            }
        }

        // ��������� ������ �������
        if (Physics.Raycast(ray, out hit, interactionDistance, waypointLayer))
        {
            if (hit.collider.CompareTag("Waypoint"))
            {
                Renderer waypointRenderer = hit.transform.GetComponent<Renderer>();
                if (waypointRenderer != null)
                {
                    // ��������� �������� �������� � ��������� ����������
                    originalMaterial = waypointRenderer.material;
                    waypointRenderer.material = highlightMaterial;
                    currentHighlightedRenderer = waypointRenderer;

                    // �������� �������� ������ (��������������, ��� ��� ������ �������� �������)
                    Transform child = hit.transform.GetChild(0); // ���������, ��� ������ ����������
                    if (child != null)
                    {
                        child.gameObject.SetActive(true);
                        currentHighlightedChild = child.gameObject;

                        // ������������� ������� ������, ���� ��� ����
                        ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                        if (particleSystem != null)
                        {
                            particleSystem.Clear(); // ������ ������� ������
                            particleSystem.Play(); // ������ ������� ������
                        }
                    }
                }
            }
        }
    }

    void HandleWaypointSelection()
    {
        if (Input.GetMouseButtonDown(0)) // ��� ������� ����� ������ ����
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
        // ����� �� ������ ������������ transform.position = waypoint.position; ��� ����������� �����������
        // ��� ������������ ��������/lerp ��� �������� �����������:
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
