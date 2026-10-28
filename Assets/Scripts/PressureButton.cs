using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    [Min(1)] public int requiredCrates = 1;

    public UnityEvent onPressed;
    public UnityEvent onReleased;

    int cratesOnButton = 0;
    bool isPressed = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Crate")) return;

        cratesOnButton++;
        CheckState();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Crate")) return;

        cratesOnButton--;
        CheckState();
    }

    void CheckState()
    {
        bool shouldBePressed = cratesOnButton >= requiredCrates;

        if (shouldBePressed && !isPressed)
        {
            isPressed = true;
            Debug.Log($"Botão pressionado ({cratesOnButton}/{requiredCrates})");
            onPressed?.Invoke();
        }
        else if (!shouldBePressed && isPressed)
        {
            isPressed = false;
            Debug.Log("Botão solto");
            onReleased?.Invoke();
        }
    }
}