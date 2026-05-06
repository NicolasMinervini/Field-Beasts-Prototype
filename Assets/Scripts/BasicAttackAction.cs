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

    //x value is minimum damage, y value is maximum. This attack deals a random amount between the two inclusively
    public Vector2 damage;

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

        isMouseOverUI = EventSystem.current.IsPointerOverGameObject();

        //check if the raycast hit a unit
        hitUnit = null;
        if(hit.collider.gameObject.TryGetComponent<Unit>(out hitUnit))
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

            if (Mouse.current.rightButton.wasPressedThisFrame
            && isMouseOverUI == false
            && IsInRange(hitUnit.transform.position))
            {
                DoAction();
            }
        }
        else
        {
            //the ray is not hitting a unit

            ShowRangeRing(externalHit.point);
            ShowAimLine(externalHit.point);
            if (unit.pathLength != null) unit.pathLength.text = "Distance: " + GetHorizontalDistance(hitUnit.transform.position);
            if (unit.pathStatus != null) unit.pathStatus.text = "Invalid target!";
        }
    }

    public override void DoAction()
    {
        hitUnit.Hurt(Random.Range((int)damage.x, (int)damage.y));

        Deselect();
    }
}
