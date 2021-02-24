using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }

    private int score;
    public int Score
    {
        get { return score;  }
        set { score = value; }
    }

    [SerializeField]
    Text textScore;

    int pointInLine = 100;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    float speedDificulty = 1f;

    


    Tetromino tetrominoInControll;
    [SerializeField]
    GameObject[] tetrominos;

    
    float delayInputTime = 0.1f;
    float inputTime = 0f;

    [SerializeField]
    int stageBottomY;
    [SerializeField]
    int stageStartX;
    [SerializeField]
    int stageEndsX;

    [SerializeField]
    int lineLenght;

    List<int> linesToClean = new List<int>();

    AudioSource musicTheme;


    void Start()
    {
        musicTheme = GetComponent<AudioSource>();
        CreateNewTetromino();
        ChangeScore();
    }

    // Update is called once per frame
    void Update()
    {
        ControllPeace();       
    }

    void ChangeScore(int newPoints)
    {
        Score += newPoints;
        textScore.text = "Score: " + Score;
        speedDificulty -= 0.05f;
    }
    void ChangeScore()
    {
        Score = 0;
        textScore.text = "Score: " + Score;
        speedDificulty = 1f;
    }


    public void NewCycle()
    {
        ClearTetromino();
        StartCoroutine(NewCycleStep("verify"));
    }

    void StartNewCycle()
    {
        Debug.Log(IsGameOver());
        if (IsGameOver())
        {
            StartCoroutine(GameOver());
        }
        else
        {
            CreateNewTetromino();
        }
    }

    bool IsGameOver()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit,  1))
        {
            if (hit.transform.gameObject.GetComponent<Peace>().IsTetrominoPart != true) {
                return true;
            }            
        }
        return false;
        
    }

    void CreateNewTetromino()
    {
        int newPeace = Random.Range(0, 6);
        tetrominoInControll = Instantiate(tetrominos[newPeace],transform.position, Quaternion.identity).GetComponent<Tetromino>();
        tetrominoInControll.Speed = speedDificulty;
    }

    void ClearTetromino()
    {
        tetrominoInControll = null;        
    }

    void VerifyLines()
    {
        int numLineComplete = 0;
        linesToClean.Clear();
        for (int i = stageBottomY; i < transform.position.y; i++)
        {            
            Ray ray = new Ray(new Vector3(stageStartX, i, 0f), Vector3.right);
            RaycastHit[] peacesList = Physics.RaycastAll(ray,50f,11);          
            

            if (peacesList.Length == lineLenght)
            {
                ChangeScore(pointInLine);
                numLineComplete++;
                linesToClean.Add(i);
                StartCoroutine(CleanLine(peacesList, i));
            }
        }

        if (numLineComplete==0)
        {
            StartCoroutine(NewCycleStep("create"));
        }
    }

    


    void ControllPeace()
    {
        inputTime += Time.deltaTime;

        if (inputTime > delayInputTime && tetrominoInControll != null)
        {

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                inputTime = 0f;
                tetrominoInControll.Controlls("left");
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                inputTime = 0f;
                tetrominoInControll.Controlls("right");
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                inputTime = 0f;
                tetrominoInControll.Controlls("rotate");
            }

            if (Input.GetKey(KeyCode.DownArrow) && tetrominoInControll.IsMoving)
            {
                inputTime = 0f;
                tetrominoInControll.Controlls("down");
            }
        }


    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        musicTheme.Stop();
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("GameOver");
    }


    IEnumerator CleanLine(RaycastHit[] raycastHits, int line)
    {        
        Debug.Log("Clear Line " + line);
        yield return new WaitForSeconds(0.1f);
        foreach (RaycastHit item in raycastHits)
        {            
            Destroy(item.transform.gameObject);
        }
       
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DownLines());
    }

    IEnumerator DownLines()
    {
        int line = linesToClean[linesToClean.Count-1];
        yield return new WaitForSeconds(0.1f);
        for (int i = line; i < transform.position.y; i++)
        {
            yield return new WaitForSeconds(0.2f);

            Ray ray = new Ray(new Vector3(stageStartX, i, 0f), Vector3.right);
            RaycastHit[] peacesList = Physics.RaycastAll(ray, 50f, 11);

           
            foreach (RaycastHit item in peacesList)
            {
                item.transform.gameObject.GetComponent<Peace>().Movement(1, "down");
            }
        }
        
        linesToClean.RemoveAt(linesToClean.Count - 1);
        if (linesToClean.Count>0)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(DownLines());
        }
        else
        {
            StartCoroutine(NewCycleStep("create"));
        }        
    }
    


    IEnumerator NewCycleStep(string step)
    {
        yield return new WaitForSeconds(0.2f);
        switch (step)
        {
            case "verify":
                VerifyLines();
                break;

            case "down":
                VerifyLines();
                break;


            case "create":
                StartNewCycle();
                break;

            default:
                Debug.Log("Wrong Step");
                break;
        }
        
        

    }








    

}
