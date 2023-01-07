using UnityEngine;

using UnityEngine.InputSystem;


namespace StarterAssets
{
    [RequireComponent(typeof(OnGameSuspend))]
    [RequireComponent(typeof(GameStartListener),
                      typeof(GameSuspendListener),
                      typeof(GameEndListener))]
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool aim;
        public bool reload;
        public bool shoot;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [Header("Events")]
        [SerializeField] OnGameSuspend onGameSuspend;

        // Listeners
        GameStartListener gameStartListener;
        GameSuspendListener gameSuspendListener;
        GameEndListener gameEndListener;

        void Start()
        {

            ToggleControls(false);

            Utils.GetListener(this, out gameStartListener);
            gameStartListener.Register(() => ToggleControls(true));

            Utils.GetListener(this, out gameSuspendListener);
            gameSuspendListener.Register((bool value) => ToggleControls(!value));

            Utils.GetListener(this, out gameEndListener);
            gameEndListener.Register(() => ToggleControls(false));
        }

        void ToggleControls(bool enable)
        {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            if (!enable)
            {
                playerInput.actions["Move"].Disable();
                playerInput.actions["Look"].Disable();
                playerInput.actions["Jump"].Disable();
                playerInput.actions["Sprint"].Disable();
                playerInput.actions["Aim"].Disable();
                playerInput.actions["WeaponSwitch"].Disable();
                playerInput.actions["Shoot"].Disable();
                playerInput.actions["Reload"].Disable();
                playerInput.actions["Interact"].Disable();
                playerInput.actions["ShoulderSwitch"].Disable();
                cursorLocked = false;
                cursorInputForLook = false;
                Cursor.visible = true;
                SetCursorState(cursorLocked);
                return;
            }

            playerInput.actions["Move"].Enable();
            playerInput.actions["Look"].Enable();
            playerInput.actions["Jump"].Enable();
            playerInput.actions["Sprint"].Enable();
            playerInput.actions["Aim"].Enable();
            playerInput.actions["WeaponSwitch"].Enable();
            playerInput.actions["Shoot"].Enable();
            playerInput.actions["Reload"].Enable();
            playerInput.actions["Interact"].Enable();
            playerInput.actions["ShoulderSwitch"].Enable();
            cursorLocked = true;
            cursorInputForLook = true;
            Cursor.visible = false;
            SetCursorState(cursorLocked);
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }

        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }

        public void OnReload(InputValue value)
        {
            ReloadInput(value.isPressed);
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }
        public void ShootInput(bool newShootState)
        {
            shoot = newShootState;
        }

        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

        public void ReloadInput(bool newReloadState)
        {
            reload = newReloadState;
        }

        private void OnPause()
        {
            Debug.Log("Pausing...");
            cursorLocked = !cursorLocked;
            cursorInputForLook = cursorLocked;
            SetCursorState(cursorLocked);
            ToggleControls(cursorLocked);
            onGameSuspend.Raise(!cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}