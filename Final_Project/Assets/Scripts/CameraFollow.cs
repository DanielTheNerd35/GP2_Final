using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followObject; //this will be assigned by the inspector

    public void Awake()
    {
        FindObjectOfType<AudioManager>().Play("BGMusic");
    }


    void FixedUpdate()
    {
        Vector3 currentPosition = transform.position; 
        // store this object's current position in a temporary variable

        currentPosition.x = Mathf.Lerp(currentPosition.x, followObject.position.x, .5f); // Does some math to figure out the position it needs to follow
        currentPosition.z = -15;
        currentPosition.y = Mathf.Lerp( 1f , followObject.position.y, .5f);

        transform.position = currentPosition;

    }
}
