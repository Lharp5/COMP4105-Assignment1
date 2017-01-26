using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesScript : MonoBehaviour {
    private Color startcolor;
    private Renderer r;
    private GameObject square;
    private bool selected;
    public GameObject startingSquare;
    private bool isKing;

    // Use this for initialization
    void Start () {
        selected = false;
        r = gameObject.GetComponent<Renderer>();
        startcolor = r.material.color;
        square = startingSquare;
        isKing = false;
    }

    public bool MoveTo(GameObject sq)
    {
        int distance = square.GetComponent<SquareScript>().rank - sq.GetComponent<SquareScript>().rank;
        if (!isKing)
        {
            // if we are moving laterally, backwards, or too far away reject that move.
            if (tag == "RedPiece" && (distance >= 0 || distance < -2) || 
                tag == "BluePiece" && (distance < 1 || distance > 2))
            {
                Debug.Log("Invalid Move!");
                return false;
            }
        }

        // If Square is open 
        if(sq.GetComponent<SquareScript>().occupiedBy() == "")
        {
            // If the distance is more then 1, check to make sure there is an enemy between us.

            // Removing ourselves from the current Square
            square.GetComponent<SquareScript>().setPiece(null);

            //Adding the square as our new piece
            square = sq;
            //adding ourselves to the square
            square.GetComponent<SquareScript>().setPiece(gameObject);
            
        }

        return true;
    }

    public void toggleSelected()
    {
        selected = !selected;
    }

    public bool isSelected()
    {
        return selected;
    }

    // Update is called once per frame
    void Update () {
	    if(square != null && transform.position.x != square.transform.position.x && transform.position.z != square.transform.position.z)
        {
            transform.Translate(square.transform.position.x - transform.position.x, 0, square.transform.position.z - transform.position.z);
        }

        if (selected)
        {
            r.material.color = Color.green;
        }
        else
        {
            r.material.color = startcolor;
        }
    }
}
