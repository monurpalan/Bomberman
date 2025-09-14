using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Management")]
    [SerializeField] private GameObject[] players;

    public void CheckWinState()
    {
        int aliveCount = CountAlivePlayers();

        if (aliveCount <= 1)
        {
            Invoke(nameof(NewRound), 3f);
        }
    }

    private int CountAlivePlayers()
    {
        int aliveCount = 0;

        foreach (var player in players)
        {
            if (player.activeSelf)
                aliveCount++;
        }

        return aliveCount;
    }

    private void NewRound()
    {
        Debug.Log("New Round");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}