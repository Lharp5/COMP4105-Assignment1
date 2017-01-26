using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour {

    private GameObject gamepiece;
    public GameObject[] neighbours;

    public int rank;

	// Use this for initialization
	void Start () {
        gamepiece = null;
	}

    public string occupiedBy()
    {
        if (!gamepiece)
        {
            return "";
        }
        else
        {
            return gamepiece.tag;
        }
    }

    public void setPiece(GameObject piece)
    {
        gamepiece = piece;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
