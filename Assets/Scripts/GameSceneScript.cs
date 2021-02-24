using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneScript : MonoBehaviour
{
    [SerializeField]
    GameController gc;
    // Start is called before the first frame update

    void Start()
    {
        gc.StartGame();
    }
}
