using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;

    public void CheckWinState()
    {
        int aliveCount = 0;
        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                aliveCount++;
            }
        }

        // 남은 플레이어가 1보다 작거나 같으면 새로운 게임을 시작함.
        if (aliveCount <= 1)
        {
            Invoke(nameof(NewRound), 3f);
        }
    }

    // 새로운 게임을 시작함.
    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
