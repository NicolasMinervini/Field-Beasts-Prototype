using UnityEngine;
using UnityEngine.InputSystem;

public class PointerMover : MonoBehaviour
{
    public Pointer pointer;

    public float moveSpeed = 4;

    InputAction moveInput;

    Vector2 move;
    Vector2 currentvel;

    public float smoothTime = 0f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        if (pointer == null || pointer.isYourTurn == false) return;

        if (smoothTime > 0)
        {
            move = Vector2.SmoothDamp(move, moveInput.ReadValue<Vector2>(), ref currentvel, smoothTime);
        }
        else
        {
            move = moveInput.ReadValue<Vector2>();
        }

        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        transform.Translate(move.x * moveSpeed * Time.deltaTime, 0, move.y * moveSpeed * Time.deltaTime);
    }
}
