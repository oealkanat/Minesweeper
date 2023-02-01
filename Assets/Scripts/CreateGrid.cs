using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public GameObject cube;
    public GameObject[,] cubeGrid;
    public TMP_Text mineValue;
    public int gridSize;
    public int mineAmount;
    public int flaggedBox;
    public int flaggedAccurate;
    public int openBoxAmount;
    

    void Start()
    {
        
        cubeGrid = new GameObject[gridSize, gridSize];
        if (mineAmount == 0)
        {
            switch (gridSize)
            {
                case int n when (n >= 10 && n < 14): //%10
                    mineAmount = (gridSize * gridSize) / 10;
                    break;
                case int n when (n >= 14 && n <= 16): //%15
                    mineAmount = ((gridSize * gridSize) / 100) * 16;
                    break;
                case int n when (n > 16 && n <= 25): //%25
                    mineAmount = ((gridSize * gridSize) / 100) * 27;
                    break;
            }
        }
        

        mineValue.text = mineAmount.ToString();

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Vector3 cubePos = new Vector3(-(gridSize+(gridSize-1)*0.1f)/2 + j * 1.1f, 0,(gridSize + (gridSize - 1) * 0.1f) / 2 + i * -1.1f);
                cubeGrid[i, j] = Instantiate(cube, cubePos, transform.rotation, transform);
                CubeAttributes cubeAtr = cubeGrid[i, j].GetComponent<CubeAttributes>();
                cubeAtr.cubeIndX = i;
                cubeAtr.cubeIndY = j;
            }
        }

        for (int i=0; i<mineAmount; i++)
        {
            int xPos = UnityEngine.Random.Range(0, gridSize);
            int yPos = UnityEngine.Random.Range(0, gridSize);
            CubeAttributes cubeAtr = cubeGrid[xPos, yPos].GetComponent<CubeAttributes>();

            if (cubeAtr.attributeValue != 9)
            {
                cubeAtr.attributeValue = 9;
            }
            else
            {
                while (cubeAtr.attributeValue != 9)
                {
                    xPos = UnityEngine.Random.Range(0, gridSize);
                    yPos = UnityEngine.Random.Range(0, gridSize);
                    cubeAtr = cubeGrid[xPos, yPos].GetComponent<CubeAttributes>();
                }
                cubeAtr.attributeValue = 9;
            }
            
        }

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                CubeAttributes cubeAtr = cubeGrid[i, j].GetComponent<CubeAttributes>();
                if (cubeAtr.attributeValue == 9)
                {
                    for (int atrX = -1; atrX <= 1; atrX++)
                    {
                        for (int atrY = -1; atrY <= 1; atrY++)
                        {
                            if (!(atrX == atrY && atrX == 0) && !(i + atrX < 0) && !(j + atrY < 0) && !(i + atrX > gridSize - 1) && !(j + atrY > gridSize - 1))
                            {
                                CubeAttributes gridAtr = cubeGrid[i + atrX, j + atrY].GetComponent<CubeAttributes>();
                                if (gridAtr.attributeValue != 9)
                                {
                                    gridAtr.attributeValue++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void Update()
    {
        mineValue.text = (mineAmount - flaggedBox).ToString();
    }

    public void openBox(int chosenX, int chosenY)
    {
        CubeAttributes chosenBox = cubeGrid[chosenX, chosenY].GetComponent<CubeAttributes>();

        if (!chosenBox.isOpen)
        {
            if (chosenBox.attributeValue != 9)
            {
                chosenBox.isOpen = true;

                if (chosenBox.attributeValue == 0)
                {
                    for (int atrX = -1; atrX <= 1; atrX++)
                    {
                        for (int atrY = -1; atrY <= 1; atrY++)
                        {
                            if (!(atrX == atrY && atrX == 0) && !(chosenX + atrX < 0) && !(chosenY + atrY < 0) && !(chosenX + atrX > gridSize - 1) && !(chosenY + atrY > gridSize - 1))
                            {
                                CubeAttributes gridAtr = cubeGrid[chosenX + atrX, chosenY + atrY].GetComponent<CubeAttributes>();

                                if (!gridAtr.isOpen)
                                {
                                    openBox(chosenX + atrX, chosenY + atrY);
                                    testGrid();
                                }
                            }
                        }
                    }
                }
                openBoxAmount++;
            }
            else
            {
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        CubeAttributes lostBox = cubeGrid[i, j].GetComponent<CubeAttributes>();
                        lostBox.isOpen = true;
                    }
                }
                CameraScript gameCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
                gameCam.exploded();
                Debug.Log("EXPLODED!");
            }
        }
        
    }

    public void openBoxAtr(int chosenX, int chosenY)
    {
        CubeAttributes chosenBox = cubeGrid[chosenX, chosenY].GetComponent<CubeAttributes>();

        int flaggedInSurround = 0;
        int minesInSurround = chosenBox.attributeValue;
        for (int atrX = -1; atrX <= 1; atrX++)
        {
            for (int atrY = -1; atrY <= 1; atrY++)
            {
                if (!(atrX == atrY && atrX == 0) && !(chosenX + atrX < 0) && !(chosenY + atrY < 0) && !(chosenX + atrX >= gridSize) && !(chosenY + atrY >= gridSize))
                {
                    CubeAttributes surroundAtr = cubeGrid[chosenX + atrX, chosenY + atrY].GetComponent<CubeAttributes>();
                    if (surroundAtr.isFlagged)
                    {
                        flaggedInSurround++;
                    }
                }
            }
        }

        if (flaggedInSurround == minesInSurround)
        {
            for (int atrX = -1; atrX <= 1; atrX++)
            {
                for (int atrY = -1; atrY <= 1; atrY++)
                {
                    if (!(atrX == atrY && atrX == 0) && !(chosenX + atrX < 0) && !(chosenY + atrY < 0) && !(chosenX + atrX >= gridSize) && !(chosenY + atrY >= gridSize))
                    {
                        CubeAttributes surroundAtr = cubeGrid[chosenX + atrX, chosenY + atrY].GetComponent<CubeAttributes>();
                        if (!surroundAtr.isFlagged)
                        {
                            openBox(chosenX + atrX, chosenY + atrY);
                        }
                    }
                }
            }
        }
        
    }

    public void testGrid()
    {
        if (mineAmount == flaggedAccurate && openBoxAmount == (gridSize*gridSize) - mineAmount)
        {
            CameraScript gameCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
            Debug.Log("All mines are flagged!");
            gameCam.allFlagged();
        }
    }
}
