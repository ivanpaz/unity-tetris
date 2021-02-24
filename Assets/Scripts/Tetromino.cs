using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float speed;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    Peace[] peaces;
    private bool isMoving = true;
    public bool IsMoving
    {
        get { return isMoving; }
    }

    private bool canMoveRight = true;
    public bool CanMoveRight
    {
        get { return canMoveRight; }
        set { canMoveRight = value; }
    }
    private bool canMoveLeft = true;
    public bool CanMoveLeft
    {
        get { return canMoveLeft; }
        set { canMoveLeft = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        PrepareChilds(true);
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {

        yield return new WaitForSeconds(speed);
        if (isMoving)
        {
            Movement("down");
            StartCoroutine(GameLoop());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    void PrepareChilds(bool partOfTetromino)
    {        
        peaces = GetComponentsInChildren<Peace>();
        foreach (Peace peace in peaces)
        {
            peace.IsTetrominoPart = partOfTetromino;
        }

    }

    public void StopMotion()
    {
        isMoving = false;
        PrepareChilds(false);
        StartCoroutine(StopMoving());
    }

    IEnumerator StopMoving()
    {

        yield return new WaitForSeconds(0.5f);
        
        GameController.Instance.NewCycle();
        transform.DetachChildren();

        //Verificar pontos
        Destroy(gameObject);

    }



    public void Controlls(string command)
    {
        Movement(command);        
    }

    public void Movement(string command)
    {
        switch (command)
        {
            case "down":
                transform.position += Vector3.down;
                break;

            case "left":
                if (CanMoveLeft)
                {
                    canMoveRight = true;
                    transform.position += Vector3.left;
                }
                
                break;

            case "right":
                if (CanMoveRight)
                {
                    canMoveLeft = true;
                    transform.position += Vector3.right;
                }
                
                break;

            case "rotate":
                transform.rotation *= Quaternion.Euler(0, 0, 90);
                break;


            default:
                Debug.Log("Wrong");
                break;
        }
    }


}
