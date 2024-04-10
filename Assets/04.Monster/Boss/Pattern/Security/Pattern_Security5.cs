using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_Security5 : Pattern_Special
{
    protected override void Start()
    {
        base.Start();

        // ��Ŀ ������ �� �׾��� �� �ߵ��Ǵ� �̺�Ʈ �߰�
    }

    public override IEnumerator ExecutePattern()
    {
        yield return null;

        // ��Ŀ ������ ����
        // �̺�Ʈ or �迭�� �����ϰ� �ִٰ� �޼ҵ� ȣ��
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
