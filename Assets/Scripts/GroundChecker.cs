using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool isGrounded;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
            isGrounded = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
            isGrounded = true;
    }
}
