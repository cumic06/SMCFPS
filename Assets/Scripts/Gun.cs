using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum GunState
{
    Idle,
    ReadyToFire
}
public class Gun : MonoBehaviour
{
    [SerializeField] FPSCameraSystem fpsCameraSystem;
    [SerializeField] int maxAmmo;
    int currentAmmo;
    public int CurrentAmmo { get => currentAmmo; set
        {
            currentAmmo = Mathf.Clamp(value, 0, maxAmmo);
            ammoText.text = currentAmmo.ToString();
        }
    }
    [SerializeField] float reloadTime;
    Coroutine reloadCor;
    [SerializeField] float fireDelay = 0.1f;
    float currentFireDelay;
    [SerializeField] Camera _camera;
    [Header("Animation Menu")]
    [SerializeField] Vector3 defaultPos;
    [SerializeField] Vector3 idleRot;
    [SerializeField] Vector3 readyRot;
    [Header("Particle")]
    [SerializeField] Transform firePoint;
    Queue<ParticleObject> fireParticlePool = new Queue<ParticleObject>();
    [SerializeField] GameObject fireParticlePrefab;
    bool nowReady = true;

    [SerializeField] Animator aimdotAnim;
    [SerializeField] TMP_Text ammoText;

    private const string shootSFX = "Zapper_1p_01";
    private const string reloadSFX = "Pistol_ClipIn_05";

    public void ChangeState(bool isReady)
    {
        if (nowReady = isReady)
            return;
        nowReady = isReady;
    }
    IEnumerator C_Reload()
    {
        AudioManager.Instance.Play(reloadSFX, SoundType.SFX);

        //Animation start
        ChangeState(false);
        yield return new WaitForSeconds(reloadTime);
        CurrentAmmo = maxAmmo;
        ChangeState(true);
        reloadCor = null;
    }
    public void Reload()
    {
        if (CurrentAmmo == maxAmmo)
            return;
        if(reloadCor == null)
            reloadCor = StartCoroutine(C_Reload());
    }
    public void Fire()
    {
        if (!nowReady)
            return;
        if (currentFireDelay < fireDelay)
            return;
        if (CurrentAmmo <= 0)
        {
            Reload();
            return;
        }
        AudioManager.Instance.Play(shootSFX, SoundType.SFX);
        CurrentAmmo--;
        fpsCameraSystem.curRecoil -= new Vector2(0, 4);
        currentFireDelay = 0;
        transform.localPosition = defaultPos - new Vector3(0, 0, 0.1f);
        //Particle
        if(fireParticlePool.Count > 0)
        {
            ParticleObject po = fireParticlePool.Dequeue();
            po.gameObject.SetActive(true);
            po.transform.position = firePoint.position;
            po.transform.rotation = firePoint.rotation;
        }
        else
        {
            ParticleObject particleObject = Instantiate(fireParticlePrefab, firePoint.position, firePoint.rotation).GetComponent<ParticleObject>();
            particleObject.onDiable = (po) =>
            {
                po.gameObject.SetActive(false);
                fireParticlePool.Enqueue(po);
            };
        }
        
        //Raycast Check
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("??", "8")))
        {
            if(hit.collider.transform.TryGetComponent<Monster>(out Monster monster))
            {
                monster.TakeDamage(-30);
            }
            aimdotAnim.SetTrigger("Hit");
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CurrentAmmo = maxAmmo;
    }
    private void Update()
    {
        currentFireDelay += Time.deltaTime;
        if (Input.GetMouseButton(0))
            Fire();
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
        //기본 위치로 되돌리기
        transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos, Time.deltaTime * 5);

        //nowReady
        Quaternion targetRot = Quaternion.Euler(nowReady ? readyRot : idleRot);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * (nowReady ? 20 : 10));
    }
}