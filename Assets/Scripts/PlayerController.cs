using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Idle,
    ReadyToAttack,
    Walk,
    Run
}
public class PlayerController : MonoBehaviour
{
    PlayerState currentState;

    [SerializeField] Rigidbody _rigid;
    [SerializeField] Transform _transform;
    [SerializeField] float moveSpeed;

    [SerializeField] float accelTime;
    [SerializeField] float accelIncrementAmount;
    [SerializeField] float accelDecrementAmount;

    bool isRun;
    float accelReciprocal;
    float currentAccelTime;
    float currentMoveTime;
    [SerializeField] GameObject cameraObject;
    float moveAnimAmount;
    [Header("Animation Menu")]


    [SerializeField] AnimationCurve idleCurve; //¾ÆÁ÷ ¾È¸¸µê
    [SerializeField] AnimationCurve moveCurve;
    [SerializeField] float walkAnimYAmount = 0.1f;
    [SerializeField] float walkAnimDuration = 0.5f;
    [SerializeField] float runAnimYAmount = 0.15f;
    [SerializeField] float runAnimDuration = 0.4f;

    Vector3 lastAxis;

    [SerializeField] GroundChecker groundChecker;

    [Space]
    [SerializeField] private GameObject pausePanel;
    private bool isStopped = false;

    public bool IsStopped => isStopped;
    void InputMovement()
    {
        Vector3 axis = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        isRun = Input.GetKey(KeyCode.LeftShift);

        if (axis == Vector3.zero)
        {
            moveAnimAmount = Mathf.Clamp(moveAnimAmount - Time.deltaTime, 0, 1);
            currentAccelTime = Mathf.Clamp(currentAccelTime - Time.deltaTime * accelDecrementAmount, 0, accelTime);
        }
        else
        {
            moveAnimAmount = Mathf.Clamp(moveAnimAmount + Time.deltaTime, 0, 1);
            currentMoveTime += Time.deltaTime;
            lastAxis = axis;
            currentAccelTime = Mathf.Clamp(currentAccelTime + Time.deltaTime * accelDecrementAmount, 0, accelTime);
        }

    }
    void Movement()
    {
        Vector3 dir = _transform.TransformDirection(lastAxis);
        float velocity = Time.fixedDeltaTime * (moveSpeed * (currentAccelTime * accelReciprocal));
        _transform.position += dir * velocity * (isRun ? 1.5f : 1);
        float curveValue = 0;
        if (isRun)
            curveValue = moveCurve.Evaluate(Time.time / runAnimDuration) * runAnimYAmount;
        else
            curveValue = moveCurve.Evaluate(Time.time / walkAnimDuration) * walkAnimYAmount;

        cameraObject.transform.localPosition = new Vector3(0, Mathf.Lerp(0, curveValue, moveAnimAmount), 0);

        //if (currentMoveTime > 0)
        //    cameraObject.transform.localPosition = new Vector3(0, moveCurve.Evaluate(currentMoveTime * moveCurveDuration) * moveCurveYAmount, 0);
        //else
        //{
        //    cameraObject.transform.localPosition = new Vector3(0, Mathf.Lerp(cameraObject.transform.localPosition.y, 0, Time.fixedDeltaTime), 0);
        //}
    }


    private void Start()
    {
        accelReciprocal = 1 / accelTime;
    }
    private void Update()
    {
        InputMovement();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseSetting();
        }

        if (Input.GetKeyDown(KeyCode.Space) && groundChecker.isGrounded)
        {
            Debug.Log("SDA");
            _rigid.velocity += Vector3.up * 7;
            _transform.position += Vector3.up * 0.2f;
            groundChecker.isGrounded = false;
        }
    }

    public void PauseSetting()
    {
        isStopped = !isStopped;
        pausePanel.SetActive(isStopped);
        CursorSet(isStopped);
        Time.timeScale = isStopped ? 0 : 1;
    }
    public void ClearSetting()
    {
        isStopped = !isStopped;
        CursorSet(isStopped);
        Time.timeScale = isStopped ? 0 : 1;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void CursorSet(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}