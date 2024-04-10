using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Move : MonoBehaviour
{
    [SerializeField] private LayerMask controlModuleLayerMask;
    [SerializeField] private GameObject textObj;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position += new Vector3(h, 0f, v) * Time.deltaTime * 5f;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 10f, controlModuleLayerMask))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                hit.collider.GetComponent<ControlModule>().StartHacking();
            }
            if(!textObj.activeSelf)
            {
                textObj.SetActive(true);
            }
        }
        else
        {
            if (textObj.activeSelf)
            {
                textObj.SetActive(false);
            }
        }
    }
}
