using UnityEngine;
using UnityEngine.SceneManagement;

public class TurretController : MonoBehaviour
{
    public GameObject[] Turrets = new GameObject[11];
    private int TurretsRemaining = 11;
    [SerializeField] private bool GameWon = false;

    void Update()
    {
        if (TurretsRemaining <= 0 && !GameWon)
        {
            GameWon = true;
            SceneManager.LoadScene("TitleMenu");
        }
    }

    public void TurretDestroyed()
    {
        TurretsRemaining--;
    }
}

// Manages all the turrets, so that if they all die, player is returned to the menu.
