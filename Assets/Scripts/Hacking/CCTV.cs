using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : HackingObject
{
    [SerializeField] private Camera cctv;
    [SerializeField] private GameObject hacker; // ��Ŀ ������
    private static int originDepth = -2;
    private static int interactDepth = 0;

    public override void Interact()
    {
        cctv.depth = interactDepth;
        // ��Ŀ ���������� ��ȣ ������
    }

    public void ReturnOrigin()
    {
        cctv.depth = originDepth;
    }
}
