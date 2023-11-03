using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBackgrounds : MonoBehaviour
{
    public float lengthForBackgroundChange = 15f;

    public GameObject[] skyList = new GameObject[3];
    public GameObject[] mistList = new GameObject[3];
    public GameObject[] forestList = new GameObject[2];

    private PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        RandomNewValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.GetDistancedMoved() >= lengthForBackgroundChange) {
            RandomNewValues();
            pc.SetDistanceMoved(0);
        }
    }

    private void RandomNewValues() {
        int skyListRandom = Random.Range(0, 3);
        int mistListRandom = Random.Range(0, 3);
        int treeListRandom = Random.Range(0, 2);

        NewSky(skyListRandom);
        NewMist(mistListRandom);
        NewTree(treeListRandom);
    }

    private void NewTree(int treeListRandom) {
        for (int i = 0; i < forestList.Length; i++) {
            if (i == treeListRandom) {
                forestList[i].SetActive(true);
            } else {
                forestList[i].SetActive(false);
            }
        }
    }

    private void NewSky(int skyListRandom) {
        for (int i = 0; i < skyList.Length; i++) {
            if (i == skyListRandom) {
                skyList[i].SetActive(true);
            } else {
                skyList[i].SetActive(false);
            }
        }
    }

    private void NewMist(int mistListRandom) {
        for (int i = 0; i < mistList.Length; i++) {
            if (i == mistListRandom) {
                mistList[i].SetActive(true);
            } else {
                mistList[i].SetActive(false);
            }
        }
    }
}
