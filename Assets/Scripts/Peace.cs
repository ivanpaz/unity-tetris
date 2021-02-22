﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peace : MonoBehaviour
{
    
    bool isTetrominoPart = false;
    public bool IsTetrominoPart
    {
        get { return isTetrominoPart; }
        set { isTetrominoPart = value; }
    }

    [SerializeField]
    bool border = false;
    public bool isBorder
    {
        get { return border; }
    }

    private void Update()
    {
        if (IsTetrominoPart)
        {
            CheckPlaceDown();
            CheckPlaceLeft();
            CheckPlaceRight();
        }
        
    }


    public void Movement(int steps, string dir)
    {
        
        switch (dir)
        {
            case "down":
                transform.position += Vector3.down * steps;
                break;           

            default:
                Debug.Log("Wrong");
                break;
        }
    }   

    void CheckPlaceDown()
    {
        RaycastHit placeDown;
        if (Physics.Raycast(transform.position, -Vector3.up, out placeDown, 1))
        {
            if (isTetrominoPart && placeDown.transform.gameObject.GetComponent<Peace>().isTetrominoPart != true)
            {
                Debug.Log("Tem coisa");
                GetComponentInParent<Tetromino>().StopMotion();               
            }           
        }
    }

    void CheckPlaceRight()
    {
        RaycastHit placeDown;
        if (Physics.Raycast(transform.position, Vector3.right, out placeDown, 1))
        {
            if (isTetrominoPart && placeDown.transform.gameObject.GetComponent<Peace>().isTetrominoPart != true)
            {
                Debug.Log("Tem coisa");
                GetComponentInParent<Tetromino>().CanMoveRight = false;
            }
        }
    }
    void CheckPlaceLeft()
    {
        RaycastHit placeDown;
        if (Physics.Raycast(transform.position, -Vector3.right, out placeDown, 1))
        {
            if (isTetrominoPart && placeDown.transform.gameObject.GetComponent<Peace>().isTetrominoPart != true)
            {
                Debug.Log("Tem coisa");
                GetComponentInParent<Tetromino>().CanMoveLeft = false;
            }
        }
    }

    public void ClearParent()
    {

    }
}