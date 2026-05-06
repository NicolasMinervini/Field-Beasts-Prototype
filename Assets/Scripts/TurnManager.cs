using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public Camera cam;

    public LineRenderer navline;
    public TMP_Text pathStatus;
    public TMP_Text pathLength;

    public TMP_Text currentTurnText;
    public int currentTurn = 0;

    public TeamManager[] teams;

    bool gameComplete = false;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        foreach(TeamManager t in teams)
        {
            t.navline = navline;
            t.pathStatus = pathStatus;
            t.pathLength = pathLength;

            t.StartGame();

            t.EndTurn();
        }

        StartTurn();
    }

    public void EndTurn()
    {
        if (gameComplete) return;

        Debug.Log(teams[currentTurn].teamName + " ended their turn.");

        int nextTeam = currentTurn + 1;
        if (nextTeam >= teams.Length) nextTeam = 0;

        while(teams[nextTeam].units.Count <= 0)
        {
            Debug.Log("Skipping team " + teams[nextTeam].teamName + " because they have no units.");

            nextTeam += 1;
            if (nextTeam >= teams.Length) nextTeam = 0;

            if(nextTeam == currentTurn)
            {
                //we have cycled through the full team list. This either means the team that just ended its turn is the winner, or that everyone loses

                Debug.Log("Game is ready to conclude!");

                if(teams[currentTurn].units.Count <= 0)
                {
                    //the current team has no units remaining. This ends the game in a draw.

                    EndGame(null);
                    return;
                }
                else
                {
                    //the current team has units remaining. They are the winner.

                    EndGame(teams[currentTurn]);
                    return;
                }
            }
        }

        teams[currentTurn].EndTurn();

        currentTurn = nextTeam;

        StartTurn();
    }

    public void StartTurn()
    {
        if (gameComplete) return;

        Debug.Log("Starting " + teams[currentTurn].teamName + "'s turn.");

        //for multiplayer. Move camera to currently active player pointer
        if(teams[currentTurn].teamPointer != null)
        {
            cam.transform.parent = teams[currentTurn].teamPointer.cameraPos;
            cam.transform.localPosition = Vector3.zero;
            cam.transform.localScale = Vector3.one;
            cam.transform.localEulerAngles = Vector3.zero;
        }

        teams[currentTurn].StartTurn();

        if(currentTurnText != null)
        {
            currentTurnText.text = teams[currentTurn].teamName + "'s turn";
            currentTurnText.color = teams[currentTurn].teamColor;
        }   
    }

    public void EndGame(TeamManager winner)
    {
        //prevent any players from moving shit around after the game ends.
        gameComplete = true;
        foreach (TeamManager t in teams)
        {
            if(t != null)
            {
                t.EndTurn();
            }
        }

        
        if (winner != null)
        {
            //display the winning team's victory

            if (currentTurnText != null)
            {
                currentTurnText.text = winner.teamName + " has won!";
                currentTurnText.color = teams[currentTurn].teamColor;
            }
        }
        else
        {
            //draw, everyone loses

            if (currentTurnText != null)
            {
                currentTurnText.text = "Draw, everyone loses!";
                currentTurnText.color = Color.white;
            }
        }
    }




    public void DefaultActionButton()
    {
        if(teams[currentTurn].teamPointer != null)
        {
            teams[currentTurn].teamPointer.SelectDefaultAction();
        }
    }
    public void JumpActionButton()
    {
        if (teams[currentTurn].teamPointer != null)
        {
            teams[currentTurn].teamPointer.SelectJumpAction();
        }
    }
    public void Action1Button()
    {
        if (teams[currentTurn].teamPointer != null)
        {
            teams[currentTurn].teamPointer.SelectFirstAction();
        }
    }
    public void Action2Button()
    {
        if (teams[currentTurn].teamPointer != null)
        {
            teams[currentTurn].teamPointer.SelectSecondAction();
        }
    }
}
