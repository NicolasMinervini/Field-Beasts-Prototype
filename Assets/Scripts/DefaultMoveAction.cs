using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultMoveAction : Action
{
    RaycastHit hit;
    Ray ray;
    public float raycastDistance = 999f;
    public LayerMask rayHitLayers;

    public override void PrepareAction()
    {
        if(unit == null) { return; }

        range = unit.moveSpeedRemaining;

        ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out hit, raycastDistance, rayHitLayers))
        {
            PrepareAction(hit);
        }
    }
    public override void PrepareAction(RaycastHit externalHit)
    {
        if (unit == null) { return; }

        range = unit.moveSpeedRemaining;
        ShowRangeRing(externalHit.point);

        unit.GetUnitPath(externalHit.point);
        //GetUnitPath(hit.point);

        hit = externalHit;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            DoAction();
        }
    }

    public override void DoAction()
    {
        unit.Move(unit.GetUnitPath(hit.point));
        //Move(GetUnitPath(hit.point));

        Deselect();
    }
}
