using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pointer : Player
{
    public Camera cam;
    public GameObject groundIndicator;
    public Transform cameraPos;

    public float raycastDistance = 999f;
    public LayerMask rayHitLayers;

    Ray pointerRay;
    RaycastHit hit;
    Vector3 mousePos;

    public float pointerPercentEdgeCutoff = 0.05f;
    float pointerPixelEdgeCutoffX, pointerPixelEdgeCutoffY;

    public Unit selectedUnit;
    public Action selectedAction;

    public bool movementKeyboardShortcuts = true;

    public TMP_Text debugSelectedAbilityName, debugSelectedUnit, healthText, moveText, actionsText;
    public Image defaultActionButton, jumpButton, actionButton1, actionButton2;
    public Slider healthBar, moveBar;

    bool isMouseOverUI = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //isYourTurn = true;

        if (cameraPos == null) cameraPos = transform;

        if(cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Actually, we don't want this pointer to be doing anything outside of its turn in the two-player prototype
        if (!isYourTurn) return;

        // ********* Mouse position and information *********

        mousePos = Mouse.current.position.ReadValue();

        pointerRay = cam.ScreenPointToRay(mousePos);
        //Debug.Log(mousePos);

        pointerPixelEdgeCutoffX = Screen.width * pointerPercentEdgeCutoff;
        pointerPixelEdgeCutoffY = Screen.height * pointerPercentEdgeCutoff;

        isMouseOverUI = EventSystem.current.IsPointerOverGameObject();

        bool isOnScreen =
            mousePos.x > pointerPixelEdgeCutoffX
            && mousePos.x < Screen.width - pointerPixelEdgeCutoffX
            && mousePos.y > pointerPixelEdgeCutoffY
            && mousePos.y < Screen.height - pointerPixelEdgeCutoffY
            && Application.isFocused;

        //clicking to select and deselect units
        if(isOnScreen && isMouseOverUI == false && Mouse.current.leftButton.wasPressedThisFrame && Physics.Raycast(pointerRay, out hit, raycastDistance, rayHitLayers))
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

                    if (movementKeyboardShortcuts)
                    {
                        MovementShortcuts();
                    }

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

                    DeselectAction();

                    groundIndicator.SetActive(false);
                }
            }
            else
            {
                //player is selecting a unit they do not own

                DeselectAction();

                groundIndicator.SetActive(false);
            }
        }
        else
        {
            //player isn't selecting a unit

            DeselectUnit();

            groundIndicator.SetActive(false);
        }

        DisplaySelectedAction();
        DisplaySelectedUnit();
    }

    public void SelectUnit(Unit clickedUnit)
    {
        DeselectUnit();
        clickedUnit.unitPointer = this;
        groundIndicator.SetActive(true);
        selectedUnit = clickedUnit;
    }

    public void DeselectUnit()
    {
        groundIndicator.SetActive(false);
        if(selectedUnit != null)
        {
            selectedUnit.unitPointer = null;
        }
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

    public void MovementShortcuts()
    {
        if (selectedUnit == null) return;

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            //shortcut to default (movement) action
            SelectDefaultAction();
        }
        else if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            //shortcut to jump action
            SelectJumpAction();
        }
    }

    public void DisplaySelectedAction()
    {
        if (debugSelectedAbilityName == null) return;

        if(selectedAction == null)
        {
            debugSelectedAbilityName.text = "";
        }
        else
        {
            debugSelectedAbilityName.text = "Selected action: " + selectedAction.actionName;
        }
    }
    public void DisplaySelectedUnit()
    {
        if (debugSelectedUnit == null) return;

        if(selectedUnit == null)
        {
            debugSelectedUnit.text = "";
        }
        else
        {
            debugSelectedUnit.text = "Selected Unit: " + selectedUnit.name;
        }

        UpdateActionUI();
        UpdateBars();
    }



    //************************************************************ Stupid UI stuff beyond this point!!! *********************************************************************



    public void SelectDefaultAction()
    {
        if (selectedUnit == null || !isYourTurn) return;

        selectedAction = selectedUnit.defaultAction;
    }
    public void SelectJumpAction()
    {
        if (selectedUnit == null || !isYourTurn) return;

        selectedAction = selectedUnit.jumpAction;
    }
    public void SelectFirstAction()
    {
        if (selectedUnit == null || !isYourTurn) return;

        selectedAction = selectedUnit.actions[0];
    }
    public void SelectSecondAction()
    {
        if (selectedUnit == null || !isYourTurn) return;

        selectedAction = selectedUnit.actions[1];
    }

    //show the proper action icons for each action button
    public void UpdateActionUI()
    {
        if (selectedUnit != null)
        {
            defaultActionButton.gameObject.SetActive(true);
            jumpButton.gameObject.SetActive(true);
            actionButton1.gameObject.SetActive(true);
            actionButton2.gameObject.SetActive(true);

            defaultActionButton.sprite = selectedUnit.defaultAction.actionSprite;
            jumpButton.sprite = selectedUnit.jumpAction.actionSprite;
            actionButton1.sprite = selectedUnit.actions[0].actionSprite;
            actionButton2.sprite = selectedUnit.actions[1].actionSprite;
        }
        else
        {
            defaultActionButton.gameObject.SetActive(false);
            jumpButton.gameObject.SetActive(false);
            actionButton1.gameObject.SetActive(false);
            actionButton2.gameObject.SetActive(false);
        }
    }

    //display the currently selected unit's health and remaining movement
    public void UpdateBars()
    {
        if(selectedUnit != null)
        {
            healthText.gameObject.SetActive(true);
            moveText.gameObject.SetActive(true);
            healthBar.gameObject.SetActive(true);
            moveBar.gameObject.SetActive(true);
            actionsText.gameObject.SetActive(true);

            actionsText.text = "Actions remaining: " + selectedUnit.actionsRemaining;

            healthBar.minValue = 0;
            healthBar.maxValue = selectedUnit.maxHealth;
            healthBar.value = selectedUnit.health;
            healthText.text = "Health: " + selectedUnit.health + " / " + selectedUnit.maxHealth;

            moveBar.minValue = 0;
            moveBar.maxValue = selectedUnit.moveSpeed;
            moveBar.value = selectedUnit.moveSpeedRemaining;
            healthText.text = "Move: " + selectedUnit.moveSpeedRemaining + " / " + selectedUnit.moveSpeed;
        }
        else
        {
            healthText.gameObject.SetActive(false);
            moveText.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(false);
            moveBar.gameObject.SetActive(false);
            actionsText.gameObject.SetActive(false);
        }
    }
}
