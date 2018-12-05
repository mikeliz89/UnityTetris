using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{

    // Define that we want to fall 1 unit per second
    public float speed = 1.0f;

    // Tracks the last time the shape moved down
    float lastMoveDown = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("a"))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            if (!IsInGrid())
            {
                // Switch to previous
                transform.position += new Vector3(1, 0, 0);
            }
            else
            {
                // Update the GameBoard array
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot
                    (SoundManager.Instance.shapeMove);
            }
        }

        if (Input.GetKeyDown("d"))
        {
            transform.position += new Vector3(1, 0, 0);

            if (!IsInGrid())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
            else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot
                    (SoundManager.Instance.shapeMove);
            }
        }

        // Move Shape down when s is pressed, or once every second
        // First time through Time.time will have a value of 1 and
        // then 2, etc. 
        // lastMoveDown will increment each time through as well

        if (Input.GetKeyDown("s") || Time.time - lastMoveDown >= 1)
        {
            transform.position += new Vector3(0, -1, 0);

            if (!IsInGrid())
            {
                transform.position += new Vector3(0, 1, 0);

                bool rowDeleted = GameBoard.DeleteAllFullRows();
                if(rowDeleted)
                {
                    GameBoard.DeleteAllFullRows();

                    IncreaseTextUIScore();
                }

                // Disconnect the script actions from the shape
                enabled = false;

                // Spawn another shape
                FindObjectOfType<ShapeSpawner>().SpawnShape();

                SoundManager.Instance.PlayOneShot
                    (SoundManager.Instance.shapeStop);
            }
            else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot
                    (SoundManager.Instance.shapeMove);
            }

            lastMoveDown = Time.time;
        }

        if (Input.GetKeyDown("w"))
        {
            transform.Rotate(0, 0, 90);

            if (!IsInGrid())
            {
                transform.Rotate(0, 0, -90);
            }
            else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot
                    (SoundManager.Instance.rotateSound);
            }
        }

        if (Input.GetKeyDown("e"))
        {
            transform.Rotate(0, 0, -90);

            if (!IsInGrid())
            {
                transform.Rotate(0, 0, 90);
            }
            else
            {
                UpdateGameBoard();

                SoundManager.Instance.PlayOneShot
                    (SoundManager.Instance.rotateSound);
            }
        }

        //miikan oma
        /*if(Input.GetKeyDown("space"))
        {
            Debug.Log("SPACE pressed down!");

            while(IsInGrid()) {

                transform.position += new Vector3(0, -1, 0);
                UpdateGameBoard();
                lastMoveDown = 0;
            };

            //transform.position += new Vector3(0, 2, 0);
            //lastMoveDown = Time.time;
        }
		*/

    }

    public bool IsInGrid()
    {
        // Cycle through every block in the shape
        foreach (Transform childBlock in transform)
        {
            // Get location of the block and round to int
            Vector2 vect = RoundVector(childBlock.position);

            // Check if the position is within the border
            if (!IsInBorder(vect))
            {
                return false;
            }

            //Check if child block is in empty point
            if(!SomethingExistsInPosition(vect))
            {
                return false;
            }
            
        }
        return true;
    }

    public bool SomethingExistsInPosition(Vector2 vect)
    {
        // Check what is located in the GameBoard array
        // This should be its own method
        if (GameBoard.gameBoard[(int)vect.x, (int)vect.y] != null &&
            GameBoard.gameBoard[(int)vect.x, (int)vect.y].parent != transform)
        {
            return false;
        }
        return true;
    }

    // Round the cubes position to an int so it can fit 
    // in the array
    public Vector2 RoundVector(Vector2 vect)
    {
        return new Vector2(Mathf.Round(vect.x), Mathf.Round(vect.y));
    }

    // Check if between the border
    public static bool IsInBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
            (int)pos.x <= 9 &&
            (int)pos.y >= 0);
    }

    // 
    public void UpdateGameBoard()
    {

        Something();

        AddChildCubesToGameBoard();

        GameBoard.PrintArray();

    }

    private void Something()
    {
        for (int y = 0; y < 20; ++y)
        {
            for (int x = 0; x < 10; ++x)
            {
                Vector2 vect = new Vector2((float)x, (float)y);
                // If the 1st isn't null that means there
                // is a cube at that position
                // Then we check if the shape passed in
                // is already there
                if (GameBoard.gameBoard[x, y] != null &&
                    GameBoard.gameBoard[x, y].parent == transform)
                {
                    // If the shape moves down then we want
                    // to remove it from the gameBoard array
                    GameBoard.gameBoard[x, y] = null;
                }

            }
        }
    }

    private void AddChildCubesToGameBoard()
    {
        // Iterate over all spaces on our gameboard
        // and add the new cubes for out shape
        foreach (Transform childBlock in transform)
        {
            // Shorten our position object
            Vector2 vect = RoundVector(childBlock.position);

            // Put our cube in the gameboard array
            GameBoard.gameBoard[(int)vect.x, (int)vect.y] = childBlock;

            Debug.Log("Cube At : " + vect.x + " " + vect.y);

        }
    }

    void IncreaseTextUIScore()
    {
        var textUIComp = GameObject.Find("Score").GetComponent<Text>();

        int score = int.Parse(textUIComp.text);

        score++;

        textUIComp.text = score.ToString();
    }

}