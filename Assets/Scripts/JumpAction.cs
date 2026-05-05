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
        float targetToUnitHeight = Mathf.Round(unit.transform.position.y - targetPosition.y);
        if ((targetToUnitHeight > 0 && heightAdvantageRangeBuff) || (targetToUnitHeight < 0 && heightDisadvantageRangeDebuff))
        {
            return (targetToUnitHeight * rangeChangePerUnitHeight);
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

        unit.navline.SetPositions(new Vector3[2]);
        unit.navline.SetPosition(0, new Vector3(unit.gameObject.transform.position.x, unit.gameObject.transform.position.y + lineVerticalOffset, unit.gameObject.transform.position.z));
        targetPosition.y += lineVerticalOffset;
        unit.navline.SetPosition(1, targetPosition);

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
