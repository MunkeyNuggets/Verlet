using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletPhysicsEngine : MonoBehaviour {

    public struct point
    {
        Vector3 newPos;
        Vector3 currentPos;
        Vector3 oldPos;
        float acceleration;
    }

    public GameObject thisObject;

    // Use this for initialization
    void Start ()
    {
        currentPos = new Vector3(thisObject.transform.position.x, thisObject.transform.position.y, thisObject.transform.position.z);
	}


	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void updatePos(Vector3 newPos)
    {
        
        newPos.x = 2 * currentPos.x - oldPos.x * Time.deltaTime + acceleration * (Time.deltaTime * 2);
        newPos.y = 2 * currentPos.y - oldPos.y * Time.deltaTime + acceleration * (Time.deltaTime * 2);
        newPos.z = 2 * currentPos.z - oldPos.z * Time.deltaTime + acceleration * (Time.deltaTime * 2);
        oldPos = currentPos;
        currentPos = newPos;
    }
}
