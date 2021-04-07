using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MovingObject
{
    public int wallDamage = 1;
    public int playerDamage;
    public int pointsPerSnack = 10;
    public int pointsPerFood = 20;
    public int enemySpeed = 1;
    public int foodPoints = 0;
    public int hp = 100;
    public int maxHp = 100;
    public int maxFoodPoints = 100;
    public int originalSpeed;
    public bool speedPotion = false;
    public bool healthPotion = false;
    public bool isSanta = false;
    private Animator animator;
    private Transform target;
    private int speedDifference;
    private int residualDifference;
    public int giftCounter = 8;
    public int stunCounter = 4;
    public bool stunned = false;
    public AudioSource efxSource;
    public AudioClip drinkPotion;
    public bool isStarving = true;
    private bool coroutineCalled = false;
    //private bool isPlayer = false;
    private Vector3 startPos;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;
    public AudioClip hitSound;
    public AudioClip attackSound1;
    public AudioClip attackSound2;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip gameOverSound;

    // Start is called before the first frame update
    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpeed = enemySpeed;
        base.Start();
        speedDifference = GameManager.instance.player.playerSpeed - enemySpeed;
        //Debug.Log(speedDifference);
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

    protected override void AttemptMove <T> (int xDir, int yDir)
    {

        
        if (!isStarving)
        {
            enemySpeed = originalSpeed;
            foodPoints -= 2;
            if (foodPoints < 1)
                isStarving = true;
            hp++;
            if (hp >= maxHp)
                hp = maxHp;
        }
        else
        {
            enemySpeed = 2 * originalSpeed;
            hp-= 2;
            if (!coroutineCalled)
            {
                StartCoroutine("colorRed");
            }
            CheckIfDeath();
        }
        
        Debug.Log("speedDifference : " + speedDifference);
        if (speedDifference <= 0)
        {
            if (!base.notMoving)
            {
                base.AttemptMove<T>(xDir, yDir);
                animator.SetTrigger("enemyMove");
                
                
            }
            else
            {
                base.AttemptMove<T>(xDir, yDir);
            }
            RaycastHit2D hit;
            if (base.TryMove(xDir, yDir, out hit))
                RandomizeSfxShot(moveSound1, moveSound2);



            if (speedDifference < 0)
            {
                GameManager.instance.player.stunned = true;
                GameManager.instance.player.stunCounter = Math.Abs(speedDifference);
                speedDifference += 1;
                return;
            }
            
            speedDifference = GameManager.instance.player.playerSpeed - enemySpeed;
            CheckIfDeath();
            
        }
        else
        {
            speedDifference -= 1;
            return;

        }

    }

    public void MoveEnemy()
    {
        if (stunned)
        {
            if (isStarving)
            {
                hp -= 2;
                if (!coroutineCalled)
                {
                    StartCoroutine("colorRed");

                }
                CheckIfDeath();
            }
            
            stunCounter -= 1;
            if (stunCounter <= 0)
            {
                stunCounter = 4;
                stunned = false;

            }
            return;
        }
        if (isSanta)
        {
            giftCounter -= 1;
            if (giftCounter < 0)
            {
                animator.SetTrigger("gift");
                //Debug.Log(transform.position.x.ToString() + "  " + transform.position.y.ToString());
                GameManager.instance.ThrowGift(transform.position.x, transform.position.y, UnityEngine.Random.Range(0,4));
                giftCounter = 8;
                return;
            }
        }
        if (healthPotion && hp < 20)
        {
            SoundManager.instance.PlaySingle(drinkPotion);
            int oldHp = hp;
            hp += 100;
            if (hp >= maxHp)
            {

                hp = maxHp;
            }
            TextPopUp.Create(transform.position, hp - oldHp, false);
            healthPotion = false;
            return;
        }
        if (speedPotion)
        {
            SoundManager.instance.PlaySingle(drinkPotion);
            speedDifference -= 5;
            speedPotion = false;
            TextPopUp.Create(transform.position, 99, false);

            return;
        }

        bool playerTarget = false;
        GameObject food = FindClosestFood();
        if (food != null)
            target = food.transform;
        else
        {
            GameObject chest = FindClosestChest();
            if (chest != null)
                target = chest.transform;
            else {
                if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().foodPoints > 0)
                {
                    target = GameObject.FindGameObjectWithTag("Player").transform;
                    playerTarget = true;
                }
                else
                {
                    target = transform;
                }

            }
        }

        int xDir = 0;
        int yDir = 0;
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = (target.position.y - 0.5) > (transform.position.y - 0.5) ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        if (!isStarving && playerTarget)
        {
            yDir = -yDir;
            xDir = -xDir;

            if (transform.position.y + yDir < 1 || transform.position.y + yDir > GameManager.instance.y_max)
            {
                xDir = +1;
            }

        }
        if (xDir == 0 && yDir == 0)
        {
            int coin = UnityEngine.Random.Range(0, 1);
            if (coin == 0)
                xDir = UnityEngine.Random.Range(-1, 1);
            else
                yDir = UnityEngine.Random.Range(-1, 1);
        }


        //AttemptMove<Player>(xDir, yDir);
        //if (isPlayer)
        //    isPlayer = false;
        //else if (!base.notMoving)
        //{

        //}
        //else{
        //    if (!isStarving)
        //    {
        //        foodPoints++;
        //        hp--;
        //    }
        //speedDifference += 1;
        //foodPoints++;
        //    AttemptMove<Wall>(xDir, yDir);
        //}

        base.TryAttemptMove<Wall>(xDir, yDir);
        if (base.notMoving)
        {
            AttemptMove<Player>(xDir, yDir);
        }
        else
            AttemptMove<Wall>(xDir, yDir);

    }

    public void CheckIfDeath()
    {
        if (hp <= 0)
        {
            //GameManager.instance.RemoveEnemyFromList(this);
            //StopAllCoroutines();
            //gameObject.SetActive(false);
            //enabled = false;
            PlaySingle(gameOverSound);
            StartCoroutine("death");
        }
            

    }
    public GameObject FindClosestFood()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Food");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    public GameObject FindClosestChest()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Chest");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Food")
        {
            RandomizeSfx(eatSound1, eatSound2);
            foodPoints += pointsPerFood;
            if (foodPoints > maxFoodPoints)
                foodPoints = maxFoodPoints;
            isStarving = false;
            //Debug.Log("player speed : " + GameManager.instance.player.playerSpeed);
            //Debug.Log("enemy speed : " + originalSpeed);
            speedDifference = GameManager.instance.player.playerSpeed - originalSpeed;
            other.gameObject.SetActive(false);
            StartCoroutine("colorGreen");
        }

    }
    public void LoseFood(int enemyDamage)
    {
        animator.SetTrigger("enemyHit");

        Invoke("PlaySingleHit", 0.4f);
        foodPoints -= enemyDamage;
        if (foodPoints < 1)
        {
            
            foodPoints = 0;
            isStarving = true;
            speedDifference = speedDifference / 2;
            if (foodPoints == 0)
            {
                TextPopUp.Create(transform.position, 0);
            }
            else
            {
                TextPopUp.Create(transform.position, foodPoints);
                Invoke("stunText", 1f);
            }
        }
        else
        {
            TextPopUp.Create(transform.position, enemyDamage);
            Invoke("stunText", 1f);
        }
    
     
        stunned = true;

        
    }

    public void stunText()
    {
        TextPopUp.Create(transform.position, 0);
    }

    protected override void OnCantMove<T> (T component)
    {
        
        if (component is Player)
        {

            //isPlayer = true;
            Player hitPlayer = component as Player;
            
            if(hitPlayer.foodPoints > 0 && isStarving) {
                animator.SetTrigger("enemyAttack");

                RandomizeSfx(attackSound1, attackSound2);
                if ((GameManager.instance.player.foodPoints - playerDamage) < 0)
                    foodPoints += GameManager.instance.player.foodPoints;
                else
                    foodPoints += playerDamage;
                hitPlayer.LoseFood(playerDamage);

                if (foodPoints > 0)
                    isStarving = false;
                StartCoroutine("colorGreen");
            }

            //Debug.Log("starving :" + isStarving + " foodPoints : " + foodPoints);
        }
        else
        {
            
            RandomizeSfx(attackSound1, attackSound2);
            animator.SetTrigger("enemyAttack");
            Wall hitWall = component as Wall;
            hitWall.DamageWall(wallDamage, "enemy");
            
        }

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setSpeedDifference()
    {
        speedDifference = GameManager.instance.player.playerSpeed - enemySpeed;
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
    IEnumerator death()
    {
        animator.SetTrigger("enemyDeath");
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        enabled = false;
    }
    public void PlaySingleHit()
    {
        efxSource.volume = PlayerPrefs.GetFloat("efxVolume", 0.8f);
        efxSource.clip = hitSound;
        efxSource.Play();
    }
    public void PlaySingle(AudioClip clip)
    {
        efxSource.volume = PlayerPrefs.GetFloat("efxVolume", 0.8f);
        efxSource.clip = clip;
        efxSource.Play();
    }
    public void setVolume(float vol)
    {
        efxSource.volume = vol;
    }
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        float randomPitch = UnityEngine.Random.Range(lowPitchRange, highPitchRange);

        efxSource.volume = PlayerPrefs.GetFloat("efxVolume", 0.8f);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
       
    }
    public void RandomizeSfxShot(params AudioClip[] clips)
    {
        int randomIndex = UnityEngine.Random.Range(0, clips.Length);
        float randomPitch = UnityEngine.Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.PlayOneShot(efxSource.clip, PlayerPrefs.GetFloat("efxVolume", 0.8f) / 2f);
        
    }
}


