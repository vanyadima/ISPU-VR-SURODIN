using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour, IInteractable
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootForce = 20f;
    public AudioSource shootSound;
    private bool isPickedUp = false;

    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isPickedUp && triggerAction.stateDown)
        //{
        //    Shoot();
        //}
    }

    public void PickedUp()
    {
        isPickedUp = true;
    }

    public void Drop()
    {
        isPickedUp = false;
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        }
        if (shootSound != null)
            shootSound.Play();
    }

    public void OnInteractKeyDown()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        originalParent = transform.parent;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        GameObject weaponPlace = GameObject.FindGameObjectWithTag("WeaponPlace");

        if (weaponPlace != null)
        {
            // ѕримагнитить оружие к WeaponPlace
            transform.SetParent(weaponPlace.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    public void OnInteractKeyUp()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        // ќтсоединить оружие и вернуть его на изначальное место
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        Drop();
    }

    public void OnAddInteractKeyDown()
    {
        Shoot();
    }

    public void OnAddInteractKeyUp()
    {
        
    }
}
