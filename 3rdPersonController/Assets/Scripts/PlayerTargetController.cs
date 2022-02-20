using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetController : MonoBehaviour
{

    [Tooltip("The move speed constant")]
    [SerializeField] private float _walkSpeed;
    [Tooltip("The sprint move speed constant")]
    [SerializeField] private float _runSpeed;
    
    [Tooltip("The raw jump force added to the Rigidbody")]
    [SerializeField] private float _jumpForce;
    
    private Rigidbody _rb;
    private Vector3 _backMoveVector;

    private enum Controls
    {
        Forward = KeyCode.W,
        Back = KeyCode.S,
        Left = KeyCode.A,
        Right = KeyCode.D,
        Jump = KeyCode.Space,
        Sprint = KeyCode.LeftShift
    }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Movement();
        
        #if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.position + Vector3.up, Color.green, .001f, true); 
        #endif
    }

    private void Movement()
    {
        float trueWalkSpeed = 0;
        if (Input.GetKey((KeyCode) Controls.Sprint)) trueWalkSpeed = _runSpeed;
        else trueWalkSpeed = _walkSpeed;
            
        Vector3 moveVector = Vector3.zero;

        //Running forwards
        if (Input.GetKey((KeyCode) Controls.Forward))
            moveVector += Vector3.Normalize(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up));

        //Running backwards
        if (Input.GetKeyDown((KeyCode) Controls.Back))
            _backMoveVector = Vector3.Normalize(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up));
        if (Input.GetKey((KeyCode) Controls.Back)) moveVector -= _backMoveVector;

        //Running Left/Right
        if (Input.GetKey((KeyCode) Controls.Left))
            moveVector -= Vector3.Normalize(Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up));
        if (Input.GetKey((KeyCode) Controls.Right))
            moveVector += Vector3.Normalize(Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up));

        #if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.position + (moveVector * Time.deltaTime * trueWalkSpeed), Color.cyan, .001f, true);
        #endif
        
        transform.position += moveVector * Time.deltaTime * trueWalkSpeed;

        if (Input.GetKeyDown((KeyCode) Controls.Jump) && PlayerLerpController.isGrounded) _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Force);
        

    }
}
