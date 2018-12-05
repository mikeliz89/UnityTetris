using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour {

    public GameObject[] shapes;

    public void SpawnShape()
    {
        //between 0 and 6
        int shapeIndex = Random.Range(0, 6);

        Instantiate(shapes[shapeIndex],
            transform.position,
            Quaternion.identity);
    }

	// Use this for initialization
	void Start () {
        SpawnShape();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
