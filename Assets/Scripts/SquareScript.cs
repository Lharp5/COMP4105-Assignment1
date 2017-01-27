using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour {

    private GameObject gamepiece;
    public GameObject[] neighbours;

    public int rank;
    public int column;

	// Use this for initialization
	void Start () {
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

    public void killPiece()
    {
        if(gamepiece != null)
        {
            gamepiece.SetActive(false);
            gamepiece = null;
        }
    }
    
    public bool isNeighbour(GameObject sq)
    {
        for(int i =0; i < neighbours.Length; i++)
        {
            if(neighbours[i].GetInstanceID() == sq.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    public bool jumpableNeighbour()
    {
        for(int i=0; i<neighbours.Length; i++)
        {
            string tag = neighbours[i].GetComponent<SquareScript>().occupiedBy();
            if (tag != "" && tag != gamepiece.tag)
            {
                for(int j=0; j<neighbours[i].GetComponent<SquareScript>().neighbours.Length; j++)
                {       
                    if( neighbours[i].GetComponent<SquareScript>().neighbours[j].GetComponent<SquareScript>().occupiedBy() == "" &&
                        neighbours[i].GetComponent<SquareScript>().neighbours[j].GetComponent<SquareScript>().column != column &&
                        neighbours[i].GetComponent<SquareScript>().neighbours[j].GetComponent<SquareScript>().rank != rank)
                    {
                        if(gamepiece.GetComponent<PiecesScript>().tag == "RedPiece" && rank  < neighbours[i].GetComponent<SquareScript>().neighbours[j].GetComponent<SquareScript>().rank ||
                            gamepiece.GetComponent<PiecesScript>().tag == "BluePiece" &&  rank > neighbours[i].GetComponent<SquareScript>().neighbours[j].GetComponent<SquareScript>().rank ||
                            gamepiece.GetComponent<PiecesScript>().isKing)
                        {
                            return true;
                        }
                        
                    }
                }
            }
        }
        return false;
    }
    public bool attemptPieceJumpTo(GameObject sq)
    {
        string tag;
        SquareScript square = sq.GetComponent<SquareScript>();
        for(int i =0; i < neighbours.Length; i++)
        {
            for(int j=0; j <square.neighbours.Length; j++)
            {   // If they share a common neighbour and we are jumping in a straight line not a jagged one.
                if( neighbours[i].GetInstanceID() == square.neighbours[j].GetInstanceID() &&
                    square.GetComponent<SquareScript>().column != column &&
                    square.GetComponent<SquareScript>().rank != rank)
                {
                    //Check to insure the neighbour's value is different form my own
                    tag = neighbours[i].GetComponent<SquareScript>().occupiedBy();
                    if(tag != "" && tag != gamepiece.tag)
                    {
                        // Remove the Piece from the game
                        neighbours[i].GetComponent<SquareScript>().killPiece();
                        return true;
                    }
                    else // DEBUG
                    {
                        Debug.Log("Neighbour found, but no enemy");
                        return false;
                    }
                    
                }
            }
        }

        return false;
    }
	
	// Update is called once per frame
	void Update () {
	}
}
