using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public ElectricTrap trap;

    [SerializeField] private Transform[] lineTrasform;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        DrawLine();
        SetMeshCollider();
    }

    private void DrawLine()
    {
        lineRenderer.positionCount = lineTrasform.Length;

        for (int i = 0; i < lineTrasform.Length; i++)
        {
            lineRenderer.SetPosition(i, lineTrasform[i].position);
        }
    }

    private void SetMeshCollider()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        Mesh mesh = new Mesh();

        lineRenderer.BakeMesh(mesh);

        meshCollider.sharedMesh = mesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            int damage = trap.GetMonsterStat().attackStat.AttackPower;
            player.TakeDamage(-damage);
            //기절 함수
            //player.
        }
    }
}
