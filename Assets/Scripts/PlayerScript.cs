using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour {
    public float movementSpeed;
    public float sensitivity;
    private Sprite cross;
    private List<GameObject> tempArray = new List<GameObject>();
    private GameObject tempObj;
    private Usable _targetObj;
    private Usable TargetObj
    {
        get => _targetObj;
        set
        {
            if (_targetObj)
                _targetObj.SetOutlineState(false);
            _targetObj = value;
            if (_targetObj)
                _targetObj.SetOutlineState(true);
        }
    }
    private Rigidbody rigid;
    public Font useFont;
    private GameObject head;
    private Camera camera;
    private Animator animator;
    private float headXAngles;
    private GameObject point;
    private GameObject canvas;
    public bool onlyHead;
    private bool picked;
    private string pickedTag;
    private GameObject pickedObject;
    public float jumpForce;
    public Text useText;
    public PlayerInput _input;
    private bool sendinginput;
    private Cassa targetCassa;
    private Vector3 lasteyesPos;
    private Quaternion lasteyesRot;
    public bool isGrounded;

    private float xRotation;
    [SerializeField]
    private CharacterController characterController;

    private Vector3 velocity;
    private float gravity = -9.81f;
    public LayerMask groundMask;
    private Transform groundCheck;
    // Use this for initialization

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.LeftClick.performed += context => LeftClick();
        animator = GetComponent<Animator>();
        camera = transform.GetChild(0).GetComponent<Camera>();
        groundCheck = transform.GetChild(1);
        canvas = GameObject.Find("GameUI");
        point = canvas.transform.Find("Point").gameObject;
        cross = Resources.Load<Sprite>("Sprites/" + "Cross");
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void LeftClick()
    {
        print("HUY");
        if (TargetObj)
        {
            TargetObj.Use();
        }
    }
    private void Move(Vector2 moveDirection)
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, 0.4f, groundMask);
        
        float scaledMoveSpeed = movementSpeed * Time.deltaTime;
        Vector3 move = transform.forward * moveDirection.y + transform.right * moveDirection.x;
        characterController.Move(move * scaledMoveSpeed);
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }
    private void Look(Vector2 lookDirection)
    {
        float scaledLookSpeed = sensitivity * Time.deltaTime;
        
        xRotation -= lookDirection.y * scaledLookSpeed;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up, lookDirection.x * scaledLookSpeed);
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector2 moveDirection = _input.Player.Move.ReadValue<Vector2>();
        Move(moveDirection);
        Vector2 lookDirection = _input.Player.Look.ReadValue<Vector2>();
        Look(lookDirection);
        if (Physics.Raycast(new Ray(camera.transform.position, camera.transform.forward), out RaycastHit rayHit, 20f, ~Physics.IgnoreRaycastLayer))
        {
            print(rayHit.collider.name);
            GameObject hitTarget = rayHit.collider.gameObject;
            if (TargetObj && hitTarget.gameObject == TargetObj.gameObject)
                return;
            if (rayHit.collider.GetComponent<Usable>())
            {
                TargetObj = rayHit.collider.GetComponent<Usable>();
                print("Usable");
            }
            else
                TargetObj = null;
        }
        else
            TargetObj = null;
    }

    /*
    if (!sendinginput) {
        //Raycasting
        RaycastHit headHit;
        foreach (GameObject gObj in tempArray)
            Destroy(gObj);
        targetObj = null;
        useText.text = string.Empty;
        if (Physics.Raycast(new Ray(eyes.transform.position, eyes.transform.forward), out headHit, 20f))
        {
            if (Vector3.Distance(head.transform.position, headHit.point) < 3f)
            {
                targetObj = headHit.collider.gameObject;
                point.GetComponent<Image>().color = Color.green;
                if (headHit.collider.CompareTag("Usable"))
                    useText.text = targetObj.GetComponent<Usable>().useString;
            }
        }
        //
        //Use
        if (Input.GetKeyDown(KeyCode.E))
            if (picked)
            {
                picked = false;
                pickedObject.tag = pickedTag;
                pickedTag = null;
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
            }
            else if (targetObj != null)
            {
                if (targetObj.GetComponent<Usable>())
                    targetObj.GetComponent<Usable>().Use(gameObject);
                else if (targetObj.GetComponent<Rigidbody>())
                {
                    PickObject(targetObj);
                }
            }
        //
        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Rigidbody>().AddRelativeForce(transform.up * jumpForce);
        //
        //Interact
        if (Input.GetMouseButtonDown(0))
        {
            if (targetObj.GetComponent<Cassa>())
            {
                sendinginput = true;
                targetCassa = targetObj.GetComponent<Cassa>();
                targetCassa.Activate();
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                lasteyesPos = eyes.transform.position;
                lasteyesRot = eyes.transform.rotation;
                float ypos = eyes.transform.position.y;
                eyes.transform.position = targetCassa.gameObject.transform.position - targetCassa.gameObject.transform.forward * 0.75f;
                eyes.transform.LookAt(targetCassa.gameObject.transform);
                eyes.transform.position = new Vector3(eyes.transform.position.x, ypos, eyes.transform.position.z);
            }
        }
        //
    }
    else
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            sendinginput = false;
            targetCassa.Deactivate();
            eyes.transform.rotation = lasteyesRot;
            eyes.transform.position = lasteyesPos;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
*/
    public void PickObject(GameObject target)
    {
        pickedObject = target;
        pickedTag = pickedObject.tag;
        pickedObject.tag = "Picked";
        pickedObject.GetComponent<Rigidbody>().useGravity = false;
        picked = true;
    }
    public void AttachTo(Transform parent)
    {
        GetComponent<PlayerScript>().onlyHead = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        transform.SetParent(parent);
    }
    
    // Update is called once per frame
    /*
    void FixedUpdate () {
        if (!sendinginput)
        {
            if (picked)
                pickedObject.transform.position = camera.transform.position + camera.transform.forward * 1.5f;
            //Rotating
            if (onlyHead)
            {
                camera.transform.localRotation = Quaternion.Euler(headXAngles = Mathf.Clamp(headXAngles + -Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime, -90f, 90f), camera.transform.localEulerAngles.y, 0f);
                camera.transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime, 0);
            }
            else
            {
                camera.transform.localRotation = Quaternion.Euler(headXAngles = Mathf.Clamp(headXAngles + -Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime, -90f, 90f), 0f, 0f);
                transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime, 0);
                //Movement
                if (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
                    transform.position += (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * Time.fixedDeltaTime * movementSpeed;
            }
            //
        }
    }
    */
}
