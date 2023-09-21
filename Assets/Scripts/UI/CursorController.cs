using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using System.Linq;

namespace BSTW.UI
{
    public class CursorController : MonoBehaviour
    {
        private RectTransform _cursor;
        private RectTransform _canvasTransform;
        private Mouse _virtualMouse;
        private bool _previousVirtualMouseState;


        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private RectTransform[] _cursors;
        [SerializeField] private float _cursorSpeed = 1000f;
        [SerializeField] private RectTransform[] _canvasTransforms;
        [SerializeField] private bool _defaultCursorSpeed = true;

        private void OnEnable()
        {
            _cursor = _cursors.FirstOrDefault(c => c.transform.parent.gameObject.activeSelf);
            _canvasTransform = _canvasTransforms.FirstOrDefault(c => c.gameObject.activeSelf);

            if (_virtualMouse == null)
            {
                _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (!_virtualMouse.added)
            {
                InputSystem.AddDevice(_virtualMouse);
            }

            InputUser.PerformPairingWithDevice(_virtualMouse, _playerInput.user);

            if (_cursors != null)
            {
                var newPosition = new Vector2(Screen.width / 2, Screen.height / 2);
                InputState.Change(_virtualMouse.position, newPosition);
                UpdateCursorAnchor(newPosition);
            }

            InputSystem.onAfterUpdate += UpdateCursorMotion;
        }

        private void OnDisable()
        {
            if (_virtualMouse != null && _virtualMouse.added) InputSystem.RemoveDevice(_virtualMouse);
            InputSystem.onAfterUpdate -= UpdateCursorMotion;

            DisableCursor();
        }

        private void UpdateCursorMotion()
        {
            if (_virtualMouse == null || Gamepad.current == null)
            {
                _cursor.gameObject.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                return;
            }

            _cursor.gameObject.SetActive(true);
            Cursor.visible = false;

            Vector2 gamepadLeftStickValue = Gamepad.current.leftStick.ReadValue();

            if (_defaultCursorSpeed)
            {
                gamepadLeftStickValue *= _cursorSpeed;
            }
            else
            {
                gamepadLeftStickValue *= _cursorSpeed * Time.deltaTime;
            }

            Vector2 currentPosition = _virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + gamepadLeftStickValue;

            newPosition.x = Mathf.Clamp(newPosition.x, 0f, Screen.width);
            newPosition.y = Mathf.Clamp(newPosition.y, 0f, Screen.height);

            InputState.Change(_virtualMouse.position, newPosition);
            InputState.Change(_virtualMouse.delta, gamepadLeftStickValue);

            var aButtonIsPressed = Gamepad.current.aButton.IsPressed();

            if (_previousVirtualMouseState != aButtonIsPressed)
            {
                _virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
                InputState.Change(_virtualMouse, mouseState);
                _previousVirtualMouseState = aButtonIsPressed;
            }

            UpdateCursorAnchor(newPosition);
        }

        private void UpdateCursorAnchor(Vector2 position)
        {
            Vector2 anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, position, null, out anchoredPosition);

            if (_defaultCursorSpeed)
            {
                _cursor.anchoredPosition = anchoredPosition;
            }
            else
            {
                _cursor.anchoredPosition += anchoredPosition;
            }
        }

        public void DisableCursor()
        {
            _cursor.gameObject.SetActive(false);
            Cursor.visible = false;
        }
    }
}
