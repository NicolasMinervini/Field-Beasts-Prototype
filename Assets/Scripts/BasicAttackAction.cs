using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BasicAttackAction : Action
{
    RaycastHit hit;
    Ray ray;
    public float raycastDistance = 999f;
    public LayerMask rayHitLayers;
    Unit hitUnit;

    public GameObject hitEffect;

    //x value is minimum damage, y value is maximum. This attack deals a random amount between the two inclusively
    public Vector2 damage;

    public override void Start()
    {
        base.Start();

        actionDescription = "Deal " + (int)damage.x + " - " + (int)damage.y + " damage to a target.";
    }

    public override void PrepareAction()
    {
        if (unit == null) { return; }

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit, raycastDistance, rayHitLayers))
        {
            PrepareAction(hit);
        }
    }
    public override void PrepareAction(RaycastHit externalHit)
    {
        if (unit == null) { return; }

        actionDescription = "Deal " + (int)damage.x + " - " + (int)damage.y + " damage to a target.";

        isMouseOverUI = EventSystem.current.IsPointerOverGameObject();

        //check if the raycast hit a unit
        hitUnit = null;
        if(externalHit.collider.gameObject.TryGetComponent<Unit>(out hitUnit) && unit.actionsRemaining > 0)
        {
            //visual indicators and HUD stuff
            ShowRangeRing(hitUnit.transform.position);
            ShowAimLine(hitUnit.transform.position);
            if (IsInRange(hitUnit.transform.position))
            {
                if (unit.pathLength != null) unit.pathLength.text = "Distance: " + GetHorizontalDistance(hitUnit.transform.position);
                if (unit.pathStatus != null) unit.pathStatus.text = "Targeting " + hitUnit.gameObject.name;
            }
            else
            {
                if (unit.pathLength != null) unit.pathLength.text = "Distance: " + GetHorizontalDistance(hitUnit.transform.position);
                if (unit.pathStatus != null) unit.pathStatus.text = "Target out of range!";
            }

            hit = externalHit;

            if (Mouse.current.rightButton.wasPressedThisFrame
            && isMouseOverUI == false
            && IsInRange(hitUnit.transform.position))
            {
                DoAction();
            }
        }
        else if(unit.actionsRemaining > 0)
        {
            //the ray is not hitting a unit but the action can still be used

            ShowRangeRing(externalHit.point);
            ShowAimLine(externalHit.point);
            if (unit.pathLength != null) unit.pathLength.text = "Distance: " + GetHorizontalDistance(externalHit.point);
            if (unit.pathStatus != null) unit.pathStatus.text = "Invalid target!";
        }
        else
        {
            Deselect();
        }
    }

    public override void DoAction()
    {
        hitUnit.Hurt(Random.Range((int)damage.x, (int)damage.y));

        if(hitEffect != null)
        {
            Instantiate(hitEffect, hitUnit.transform.position, Quaternion.identity);
        }

        unit.actionsRemaining -= 1;

        Deselect();
    }
}
