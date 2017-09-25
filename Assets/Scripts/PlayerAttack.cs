using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{

    public float MaxCharge = 5f;
    public float Range = 2f;
    public float PushForce = 200f;
    public float PushVertMult = 100f;
    public List<GameObject> ChargeLights;
    public Material EnabledMaterial;
    public Material DisabledMaterial;
    private float currentCharge;
    private AudioSource audioSource;
    private ParticleSystem particleSystem;
    private PlayerInfo pInfo;
    
	// Use this for initialization
	void Start ()
	{
	    currentCharge = MaxCharge;
	    audioSource = GetComponent<AudioSource>();
	    particleSystem = GetComponentInChildren<ParticleSystem>();
	    pInfo = GetComponent<PlayerInfo>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetAxisRaw("Fire " + (int) pInfo.ControllerType) >= 0.99f && currentCharge / MaxCharge >= 1f / ChargeLights.Count)
	    {
	        audioSource.Play();
	        particleSystem.Emit(50);
            DisableLights();
	        
	        Transform lookTransform = GetComponentInChildren<Camera>().transform;
            Collider[] hit = Physics.OverlapBox(lookTransform.position + lookTransform.forward * Range, new Vector3(1.2f, 0.75f, Range / 2),
	            lookTransform.rotation);
	        foreach (Collider toPush in hit.Where(obj => obj.CompareTag("Pushable")))
	        {
	            Debug.Log("Pushing thing.");
	            Vector3 pushForce = (lookTransform.forward).normalized * PushForce; //* (currentCharge / MaxCharge);
	            pushForce.y *= PushVertMult;
                Debug.Log(string.Format("Push Vector x:{0},y{1},z{2}",pushForce.x,pushForce.y,pushForce.z));
                toPush.GetComponent<Rigidbody>().AddForce(pushForce);
	        }
	        currentCharge = 0.0f;
        }
	    EnableLights();
	    if (currentCharge < MaxCharge)
	    {
	        currentCharge += Time.deltaTime;
	    }
	}

    void EnableLights()
    {
        for (int i = 0; i < ChargeLights.Count; i++)
        {
            float lightPercent = (i + 1f) / ChargeLights.Count;

            if (currentCharge / MaxCharge >= lightPercent)
            {
                ChargeLights[i].GetComponent<Renderer>().material = EnabledMaterial;
            }
        }
    }

    void DisableLights()
    {
        foreach (GameObject light in ChargeLights)
        {
            Material mat = light.GetComponent<Renderer>().material = DisabledMaterial;
        }
    }

    void FixedUpdate()
    {
        
    }

}
