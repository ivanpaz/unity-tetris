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

   

    Peace[] peacesChilds;

    // Start is called before the first frame update
    void Start()
    {
        peacesChilds = GetComponentsInChildren<Peace>();
        PrepareChilds(true);
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {

        yield return new WaitForSeconds(speed);
        if (isMoving)
        {
            if (CanGoDir("down"))
            {
                Movement("down");
                StartCoroutine(GameLoop());
            }
            else
            {
                StopMotion();
            }
            
        }

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
        
        StartCoroutine(StopMoving());
    }

    IEnumerator StopMoving()
    {

        yield return new WaitForSeconds(0.5f);
        PrepareChilds(false);
        GameController.Instance.NewCycle();
        transform.DetachChildren();

        //Verificar pontos
        Destroy(gameObject);

    }



    public void Controlls(string command)
    {
        Movement(command);        
    }

    bool CanGoDir(string dir)
    {
        int canMove = 0;

        foreach (Peace peaceChild in peacesChilds)
        {

            switch (dir)
            {
                case "down":
                    canMove += peaceChild.CheckPlaceDown();
                    break;

                case "left":
                    canMove += peaceChild.CheckPlaceLeft();
                    break;

                case "right":
                    canMove += peaceChild.CheckPlaceRight();
                    break;

               
                default:
                    Debug.Log("Wrong");
                    break;
            }            
        }

        if (canMove>0)
        {
            return false;
        }

        return true;
    }

    public void Movement(string command)
    {
        switch (command)
        {
            case "down":
                if (CanGoDir(command))
                {
                    transform.position += Vector3.down;
                }
                
                break;

            case "left":               
                if (CanGoDir(command))
                {
                    //canMoveRight = true;
                    transform.position += Vector3.left;
                }
                
                break;

            case "right":
                if (CanGoDir(command))
                {
                    //canMoveLeft = true;
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
