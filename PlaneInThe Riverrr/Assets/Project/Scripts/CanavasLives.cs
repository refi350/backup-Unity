using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanavasLives : MonoBehaviour
{
    public Canvas canvasLayer;
    public GameObject livesPrefab;
    public float lives = 3;


    private GameObject[] livesGameObject;
    private RectTransform positionPlacer;


    // Start is called before the first frame update
    void Start()
    {
        positionPlacer = canvasLayer.GetComponent<RectTransform>();
        for (int i = 0; i < lives; i++)
        {
            livesGameObject[i] = Instantiate(livesPrefab,transform);
            livesGameObject[i].GetComponent<RectTransform>().transform.position = new Vector2(-25, 25+50*i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
