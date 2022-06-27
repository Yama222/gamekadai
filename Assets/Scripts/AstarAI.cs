using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;

public class AstarAI : MonoBehaviour {
    public Vector3 targetPosition;

    public void Start () {
        Seeker seeker = GetComponent<Seeker>();
        
        //Start a new path to the targetPosition, return the result to the MyCompleteFunction
        seeker.StartPath (transform.position,targetPosition, MyCompleteFunction);
    }
    
    public void MyCompleteFunction (Path p) {
        //Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
    }
} 