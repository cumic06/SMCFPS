using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sensor : MonoBehaviour
{
    #region 변수
    [SerializeField] private float angle;
    [SerializeField] private float height;
    [SerializeField] private float distance;
    [SerializeField] private Color meshColor = new Color(1, 0, 0, 0.5f);

    [SerializeField] private int scanFrequency = 30;
    [SerializeField] LayerMask layers;
    [SerializeField] LayerMask occulsionLayers;

    [SerializeField] private Vector3 detectPos;

    private Collider[] collders = new Collider[50];

    public List<GameObject> objectList = new();

    private Mesh mesh;
    private int count;
    private float scanInterval;
    private float scanTimer;
    #endregion

    private void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, collders, layers, QueryTriggerInteraction.Collide);

        objectList.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = collders[i].gameObject;
            if (IsInSight(obj, angle))
            {
                objectList.Add(obj);
            }
        }
    }

    public void SensorOnOff(bool onOff)
    {
        if (!onOff)
        {
            mesh.Clear();
        }
        else
        {
            return;
        }
    }

    public bool IsCheckPlayer()
    {
        return IsInSight(Player.Instance.gameObject, angle) && objectList.Count > 0;
    }

    public bool IsInSight(GameObject obj, float angle = 45f)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);

        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, occulsionLayers))
        {
            return false;
        }
        return true;
    }

    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new();

        int segments = 10;
        int numTriangles = (segments * 4) + 4;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            //Gizmos.DrawSphere(collders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in objectList)
        {
            //Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}