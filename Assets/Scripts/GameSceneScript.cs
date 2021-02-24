using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneScript : MonoBehaviour
{
    [SerializeField]
    GameController gc;
    // Start is called before the first frame update

    void Start()
    {
        gc.StartGame();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
