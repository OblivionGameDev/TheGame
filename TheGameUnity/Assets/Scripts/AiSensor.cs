using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AiSensor : MonoBehaviour
{
    public GameObject zombieLookRef;
    public float distance = 10f;
    public float angle = 30f;
    public float height = 1f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask occlusionLayers;
    public bool isInSight;
    Collider[] colliders = new Collider[50];
    private CapsuleCollider playerCollider;
    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;

    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1 / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        IsInSight();
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
        }
        
    }
    public void IsInSight()
    {
        Vector3 dest = zombieLookRef.transform.position;
        Vector3 origin = transform.position;
        Vector3 direction = dest - origin;
        float dist = Vector3.Distance(zombieLookRef.transform.position, transform.position);
        origin.y = height - 0.25f;
        dest.y = origin.y;
        
        

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);

        if (direction.y < 0 || direction.y > height || dist > distance)
        {
            isInSight = false;
        } 
        else
        if (deltaAngle > angle)
        {
            isInSight = false;
        }   
        else
        if(Physics.Linecast(origin, zombieLookRef.transform.position, occlusionLayers))
        {
            Debug.DrawLine(origin, zombieLookRef.transform.position, Color.yellow, 0.1f);
            isInSight = false;
        } 
        else isInSight = true;
    }
    private void Scan()
    {
       //count = Physics.OverlapSphereNonAlloc(transform.position, distance, playerCollider, layers, QueryTriggerInteraction.Collide);

    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
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

        //Left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //Right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for ( int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
         
            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

             //Far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //Top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //Bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
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
        scanInterval = 1 / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        if (isInSight) 
        {
            Gizmos.color = Color.green; 
        }
        else 
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(zombieLookRef.transform.position, 0.5f);
    }
}
