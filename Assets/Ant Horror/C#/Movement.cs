using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 10;
    [SerializeField] private float sens = 10;

    private InputSystem_Actions inputSystem;
    private Vector3 v3See;
    private Vector3 v3Move;
    private Vector2 moveVec;
    private Vector2 lookVec;
    private float xRot;
    private float yRot;
    private void OnEnable()
    {
        inputSystem = new InputSystem_Actions();
        inputSystem.Enable();
        inputSystem.Player.Move.performed += moveVoid;
        inputSystem.Player.Look.performed += lookVoid;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        move();
        moveCam();
    }
    private void move()
    {
        v3Move = (player.forward*moveVec.y + player.right*moveVec.x)*speed;
        v3Move.y = rb.linearVelocity.y;
        rb.linearVelocity = v3Move;
    }
    private void moveCam()
    {
        xRot += lookVec.x;
        yRot += lookVec.y;
        yRot = math.clamp(yRot, -35, 80);
        v3See = new Vector3(-yRot, xRot, 0)*sens;
        player.rotation = Quaternion.Euler(0, v3See.y, 0);
        cam.rotation = Quaternion.Euler(v3See);

    }
    private void moveVoid(InputAction.CallbackContext ctx)
    {
        moveVec = ctx.ReadValue<Vector2>();
    }
    private void lookVoid(InputAction.CallbackContext ctx)
    {
        lookVec = ctx.ReadValue<Vector2>();
    }
    private void OnDisable()
    {
        inputSystem.Player.Move.performed -= moveVoid;
        inputSystem.Player.Look.performed -= lookVoid;
    }
}
