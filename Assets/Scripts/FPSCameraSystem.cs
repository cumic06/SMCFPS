using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCameraSystem : MonoBehaviour
{
    [SerializeField] float sensitivity = 1;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    float curHorizontal;
    float curVertical;
    public Vector2 curRecoil;
    [SerializeField] private PlayerController controller;

    void GetPlayerInput()
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        curHorizontal += input.x;
        curVertical -= input.y;
    }

    private void Update()
    {
        if (controller.IsStopped)
            return;

        float t = Time.deltaTime * (15 - curRecoil.y);
        curRecoil = new Vector2(0, Mathf.Clamp(curRecoil.y + t, -100, 0));
        GetPlayerInput();
        playerTransform.rotation = Quaternion.Euler(0, curHorizontal + curRecoil.x, 0);
        cameraTransform.localRotation = Quaternion.Euler(curVertical + curRecoil.y, 0, 0);
    }
}