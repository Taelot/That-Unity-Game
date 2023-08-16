// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public class TrackObject : MonoBehaviour
{

    public Transform targetObject;

    public float distanceUp = 3.5f;
    public float distanceAway = 7f;
    // Note that *late* update is used to ensure the most recent position of the
    // object being tracked is used. Otherwise the camera can lag behind the
    // tracked object by a frame.
    public float positionLerp = 0.05f;
    public float rotationLerp = 0.01f;
    void start()
    {
        // cameraOffset = transform.position - targetObject.transform.position;
    }

    private void LateUpdate()
    {

        Vector3 newPosition = targetObject.transform.position + Vector3.up * this.distanceUp - targetObject.transform.forward * this.distanceAway;
        newPosition.y = distanceUp; // fixes camera at y = 3.5
        transform.position = Vector3.Slerp(transform.position, newPosition, positionLerp);
        //transform.LookAt(targetObject.transform.position);
        transform.LookAt(new Vector3(targetObject.transform.position.x, 2f, targetObject.transform.position.z));

    }
}