using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour, IDamageable
{
    public static Player Instance;

    private MeshRenderer meshRenerer;
    private Rigidbody rigid;

    private Coroutine changeCor;
    [SerializeField] TMP_Text hpText;
    [SerializeField] UnityEngine.UI.Image hitImage;

    Coroutine hitCor;

    float maxHp = 30;
    float curHp;
    public float CurHp
    {
        get => curHp; set
        {
            curHp = Mathf.Clamp(value, 0, maxHp);
            hpText.text = curHp.ToString();
        }
    }

    private const string normalStageBGM = "Lunar Expedition loop";
    private const string bossStageBGM = "Orbital Journey loop";

    private void Awake()
    {
        meshRenerer = GetComponent<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
        Instance = GetComponent<Player>();

        curHp = maxHp;
        Debug.Log(Instance.gameObject.name);
    }

    private void Start()
    {
        string bgm = SceneManager.GetActiveScene().buildIndex == (int)EStage.Stage1Scene ? normalStageBGM : bossStageBGM;
        AudioManager.Instance.Play(bgm, SoundType.BGM);
    }

    public void TakeDamage(float damage)
    {
        CurHp--;
        if (CurHp <= 0)
        {
            AsynceLoadSystem.LoadGameScene(0);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        Debug.Log("TakeDamage");

        if (hitCor != null)
        {
            StopCoroutine(hitCor);
        }
        hitCor = StartCoroutine(HitPanel());

        IEnumerator HitPanel()
        {
            hitImage.gameObject.SetActive(true);
            for (int i = 0; i < 2; i++)
            {
                hitImage.color = new Color(1, 0, 0, 0.5f);
                yield return new WaitForSeconds(0.1f);
                hitImage.color = new Color(0, 0, 0, 0);
                yield return new WaitForSeconds(0.1f);
            }
            hitImage.gameObject.SetActive(false);
        }

        //changeCor = StartCoroutine(ChangeColor());
        //if (changeCor != null)
        //{
        //    StopCoroutine(changeCor);
        //}
        //StartCoroutine(ChangeColor());
    }

    //private IEnumerator ChangeColor()
    //{
    //    for (int i = 0; i < 1; i++)
    //    {
    //        meshRenerer.material.color = new Color(1, 0, 0, 0.4f);
    //        yield return new WaitForSeconds(0.1f);
    //        meshRenerer.material.color = new Color(1, 1, 1, 1);
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    public void SetVelocity(Vector3 velocity)
    {
        rigid.velocity = Vector3.zero;
        rigid.velocity = velocity;
    }
}
