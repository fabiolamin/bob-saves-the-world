using BSTW.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BSTW.Utils
{
    public class GamepadRumbleController : MonoBehaviour
    {
        private Coroutine _gamepadRumbleCoroutine;

        public void StartGamepadRumble(GamepadRumbleData gamepadRumbleData)
        {
            StarGamepadRumble(gamepadRumbleData.LowFrequency, gamepadRumbleData.HighFrequency, gamepadRumbleData.Duration);
        }

        public void StarGamepadRumble(float lowFrequency, float highFrequency, float duration)
        {
            if (_gamepadRumbleCoroutine != null)
            {
                StopCoroutine(_gamepadRumbleCoroutine);
            }

            if (Gamepad.current != null)
            {
                _gamepadRumbleCoroutine = StartCoroutine(StarGamepadRumbleCoroutine(lowFrequency, highFrequency, duration));
            }
        }

        public void SetGamepadMotorSpeeds(GamepadRumbleData gamepadRumbleData)
        {
            if (_gamepadRumbleCoroutine != null)
            {
                StopCoroutine(_gamepadRumbleCoroutine);
            }

            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(gamepadRumbleData.LowFrequency, gamepadRumbleData.HighFrequency);
            }
        }

        public void StopGamepadMotorSpeeds()
        {
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0f, 0f);
            }
        }

        private IEnumerator StarGamepadRumbleCoroutine(float lowFrequency, float highFrequency, float duration)
        {
            float elapsedTime = 0f;

            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            StopGamepadMotorSpeeds();
        }

        private void OnApplicationQuit()
        {
            StopGamepadMotorSpeeds();
        }
    }
}

