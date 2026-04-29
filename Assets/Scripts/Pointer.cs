using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer : Player
{
    public Camera cam;
    public GameObject groundIndicator;

    public float raycastDistance = 999f;
    public LayerMask rayHitLayers;

    Ray pointerRay;
    RaycastHit hit;
    Vector3 mousePos;

    public float pointerPercentEdgeCutoff = 0.05f;
    float pointerPixelEdgeCutoffX, pointerPixelEdgeCutoffY;

    public Unit selectedUnit;
    public Action selectedAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //isYourTurn = true;

        if(cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ********* Mouse position and information *********

        mousePos = Mouse.current.position.ReadValue();

        pointerRay = cam.ScreenPointToRay(mousePos);
        //Debug.Log(mousePos);

        pointerPixelEdgeCutoffX = Screen.width * pointerPercentEdgeCutoff;
        pointerPixelEdgeCutoffY = Screen.height * pointerPercentEdgeCutoff;

        bool isOnScreen =
            mousePos.x > pointerPixelEdgeCutoffX
            && mousePos.x < Screen.width - pointerPixelEdgeCutoffX
            && mousePos.y > pointerPixelEdgeCutoffY
            && mousePos.y < Screen.height - pointerPixelEdgeCutoffY
            && Application.isFocused;

        //clicking to select and deselect units
        if(isOnScreen && Mouse.current.leftButton.wasPressedThisFrame && Physics.Raycast(pointerRay, out hit, raycastDistance, rayHitLayers))
        {
            Unit clickedUnit;

            if (hit.collider.CompareTag("Unit") && hit.collider.gameObject.TryGetComponent<Unit>(out clickedUnit))
            {
                SelectUnit(clickedUnit);
            }
            else
            {
                DeselectUnit();
            }
        }

        if(selectedUnit != null)
        {
            //player is selecting a unit

            if (selectedUnit.IsSameTeam(playerNumber))
            {
                //player is selecting their own unit

                if (isYourTurn)
                {
                    //It is this player's turn, they are allowed to command their own units

                    //Prepare action
                    if (isOnScreen && Physics.Raycast(pointerRay, out hit, raycastDistance, rayHitLayers))
                    {
                        groundIndicator.SetActive(true);
                        groundIndicator.transform.position = hit.point;

                        if(selectedAction == null)
                        {
                            selectedAction = selectedUnit.defaultAction;
                            selectedUnit.defaultAction.PrepareAction(hit);
                        }
                        else
                        {
                            selectedAction.PrepareAction(hit);
                        }
                    }
                    else
                    {
                        // cursor is off screen or isn't pointing towards anything

                        groundIndicator.SetActive(false);
                    }
                }
                else
                {
                    //it is not this player's turn

                    groundIndicator.SetActive(false);
                }
            }
            else
            {
                //player is selecting a unit they do not own

                groundIndicator.SetActive(false);
            }
        }
    }

    public void SelectUnit(Unit clickedUnit)
    {
        DeselectUnit();
        groundIndicator.SetActive(true);
        selectedUnit = clickedUnit;
    }

    public void DeselectUnit()
    {
        groundIndicator.SetActive(false);
        selectedUnit = null;
        DeselectAction();
    }

    public void DeselectAction()
    {
        if(selectedAction != null)
        {
            selectedAction.Deselect();
        }
        selectedAction = null;
    }
}
