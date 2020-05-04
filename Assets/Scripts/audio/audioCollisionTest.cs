using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioCollisionTest : MonoBehaviour
{
    public AudioSource collisionSource;

    // Start is called before the first frame update
    void Start()
    {
        collisionSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("object velocity is: " + gameObject.GetComponent<Rigidbody>().velocity.y);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("object relative velocity is: " + collision.relativeVelocity.magnitude);

        if (collision.relativeVelocity.magnitude > 1.0f)
        {
            float newVolume = collision.relativeVelocity.magnitude * 0.1f;
            collisionSource.volume = Mathf.Clamp(newVolume, 0.0f, 0.5f);

            Debug.Log("audio volume is: " + Mathf.Clamp(newVolume, 0.0f, 0.5f));

            collisionSource.PlayOneShot(collisionSource.clip);
        }
    }
}
