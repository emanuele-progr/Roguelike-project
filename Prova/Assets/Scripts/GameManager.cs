using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public float turnDelay = 0.4f;
    public float levelStartDelay = 2f;
    public BoardManager boardScript;
    public static GameManager instance = null;
    public int playerHpPoints = 100;
    public int playerMaxHpPoints = 100;
    public Player player;
    public List<Sprite> lootPool;
    public int playerSpeed = 2;
    public bool maxHpBuffed = false;
    public bool speedBuffed = false;
    public int playerFoodPoints = 0;
    [HideInInspector] public bool playersTurn = true;
    public Text hpText;
    public Text speedText;
    public List<Sprite> inventoryIcons;
    public List<int> position;
    public Sprite hpPotionSprite;
    public Sprite speedPotionSprite;
    public List<Enemy> enemies;
    public Transform pfTextPopUp;
    public AudioClip drinkPotion;
    public int x_max;
    public int y_max;
    public GameObject gift;
    public GameObject bombGift;


    public int level = 1;
    private Text levelText;
    private GameObject levelImage;
    private Wall wall;
    private bool enemiesMoving;
    private bool doingSetup;
    private Image inventorySlot;
    private DialogueTrigger trigger;
    private bool firstTurn = true;
    private bool firstLevel = true;
    private Button button;
    private LevelLoader loader;
    private int starvingCounter = 0;
    private int firstTimeSpawn = 7;

    private Button retryButton;
    private Button exitButton;


    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        x_max = boardScript.rows - 1;
        y_max = boardScript.columns - 1;
        position = new List<int>();
    
        InitGame();
    }

/*
    //This is called each time a scene is loaded.
    void OnLevelWasLoaded(int index)
    {
        if (firstLevel)
        {
            firstLevel = false;
            return;
        }


        //Add one to our level number.
        level++;
        //Call InitGame to initialize our level.
        InitGame();
    }
*/

       //This is called each time a scene is loaded.
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (firstLevel)
        {
            firstLevel = false;
            return;
        }
        //Add one to our level number.
        level++;
        //Call InitGame to initialize our level.
        InitGame();
    }

    void OnEnable()
    {
        //Tell our ‘OnLevelFinishedLoading’ function to 
        //start listening for a scene change event as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnDisable()
    {
        //Tell our ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled. 
        //Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
     }

    public bool AllStarving()
    {
        if (enemies.Count == 0)
            return false;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].isStarving)
                return false;


        }

        if (player.isStarving)
            return true;
        else
            return false;
    }

    bool CheckPosition(Vector3 position)
    {
        GameObject[] list1 = GameObject.FindGameObjectsWithTag("Food");
        for (int i = 0; i < list1.Length; i++) {
            if (list1[i].transform.position == position)
                return false;
                    }

        GameObject[] list2 = GameObject.FindGameObjectsWithTag("Chest");
        for (int i = 0; i < list2.Length; i++)
        {
            if (list2[i].transform.position == position)
                return false;
        }
        GameObject[] list3 = GameObject.FindGameObjectsWithTag("Wall");
        for (int i = 0; i < list3.Length; i++)
        {
            if (list3[i].transform.position == position)
                return false;
        }
        GameObject[] list4 = GameObject.FindGameObjectsWithTag("Bomb");
        for (int i = 0; i < list4.Length; i++)
        {
            if (list4[i].transform.position == position)
                return false;
        }

        return true;
    }
    public void ThrowGift(float x, float y, int random)
    {
        GameObject ToInstantiate;
        if (random > 0)
        {
            ToInstantiate = bombGift;
        }
        else
        {
            ToInstantiate = gift;
        }


        float yPos = y;
        float xPos = x - 2;
        //int tryPos = boardScript.ReturnIndexPosition(xPos, yPos);
        Vector3 spawnPos = new Vector3(xPos, yPos, 0f);
        
        if (!CheckPosition(spawnPos))
        {
            spawnPos = new Vector3(xPos + 1, yPos, 0f);
            if (!CheckPosition(spawnPos))
            {
                spawnPos = new Vector3(xPos + 2, yPos, 0f);
                StartCoroutine(InstatiateGift(ToInstantiate, spawnPos));
            }

            else
            {
                StartCoroutine(InstatiateGift(ToInstantiate, spawnPos));
            }

        }
        else
        {
            StartCoroutine(InstatiateGift(ToInstantiate, spawnPos));
            
        }
    }

    IEnumerator InstatiateGift(GameObject ToInstantiate, Vector3 position)
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(ToInstantiate, position, Quaternion.identity);
    }
    public void GameOver()
    {
        retryButton.enabled = true;
        exitButton.enabled = true;
        retryButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().enabled = true;
        exitButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().enabled = true;
        levelText.fontSize = 15;
        levelText.text = "After " + level + " stages, you died and lost the starving game";
        levelImage.SetActive(true);
        inventoryIcons.Clear();
        //enabled = false;
        level = 0;
       
    }

    void InitGame()
    {
        doingSetup = true;
        loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        speedText = GameObject.Find("SpeedText").GetComponent<Text>();
        levelText.text = "Stage " + level;
        levelImage.SetActive(true);
        if (GameObject.Find("RetryButton").GetComponent<Button>())
            retryButton = GameObject.Find("RetryButton").GetComponent<Button>();
        if (GameObject.Find("ExitButton").GetComponent<Button>())
            exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        Invoke("HideLevelImage", levelStartDelay);
        //loader.LoadNextScene(0);
        enemies.Clear();
        boardScript.SetupScene(level);
        firstTimeSpawn = 7;
        //Debug.Log(inventoryIcons.Count);
        for (int i = 1; i < inventoryIcons.Count + 1; i++)
        {
            inventorySlot = GameObject.Find("Icon" + position[i-1]).GetComponent<Image>();
            if(GameObject.Find("Icon" + position[i - 1]).GetComponent<Button>())
            {
                button = GameObject.Find("Icon" + position[i - 1]).GetComponent<Button>();
                button.enabled = true;
            }
                
            inventorySlot.enabled = true;
            inventorySlot.sprite = inventoryIcons[i - 1];
            if (position[i - 1] == 3)
                inventorySlot.sprite = hpPotionSprite;
            else if (position[i - 1] == 4)
                inventorySlot.sprite = speedPotionSprite;

        }
        
        if(GameObject.Find("Wall4") != null)
        {
            wall = GameObject.Find("Wall4").GetComponent<Wall>();
            //inventoryIcons = wall.lootPool;
        }

        if(level == 2)
        {
            SoundManager.instance.GetComponent<DialogueTrigger>().TriggerDialogue();
        }


    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (AllStarving() && starvingCounter > firstTimeSpawn)
        {
            boardScript.InstatiateRandomFood();
            starvingCounter = 0;
            firstTimeSpawn = 20;
        }

            //instantiate a food after a while.

        if (enemies.Count == 0)
        {
            player.isSurvived = true;
            
        }
        speedText.text = player.playerSpeed.ToString();
        bool moving = player.IsMoving();
        if (moving)
            playersTurn = false;
        if (playersTurn || enemiesMoving || doingSetup )
            return;

        if (firstTurn)
        {
            trigger = Camera.main.GetComponent<DialogueTrigger>();
            trigger.TriggerDialogue();
            firstTurn = false;
        }
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
    public void RemoveEnemyFromList(Enemy script)
    {
        
        enemies.Remove(script);
        //Debug.Log(enemies.Count);
        
    }
    public void AddSpriteToList(Sprite inventoryIcon)
    {
        inventoryIcons.Add(inventoryIcon);
    }
   public void SetPosition(int index)
    {
        position.Add(index);
    }
    public void drinkHpPotion()
    {
        SoundManager.instance.PlaySingle(drinkPotion);
        int oldHp = player.hp;
        player.hp += 100;
        if (player.hp >= player.maxHp)
        {
            
            player.hp = player.maxHp;
        }
        TextPopUp.Create(player.transform.position, player.hp - oldHp, false);
        position.Remove(3);
        
    }

    public void drinkSpeedPotion()
    {
        SoundManager.instance.PlaySingle(drinkPotion);
        player.AddSpeedBuff();
        position.Remove(4);
        TextPopUp.Create(player.transform.position, 99, false);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].setSpeedDifference();

        }

    }

    public void setPlayerSpeed(int bonus)
    {
        player.setSpeed(bonus);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].setSpeedDifference();

        }
        
    }



    IEnumerator MoveEnemies()
    {

        enemiesMoving = true;
        float soloPlayerDelay = 0.3f;
        if(enemies.Count > 0)
            yield return new WaitForSeconds(turnDelay);
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].hp <= 0)
                enemies.Remove(enemies[i]);

            
        }

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(soloPlayerDelay);
        }

        for( int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        bool moving = player.IsMoving();
        if (moving)
            playersTurn = false;
        else
        {
            playersTurn = true;

            
        }
        if (AllStarving())
        {
            starvingCounter++;
        }
        else
            starvingCounter = 0;
        enemiesMoving = false;
    }
}
