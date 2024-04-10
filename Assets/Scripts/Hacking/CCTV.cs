using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : HackingObject
{
    [SerializeField] private Camera cctv;
    [SerializeField] private GameObject hacker; // 해커 지망생
    private static int originDepth = -2;
    private static int interactDepth = 0;

    public override void Interact()
    {
        cctv.depth = interactDepth;
        // 해커 지망생한테 신호 보내기
    }

    public void ReturnOrigin()
    {
        cctv.depth = originDepth;
    }
}
