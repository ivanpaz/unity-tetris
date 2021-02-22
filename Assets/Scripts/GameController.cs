using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }
     

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


    void Start()
    {
        CreateNewTetromino();
        
    }

    // Update is called once per frame
    void Update()
    {
        ControllPeace();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateNewTetromino();
        }
    }

    public void NewCycle()
    {
        ClearTetromino();
        StartCoroutine(NewCycleStep("verify"));
    }

    void CreateNewTetromino()
    {
        int newPeace = Random.Range(0, 6);
        tetrominoInControll = Instantiate(tetrominos[newPeace],transform.position, Quaternion.identity).GetComponent<Tetromino>();
    }

    void ClearTetromino()
    {
        tetrominoInControll = null;
    }

    void VerifyLines()
    {
        linesToClean.Clear();
        for (int i = stageBottomY; i < transform.position.y; i++)
        {
            
            Ray ray = new Ray(new Vector3(stageStartX, i, 0f), Vector3.right);
            RaycastHit[] peacesList = Physics.RaycastAll(ray,50f,11);          
            

            if (peacesList.Length == lineLenght)
            {
                linesToClean.Add(i);
                StartCoroutine(CleaLine(peacesList, i));
            }

        }
        
    }

   
    IEnumerator CleaLine(RaycastHit[] raycastHits, int line)
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

            Debug.Log("Down line " + i + " with " + peacesList.Length + " elements");
            foreach (RaycastHit item in peacesList)
            {
                item.transform.gameObject.GetComponent<Peace>().Movement(1, "down");
            }
        }
        
        linesToClean.RemoveAt(linesToClean.Count - 1);
        Debug.Log(linesToClean.Count + " xxxxxxxxxxxxxxxxx");
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


    IEnumerator OrganizeLine(int line)
    {
        yield return new WaitForSeconds(0.1f);
        Ray ray = new Ray(new Vector3(stageStartX, line, 0f), Vector3.right);
        RaycastHit[] peacesList = Physics.RaycastAll(ray, 50f, 11);

        if (peacesList.Length == 0)
        {

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
                CreateNewTetromino();
                break;

            default:
                Debug.Log("Wrong Step");
                break;
        }
        
        

    }








    void ControllPeace()
    {
        inputTime += Time.deltaTime;

        if (inputTime> delayInputTime && tetrominoInControll != null)
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

            if (Input.GetKey(KeyCode.DownArrow))
            {
                inputTime = 0f;
                tetrominoInControll.Controlls("down");
            }
        }

        
    }

}
