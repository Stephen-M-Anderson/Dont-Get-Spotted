﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuardMechanics : MonoBehaviour
{
    public float speed;
    private float waitTime;
    public float startWaitTime;
    public GameObject actualPlayer;

    public bool inView = false;
    public bool heardSpeaker;

    public GameObject[] routeWaypoints;
    private Transform[] patrolRoute;
    public GameObject[] pathfindingNodes;
    public List<Transform> pathfindingPos;
    public List<Transform> pathfindingReturnPos;
    public bool isPathfinding;
    public bool isReturning;
    private int pathfindingIndex;
    private int randomRoute;
    private int index;

    private Transform playerPosition;
    private Transform speakerPosition;

    // Stephen's Variables start here
    public ClosestWaypoint closestWaypoint;
    public WaypointScript wscopy;

    void Start()
    {
        waitTime = startWaitTime;
        pathfindingIndex = 0;
        isPathfinding = false;
        actualPlayer = GameObject.FindGameObjectWithTag("Player");
        closestWaypoint = gameObject.GetComponent<ClosestWaypoint>();
        wscopy = closestWaypoint.GetComponent<WaypointScript>();
        patrolRoute = new Transform[routeWaypoints.Length];

        index = 0;
        foreach (GameObject node in routeWaypoints)
        {
            patrolRoute[index] = node.transform;
            index++;
        }

        waitTime = startWaitTime;
        /* Mario Code starts here
        waitTime = startWaitTime;
        randomRoute = Random.Range(0, patrolRoute.Length);
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        speakerPosition = GameObject.FindGameObjectWithTag("Speaker").GetComponent<Transform>();
        guardPosition = GameObject.FindGameObjectWithTag("Guard").GetComponent<Transform>();
        */
    }

    private void Update()
    {
        wscopy = closestWaypoint.closestWaypoint.GetComponent<WaypointScript>();
        if (isPathfinding)
        {
            FollowNodePath();
        }
        else if (inView == false && heardSpeaker == false)
        {
            Patrol();
        }
       /* else if (inView == true)
        {
            Chase();
        }*/
    }

   /* public void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerPosition.position, speed * Time.deltaTime);
        Vector2 direction = new Vector2(playerPosition.position.x - transform.position.x, playerPosition.position.y - transform.position.y);
        direction = direction.normalized;
        transform.up = direction;
    }
    */
    public void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolRoute[randomRoute].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolRoute[randomRoute].position) < 0.1f)
        {
            if (waitTime <= 0)
            {
                randomRoute = Random.Range(0, patrolRoute.Length);
                waitTime = startWaitTime;

                transform.position = Vector2.MoveTowards(transform.position, patrolRoute[randomRoute].position, speed * Time.deltaTime);
                Vector2 direction = new Vector2(patrolRoute[randomRoute].position.x - transform.position.x, patrolRoute[randomRoute].position.y - transform.position.y);
                direction = direction.normalized;
                transform.up = direction;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        else // this bit lets the guard walk back to their patrol route normally, instead of moonwalking
        {
            Vector2 direction = new Vector2(patrolRoute[randomRoute].position.x - transform.position.x, patrolRoute[randomRoute].position.y - transform.position.y);
            direction = direction.normalized;
            transform.up = direction;
        }
    }
    public void FollowNodePath()
    {
        transform.position = Vector2.MoveTowards(transform.position, pathfindingPos[0].position, speed * Time.deltaTime); // head to the next node in the array
        if (Vector2.Distance(transform.position, pathfindingPos[0].position) < 0.1f) // if we're at the node, start going to the next one
        {
            pathfindingPos.RemoveAt(0);
        }
        if (pathfindingPos.Count == 0) // if we are at the target node, meaning the list of nodes to travel to is empty
        {
            if (isReturning == false)
            {
                isReturning = true;
                pathfindingPos = GetPathfindingPositions(pathfindingNodes);
                pathfindingPos.Reverse();
            }
            else
            {
                isReturning = false;
                isPathfinding = false;
            }
        }
        else
        {
            Vector2 direction = new Vector2(pathfindingPos[0].position.x - transform.position.x, pathfindingPos[0].position.y - transform.position.y);
            direction = direction.normalized;
            transform.up = direction;
        }
    }
    public void OpenDoor(GameObject doorWaypoint)
    {

    }
    public void GetNodePath(GameObject targetNode) // This is where the shortest path algorithm in WaypointScript is called
    {
        if (targetNode)
        {
            wscopy.targetNode = targetNode; // tells the waypoints script what our target node is
            wscopy.SendMessage("StartShortestPath", this); // starts the pathfinding functions, tells the waypoint script who the guard is
            //Debug.Log("path nodes array = " + wscopy.pathNodesArray.ToString());
            pathfindingNodes = wscopy.pathNodesArray;
        }
    }

    public IEnumerator MoveToWaypoint(GameObject targetNode)
    {
        GetNodePath(targetNode); // the path that we have to follow is now stored in pathfindingNodes
        if (pathfindingNodes.Length == 0)
        {
            yield return null;
        }
        //Debug.Log("Pathfinding Nodes = " + pathfindingNodes.ToString());
    }
    public void StartMoveToWaypoint(GameObject targetNode) // This is what other objects will message to send the guard off somewhere
    {
        StartCoroutine("MoveToWaypoint", targetNode);
    }
    private List<Transform> GetPathfindingPositions(GameObject[] origPath)
    {
        Transform[] positions = new Transform[origPath.Length];
        for (int i = 0; i < origPath.Length; ++i)
        {
            positions[i] = origPath[i].transform;
        }
        return positions.ToList();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Door")
        {
            DoorMechanics temp = col.gameObject.GetComponent<DoorMechanics>();
            if (temp.isOpen == false)
            {
                temp.SendMessage("GuardOpen");
                Debug.Log("Guard should have opened me!");
            }
        }
    }

}
