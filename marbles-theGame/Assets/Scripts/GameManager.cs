using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SimpleDragNewInput[] players;
    public int currentPlayerIndex = 0;

    public GamePhase phase = GamePhase.HoleShot;

    private void Awake()
    {
        Instance = this;
    }

    public SimpleDragNewInput CurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    public void OnShotFinished(bool enteredHole, bool hitEnemy)
    {
        Debug.Log("Shot Finished. Hole: " + enteredHole + " | Hit: " + hitEnemy);

        if (phase == GamePhase.HoleShot)
        {
            if (enteredHole)
            {
                // Player stays and gets attack shot
                phase = GamePhase.AttackShot;
                Debug.Log("Entering ATTACK SHOT");
            }
            else
            {
                SwitchTurn();
            }
        }
        else if (phase == GamePhase.AttackShot)
        {
            if (hitEnemy)
            {
                // GAME OVERs
                phase = GamePhase.GameOver;
                Debug.Log("PLAYER " + (currentPlayerIndex + 1) + " WINS!!!");
            }
            else
            {
                // Missed attack, switch turn
                SwitchTurn();
                phase = GamePhase.HoleShot;
            }
        }
    }


    public void SwitchTurn()
    {
        currentPlayerIndex = (currentPlayerIndex == 0) ? 1 : 0;
        phase = GamePhase.HoleShot;
    }
}
