using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLerpController : MonoBehaviour
{
    [Tooltip("The movement target for the player")]
    [SerializeField] private GameObject _lerpTarget;
    private Rigidbody _rb;

    [Tooltip("How responsive the player's movement is from a resting position")]
    [SerializeField] private float _lerpThreshold;
    [Tooltip("The responsivnesss of the player's object")]
    [SerializeField] private float _lerpSpeed;
    [SerializeField] private float _jumpLerpSpeed;
    
    [Tooltip("The max distance from the ground that we consider the player to be \"Grounded\"")]
    [SerializeField] private float _inAirThreshhold;

    public static bool isGrounded = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        RotationLock();
        Lerp();
        AirtimeCheck();
        
                
        #if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.GetChild(0).transform.position, Color.cyan, .001f, true); 
        #endif
    }

    private void RotationLock()
    {
        //Locking the rotation and setting LookRotation so the object faces the correct direction
        transform.rotation = Quaternion.LookRotation(transform.position - _lerpTarget.transform.position, Vector3.up);
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
    }

    private void Lerp()
    {
        //A lerp
        if (Vector3.Distance(_lerpTarget.transform.position, transform.position) > _lerpThreshold)
        {
            transform.position = Vector3.Slerp(transform.position, _lerpTarget.transform.position, Time.deltaTime * _lerpSpeed);
        }
    }

    private void AirtimeCheck()
    {
        //The true ground raycast
        RaycastHit hit;
        var ray = new Ray(transform.position, -Vector3.up);
        isGrounded = Physics.Raycast(ray, out hit, _inAirThreshhold);
        
        //Our debug ray
        #if UNITY_EDITOR
        Debug.DrawRay(transform.position, -Vector3.up * _inAirThreshhold, Color.green, .001f, true);
        #endif

        //Setting physyics on/off for smooth jumping
        if (isGrounded) _rb.isKinematic = false;
        else _rb.isKinematic = true;
    }
}
