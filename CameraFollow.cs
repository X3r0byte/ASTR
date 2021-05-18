using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// a simple camera script that has a smooth follow.

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Object to follow")]
    // This is the object that the camera will follow
    public Transform target;
    public PlayerController pc;
    float xPosModifier = 0;
    float yPosModifier = 0;
    float zPosModifier = 0;
    float yAdjustmentModifier = 0;
    private Vector3 lerpedPosition;
    private Vector3 tempPosition; 

    void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // FixedUpdate is called every frame, when the physics are calculated
    void FixedUpdate()
    {
        if (target != null)
        {
            yPosModifier = 0;
            print(pc.acceleration * pc.velocity);

            lerpedPosition = Vector3.Lerp(transform.position, new Vector3((target.position.x + xPosModifier),
                (target.position.y + yAdjustmentModifier + yPosModifier),
                (target.position.z + zPosModifier)),
                Time.deltaTime * 4f);

            if (zPosModifier == 0)
            {
                lerpedPosition.z = -10f;
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Move the camera in the position found previously
            transform.position = lerpedPosition;
        }
    }

    public void setTarget(Transform tgt)
    {
        target = tgt;
        print(target.name);
    }
}
