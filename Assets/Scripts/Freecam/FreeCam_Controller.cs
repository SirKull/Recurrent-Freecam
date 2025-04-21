using UnityEngine;
using UnityEngine.Events;

public class FreeCam_Controller : MonoBehaviour
{
    //Player Input
    public Player_Input playerInput;
    //freecam object to enable/disable
    public GameObject freeCamera;
    //maincam object to disable
    public GameObject mainCamera;
    //store the position of your camera object 
    public Transform cameraPosition;
    //store the position of your player controller
    public GameObject playerController;

    private bool freeCamEnabled;

    private void Awake()
    {
        //store Player Input
        playerInput = FindAnyObjectByType<Player_Input>();

        Player_Input.enableFreeCamAction += EnableFreecam;
        Player_Input.disableFreeCamAction += DisableFreecam;
        freeCamEnabled = false;
    }

    private void Update()
    {
        if (!freeCamEnabled)
        {
            //have freecam follow main cam if freecam is disabled
            freeCamera.transform.position = cameraPosition.position;
        }
    }

    private void EnableFreecam()
    {
        if (!freeCamEnabled)
        {
            //apply main cam rot to freecam
            freeCamera.transform.rotation = mainCamera.transform.rotation;
            //swap active cams
            freeCamera.SetActive(true);
            mainCamera.SetActive(false);
            //swap active input action
            playerInput.playerActions.FreeCam.Enable();
            playerInput.playerActions.Player.Disable();
            freeCamEnabled = true;
        }
    }
    private void DisableFreecam()
    {
        if (freeCamEnabled)
        {
            //apply freecam transform to player controller
            playerController.transform.position = freeCamera.transform.position;
            //apply freecam position to main cam
            mainCamera.transform.rotation = freeCamera.transform.rotation;
            //swap active cams
            freeCamera.SetActive(false);
            mainCamera.SetActive(true);
            //swap active input action
            playerInput.playerActions.FreeCam.Disable();
            playerInput.playerActions.Player.Enable();
            freeCamEnabled = false;
        }
    }
}
