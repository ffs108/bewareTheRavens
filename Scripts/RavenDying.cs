using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenDying : MonoBehaviour
{
    public GameObject raven;
    public GameObject fireball;
    public Collider2D col;
    private GameObject enemies;

    private Animator animator;
    private float g = -9.8f;
    private float time = 0f;
    private Vector3 gravity;
    private GameObject mist;
    private GameObject des;
    private bool fall = false;
    //private List<GameObject> fireBallList;

    private void CheckForAABBCollision(GameObject element1, GameObject element2)
    {
        // calculates X min and X max for both element 1 and 2
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
        {
            if(element2.tag == "Hazard")
            {
                Debug.Log(element1.name + " collided with " + element2.name);
                animator.speed = 0;
                fall = true;
            }
            else if(element2.tag == "Mist" || element2.tag == "Destroy")
            {
                Destroy(gameObject);
            }
        }
    }

    private void GetDown(bool down)
    {
        if (down)
        {
            gravity.y = g * (time += Time.deltaTime);//Get a time-varying velocity on the y-axis of the gravity vector
            transform.position += gravity * Time.deltaTime;//Make raven move with the velocity of the gravity vector
        }
    }

    void Start()
    {
        gravity = Vector3.zero;//Assign a value of 0 to the gravity vector to make the initial velocity 0
        animator = gameObject.GetComponent<Animator>();
        mist = GameObject.FindWithTag("Mist");
        //des = GameObject.FindWithTag("Destroy");

        //ravenList = new List<GameObject>();
        enemies = GameObject.Find("Enemies");
        //Debug.Log(enemies.name);
    }

    void Update()
    {
        foreach (Transform child in enemies.transform)
        {
            if (child.gameObject.tag == "Hazard")
            {
                CheckForAABBCollision(gameObject, child.gameObject);
            }
        }
        GetDown(fall);
        CheckForAABBCollision(gameObject, mist);
        //CheckForAABBCollision(gameObject, des);
    }


}