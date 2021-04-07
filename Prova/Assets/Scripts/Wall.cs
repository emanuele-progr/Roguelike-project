using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;
    public List<Sprite> lootPool;
    public float hp = 4;
    public bool isChest = false;
    public SpriteRenderer lootSprite;
    public Image inventoryImg;
    public Button button;
    public AudioClip chestOpen;

    private SpriteRenderer spriteRenderer;
    private int randomLoot = 0;
    private bool opened = false;
    private Enemy en;
    
    
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall (int loss, string dealer)
    {
        
        hp -= loss;
        if (gameObject.GetComponent<ShakeTransform>())
            gameObject.GetComponent<ShakeTransform>().Begin();
        if (hp <= 0)
            if (!isChest)
            {
                Invoke("Disappear", 0.3f);
            }
            else if (!opened)
            {
                SoundManager.instance.efxSource.PlayOneShot(chestOpen, PlayerPrefs.GetFloat("efxVolume", 0.8f));
                spriteRenderer.sprite = dmgSprite;
                this.transform.GetChild(1).gameObject.SetActive(true);
                randomLoot = Random.Range(0, lootPool.Count);
                lootSprite = this.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
                if (dealer == "player")
                {
                    if (randomLoot == 0)
                    {

                        if (!GameManager.instance.speedBuffed)
                        {
                            lootSprite.sprite = lootPool[0];
                            //lootPool.Remove(lootPool[0]);
                            //GameManager.instance.inventoryIcons = lootPool;
                            GameManager.instance.setPlayerSpeed(1);
                            GameManager.instance.speedBuffed = true;
                        }
                        else
                        {
                            StartCoroutine(disappearObject());
                            opened = true;
                            return;
                        }



                    }
                    else if (randomLoot == 1)
                    {
                        if (!GameManager.instance.maxHpBuffed)
                        {
                            lootSprite.sprite = lootPool[1];
                            //lootPool.Remove(lootPool[1]);
                            //GameManager.instance.inventoryIcons = lootPool;
                            GameManager.instance.player.maxHp += 50;
                            GameManager.instance.maxHpBuffed = true;
                        }
                        else
                        {
                            StartCoroutine(disappearObject());
                            opened = true;
                            return;
                        }

                    }
                    else if (randomLoot == 2)
                    {
                        lootSprite.sprite = lootPool[2];
                        //lootPool.Remove(lootPool[2]);
                        //GameManager.instance.inventoryIcons = lootPool;
                    }
                    else if (randomLoot == 3)
                    {
                        lootSprite.sprite = lootPool[3];
                        //lootPool.Remove(lootPool[2]);
                        //GameManager.instance.inventoryIcons = lootPool;
                    }
                    int i = 1;

                    inventoryImg = GameObject.Find("Icon" + i).GetComponent<Image>();
                    while (inventoryImg.enabled && i < 5)
                    {
                        i++;
                        inventoryImg = GameObject.Find("Icon" + i).GetComponent<Image>();

                    }

                    if (randomLoot == 2)
                    {
                        inventoryImg = GameObject.Find("Icon3").GetComponent<Image>();
                        button = GameObject.Find("Icon3").GetComponent<Button>();
                        button.enabled = true;
                        GameManager.instance.SetPosition(3);
                    }
                    else if (randomLoot == 3)
                    {
                        inventoryImg = GameObject.Find("Icon4").GetComponent<Image>();
                        button = GameObject.Find("Icon4").GetComponent<Button>();
                        button.enabled = true;
                        GameManager.instance.SetPosition(4);
                    }
                    else
                    {
                        GameManager.instance.SetPosition(i);

                    }

                    inventoryImg.enabled = true;
                    inventoryImg.sprite = lootSprite.sprite;
                    GameManager.instance.AddSpriteToList(lootSprite.sprite);
                    StartCoroutine(disappearObject());
                    opened = true;

                }
                else
                {
                    en = FindClosestEnemy().GetComponent<Enemy>();

                    if (randomLoot == 0)
                    {
                        lootSprite.sprite = lootPool[0];
                        //GameManager.instance.enemies[0].enemySpeed += 1;
                        //GameManager.instance.enemies[0].originalSpeed += 1;
                        en.enemySpeed += 1;
                        en.originalSpeed += 1;
                    }
                    else if (randomLoot == 1)
                    {
                        lootSprite.sprite = lootPool[1];
                        //GameManager.instance.enemies[0].maxHp += 50;
                        en.maxHp += 50;
                    }
                    else if (randomLoot == 2)
                    {
                        lootSprite.sprite = lootPool[2];
                        //GameManager.instance.enemies[0].healthPotion = true;
                        en.healthPotion = true;
                    }
                    else if (randomLoot == 3)
                    {
                        lootSprite.sprite = lootPool[3];
                        //GameManager.instance.enemies[0].speedPotion = true;
                        en.speedPotion = true;
                    }

                    StartCoroutine(disappearObject());
                    opened = true;
                }
                

            }else if(opened && hp < 0)
            {
                Invoke("Disappear", 0.3f);
            }

    }

    IEnumerator disappearObject()
    {

        yield return new WaitForSeconds(1.5f);

        this.transform.GetChild(1).gameObject.SetActive(false);

    }

    void Update()
    {
       
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
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

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
