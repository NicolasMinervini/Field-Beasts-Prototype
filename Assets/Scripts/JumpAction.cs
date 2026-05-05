using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpAction : Action
{
    RaycastHit hit;
    Ray ray;
    public float raycastDistance = 999f;
    public LayerMask rayHitLayers;

    public float maxHorizontalJumpDistance = 5f;

    public override void PrepareAction()
    {
        if (unit == null) { return; }

        range = unit.moveSpeedRemaining;

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit, raycastDistance, rayHitLayers))
        {
            PrepareAction(hit);
        }
    }
    public override void PrepareAction(RaycastHit externalHit)
    {
        if (unit == null) { return; }

        range = unit.moveSpeedRemaining;
        
        //visual indicators and HUD stuff
        ShowRangeRing(externalHit.point);
        ShowAimLine(externalHit.point);
        if(unit.pathLength != null)
        {
            if (IsValidTargetPosition(externalHit.point))
            {
                unit.pathLength.text = "Distance: " + GetMovementCost(externalHit.point) + " m";
            }
            else
            {
                unit.pathLength.text = "Cannot jump to position!";
            }
        }

        //need to do this so DoAction can read the raycasthit
        hit = externalHit;

        //jump when the player right clicks a valid position within range
        if (Mouse.current.rightButton.wasPressedThisFrame
            && IsValidTargetPosition(externalHit.point)
            && (GetMovementCost(externalHit.point) <= range)
            && IsWithinJumpRange(externalHit.point))
        {
            DoAction();
        }
    }

    public override void DoAction()
    {
        unit.Move(hit.point);

        //unit.moveSpeedRemaining = GetMovementCost(hit.point);

        Deselect();
    }

    //Calculates the potential cost of moving from the current height to the targeted height. Positive value means extra cost (moving upwards), negative value means bonus distance (moving down)
    public float GetHeightCost(Vector3 targetPosition)
    {
        float unitToTargetHeight = Mathf.Round(targetPosition.y - unit.transform.position.y);
        if ((unitToTargetHeight < 0 && heightAdvantageRangeBuff) || (unitToTargetHeight > 0 && heightDisadvantageRangeDebuff))
        {
            return (unitToTargetHeight * rangeChangePerUnitHeight);
        }
        return 0;
    }

    //Returns the calculated cost to move to a target position
    public float GetMovementCost(Vector3 targetPosition)
    {
        return GetHorizontalDistance(targetPosition) + GetHeightCost(targetPosition);
    }

    //checks if the targeted position is on the navmesh
    public bool IsValidTargetPosition(Vector3 targetPosition)
    {
        unit.agent.CalculatePath(targetPosition, unit.targetPath);
        if(unit.targetPath != null && unit.targetPath.status != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return true;
        }
        return false;
    }

    public bool IsWithinJumpRange(Vector3 targetPosition)
    {
        return GetHorizontalDistance(targetPosition) <= maxHorizontalJumpDistance;
    }

    public override void ShowAimLine(Vector3 targetPosition, float lineVerticalOffset = 0.5f)
    {
        if (unit.navline == null) return;

        //nav line stuff
        navLinePositions.Clear();
        navLinePositions.Add(new Vector3(unit.transform.position.x, unit.transform.position.y + lineVerticalOffset, unit.transform.position.z));
        navLinePositions.Add(new Vector3(targetPosition.x, targetPosition.y + lineVerticalOffset, targetPosition.z));
        unit.navline.positionCount = navLinePositions.Count;
        unit.navline.SetPositions(navLinePositions.ToArray());
        
        if (IsInRange(targetPosition) && IsWithinJumpRange(targetPosition))
        {
            unit.navline.endColor = Color.white;
        }
        else
        {
            unit.navline.endColor = Color.red;
        }
    }

    public override void ShowRangeRing(Vector3 pointerPos)
    {
        if (unit == null || unit.rangeRing == null) return;

        unit.rangeRing.SetActive(true);

        unit.rangeRing.transform.position = new Vector3(
            unit.rangeRing.transform.position.x,
            pointerPos.y + unit.rangeRingVerticalOffset,
            unit.rangeRing.transform.position.z);

        unit.rangeRing.transform.localScale = Vector3.one * maxHorizontalJumpDistance;
    }
}
