using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class CubeAttributes : MonoBehaviour
{
    public int cubeIndX = 0;
    public int cubeIndY = 0;
    public bool isOpen = false;
    public bool isFlagged = false;
    public int attributeValue = 0;
    public Material[] closeMaterial = new Material[2];
    public Material[] atrMaterial = new Material[10];
    CreateGrid gridScript;


    //0: empty
    //1-8: values
    //9: mine
    void Start()
    {

    }

    
    void Update()
    {
        if (!isOpen)
        {
            if (isFlagged)
            {
                GetComponent<Renderer>().material = closeMaterial[1];
            }
            else
            {
                GetComponent<Renderer>().material = closeMaterial[0];
            }
        }

        else
        {
            GetComponent<Renderer>().material = atrMaterial[attributeValue];
        }
    }

    private void OnMouseOver()
    {
        gridScript = GameObject.FindGameObjectWithTag("gridField").GetComponent<CreateGrid>();
        if (Input.GetMouseButtonDown(0) && !isFlagged)
        {
            Debug.Log("Left Click!");
            if (!isOpen)
            {
                gridScript.openBox(cubeIndX, cubeIndY);
                gridScript.testGrid();
            }
            
            else if (attributeValue > 0 && isOpen)
            {
                gridScript.openBoxAtr(cubeIndX, cubeIndY);
                gridScript.testGrid();
            }
        }
        if (Input.GetMouseButtonDown(1) && !isOpen)
        {
            if (isFlagged)
            {
                isFlagged = !isFlagged;
                gridScript.flaggedBox--;
                if (attributeValue == 9)
                {
                    gridScript.flaggedAccurate--;
                }
            }
            else if(gridScript.flaggedBox != gridScript.mineAmount)
            {
                isFlagged = !isFlagged;
                gridScript.flaggedBox++;
                if (attributeValue == 9)
                {
                    gridScript.flaggedAccurate++;
                    gridScript.testGrid();
                }
            }
        }
    }
}
