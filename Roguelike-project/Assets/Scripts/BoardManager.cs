using UnityEngine;
using System;
using System.Collections.Generic;         //Allows us to use Lists.
using Random = UnityEngine.Random;         //Tells Random to use the Unity Engine random number generator.




public class BoardManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        public int minimum;             //Minimum value for our Count class.
        public int maximum;             //Maximum value for our Count class.


        //Assignment constructor.
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public AudioClip newDrop;
    public int columns = 0;                                         //Number of columns in our game board.
    public int rows = 0;                                            //Number of rows in our game board.
    public Count wallCount = new Count(5, 9);                        //Lower and upper limit for our random number of walls per level.
    public Count foodCount = new Count(1, 4);                        //Lower and upper limit for our random number of food items per level.
    public Count chestCount = new Count(1, 3);
    public Count bombCount = new Count(1, 3);
    public Count trapCount = new Count(0, 2);
    public GameObject exit;                                          //Prefab to spawn for exit.
    public GameObject[] floorTiles;                                    //Array of floor prefabs.
    public GameObject[] wallTiles;                                     //Array of wall prefabs.
    public GameObject[] chestTiles;
    public GameObject[] bombTiles;
    public GameObject[] trapTiles;
    public GameObject[] foodTiles;                                    //Array of food prefabs.
    public GameObject[] enemyTiles;
    public GameObject[] bossTiles;
    public GameObject[] outerWallTiles;                                //Array of outer tile prefabs.
    public GameObject[] outerWallEndTiles;
    public GameObject[] sideWallTiles;
    public GameObject[] edgeWallTiles;
    public GameObject[] edgeLowWallTiles;
    public GameObject[] edgeHighWallTiles;
    public GameObject[] baseFloor;
    public GameObject[] lowDecoration;
    public GameObject[] highDecoration;
    public GameObject frontWallPush;
    public GameObject leftWallPush;
    public GameObject altar;
    public GameObject player;

    private Transform boardHolder;                                    //A variable to store a reference to the transform of our Board object.
    private List<Vector3> gridPositions = new List<Vector3>();    //A list of possible locations to place tiles.
    private SpriteRenderer mySpriteRenderer;
    private Testing test;


    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        //Clear our list gridPositions.
        gridPositions.Clear();

        //Loop through x axis (columns).
        for (int x = 1; x < columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    public int ReturnIndexPosition(int xPos, int yPos)
    {
        int counter = 1;
        for (int x = 1; x < xPos; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < yPos; y++)
            {
                counter++;
            }
        }

        return counter;
    }


    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;
        this.columns = Random.Range(13, 16);
        this.rows = Random.Range(7, 9);
        //Debug.Log(columns);
        //Debug.Log(rows);
    

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = -2; x < columns + 2; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = -2; y < rows + 2; y++)
            {
                GameObject toInstantiateback = baseFloor[Random.Range(0, baseFloor.Length)];
                GameObject instance2 =
                    Instantiate(toInstantiateback, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject toInstantiate2 = floorTiles[Random.Range(0, floorTiles.Length)];

                //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if ( y == -2 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                if ( y == -1 || y == rows + 1)
                    toInstantiate = outerWallEndTiles[Random.Range(0, outerWallEndTiles.Length)];
                
                if (x == columns - 3 & y == -2)
                {
                    toInstantiate = lowDecoration[Random.Range(0, lowDecoration.Length)];
                }
                if (x == columns - 3 & y == -1)
                {
                    toInstantiate = highDecoration[Random.Range(0, lowDecoration.Length)];
                }
                if (x ==  2 & y == -2)
                {
                    toInstantiate = lowDecoration[Random.Range(0, lowDecoration.Length)];
                }
                if (x ==  2 & y == -1)
                {
                    toInstantiate = highDecoration[Random.Range(0, lowDecoration.Length)];
                }
                if (x == columns - 3 & y == rows)
                {
                    toInstantiate = lowDecoration[Random.Range(0, lowDecoration.Length)];
                }
                if (x == columns - 3 & y == rows + 1)
                {
                    toInstantiate = highDecoration[Random.Range(0, lowDecoration.Length)];
                }
                if (x == 2 & y == rows)
                {
                    toInstantiate = lowDecoration[Random.Range(0, lowDecoration.Length)];
                }
                if (x == 2 & y == rows + 1)
                {
                    toInstantiate = highDecoration[Random.Range(0, lowDecoration.Length)];
                }
                if (x == columns + 1 & y == rows + 1)
                {
                    toInstantiate2 = edgeHighWallTiles[Random.Range(0, sideWallTiles.Length)];
                    toInstantiate = outerWallEndTiles[Random.Range(0, edgeLowWallTiles.Length)];
                    instance2 =
                    Instantiate(toInstantiate2, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance2.transform.SetParent(boardHolder);
                    mySpriteRenderer = instance2.GetComponent<SpriteRenderer>();
                    mySpriteRenderer.flipX = true;
                }
                else if (x == columns + 1 & y == -2)
                {
                    toInstantiate = baseFloor[Random.Range(0, baseFloor.Length)];
                    toInstantiate2 = edgeLowWallTiles[Random.Range(0, edgeLowWallTiles.Length)];
                    instance2 = Instantiate(toInstantiate2, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance2.transform.SetParent(boardHolder);
                    mySpriteRenderer = instance2.GetComponent<SpriteRenderer>();
                    mySpriteRenderer.flipX = true;
                }
                else if (x == columns + 1 & y == -1)
                {
                    toInstantiate = baseFloor[Random.Range(0, baseFloor.Length)];
                    toInstantiate2 = edgeWallTiles[Random.Range(0, edgeLowWallTiles.Length)];
                    instance2 = Instantiate(toInstantiate2, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance2.transform.SetParent(boardHolder);
                    mySpriteRenderer = instance2.GetComponent<SpriteRenderer>();
                    mySpriteRenderer.flipX = true;
                }


                else if (x == columns + 1)
                {
                    toInstantiate2 = sideWallTiles[Random.Range(0, sideWallTiles.Length)];
                    instance2 = Instantiate(toInstantiate2, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance2.transform.SetParent(boardHolder);
                    mySpriteRenderer = instance2.GetComponent<SpriteRenderer>();
                    mySpriteRenderer.flipX = true;
                }
                if (x == -2 & y == -1)
                {
                    toInstantiate = edgeWallTiles[Random.Range(0, edgeWallTiles.Length)];
                }
                else if (x == -2 & y == -2)
                {
                    toInstantiate = edgeLowWallTiles[Random.Range(0, edgeLowWallTiles.Length)];
                }
                else if (x == -2 & y == rows)
                {
                    toInstantiate2 = outerWallTiles[Random.Range(0, edgeLowWallTiles.Length)];
                    instance2 =
                    Instantiate(toInstantiate2, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    toInstantiate = sideWallTiles[Random.Range(0, sideWallTiles.Length)];
                    instance2.transform.SetParent(boardHolder);
                }
                else if (x == -2 & y == rows + 1)
                {
                    toInstantiate2 = outerWallEndTiles[Random.Range(0, edgeLowWallTiles.Length)];
                    instance2 =
                    Instantiate(toInstantiate2, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    toInstantiate = edgeHighWallTiles[Random.Range(0, sideWallTiles.Length)];
                    instance2.transform.SetParent(boardHolder);
                }

                else if (x == -2)
                {
                    toInstantiate = sideWallTiles[Random.Range(0, sideWallTiles.Length)];
                }
                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                //mySpriteRenderer = instance.GetComponent<SpriteRenderer>();
                //mySpriteRenderer.flipY = true;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);
                instance2.transform.SetParent(boardHolder);
                
                // y -(float)0.5
                //-(rows + 5)
                Camera.main.transform.position = new Vector3((float)columns / 2 - (float)0.5, (float)rows / 2 + (float)0.5, -(rows + 4));
                test = Camera.main.GetComponent<Testing>();
                test.setDim(columns + 2, rows);
                
                    
               
                
            }
        }
    }


    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }


    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void InstatiateRandomFood()
    {
        SoundManager.instance.efxSource.PlayOneShot(newDrop, PlayerPrefs.GetFloat("efxVolume"));
        LayoutObjectAtRandom(foodTiles, 1, 1);
    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)
    {
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset our list of gridpositions.
        InitialiseList();
        if (!(GameManager.instance.level == 8))
        {
            //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

            //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
            LayoutObjectAtRandom(chestTiles, chestCount.minimum, chestCount.maximum);
            if (GameManager.instance.level != 1)
            {
                LayoutObjectAtRandom(bombTiles, bombCount.minimum, bombCount.maximum);
                LayoutObjectAtRandom(trapTiles, trapCount.minimum, trapCount.maximum);
            }
            //Determine number of enemies based on current level number, based on a logarithmic progression
            int enemyCount = (int)Mathf.Log(level, 2f);

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            if (GameManager.instance.level == 5)
            {
                Vector3 randomPosition = RandomPosition();
                Instantiate(bossTiles[0], randomPosition, Quaternion.identity);
                Dialogue dialogue;
                dialogue = new Dialogue("Game Master", "Ah-ah, time for a boss fight! and he's very pissed off ohohohoh, good luck my friend.");

                DialogueManager.instance.StartDialogue(dialogue);
            }
            else if (GameManager.instance.level == 10)
            {
                Vector3 randomPosition = RandomPosition();
                Instantiate(bossTiles[1], randomPosition, Quaternion.identity);
                Dialogue dialogue;
                dialogue = new Dialogue("Game Master", "Oh-oh-oh, it's Christmas time !?.");

                DialogueManager.instance.StartDialogue(dialogue);
            }
            else
                LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        }
        else
        {

            Instantiate(altar, new Vector3(columns/2, rows/2, 0f), Quaternion.identity);
            Dialogue dialogue;
            dialogue = new Dialogue("Game Master", "Mhh, time for a power-up!");

            DialogueManager.instance.StartDialogue(dialogue);
        }

        //Instantiate the exit tile in the upper right hand corner of our game board
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
        Instantiate(frontWallPush, new Vector3(Random.Range(4, 8), rows, 0f), Quaternion.identity);
        Instantiate(leftWallPush, new Vector3(-2, Random.Range(4, 6), 0f), Quaternion.identity);
        Instantiate(player, new Vector3(0, 0, 0f), Quaternion.identity);
    }
}

