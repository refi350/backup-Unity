using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZenvaVR;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public ObjectPool enemyPool;
    public Player player;
    public GameObject gameCamera;
    public GameObject fuelPrefab;
    public Canvas canvasLayer;
    public Slider sliderController;
    public float enemySpawnInterval;
    public float horizontalLimit = 2.8f;
    public float fuelDecreaseSpeed = 5f;
    public float fuelSpawnInterval = 9f;
    //public int lives = 3;
    public Text scoreText;
    public Text fuelText;

    //private GameObject[] livesGameObject;
    private float enemySpawnTimer;
    private int score = 0;
    private float fuel = 100f;
    private float fuelSpawnTimer;
    private float restartTimer = 3f;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawnTimer = enemySpawnInterval;
        player.OnFuel += OnFuel;
        fuelSpawnTimer = Random.Range(0f, fuelSpawnInterval);

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            enemySpawnTimer -= Time.deltaTime;
            if (enemySpawnTimer <= 0)
            {
                enemySpawnTimer = enemySpawnInterval;

                GameObject enemyInstance = enemyPool.GetObj();
                enemyInstance.transform.SetParent(transform);
                enemyInstance.transform.position = new Vector2(Random.Range(-horizontalLimit, horizontalLimit), player.transform.position.y + (Screen.height / 100f));

                enemyInstance.GetComponent<Enemy>().OnKill += OnEnemyKill;
            }

            foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
            {
                if (gameCamera.transform.position.y - enemy.transform.position.y > Screen.height / 100f)
                {
                    enemy.gameObject.SetActive(false);
                }
            }
            fuelSpawnTimer -= Time.deltaTime;
            if(fuelSpawnTimer <= 0)
            {
                fuelSpawnTimer = fuelSpawnInterval;

                GameObject fuelInstance = Instantiate(fuelPrefab);
                fuelInstance.transform.SetParent(transform);
                fuelInstance.transform.position = new Vector2(Random.Range(-horizontalLimit, horizontalLimit), player.transform.position.y + (Screen.height / 100f));
            }

            fuel -= Time.deltaTime * fuelDecreaseSpeed;
            sliderController.value = fuel;
            //fuelText.text = "Fuel: " + (int)fuel;
            if(fuel <= 0)
            {
                //fuelText.text = "Fuel: 0";
                Destroy(player.gameObject);
            }
        }
        else
        {
            restartTimer -= Time.deltaTime;
            if (restartTimer <= 0f)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

    void OnEnemyKill()
    {       
        score += 10;
        scoreText.text = "Score: " + score;
    }

    void OnFuel()
    {
        fuel = 100f;
    }
}
