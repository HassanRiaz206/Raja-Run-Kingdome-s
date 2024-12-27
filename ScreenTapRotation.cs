using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenTapRotation : MonoBehaviour
{
    // Array to hold GameObjects assigned in the Inspector
    public GameObject[] objectsToRotate;

    // Reference to the SoundEffectsManager
    public SoundEffectsManager soundEffectsManager;

    void Update()
    {
        // Process both touch, mouse, and key inputs
        if (IsInputOnLeftSideOfScreen(out Vector3 inputPosition) || Input.GetKeyDown(KeyCode.M))
        {
            RotateObjects();
        }
    }

    // Unified method to process input on the left side of the screen
    bool IsInputOnLeftSideOfScreen(out Vector3 inputPosition)
    {
        inputPosition = Vector3.zero;

        // Check for touch input (for mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputPosition = touch.position;

            if (touch.phase == TouchPhase.Began && !IsPointerOverUI(touch))
            {
                return touch.position.x < Screen.width / 2;
            }
        }

        // Check for mouse input (for editor/PC testing)
        if (Input.GetMouseButtonDown(0))
        {
            inputPosition = Input.mousePosition;

            if (!IsPointerOverUI())
            {
                return Input.mousePosition.x < Screen.width / 2;
            }
        }

        return false;
    }


    // Function to rotate the objects
    void RotateObjects()
    {
        foreach (GameObject obj in objectsToRotate)
        {
            if (obj != null)
            {
                Vector3 currentRotation = obj.transform.eulerAngles;
                currentRotation.y -= 90f;

                if (currentRotation.y < -90f)
                {
                    currentRotation.y = 180f;
                }

                obj.transform.eulerAngles = currentRotation;

                if (soundEffectsManager != null)
                {
                    soundEffectsManager.PlayPlatformChangeSound();
                }
            }
        }
    }

    // Check if the touch is over a UI element (for mobile)
    bool IsPointerOverUI(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = touch.position
        };
        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    // Check if the mouse is over a UI element (for editor)
    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
