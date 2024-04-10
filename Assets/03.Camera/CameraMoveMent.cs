using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMent : MonoSigleton<CameraMoveMent>
{
    #region º¯¼ö
    [SerializeField] private float cameraMoveSpeed;

    [SerializeField] private Transform targetObject;


    [SerializeField] private float limitRotationXLowValue;
    [SerializeField] private float limitRotationXHighValue;

    private float mouseXRotate = 0.0f;

    private Animator anim;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        RotationCamera();
        FollowCamera();
    }

    private void FollowCamera()
    {
        transform.position = targetObject.position;
    }

    private void RotationCamera()
    {
        transform.eulerAngles = GetCameraRotation();
    }

    public bool CheckMouseMove()
    {
        bool mouseMove = Input.GetAxis("Mouse Y") != 0 && Input.GetAxis("Mouse X") != 0;
        return mouseMove;
    }

    public Vector2 GetCameraRotation()
    {
        float mouseX = -Input.GetAxis("Mouse Y") * cameraMoveSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse X") * cameraMoveSpeed * Time.deltaTime;

        float mouseYRotate = transform.eulerAngles.y + mouseY;

        mouseXRotate += mouseX;
        mouseXRotate = Mathf.Clamp(mouseXRotate, limitRotationXLowValue, limitRotationXHighValue);

        Vector2 mouseVec = new Vector2(mouseXRotate, mouseYRotate);

        return mouseVec;
    }

    public void CameraMove()
    {

    }
}
