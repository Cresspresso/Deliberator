using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_mouselook : MonoBehaviour
{
    public Old_PlayerController player { get; private set; }

    //public variables
    public float mouseSensitivity = 100.0f;
    public Transform playerBody;

    //private variables
    private float xRotation = 0.0f;

    private void Awake()
    {
        player = GetComponentInParent<Old_PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float speedMult = player.isGameControlEnabled ? 1.0f : 0.1f;
        float ds = mouseSensitivity * speedMult * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * ds;
        float mouseY = Input.GetAxis("Mouse Y") * ds;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
