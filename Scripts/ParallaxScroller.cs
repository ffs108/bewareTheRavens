// ISTA 425 / INFO 525 Algorithms for Games
//
// Sample code file

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [Tooltip("Camera to which parallax is relative")]
    public GameObject parallaxCamera;

    [Tooltip("Level of parallax for this depth layer")]
    public float parallaxLevel;

    // the GC provides useful I/O and utility methods
    GameObject GC;
    GameController eventSystem;

    float startPos;

    float len;

    //public float PixelsPerUnit;


    // Start is called before the first frame update
    void Start()
    {
        //PixelsPerUnit = gameObject.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        GC = GameObject.FindGameObjectWithTag("GameController");
        eventSystem = GC.GetComponent<GameController>();

        //len = GC.GetComponent<SpriteRenderer>().bounds.size.x;
        len = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;

        parallaxCamera = GameObject.FindGameObjectWithTag("MainCamera");
        startPos = transform.position.x;

        SoundManager.Instance.PlaySound(SoundManager.SoundType.Night, true);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // TODO: Part of the parallax scrolling algorithm may go here.
        float input = eventSystem.playerMove.x; //change in player x
        float dist = (parallaxCamera.transform.position.x - input) * parallaxLevel;
        Vector3 newPos = new Vector3(startPos + dist, transform.position.y, transform.position.z);
        transform.position = newPos;

        float tmp = transform.position.x * (1 - parallaxLevel);

        float pos = startPos + dist + (len * parallaxLevel);
        float neg = startPos + dist - (len * parallaxLevel);

        if (tmp > pos) startPos += len; else if (tmp < neg) startPos -= len; //infinite

    }
}
