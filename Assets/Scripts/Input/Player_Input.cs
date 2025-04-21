using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Input : MonoBehaviour
{
    public Player_Actions playerActions;

    //Player Input Controls
    //For demo, the only control is enabling freecam
    [Header("Player Controls")]
    private InputAction enableFreeCam;

    //player Actions
    [Header("Player Actions")]
    public static Action enableFreeCamAction;

    //Freecam Input Controls
    [Header("Freecam Controls")]
    private InputAction _disableFreeCam;
    public InputAction _camMove;
    public InputAction _camLook;
    private InputAction _camUp;
    private InputAction _camDown;
    private InputAction _camFast;
    private InputAction _camTimeSwap;

    //Freecam Actions
    [Header("Freecam Actions")]
    public static Action disableFreeCamAction;
    public static Action _camMoveAction;
    public static Action _camLookAction;
    public static Action _camUpAction;
    public static Action _camUpStopAction;
    public static Action _camDownAction;
    public static Action _camDownStopAction;
    public static Action _camFastStartAction;
    public static Action _camFastStopAction;
    public static Action _camTimeSwapAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerActions = new Player_Actions();
        playerActions.Player.Enable();
        playerActions.FreeCam.Disable();
    }

    private void OnEnable()
    {
        //Player
        enableFreeCam = playerActions.Player.EnableFreeCam;
        enableFreeCam.Enable();
        enableFreeCam.performed += EnableFreeCam;

        //FreeCam
        _camMove = playerActions.FreeCam.camMove;
        _camMove.Enable();

        _camLook = playerActions.FreeCam.camLook;
        _camLook.Enable();

        _camUp = playerActions.FreeCam.camUp;
        _camUp.Enable();
        _camUp.performed += CamUp;

        _camDown = playerActions.FreeCam.camDown;
        _camDown.Enable();
        _camDown.performed += CamDown;

        _camFast = playerActions.FreeCam.camFast;
        _camFast.Enable();
        _camFast.performed += CamFast;

        _camTimeSwap = playerActions.FreeCam.camTimeSwap;
        _camTimeSwap.Enable();
        _camTimeSwap.performed += CamTimeSwap;

        _disableFreeCam = playerActions.FreeCam.disableFreeCam;
        _disableFreeCam.Enable();
        _disableFreeCam.performed += DisableFreeCam;
    }
    private void OnDisable()
    {
        enableFreeCam.Disable();
        _camMove.Disable();
        _camLook.Disable();
        _camUp.Disable();
        _camDown.Disable();
        _camFast.Disable();
        _camTimeSwap.Disable();
        _disableFreeCam.Disable();
    }
    //Player Actions
    private void EnableFreeCam(InputAction.CallbackContext context)
    {
        enableFreeCamAction?.Invoke();
    }
    //FreeCam Actions
    private void DisableFreeCam(InputAction.CallbackContext context)
    {
        disableFreeCamAction?.Invoke();
    }
    private void CamUp(InputAction.CallbackContext context)
    {
        if (playerActions.FreeCam.camUp.WasPressedThisFrame())
        {
            _camUpAction?.Invoke();
        }
        if (playerActions.FreeCam.camUp.WasReleasedThisFrame())
        {
            _camUpStopAction?.Invoke();
        }
    }
    private void CamDown(InputAction.CallbackContext context)
    {
        if (playerActions.FreeCam.camDown.WasPressedThisFrame())
        {
            _camDownAction?.Invoke();
        }
        if (playerActions.FreeCam.camDown.WasReleasedThisFrame())
        {
            _camDownStopAction?.Invoke();
        }
    }
    private void CamFast(InputAction.CallbackContext context)
    {
        if (playerActions.FreeCam.camFast.WasPressedThisFrame())
        {
            _camFastStartAction?.Invoke();
        }
        if (playerActions.FreeCam.camFast.WasReleasedThisFrame())
        {
            _camFastStopAction?.Invoke();
        }
    }
    private void CamTimeSwap(InputAction.CallbackContext context)
    {
        _camTimeSwapAction?.Invoke();
    }
}
