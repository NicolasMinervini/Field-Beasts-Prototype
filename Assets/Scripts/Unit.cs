using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 moveTarget;
    public Pointer unitPointer;
    public TeamManager teamManager;
    
    [HideInInspector]
    public NavMeshPath targetPath;
    [HideInInspector]
    public LineRenderer navline;
    [HideInInspector]
    public TMP_Text pathStatus, pathLength;

    //0 = NPC
    //1 = Player 1
    //2 = Player 2
    public int team = 1;
    public float maxHealth = 20;
    public float health = 20;

    //Unit movement stuff
    public float moveSpeed = 20f;
    [HideInInspector]
    public float moveSpeedRemaining = 20f;

    //Unit action stuff
    public int actionsPerTurn = 1;
    public int actionsRemaining = 1;
    public Action defaultAction, jumpAction;
    public Action[] actions;

    public GameObject rangeRing;
    public float rangeRingVerticalOffset = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeedRemaining = moveSpeed;
        actionsRemaining = actionsPerTurn;

        targetPath = new NavMeshPath();

        //tell this unit's actions who they belong to
        if(defaultAction != null) defaultAction.unit = this;

        foreach (Action act in actions)
        {
            if(act != null) act.unit = this;
        }

        if(rangeRing != null)
        {
            rangeRing.SetActive(false);
        }

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Unit regains its movement and action at the start of its turn
    public void ResetTurn()
    {
        moveSpeedRemaining = moveSpeed;
        actionsRemaining = actionsPerTurn;
    }

    //remove all of this unit's remaining movement speed and actions when its turn is ended
    public void EndTurn()
    {
        agent.isStopped = true;
        moveSpeedRemaining = 0;
        actionsRemaining = 0;
    }

    public void Move(Vector3 destination)
    {
        agent.enabled = false;
        transform.position = destination;
        agent.enabled = true;
    }



    //Health Management

    public void Heal(float amount)
    {
        if(amount < 0)
        {
            Heal(Mathf.Abs(amount));
            return;
        }

        health = Mathf.Min(health + amount, maxHealth);
    }
    public void Hurt(float amount)
    {
        if (amount < 0)
        {
            Hurt(Mathf.Abs(amount));
            return;
        }

        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if(teamManager != null)
        {
            teamManager.RemoveUnit(this);
        }
        Destroy(gameObject);
    }


    
    //turn a navmeshpath into a list of vector3s, chopping off the parts beyond this unit's available movement range
    public Vector3 GetUnitPath(NavMeshPath path = null)
    {
        if(path == null)
        {
            agent.CalculatePath(moveTarget, targetPath);
            path = targetPath;
        }

        List<Vector3> positions = new List<Vector3>();

        Vector3 previousPosition = transform.position;
        float pathDistance = 0;

        positions.Clear();
        positions.Add(previousPosition);

        //Iterate through each corner on the path to find the final reachable position within the unit's remaining movement distance
        foreach(Vector3 corner in path.corners)
        {
            //get the total distance from unit to next corner
            float newDist = pathDistance + Vector3.Distance(previousPosition, corner);

            //unit cannot reach next corner. Final position is somewhere between previousPosition and corner
            if(newDist > moveSpeedRemaining)
            {
                //get the direction from the previous position to the next corner.
                Vector3 finalPositionDirection = corner - previousPosition;

                //traverse that direction for the amount of remaining move speed to find the final position
                Vector3 finalPosition = previousPosition + (finalPositionDirection.normalized * (moveSpeedRemaining - pathDistance));

                positions.Add(finalPosition);

                //save the new distance and final position for display
                pathDistance = moveSpeedRemaining;
                previousPosition = finalPosition;

                break;
            }
            //edge case. corner is just within range.
            else if(newDist == moveSpeedRemaining)
            {
                positions.Add(corner);

                //save the new distance and final corner for display
                pathDistance = newDist;
                previousPosition = corner;

                break;
            }
            //corner is within range with extra movement remaining. This is the only case where the loop continues.
            else
            {
                positions.Add(corner);

                //save the new distance and previous corner for the next loop
                pathDistance = newDist;
                previousPosition = corner;
            }
        }

        //show the player where their unit will go
        DisplayUnitPath(path, positions, pathDistance);

        return positions[positions.Count - 1];
    }
    public Vector3 GetUnitPath(Transform targetPosition, NavMeshPath path = null)
    {
        moveTarget = targetPosition.position;
        return GetUnitPath(path);
    }
    public Vector3 GetUnitPath(Vector3 targetPositionVector, NavMeshPath path = null)
    {
        moveTarget = targetPositionVector;
        return GetUnitPath(path);
    }

    //Used by GetUnitPath to display a calculated unit path as a line and onto the HUD
    public void DisplayUnitPath(NavMeshPath path, List<Vector3> positions, float pathDistance)
    {
        if(pathStatus != null)
        {
            pathStatus.text = "Path " + path.status.ToString();
        }

        if(pathLength != null)
        {
            pathLength.text = "Length: " + pathDistance + " m";
        }
        
        if(navline != null)
        {
            navline.startColor = Color.white;
            navline.endColor = Color.white;

            navline.enabled = true;
            navline.positionCount = positions.Count;
            navline.SetPositions(positions.ToArray());
        }
    }
    

    //determines if this unit can reach a position with its remaining movement this turn. Incomplete paths are considered false
    public bool IsInMovementRange(Vector3 endpoint)
    {
        NavMeshPath path = new NavMeshPath();

        //create path and check if it is invalid
        if (agent.CalculatePath(endpoint, path) == false || path.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        }
        //check if path is incomplete
        else if (path.status == NavMeshPathStatus.PathPartial)
        {
            return false;
        }
        //check if the path is too long
        else if (GetPathDistance(path) > moveSpeedRemaining)
        {
            return false;
        }

        return true;
    }

    public float GetPathDistance(NavMeshPath path)
    {
        float dist = 0;

        Vector3 previousCorner = transform.position;
        foreach (Vector3 corner in path.corners)
        {
            dist += Vector3.Distance(previousCorner, corner);
        }

        return dist;
    }

    public bool IsSameTeam(int otherTeam)
    {
        return otherTeam == team;
    }
}
