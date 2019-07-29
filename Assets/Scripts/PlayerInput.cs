using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Image AimImage;
    [SerializeField] Chain chainPrefab;
    [SerializeField] Transform ShootPoint;

    [SerializeField] float MovementSpeed;
    [SerializeField] float RotationSpeed;
    [SerializeField] float ReloadTime;

    bool canShoot;

    float h, v;
    Vector3 inputDirection;
    float mouseX, mouseY;

    private void Awake()
    {
        AimImage.gameObject.SetActive(false);
        canShoot = true;
    }

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        inputDirection = new Vector3(h, 0, v);

        camera.transform.Rotate(new Vector3(mouseY, 0, 0) * RotationSpeed, Space.Self);
        Movement();
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1))
        {
            Aim();
        }
        else
        {
            if (AimImage.gameObject.activeInHierarchy)
            {
                AimImage.gameObject.SetActive(false);
            }
        }
    }

    void Movement()
    {
        transform.Translate(MovementSpeed * inputDirection * Time.deltaTime, Space.Self);
        transform.Rotate(new Vector3(0, mouseX, 0) * RotationSpeed);
    }

    void Aim()
    {
        if (!AimImage.gameObject.activeInHierarchy)
            AimImage.gameObject.SetActive(true);

        if (Input.GetMouseButton(0) && canShoot)
        {
            canShoot = false;
            StartCoroutine(Reload());
            Shoot();
        }
    }

    void Shoot()
    {
        Chain shootedChain = Instantiate(chainPrefab, ShootPoint.position, transform.rotation, transform);
        shootedChain.Player = this;
        Ray aimRay = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit aimPosition;
        if (Physics.Raycast(aimRay, out aimPosition))
        {
            shootedChain.SetTargetPoint(aimPosition.point);
        }
        else
        {
            shootedChain.SetTargetPoint(transform.forward.normalized);
        }
        shootedChain.Go();
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(ReloadTime);
        canShoot = true;
    }
}
