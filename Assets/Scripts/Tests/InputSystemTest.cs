using UnityEngine;

namespace Tests
{
    #if ENABLE_LEGACY_INPUT_MANAGER
        public class InputSystemTest : MonoBehaviour
        {
            // Update is called once per frame
            void Update()
            {
                // InvalidOperationException: You are trying to read Input using the UnityEngine.Input class,
                // but you have switched active Input handling to Input System package in Player Settings.
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    Debug.Log("Space (via Legacy Input System)");
                }
            }
        }
    #endif

    #if ENABLE_INPUT_SYSTEM
        using UnityEngine.InputSystem;

        [AddComponentMenu("Tests/Input System Test")]
        public class InputSystemTest : MonoBehaviour
        {
            // Update is called once per frame
            void Update()
            {
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    Debug.Log("Space (via Input System)");
                }
            }
        }
    #endif
}