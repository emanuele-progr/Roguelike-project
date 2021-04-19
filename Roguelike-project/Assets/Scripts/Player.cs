using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerSnack = 10;
    public int pointsPerFood = 50;
    public Text hpText;
    public Text foodText;
    public int playerSpeed = 2;
    public HealthBar healthBar;
    public FoodBar foodBar;
    public bool isSurvived = false;
    public int speedBuff = 0;
    public int enemyDamage = 10;
    public bool boost = false;
    public bool stunned = false;
    public bool isMobile = false;
    public LevelLoader loader;

    public AudioClip hitSound;
    public AudioClip attackSound1;
    public AudioClip attackSound2;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip gameOverSound;
    public AudioClip openDoorSound;
    public AudioClip altar;


    public float restartLevelDelay = 1f;
    private Animator animator;
    private Dialogue dialogue;
    private bool firstFire = true;
    public int originalSpeed = 2;
    public int stunCounter = 4;
    private bool coroutineCalled = false;
    public bool isStarving = true;
    public int foodPoints = 0;
    public int hp;
    public int maxHp = 100;
    public int maxFp = 100;
    private Vector3 touchOrigin;
    private Joystick joy;

#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
        
    static extern bool IsMobile();
        
#endif
    // Start is called before the first frame update
    protected override void Start()
    {
        CheckIfMobile();
        loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        hpText = GameObject.Find("HpText").GetComponent<Text>();
        healthBar = GameObject.Find("Healthbar").GetComponent<HealthBar>();
        foodBar = GameObject.Find("Foodbar").GetComponent<FoodBar>();
        joy = Camera.main.GetComponent<Joystick>();
        playerSpeed = GameManager.instance.playerSpeed;
        originalSpeed = playerSpeed;
        animator = GetComponent<Animator>();
        hp = GameManager.instance.playerHpPoints;
        maxHp = GameManager.instance.playerMaxHpPoints;
        foodPoints = GameManager.instance.playerFoodPoints;
        enemyDamage = GameManager.instance.playerDmg;
        
        if (foodPoints > 0)
            isStarving = false;
        healthBar.SetHealth(hp);
        healthBar.SetMaxHealth(maxHp);
        foodBar.SetFood(foodPoints);


        GameManager.instance.player = this;

        hpText.text = "HP : " + hp;
        foodText.text = "Food points : " + foodPoints;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerMaxHpPoints = maxHp;
        GameManager.instance.playerHpPoints = hp;
        GameManager.instance.playerSpeed = originalSpeed;
        GameManager.instance.playerFoodPoints = foodPoints;
        
    }

    public void drinkSpeedPotion()
    {
        
        GameManager.instance.drinkSpeedPotion();
        GameManager.instance.playersTurn = false;
    }

    public void drinkHpPotion()
    {
        
        GameManager.instance.drinkHpPotion();
        GameManager.instance.playersTurn = false;
    }
    public void setSpeed(int i)
    {
        playerSpeed += i;
        originalSpeed += i;
        TextPopUp.Create(transform.position, 101, false);

    }

    public void setDmg(int dmg)
    {
        hp = maxHp;
        foodPoints = maxFp;
        foodText.text = "Food points : " + foodPoints;

        enemyDamage = dmg;
        GameManager.instance.setPlayerDmg(dmg);
        if (firstFire)
        {
            TextPopUp.Create(transform.position, 103, false);
            firstFire = false;
            SoundManager.instance.PlaySingleShot(altar);
        }
    }

    public bool MoveFromTrap(int xDir, int yDir)
    {
        RaycastHit2D hit;
        if (TryMove(xDir, yDir, out hit))
        {
            base.AttemptMove<Wall>(xDir, yDir);
            return true;
        }
        else
            return false;
        
        
        
 
    }
    public bool IsMoving()
    {
        bool isMoving = base.isMoving;
        //Debug.Log("move :" + base.isMoving.ToString());
        return isMoving;
    }

    public void AddSpeedBuff()
    {
        speedBuff = 5;
        boost = true;
        playerSpeed += 5;
    }
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        
        
        if (boost)
        {
            speedBuff--;
            if(speedBuff < 0)
            {
                playerSpeed -= 5;
                boost = false;
            }
                
        }

            
        if (isStarving)
        {
            playerSpeed = 2 * originalSpeed;

            //hp--;
            hp -= 2;
            if (!coroutineCalled)
            {
                StartCoroutine("colorRed");
            }
        }
        else
        {
            playerSpeed = originalSpeed;
            foodPoints-= 2;
            if (foodPoints < 0)
            {
                isStarving = true;
                foodPoints = 0;
            }

            hp++;
            if (hp >= maxHp)
                hp = maxHp;
        }
        foodText.text = "Food points : " + foodPoints;
        hpText.text = "HP : " + hp;

        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        if (Move (xDir, yDir, out hit))
        {
            animator.SetTrigger("playerMove");
            
            SoundManager.instance.RandomizeSfxShot(moveSound1, moveSound2);
            
        }
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
    }

    private void CheckIfGameOver()
    {
        if (hp <= 0)
        {
            AnalyticsResult result = Analytics.CustomEvent("playerDeath", new Dictionary<string, object>
        {
            { "level", GameManager.instance.level },
            { "speedPotionused", GameManager.instance.speedPotionUsed },
            { "hpPotioUused", GameManager.instance.healthPotionUsed },
            { "chestOpened", GameManager.instance.chestOpened },
            { "pushTrapUsed", GameManager.instance.pushTrapUsed }
        });

            
            Debug.Log("result" + result);
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
            GameManager.instance.hasHealthPotion = false;
            GameManager.instance.hasSpeedPotion = false;
            GameManager.instance.maxHpBuffed = false;
            GameManager.instance.speedBuffed = false;
            GameManager.instance.playerMaxHpPoints = 100;
            GameManager.instance.playerSpeed = 2;
            GameManager.instance.playerDmg = 10;
            //Invoke("Restart", restartLevelDelay);
            hp = 100;
            maxHp = 100;
            enabled = false; 

        }
        
    }

    // Update is called once per frame
    void Update()
    {

        healthBar.SetMaxHealth(maxHp);
        healthBar.SetHealth(hp);
        hpText.text = "HP : " + hp;
        foodText.text = "Food points : " + foodPoints;
        foodBar.SetFood(foodPoints);
        if (!GameManager.instance.playersTurn) return;

        
        int horizontal = 0;
        int vertical = 0;
        if (stunned)
        {
            if (isStarving)
            {
                //hp--;
                hp -= 2;
                if (!coroutineCalled)
                {
                    StartCoroutine("colorRed");
                }
            }
            //GameManager.instance.playersTurn = false;
            foodPoints -= 2;
            hp++;
            if(foodPoints < 0)
            {
                isStarving = true;
                foodPoints = 0;
            }
            stunCounter -= 1;
            if (stunCounter <= 0)
            {
                stunCounter = 4;
                stunned = false;

            }
            
            GameManager.instance.playersTurn = false;
            return;

        }







        //horizontal = (int)Input.GetAxisRaw("Horizontal");
        //vertical = (int)Input.GetAxisRaw("Vertical");

        //if (horizontal != 0)
        //vertical = 0;
        //Check if we are running either in the Unity editor or in a standalone build.
#if ((UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL) && (!isMobile))

        if(!isMobile){
            joy.enabled = false;
            //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
            horizontal = (int) (Input.GetAxisRaw ("Horizontal"));

            //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
            vertical = (int) (Input.GetAxisRaw ("Vertical"));

            //Check if moving horizontally, if so set vertical to zero.
            if(horizontal != 0)
            {
                vertical = 0;
            }
        }
        else{
                if (joy.touchStart && joy.overTolerance){
        Vector2 movement = joy.GetInput();
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        //If x is greater than zero, set horizontal to 1, otherwise set it to -1
            horizontal = movement.x > 0 ? 1 : -1;
         else
         //If y is greater than zero, set horizontal to 1, otherwise set it to -1
            vertical = movement.y > 0 ? 1 : -1;

        joy.overTolerance = false;

        }
        }
            //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE || isMobile


        if (joy.touchStart && joy.overTolerance){
        Vector2 movement = joy.GetInput();
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        //If x is greater than zero, set horizontal to 1, otherwise set it to -1
            horizontal = movement.x > 0 ? 1 : -1;
         else
         //If y is greater than zero, set horizontal to 1, otherwise set it to -1
            vertical = movement.y > 0 ? 1 : -1;

        joy.overTolerance = false;

        }
    }
        /*
            //Check if Input has registered more than zero touches
            if (Input.touchCount > 0)
            {
                //Store the first touch detected.
                Touch myTouch = Input.touches[0];

                //Check if the phase of that touch equals Began
                if (myTouch.phase == TouchPhase.Began)
                {
                    //If so, set touchOrigin to the position of that touch
                    touchOrigin = myTouch.position;
                }

                //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
                else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                    //Set touchEnd to equal the position of this touch
                    Vector2 touchEnd = myTouch.position;

                    //Calculate the difference between the beginning and end of the touch on the x axis.
                    float x = touchEnd.x - touchOrigin.x;

                    //Calculate the difference between the beginning and end of the touch on the y axis.
                    float y = touchEnd.y - touchOrigin.y;

                    //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
                    touchOrigin.x = -1;

                    //Check if the difference along the x axis is greater than the difference along the y axis.
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                        //If x is greater than zero, set horizontal to 1, otherwise set it to -1
                        horizontal = x > 0 ? 1 : -1;
                    else
                        //If y is greater than zero, set horizontal to 1, otherwise set it to -1
                        vertical = y > 0 ? 1 : -1;
                }
            }
        */



#endif //End of mobile platform dependendent compilation section started above with #elif
        if (horizontal != 0 || vertical != 0)
        {
            base.AttemptMove<Wall>(horizontal, vertical);
            if (base.notMoving)
                AttemptMove<Enemy>(horizontal, vertical);
            else
            {

                AttemptMove<Wall>(horizontal, vertical);
            }
                

        }
            
    }

    protected override void OnCantMove <T> (T component)
    {
        
        if (component is Wall) {

            animator.SetTrigger("playerChop");
            SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);
            Wall hitWall = component as Wall;
            hitWall.DamageWall(wallDamage, "player");

        }
        else
        {
            //Debug.Log("Not a wall.");
            if (isStarving)
            {
                Enemy hitEnemy = component as Enemy;
                animator.SetTrigger("playerChop");
                SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);
                if ((hitEnemy.foodPoints - enemyDamage) < 0)
                    foodPoints += hitEnemy.foodPoints;
                else
                    foodPoints += enemyDamage;
                hitEnemy.LoseFood(enemyDamage);
                foodText.text = "Food points : " + foodPoints;
                if (foodPoints > 0)
                    isStarving = false;
                StartCoroutine("colorGreen");
            }
            else
            {
                dialogue = new Dialogue("Game Master", "Mhh, you have an aggressive attitude huh? relax my friend, you can only attack the opponent if you are starving.");

                DialogueManager.instance.StartDialogue(dialogue);
            }

        }


    }

    void CheckIfMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        isMobile = IsMobile();

#endif
    }

    private void Restart()
    {
        SceneManager.LoadScene("BoardScene");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit" && isSurvived)
        {
            //loader.LoadNextScene(0);
            SoundManager.instance.PlaySingle(openDoorSound);
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Exit" && !isSurvived)
        {
            dialogue = new Dialogue("Game Master", "Why do you wanna spoil the fun? the door remains locked until there is only one survivor left. Good luck my friend :) .");

            DialogueManager.instance.StartDialogue(dialogue);
        }

        else if (other.tag == "Food")
        {

            foodPoints += pointsPerFood;
            if (foodPoints > maxFp)
                foodPoints = maxFp;
            isStarving = false;
            foodText.text = " Food points : " + foodPoints;
            other.gameObject.SetActive(false);
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            StartCoroutine("colorGreen");
        }

        }

    public void stunText()
    {
        TextPopUp.Create(transform.position, 0);
    }


    public void LoseFood (int loss)
    {
        animator.SetTrigger("playerHit");
        Invoke("PlaySingleHit", 0.4f);
        int oldFoodPoints = foodPoints;
        foodPoints -= loss;
        if (foodPoints < 1) {

            if (foodPoints == 0)
            {
                TextPopUp.Create(transform.position, loss);
                Invoke("stunText", 1f);
            }
            else
            {
                TextPopUp.Create(transform.position, oldFoodPoints);
                Invoke("stunText", 1f);
            }
            foodPoints = 0;
            isStarving = true;
        }
        else
        {
            TextPopUp.Create(transform.position, loss);
            Invoke("stunText", 1f);
        }

        foodText.text = " Food points : " + foodPoints;
        stunned = true;

        
    }


    public void LoseHealth(int loss)
    {
        animator.SetTrigger("playerHit");
        Invoke("PlaySingleHit", 0.4f);
        hp -= loss;
        if (hp < 0)
            hp = 0;
        CheckIfGameOver();
        TextPopUp.Create(transform.position, loss);
        Invoke("stunText", 1f);
        

        hpText.text = " HP : " + hp;
        stunCounter = 6;
        stunned = true;

        
    }

    public void PlaySingleHit()
    {
        SoundManager.instance.PlaySingle(hitSound);
    }

    IEnumerator colorRed()
    {
        coroutineCalled = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        
        coroutineCalled = false;
    }

    IEnumerator colorGreen()
    {
        coroutineCalled = true;
        GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);

        coroutineCalled = false;
    }
}
