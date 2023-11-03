using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepAndPrune : MonoBehaviour
{
    // create the master list of bi and ei
    private List<ObjectPosition> masterList = new List<ObjectPosition>();
    private EnemyController enemyController;
    // Start is called before the first frame update

    GameController eventSystem;
    void Start()
    {
        // add the player character b and e to the master list
        // player transform is always zero in x postion
        GameObject player = GameObject.Find("Player");
        BoxCollider2D playerCollider = player.GetComponent<BoxCollider2D>();
        // this calculates the beginning and end pos for the x axis of the AABB collider
        float b = player.transform.position.x - playerCollider.size.x / 2;
        float e = player.transform.position.x + playerCollider.size.x / 2;
        // to keep track of bi and ei we have a class that stores the game object and an x pos
        ObjectPosition playerB = new ObjectPosition(player, b);
        ObjectPosition playerE = new ObjectPosition(player, e);
        masterList.Add(playerB);
        masterList.Add(playerE);

        enemyController = GameObject.Find("Enemies").GetComponent<EnemyController>();

        eventSystem = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateMasterList();
        //PrintList();
        SweepAndPruneAlgorithm();
    }

    // sort the masterlist using insertion sort
    private void SortMasterList()
    {
        int n = masterList.Count;
        for (int i = 1; i < n; i++)
        {
            ObjectPosition key = masterList[i];
            int j = i - 1;
            while (j >= 0 && masterList[j].xPos > key.xPos)
            {
                masterList[j + 1] = masterList[j];
                j = j - 1;
            }
            masterList[j + 1] = key;
        }
    }

    // debugger to print list
    private void PrintList()
    {
        for (int i = 0; i < masterList.Count; i++)
        {
            Debug.Log(masterList[i].element + " " + masterList[i].xPos);
        }
    }

    // adds an enemy to the master list with the proper postions
    // when enemy is spawned in the EnemyController script it gets added to the master list in this script
    public void AddEnemyMasterList(GameObject o)
    {
        float b = o.transform.position.x - o.GetComponent<BoxCollider2D>().size.x / 2;
        float e = o.transform.position.x + o.GetComponent<BoxCollider2D>().size.x / 2;
        ObjectPosition newEnemyB = new ObjectPosition(o, b);
        ObjectPosition newEnemyE = new ObjectPosition(o, e);
        masterList.Add(newEnemyB);
        masterList.Add(newEnemyE);
    }

    // update our b and e for each object in the master list
    private void UpdateMasterList()
    {
        List<GameObject> seenObjects = new List<GameObject>();
        for (int i = 0; i < masterList.Count; i++)
        {
            // if the gameobject was destroyed makes sure it is removed from master list
            if (masterList[i].element == null)
            {
                masterList.RemoveAt(i);
                continue;
            }
            // First if updates the b of each object
            if (!seenObjects.Contains(masterList[i].element))
            {
                float newB = masterList[i].element.transform.position.x - masterList[i].element.GetComponent<BoxCollider2D>().size.x / 2;
                masterList[i].xPos = newB;
                seenObjects.Add(masterList[i].element);
            }
            else
            { // else updates the e of each object
                float newE = masterList[i].element.transform.position.x + masterList[i].element.GetComponent<BoxCollider2D>().size.x / 2;
                masterList[i].xPos = newE;
                //seenObjects.Remove()  // this maybe helps the algortihm run faster but not sure
            }
        }
    }

    // basic sweep and prune algorithm
    private void SweepAndPruneAlgorithm()
    {
        SortMasterList();
        List<ObjectPosition> activeList = new List<ObjectPosition>();
        // create second list to help keep track of coressponding bi and ei
        // also helps when deciding when to remove or add object to activeList
        List<GameObject> activeListObjects = new List<GameObject>();
        for (int i = 0; i < masterList.Count; i++)
        {
            if (!activeListObjects.Contains(masterList[i].element))
            {
                // check active list objects before the adding element to active list
                // so that it does not check itself
                if (activeList != null)
                {
                    for (int j = 0; j < activeList.Count; j++)
                    {
                        if (activeList[j].element != null && masterList[i].element != null)
                        {
                            CheckForAABBCollision(activeList[j].element, masterList[i].element);
                        }
                    }
                }
                activeList.Add(masterList[i]);
                activeListObjects.Add(masterList[i].element);
            }
            else
            { // remove from active list once we found the ei
                activeList.Remove(masterList[i]);
                activeListObjects.Remove(masterList[i].element);
            }
        }
    }

    // checks if element1 is coliding with element2 using the AABB box collider
    private void CheckForAABBCollision(GameObject element1, GameObject element2)
    {
        // calculates X min and; X max for both element 1 and 2
        float element1XMin = element1.transform.position.x - element1.GetComponent<BoxCollider2D>().size.x / 2;
        float element1XMax = element1.transform.position.x + element1.GetComponent<BoxCollider2D>().size.x / 2;
        float element2XMin = element2.transform.position.x - element2.GetComponent<BoxCollider2D>().size.x / 2;
        float element2XMax = element2.transform.position.x + element2.GetComponent<BoxCollider2D>().size.x / 2;
        // calculates Y min and Y max for both element 1 and 2
        float element1YMin = element1.transform.position.y - element1.GetComponent<BoxCollider2D>().size.y / 2;
        float element1YMax = element1.transform.position.y + element1.GetComponent<BoxCollider2D>().size.y / 2;
        float element2YMin = element2.transform.position.y - element2.GetComponent<BoxCollider2D>().size.y / 2;
        float element2YMax = element2.transform.position.y + element2.GetComponent<BoxCollider2D>().size.y / 2;

        bool test = (element1XMax < element2XMin) ||
                    (element2XMax < element1XMin) ||
                    (element1YMax < element2YMin) ||
                    (element2YMax < element1YMin);

        if (!test)
        { // when collision occurs
            // COMMENT THIS OUT IF YOU DONT WANT THE DEBUG SPAM
            Debug.Log(element1.name + " collided with " + element2.name);
            // create the box indicators for both elements
            AABB aabbElement1 = eventSystem.CreateAABB(element1.GetComponent<BoxCollider2D>());
            eventSystem.CreateIndicator(aabbElement1);
            AABB aabbElement2 = eventSystem.CreateAABB(element2.GetComponent<BoxCollider2D>());
            eventSystem.CreateIndicator(aabbElement2);
            // handle player death
            if (element1.tag == "Player" && (element2.tag == "Hazard" || element2.tag == "Enemy"))
            {
                eventSystem.playerDeath();
                GameObject.Find("Player").GetComponent<PlayerController>().SetAnim(0.0f, 0.0f);
            }
        }
    }

    private float distance(GameObject gameobj, float yOrigin)
    {
        float x = gameobj.transform.position.x;
        float y = gameobj.transform.position.y - yOrigin;
        return Mathf.Sqrt((x * x) + (y * y));
    }

    public GameObject GetClosestEnemy(float y)
    {
        GameObject closest = masterList[0].element;
        for (int i = 0; i < masterList.Count; i++)
        {
            if (distance(masterList[i].element, y) < distance(closest, y))
            {
                closest = masterList[i].element;
            }
        }
        return closest;
    }
}

// the element and its x position
class ObjectPosition
{
    public GameObject element;
    public float xPos;

    public ObjectPosition(GameObject e, float pos)
    {
        element = e;
        xPos = pos;
    }
}
