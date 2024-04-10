using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Security5 : Pattern_Special
{
    protected override void Start()
    {
        base.Start();

        // 해커 지망생 다 죽었을 때 발동되는 이벤트 추가
    }

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        // 해커 지망생 생성
        // 이벤트 or 배열로 저장하고 있다가 메소드 호출
    }

    private void OnAllHackerDied()
    {
        if (limitCount > 0)
            StartCoroutine(C_WaitTime());
    }

    private IEnumerator C_WaitTime()
    {
        yield return Util.GetWait(20f);

        patternAble = true;
    }
}
