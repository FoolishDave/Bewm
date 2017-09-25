using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public Vector3 MaxVelocity;
    public Vector3 MinVelocity;
    private Rigidbody rigidBody;
    private PlayerInfo pInfo;
    private bool onGround = true;
    public float JumpForce;


	// Use this for initialization
	void Start ()
	{
	    rigidBody = GetComponent<Rigidbody>();
	    pInfo = GetComponent<PlayerInfo>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        Vector3 forceVector = new Vector3();
	    forceVector += transform.forward * Input.GetAxisRaw("Vertical " + (int) pInfo.ControllerType) * Speed;
	    forceVector += transform.right * Input.GetAxisRaw("Horizontal " + (int) pInfo.ControllerType) * Speed;

	    if (Input.GetAxisRaw("Jump " + (int) pInfo.ControllerType) >= 0.99f && onGround)
	    {
	        forceVector += Vector3.up * Input.GetAxisRaw("Jump " + (int) pInfo.ControllerType) * JumpForce;
	        onGround = false;
	    }
	    rigidBody.AddForce(forceVector);

	    rigidBody.velocity = ClampVec3(rigidBody.velocity, MinVelocity, MaxVelocity);
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collided with " + col.collider.tag);
        if (col.collider.CompareTag("Ground") && col.transform.position.y < transform.position.y)
        {
            Debug.Log("Player is grounded.");
            onGround = true;
        }
    }

    Vector3 ClampVec3(Vector3 toClamp, Vector3 min, Vector3 max)
    {
        Vector3 returnVector = new Vector3();

        returnVector.x = Mathf.Clamp(toClamp.x, min.x, max.x);
        returnVector.y = Mathf.Clamp(toClamp.y, min.y, max.y);
        returnVector.z = Mathf.Clamp(toClamp.z, min.z, max.z);

        return returnVector;
    }
}
