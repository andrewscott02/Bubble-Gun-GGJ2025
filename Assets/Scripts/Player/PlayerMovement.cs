using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 60;

    private Vector2 moveInput = Vector2.zero;

    public void SetMovement(Vector2 dir)
        => moveInput = dir;

    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 newPos = transform.position;

            newPos += GetMovement();

            transform.position = Utils.LerpVector3(transform.position, newPos, Time.fixedDeltaTime);
        }
    }

    private Vector3 GetMovement()
    {
        Vector3 right = transform.right.normalized;
        Vector3 forward = transform.forward.normalized;

        Vector3 moveVector = right * moveInput.x + forward * moveInput.y;
        return moveVector * moveSpeed;
    }
}