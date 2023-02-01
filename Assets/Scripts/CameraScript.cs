using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    CreateGrid gridScript;
    public Camera gameCam;
    void Start()
    {
        gameCam.backgroundColor = new Color(0.1960f, 0.1960f, 0.1960f, 255f);

        gridScript = GameObject.FindGameObjectWithTag("gridField").GetComponent<CreateGrid>();

        Vector3 cameraPosition = new Vector3(0.7f, 25f, 0.6f);
        gameCam.orthographicSize = 21f;

        switch (gridScript.gridSize)
        {
            case int n when (n > 9 && n <= 12):
                gameCam.orthographicSize = 7f;
                break;
            case int n when (n > 12 && n <= 16):
                gameCam.orthographicSize = 10.2f;
                break;
            case int n when (n > 16 && n <= 25):
                gameCam.orthographicSize = 14.6f;
                break;
        }

        transform.position = cameraPosition;
    }

    void Update()
    {
        
    }

    public void exploded()
    {
        gameCam.backgroundColor = new Color(0.2745f, 0.1960f, 0.1960f, 1f);
    }

    public void allFlagged()
    {
        gameCam.backgroundColor = new Color(0.1960f, 0.2745f, 0.1960f, 1f);
    }
}
