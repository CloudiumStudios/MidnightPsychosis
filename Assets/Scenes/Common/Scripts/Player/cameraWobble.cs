using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class cameraWobble : MonoBehaviour
{
    public AudioSource sound;

    public float amount = 10.0f;
    public float speed = 1.0f;
    private Vector3 lastPos;
    private float dist = 0.0f;
    private Vector3 rotation = Vector3.zero;

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastPos = transform.position;
    }

    void Update()
    {
        dist += (transform.position - lastPos).magnitude;
        lastPos = transform.position;
        rotation.z = Mathf.Sin(dist * speed) * amount;
        transform.localEulerAngles = rotation;

        if (player.GetComponent<PlayerController>().isMoving == true && sound.isPlaying == false)
        {
            sound.volume = Random.Range(0.6f, 0.8f);
            sound.pitch = Random.Range(0.8f, 1.1f);
            sound.Play();
        }
        if (player.GetComponent<PlayerController>().isMoving == false)
        {
            sound.Pause();
        }
    }
}
