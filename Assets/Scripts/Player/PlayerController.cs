using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;

    [SerializeField]
    private InputActionAsset inputs;

    private CinemachineOrbitalFollow cinemachineOrbital;
    [SerializeField]
    private float camRotMultiplier = 5;
    [SerializeField]
    private GameObject gunGO;
    [SerializeField]
    private Vector2 gunRotationBounds;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerMovement = GetComponent<PlayerMovement>();
        playerShoot = GetComponent<PlayerShoot>();
        cinemachineOrbital = GetComponentInChildren<CinemachineOrbitalFollow>();

        inputs.FindAction("Fire").performed += FireInput;

        inputs.FindAction("Move").performed += MoveInput;
        inputs.FindAction("Move").canceled += MoveInput;

        inputs.FindAction("Quit").performed += (c) => SceneManager.LoadScene("SplashScreen");
    }

    private void FireInput(InputAction.CallbackContext context)
    {
        playerShoot.Fire();
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        playerMovement.SetMovement(context.ReadValue<Vector2>());
    }

    #region Rotations

    private void FixedUpdate()
    {
        ApplyRotationToPlayerObject();
        ApplyRotationToGunObject();
    }

    private void ApplyRotationToPlayerObject()
    {
        Vector3 desiredRot = transform.rotation.eulerAngles;
        desiredRot.y = cinemachineOrbital.HorizontalAxis.Value;

        //Vector3 newRot = Utils.LerpVector3(transform.rotation.eulerAngles, desiredRot, Time.fixedDeltaTime);

        transform.rotation = Quaternion.Euler(desiredRot);
    }

    private void ApplyRotationToGunObject()
    {
        Vector3 desiredRot = gunGO.transform.rotation.eulerAngles;
        desiredRot.x = Mathf.Clamp(cinemachineOrbital.VerticalAxis.Value, gunRotationBounds.x, gunRotationBounds.y);

        //Vector3 newRot = Utils.LerpVector3(transform.rotation.eulerAngles, desiredRot, Time.fixedDeltaTime);

        gunGO.transform.rotation = Quaternion.Euler(desiredRot);
    }

    #endregion
}
