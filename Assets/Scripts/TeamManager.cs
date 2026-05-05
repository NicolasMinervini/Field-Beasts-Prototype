using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [HideInInspector]
    public LineRenderer navline;
    [HideInInspector]
    public TMP_Text pathStatus;
    [HideInInspector]
    public TMP_Text pathLength;

    public Pointer teamPointer;

    public string teamName;
    public Color teamColor;
    public int teamNumber;

    public List<Unit> units;

    public void StartGame()
    {
        if (teamPointer != null) teamNumber = teamPointer.playerNumber;

        if (units == null) units = new List<Unit>();

        if (units.Count <= 0) FindUnits();
        SetupUnits();
    }

    public void FindUnits()
    {
        units.Clear();
        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag("Unit");

        Unit u = null;

        foreach(GameObject obj in foundObjects)
        {
            if(obj.TryGetComponent<Unit>(out u) && u.team == teamNumber)
            {
                units.Add(u);
            }
        }
    }

    public void SetupUnits()
    {
        foreach(Unit u in units)
        {
            u.navline = navline;
            u.pathLength = pathLength;
            u.pathStatus = pathStatus;
            u.teamManager = this;
        }
    }

    public void RemoveUnit(Unit u)
    {
        units.Remove(u);
        units.RemoveAll(x => !x);
    }

    public void StartTurn()
    {
        units.RemoveAll(x => !x);

        foreach(Unit u in units)
        {
            u.ResetTurn();
        }

        if (teamPointer != null)
        {
            teamPointer.isYourTurn = true;
        }
    }
    public void EndTurn()
    {
        units.RemoveAll(x => !x);

        foreach (Unit u in units)
        {
            u.EndTurn();
        }

        if(teamPointer != null)
        {
            teamPointer.isYourTurn = false;
            teamPointer.DeselectUnit();
        }
    }
}
