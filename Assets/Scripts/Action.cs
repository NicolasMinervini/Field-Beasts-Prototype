using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public Sprite actionSprite;
    public string actionName;
    public string actionDescription;
    public TargetType targetType = TargetType.TargetUnit;

    public Unit unit;

    public float range = 5;
    public bool heightDisadvantageRangeDebuff = false;
    public bool heightAdvantageRangeBuff = true;
    public float rangeChangePerUnitHeight = 1;

    [HideInInspector]
    public List<Vector3> navLinePositions = new List<Vector3>();

    [HideInInspector]
    public bool isMouseOverUI = false;

    public virtual void Start()
    {
        if(unit == null)
        {
            Unit u;
            if(gameObject.TryGetComponent<Unit>(out u))
            {
                unit = u;
            }
            else
            {
                Debug.LogError("Action " + actionName + " on GameObject " + gameObject.name + " couldn't find its unit!");
            }
        }
    }

    //this function is run by the player pointer when this action is selected.
    public virtual void PrepareAction()
    {

    }
    public virtual void PrepareAction(RaycastHit hit)
    {

    }

    //Follow through on using this action.
    public virtual void DoAction()
    {

    }

    //If an NPC unit decides to use this action, this function will determine how it uses it.
    public virtual void AutomaticAction()
    {

    }

    //removed this action from the player pointer's selected action, disabling related visual effects.
    public virtual void Deselect()
    {
        if (unit != null && unit.navline != null)
        {
            unit.navline.enabled = false;
        }

        if (unit.unitPointer != null)
        {
            unit.unitPointer.selectedAction = null;
        }

        if (unit.rangeRing != null)
        {
            unit.rangeRing.SetActive(false);
        }
    }

    //Display a circle around this action's unit with a radius equal to the action's functional range
    public virtual void ShowRangeRing()
    {
        ShowRangeRing(unit.transform.position);
    }
    public virtual void ShowRangeRing(Vector3 pointerPos)
    {
        if (unit == null || unit.rangeRing == null) return;

        unit.rangeRing.SetActive(true);

        unit.rangeRing.transform.position = new Vector3(
            unit.rangeRing.transform.position.x,
            pointerPos.y + unit.rangeRingVerticalOffset,
            unit.rangeRing.transform.position.z);

        unit.rangeRing.transform.localScale = Vector3.one * GetFunctionalRange(pointerPos);
    }

    //draws a line from the unit to its target. Makes the line red if the target is out of range
    public virtual void ShowAimLine(Vector3 targetPosition, float lineVerticalOffset = 0.5f)
    {
        if (unit.navline == null) return;

        navLinePositions.Clear();
        navLinePositions.Add(new Vector3(unit.transform.position.x, unit.transform.position.y + lineVerticalOffset, unit.transform.position.z));
        navLinePositions.Add(new Vector3(targetPosition.x, targetPosition.y + lineVerticalOffset, targetPosition.z));
        unit.navline.positionCount = navLinePositions.Count;
        unit.navline.SetPositions(navLinePositions.ToArray());

        if (IsInRange(targetPosition))
        {
            unit.navline.startColor = Color.white;
            unit.navline.endColor = Color.white;
        }
        else
        {
            unit.navline.startColor = Color.red;
            unit.navline.endColor = Color.red;
        }
    }

    //Gets the actual range of this action against a target position, accounting for range changes due to height
    public virtual float GetFunctionalRange(Vector3 targetPosition)
    {
        float targetToUnitHeight = Mathf.Round(unit.transform.position.y - targetPosition.y);
        if((targetToUnitHeight > 0 && heightAdvantageRangeBuff) || (targetToUnitHeight < 0 && heightDisadvantageRangeDebuff))
        {
            return Mathf.Max(0, range + (targetToUnitHeight * rangeChangePerUnitHeight));
        }
        return range;
    }

    //get horiztonal distance from this unit to a target, disregarding height differences
    public virtual float GetHorizontalDistance(Vector3 targetPosition)
    {
        Vector3 horizontalAgnosticTargetPosition = new Vector3(targetPosition.x, unit.transform.position.y, targetPosition.z);
        return Vector3.Distance(unit.transform.position, horizontalAgnosticTargetPosition);
    }

    //Range Checking functions. For checking if a targeted position is within range of this action.
    public virtual bool IsInRange(Transform target)
    {
        return IsInRange(target.position);
    }
    public virtual bool IsInRange(Vector3 targetPosition)
    {
        //get the target's position without regards to height difference
        Vector3 horizontalAgnosticTargetPosition = new Vector3(targetPosition.x, unit.transform.position.y, targetPosition.z);

        return GetFunctionalRange(targetPosition) >= Vector3.Distance(unit.transform.position, horizontalAgnosticTargetPosition);
    }

    public enum TargetType
    {
        TargetGround,
        TargetUnit,
        TargetSelf
    }
}
