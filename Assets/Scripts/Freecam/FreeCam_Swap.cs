using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class FreeCam_Swap : MonoBehaviour
{
    //get the input system
    public Player_Input input;

    [Header("Cam Stats")]
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float fastSpeed = 20f;
    [SerializeField] private float sensX = 3f;
    [SerializeField] private float sensY = 3f;
    //adjust this based on mouse sens
    [SerializeField] private float lookMultiplier = 0.02f;
    //color temp for camera effect
    [SerializeField] private float presentTemp = 58f;
    [SerializeField] private float pastTemp = -42f;

    private float camSpeed;

    //checks for cam Up/Down
    private bool camUp;
    private bool camDown;

    //storing rotation
    private float yRot;
    private float xRot;

    [Header("Camera TimeSwap")]
    private bool inPresent;
    public Camera activeCam = null;

    [SerializeField] private Volume volumeTemp;

    public UnityEvent switchEvent = new UnityEvent();
    private void Awake()
    {
        Player_Input._camUpAction += CamUp;
        Player_Input._camUpStopAction += CamUpStop;
        Player_Input._camDownAction += CamDown;
        Player_Input._camDownStopAction += CamDownStop;
        Player_Input._camFastStartAction += StartCamFast;
        Player_Input._camFastStopAction += StopCamFast;
        Player_Input._camTimeSwapAction += CamTimeSwap;

        inPresent = false;
        CamTimeSwap();

        //set cam speed to base speed
        camSpeed = baseSpeed;

        //set object to inactive after initialize
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        MoveCam();
    }

    private void MoveCam()
    {
        //get camera move input value
        Vector2 moveInput = input._camMove.ReadValue<Vector2>();

        //store horizontal and vertical movement
        float horizontalMovement = moveInput.x;
        float verticalMovement = moveInput.y;
        Vector3 moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;

        //get camera look input value
        Vector2 targetMouseDelta = input._camLook.ReadValue<Vector2>();
        float mouseX = targetMouseDelta.x;
        float mouseY = targetMouseDelta.y;

        //multiply and store input float value times sensitivity
        //lookMultiplier for sensitivity
        yRot += -mouseY * sensY * lookMultiplier;
        xRot += mouseX * sensX * lookMultiplier;

        //apply rotation to transform euler angles
        transform.localEulerAngles = new Vector3(yRot, xRot, 0f);

        //move the camera
        transform.position = transform.position + moveDirection * camSpeed * Time.deltaTime;

        //move camera up and down
        if (camUp && !camDown)
        {
            transform.position = transform.position + (transform.up * camSpeed * Time.deltaTime);
        }
        if (camDown && !camUp)
        {
            transform.position = transform.position + (-transform.up * camSpeed * Time.deltaTime);
        }
    }

    //camera time swapping for Recurrent
    private void CamTimeSwap()
    {
        //static layers to always render
        LayerMask combinedMask = LayerMask.GetMask("StaticObject");
        //store combined mask
        if(volumeTemp != null)
        {
            //get whitebalance var from volume temp
            //if you want camera effects, make sure your freecam has post processing checked under rendering
            volumeTemp.profile.TryGet<WhiteBalance>(out WhiteBalance whiteBalance);

            //if in the present, flip to past
            if (inPresent)
            {
                //add past layer to combined mask
                LayerMask pastMask = LayerMask.GetMask("Past") | combinedMask;
                activeCam.cullingMask = pastMask;
                

                //apply past temp to volume
                whiteBalance.temperature.value = pastTemp;
                inPresent = !inPresent;
            }
            //if in the past, flip to present
            else
            {
                //add present layer to combined mask
                LayerMask presentMask = LayerMask.GetMask("Present") | combinedMask;
                activeCam.cullingMask = presentMask;

                //apply present temp to volume
                whiteBalance.temperature.value = presentTemp;
                inPresent = !inPresent;
            }
        }
    }

    //just new input system things
    private void CamUp() { camUp = true; }
    private void CamUpStop() { camUp = false; }
    private void CamDown() { camDown = true; }
    private void CamDownStop() { camDown = false; }
    private void StartCamFast() { camSpeed = fastSpeed; }
    private void StopCamFast() { camSpeed = baseSpeed; }
}
