using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Point
{
    public bool m_isPinned;
    public GameObject sphere;
    public Vector3 currentPos;
    public Vector3 oldPos;
    public float mass = 1;
}

public class VerletPhysicsEngine : MonoBehaviour {
    
    [SerializeField]
    public int numPoints = 5;
    public List<Point> points = new List<Point>();
    public GameObject anchorPoint;
    public LineRenderer ropeLine;
    public float dampening = 1;

    public Vector3 acceleration;
    
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numPoints; i++)
        {
            points.Add(new Point());
            points[i].currentPos = transform.position + Vector3.down * numPoints * i;
            points[i].oldPos = transform.position + Vector3.down * numPoints * i;
        }
        ropeLine = GetComponent<LineRenderer>();
        points[0].m_isPinned = true;

        //Give the rope it's points
        ropeLine.positionCount = points.Count;
    }

    void FixedUpdate()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (i != 0)
            {
                Restraint(points[i], points[i - 1]);
            }
        }
        VerletPhysics(acceleration);
    }

    public float distance = 1.0f;
    public float stiffness = 1.0f;
    void Restraint(Point pointA, Point pointB)
    {
        float diffBetweenX = pointA.currentPos.x - pointB.currentPos.x;
        float diffBetweenY = pointA.currentPos.y - pointB.currentPos.y;
        float diffBetweenZ = pointA.currentPos.z - pointB.currentPos.z;
        
        Vector3 dir = (pointA.currentPos - pointB.currentPos).normalized;
        float dist = Vector3.Distance(pointA.currentPos, pointB.currentPos);

        float diff = ((distance - dist) / dist);

        //fix the mass weighting
        float m1 = 1.0f / pointA.mass;
        float m2 = 1.0f / pointB.mass;

        float stiffness1 = m1 / (m1 / m2) * stiffness;
        float stiffness2 = stiffness - stiffness1;

        //This is the translation of 2 points. They are moved half the distance (0.5f) to keep their Resting Distance.
        float translatePointX = diffBetweenX * 0.5f * diff;
        float translatePointY = diffBetweenY * 0.5f * diff;
        float translatePointZ = diffBetweenZ * 0.5f * diff;

        pointA.currentPos = new Vector3(pointA.currentPos.x + translatePointX, pointA.currentPos.y + translatePointY, pointA.currentPos.z + translatePointZ);
        pointB.currentPos = new Vector3(pointB.currentPos.x - translatePointX, pointB.currentPos.y - translatePointY, pointB.currentPos.z - translatePointZ);
        //pointA.currentPos += dir * dist * stiffness1 * diff;
        //pointB.currentPos += dir * dist * stiffness2 * diff;
    }

    void VerletPhysics(Vector3 accel)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (!points[i].m_isPinned)
            {
                Point curPoint = points[i];

                Vector3 curVel = (curPoint.currentPos - curPoint.oldPos) * dampening; // * dampening;
                                                                                      //  Add the velocity to the currentPos;
                curPoint.oldPos = curPoint.currentPos;
                curPoint.currentPos += (curVel + accel * (Time.fixedDeltaTime * Time.fixedDeltaTime));// + acceleration/force 
                
                    ropeLine.SetPosition(i, points[i - 1].currentPos);
            }
        }
    }
}
