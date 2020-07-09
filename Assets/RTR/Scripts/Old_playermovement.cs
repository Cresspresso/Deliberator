using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Old_PlayerController))]
[RequireComponent(typeof(CharacterController))]
public class Old_playermovement : MonoBehaviour
{
	public Old_PlayerController player { get; private set; }
	public CharacterController controller { get; private set; }

	public float moveSpeed = 8.0f;
    public float jumpHeight = 3.0f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;

    public Transform groundCheck;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

	private AudioSource audioSource;

	private void Awake()
	{
		player = GetComponent<Old_PlayerController>();
		controller = GetComponent<CharacterController>();
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2.0f;
        }

		if (player.isGameControlEnabled)
		{
			float x = Input.GetAxis("Horizontal");
			float z = Input.GetAxis("Vertical");

			Vector3 move = transform.right * x + transform.forward * z;

			controller.Move(move * moveSpeed * Time.deltaTime);

			if (Input.GetButtonDown("Jump") && isGrounded)
			{
				velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
			}
		}

		if (isGrounded && audioSource.isPlaying == false && controller.velocity.magnitude > 3.0f)
		{
			audioSource.volume = Random.Range(0.3f, 0.7f);
			audioSource.pitch = Random.Range(0.7f, 0.9f);
			audioSource.Play();
		}

		velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
