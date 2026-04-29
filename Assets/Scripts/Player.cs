using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isYourTurn = false;

    public List<Unit> ownedUnits;

    //number 0 reserved for NPC
    public int playerNumber = 1;

    public string characterName;

    public void StartTurn()
    {
        foreach(Unit unit in ownedUnits)
        {
            unit.ResetTurn();
        }
        isYourTurn = true;
    }

    public void EndTurn()
    {
        foreach (Unit unit in ownedUnits)
        {
            unit.EndTurn();
        }
        isYourTurn = false;
    }
}
