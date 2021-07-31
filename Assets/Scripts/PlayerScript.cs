using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    public float movementSpeed;
    public float sensitivity;
    private Sprite cross;
    private List<GameObject> tempArray = new List<GameObject>();
    private GameObject tempObj;
    private GameObject targetObj;
    private Rigidbody rigid;
    public Font useFont;
    private GameObject head;
    private GameObject eyes;
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
    private bool sendinginput;
    private Cassa targetCassa;
    private Vector3 lasteyesPos;
    private Quaternion lasteyesRot;
    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        head = transform.GetChild(0).GetChild(0).gameObject;
        eyes = head;
        canvas = GameObject.Find("GameUI");
        point = canvas.transform.Find("Point").gameObject;
        cross = Resources.Load<Sprite>("Sprites/" + "Cross");
        rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /*
    private void Update()
    {
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
    void FixedUpdate () {
        if (!sendinginput)
        {
            if (picked)
                pickedObject.transform.position = head.transform.position + head.transform.forward * 1.5f;
            //Rotating
            if (onlyHead)
            {
                head.transform.localRotation = Quaternion.Euler(headXAngles = Mathf.Clamp(headXAngles + -Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime, -90f, 90f), head.transform.localEulerAngles.y, 0f);
                head.transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime, 0);
            }
            else
            {
                head.transform.localRotation = Quaternion.Euler(headXAngles = Mathf.Clamp(headXAngles + -Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime, -90f, 90f), 0f, 0f);
                transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime, 0);
                //Movement
                if (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
                    transform.position += (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * Time.fixedDeltaTime * movementSpeed;
            }
            //
        }
    }
}
