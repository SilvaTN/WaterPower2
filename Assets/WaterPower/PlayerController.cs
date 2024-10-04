using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;  // Needed to use lists
using TMPro; // Needed for TextMeshPro

public class PlayerController : MonoBehaviour
{
    // Drag the New Controls input action asset here
    public InputActionAsset inputActions;

    private InputAction rightStickAction; // Action for right stick rotation

    private List<int> sliceOrder;  // List to track slice order
    private int currentExpectedSlice;  // To track the next expected slice
    private float timer;  // Timer to count time taken for full rotation
    private bool isTimerRunning;  // Flag to check if the timer is active
    private int rotationsCount; // Counter for full rotations

    public TextMeshProUGUI timerText; // Reference to the TextMeshPro UI Text
    public TextMeshProUGUI rotationsText; // Reference to the TextMeshPro UI Text for rotations

    [SerializeField] ParticleSystem particlesTimer;

    private void OnEnable()
    {
        // Find the action from the InputActionAsset
        rightStickAction = inputActions.FindAction("RightStickRotation");

        // Enable the action
        rightStickAction.Enable();

        // Initialize the slice order list and set the first expected slice
        ResetSliceOrder();
    }

    private void OnDisable()
    {
        // Disable the action
        rightStickAction.Disable();
    }

    private void Update()
    {
        // Get right stick input
        Vector2 rightStickInput = rightStickAction.ReadValue<Vector2>();

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(rightStickInput.y, rightStickInput.x) * Mathf.Rad2Deg;

        // Update the timer text
        if (isTimerRunning)
        {
            timer += Time.deltaTime; // Increment timer by the time passed since last frame

            // Update the timer display
            timerText.text = "Time: " + timer.ToString("F2") + "s"; // Display timer in seconds, formatted to 2 decimal places

            // Reset if the timer reaches 5 seconds
            if (timer >= 5f)
            {
                Debug.Log("Timer reset after 5 seconds.");
                ResetSliceOrder(); // Reset both the timer and slice order
            }
        }

        if (rightStickInput.magnitude > 0.1f)
        {
            int currentSlice = GetSliceFromAngle(angle);

            // Start the timer if it's not already running
            if ((!isTimerRunning) && (currentSlice == 1))
            {
                isTimerRunning = true;
                timer = 0f; // Reset timer at the start of the first slice
            }

            // If the user moved to the expected slice
            if (currentSlice == currentExpectedSlice)
            {
                // Tick off this slice and move to the next expected slice
                Debug.Log("Ticked off slice: " + currentSlice);
                currentExpectedSlice++;

                // Check if all slices have been ticked off
                if (currentExpectedSlice > 6)
                {
                    Debug.Log("Full rotation done in " + timer + " seconds");
                    rotationsCount++; // Increment the full rotation counter
                    rotationsText.text = "Rotations: " + rotationsCount; // Update the rotations text
                    ResetSliceOrder();
                    particlesTimer.Play();
                }
            }
        }
    }

    // Method to determine which slice the angle falls into
    private int GetSliceFromAngle(float angle)
    {
        if (angle >= 30f && angle <= 90f) return 1;       // Slice 1: Top Right
        if (angle >= -30f && angle < 30f) return 2;       // Slice 2: Mid Right
        if (angle >= -90f && angle < -30f) return 3;      // Slice 3: Bottom Right
        if (angle >= -150f && angle < -90f) return 4;     // Slice 4: Bottom Left
        if (angle >= -180f && angle < -150f) return 5;    // Slice 5: Mid Left
        if (angle > 90f && angle <= 150f) return 6;       // Slice 6: Top Left
        return -1;  // Invalid slice
    }

    // Method to reset the slice order and set the first expected slice
    private void ResetSliceOrder()
    {
        // Reset the list and the next expected slice
        sliceOrder = new List<int> { 1, 2, 3, 4, 5, 6 };
        currentExpectedSlice = 1;  // Start expecting slice 1 again
        isTimerRunning = false;  // Stop the timer
        timer = 0f;  // Reset the timer
        timerText.text = "Time: 0.00s"; // Reset the timer display        
        Debug.Log("Reset slice order. Expecting slice 1.");
    }
}












//using UnityEngine;
//using UnityEngine.InputSystem;
//using System.Collections.Generic;  // Needed to use lists

//public class PlayerController : MonoBehaviour
//{
//    // Drag the New Controls input action asset here
//    public InputActionAsset inputActions;

//    private InputAction rightStickAction; // Action for right stick rotation

//    private List<int> sliceOrder;  // List to track slice order
//    private int currentExpectedSlice;  // To track the next expected slice
//    private float timer;  // Timer to count time taken for full rotation
//    private bool isTimerRunning;  // Flag to check if the timer is active

//    private void OnEnable()
//    {
//        // Find the action from the InputActionAsset
//        rightStickAction = inputActions.FindAction("RightStickRotation");

//        // Enable the action
//        rightStickAction.Enable();

//        // Initialize the slice order list and set the first expected slice
//        ResetSliceOrder();
//    }

//    private void OnDisable()
//    {
//        // Disable the action
//        rightStickAction.Disable();
//    }

//    private void Update()
//    {
//        // Get right stick input
//        Vector2 rightStickInput = rightStickAction.ReadValue<Vector2>();

//        // Calculate the angle in degrees
//        float angle = Mathf.Atan2(rightStickInput.y, rightStickInput.x) * Mathf.Rad2Deg;

//        if (rightStickInput.magnitude > 0.1f)
//        {
//            // Start the timer if it's not already running
//            if (!isTimerRunning)
//            {
//                isTimerRunning = true;
//                timer = 0f; // Reset timer at the start of the first slice
//            }

//            int currentSlice = GetSliceFromAngle(angle);

//            // If the user moved to the expected slice
//            if (currentSlice == currentExpectedSlice)
//            {
//                // Tick off this slice and move to the next expected slice
//                Debug.Log("Ticked off slice: " + currentSlice);
//                currentExpectedSlice++;

//                // Check if all slices have been ticked off
//                if (currentExpectedSlice > 6)
//                {
//                    Debug.Log("Full rotation done in " + timer + " seconds");
//                    ResetSliceOrder();
//                }
//            }
//        }

//        // If the timer is running, increment the timer
//        if (isTimerRunning)
//        {
//            timer += Time.deltaTime; // Increment timer by the time passed since last frame
//        }
//    }

//    // Method to determine which slice the angle falls into
//    private int GetSliceFromAngle(float angle)
//    {
//        if (angle >= 30f && angle <= 90f) return 1;       // Slice 1: Top Right
//        if (angle >= -30f && angle < 30f) return 2;       // Slice 2: Mid Right
//        if (angle >= -90f && angle < -30f) return 3;      // Slice 3: Bottom Right
//        if (angle >= -150f && angle < -90f) return 4;     // Slice 4: Bottom Left
//        if (angle >= -180f && angle < -150f) return 5;    // Slice 5: Mid Left
//        if (angle > 90f && angle <= 150f) return 6;       // Slice 6: Top Left
//        return -1;  // Invalid slice
//    }

//    // Method to reset the slice order and set the first expected slice
//    private void ResetSliceOrder()
//    {
//        // Reset the list and the next expected slice
//        sliceOrder = new List<int> { 1, 2, 3, 4, 5, 6 };
//        currentExpectedSlice = 1;  // Start expecting slice 1 again
//        isTimerRunning = false;  // Stop the timer
//        Debug.Log("Reset slice order. Expecting slice 1.");
//    }
//}












//using UnityEngine;
//using UnityEngine.InputSystem;
//using System.Collections.Generic;  // Needed to use lists

//public class PlayerController : MonoBehaviour
//{
//    // Drag the New Controls input action asset here
//    public InputActionAsset inputActions;

//    private InputAction rightStickAction; // Action for right stick rotation

//    private List<int> sliceOrder;  // List to track slice order
//    private int currentExpectedSlice;  // To track the next expected slice

//    private void OnEnable()
//    {
//        // Find the action from the InputActionAsset
//        rightStickAction = inputActions.FindAction("RightStickRotation");

//        // Enable the action
//        rightStickAction.Enable();

//        // Initialize the slice order list and set the first expected slice
//        ResetSliceOrder();
//    }

//    private void OnDisable()
//    {
//        // Disable the action
//        rightStickAction.Disable();
//    }

//    private void Update()
//    {
//        // Get right stick input
//        Vector2 rightStickInput = rightStickAction.ReadValue<Vector2>();

//        // Calculate the angle in degrees
//        float angle = Mathf.Atan2(rightStickInput.y, rightStickInput.x) * Mathf.Rad2Deg;

//        if (rightStickInput.magnitude > 0.1f)
//        {
//            int currentSlice = GetSliceFromAngle(angle);

//            // If the user moved to the expected slice
//            if (currentSlice == currentExpectedSlice)
//            {
//                // Tick off this slice and move to the next expected slice
//                Debug.Log("Ticked off slice: " + currentSlice);
//                currentExpectedSlice++;

//                // Check if all slices have been ticked off
//                if (currentExpectedSlice > 6)
//                {
//                    Debug.Log("Full rotation done");
//                    ResetSliceOrder();
//                }
//            }
//        }
//    }

//    // Method to determine which slice the angle falls into
//    private int GetSliceFromAngle(float angle)
//    {
//        if (angle >= 30f && angle <= 90f) return 1;       // Slice 1: Top Right
//        if (angle >= -30f && angle < 30f) return 2;       // Slice 2: Mid Right
//        if (angle >= -90f && angle < -30f) return 3;      // Slice 3: Bottom Right
//        if (angle >= -150f && angle < -90f) return 4;     // Slice 4: Bottom Left
//        if (angle >= -180f && angle < -150f) return 5;    // Slice 5: Mid Left
//        if (angle > 90f && angle <= 150f) return 6;       // Slice 6: Top Left
//        return -1;  // Invalid slice
//    }

//    // Method to reset the slice order and set the first expected slice
//    private void ResetSliceOrder()
//    {
//        // Reset the list and the next expected slice
//        sliceOrder = new List<int> { 1, 2, 3, 4, 5, 6 };
//        currentExpectedSlice = 1;  // Start expecting slice 1 again
//        Debug.Log("Reset slice order. Expecting slice 1.");
//    }
//}









//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerController : MonoBehaviour
//{
//    // Drag the New Controls input action asset here
//    public InputActionAsset inputActions;

//    private InputAction rightStickAction; // Action for right stick rotation

//    private void OnEnable()
//    {
//        // Find the action from the InputActionAsset
//        rightStickAction = inputActions.FindAction("RightStickRotation");

//        // Enable the action
//        rightStickAction.Enable();
//    }

//    private void OnDisable()
//    {
//        // Disable the action
//        rightStickAction.Disable();
//    }

//    private void Update()
//    {
//        // Get right stick input
//        Vector2 rightStickInput = rightStickAction.ReadValue<Vector2>();

//        // Calculate the angle in degrees
//        float angle = Mathf.Atan2(rightStickInput.y, rightStickInput.x) * Mathf.Rad2Deg;

//        // Print which slice is being pressed based on the angle
//        if (rightStickInput.magnitude > 0.1f)
//        {
//            if (angle >= 30f && angle <= 90f)
//            {
//                Debug.Log("Slice 1: Top Right");
//            }
//            else if (angle >= -30f && angle < 30f)
//            {
//                Debug.Log("Slice 2: Mid Right");
//            }
//            else if (angle >= -90f && angle < -30f)
//            {
//                Debug.Log("Slice 3: Bottom Right");
//            }
//            else if (angle >= -150f && angle < -90f)
//            {
//                Debug.Log("Slice 4: Bottom Left");
//            }
//            else if (angle >= -180f && angle < -150f)
//            {
//                Debug.Log("Slice 5: Mid Left");
//            }
//            else if (angle > 90f && angle <= 150f)
//            {
//                Debug.Log("Slice 6: Top Left");
//            }
//        }
//    }
//}




//Debug.Log("Right Stick Angle: " + angle);














//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerController : MonoBehaviour
//{
//    // Drag the New Controls input action asset here
//    public InputActionAsset inputActions;

//    public float moveSpeed = 5f; // Speed for horizontal movement
//    public float rotationThreshold = 1f; // Minimum rotation speed threshold (rotations per second)
//    public float rotationRateThresholdDegrees; // Minimum rotation rate (degrees per second) to register a rotation

//    private InputAction movementAction; // Action for horizontal movement
//    private InputAction rightStickAction; // Action for right stick rotation

//    private Vector2 previousRightStickInput; // Store previous right stick input for angle calculation
//    private float accumulatedRotation; // Accumulated rotation angle
//    private float previousTime; // Store previous frame time for rotation speed calculation

//    private void OnEnable()
//    {
//        // Find the actions from the InputActionAsset
//        movementAction = inputActions.FindAction("Movement");
//        rightStickAction = inputActions.FindAction("RightStickRotation");

//        // Enable the actions
//        movementAction.Enable();
//        rightStickAction.Enable();

//        // Initialize previous time
//        previousTime = Time.time;

//        // Calculate the rotation rate threshold in degrees
//        rotationRateThresholdDegrees = rotationThreshold * 360f; // Convert rotations per second to degrees per second
//    }

//    private void OnDisable()
//    {
//        // Disable the actions
//        movementAction.Disable();
//        rightStickAction.Disable();
//    }

//    private void Update()
//    {
//        // Horizontal movement
//        Vector2 movementInput = movementAction.ReadValue<Vector2>();
//        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime;
//        transform.Translate(movement, Space.World);

//        // Calculate rotation from right stick input
//        Vector2 rightStickInput = rightStickAction.ReadValue<Vector2>();

//        // Only track rotation if the input exceeds the threshold
//        if (rightStickInput.magnitude >= rotationThreshold)
//        {
//            // Calculate the angle change
//            float angleChange = Vector2.SignedAngle(previousRightStickInput, rightStickInput);

//            // Calculate the time delta
//            float currentTime = Time.time;
//            float deltaTime = currentTime - previousTime;
//            previousTime = currentTime;

//            // Calculate rotation speed in degrees per second
//            float rotationSpeed = angleChange / deltaTime;

//            // Only accumulate rotation if the rotation speed exceeds the threshold
//            if (Mathf.Abs(rotationSpeed) >= rotationRateThresholdDegrees)
//            {
//                // Accumulate the rotation angle
//                accumulatedRotation += angleChange;

//                // Check for a full rotation (360 degrees)
//                if (Mathf.Abs(accumulatedRotation) >= 360f)
//                {
//                    // Print a message for a full rotation and reset the accumulator
//                    Debug.Log("Full rotation completed!");
//                    accumulatedRotation = 0f; // Reset the accumulator after logging
//                }
//            }
//        }

//        // Update previous input for the next frame
//        previousRightStickInput = rightStickInput;
//    }
//}
