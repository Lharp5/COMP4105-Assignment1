using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesScript : MonoBehaviour {
    private Color startcolor;
    private Renderer r;
    private GameObject square;
    private bool selected;
    public GameObject startingSquare;
    public bool isKing;

    // Use this for initialization
    void Start () {
        selected = false;
        r = gameObject.GetComponent<Renderer>();
        startcolor = r.material.color;
        square = startingSquare;
        square.GetComponent<SquareScript>().setPiece(gameObject);
        isKing = false;
    }

    void onDisable()
    {
        square = null;
    }

    public void MoveTo(GameObject sq)
    {
        // Removing ourselves from the current Square
        square.GetComponent<SquareScript>().setPiece(null);

        //Adding the square as our new piece
        square = sq;

        //adding ourselves to the square
        square.GetComponent<SquareScript>().setPiece(gameObject);
        if( tag == "RedPiece" && square.GetComponent<SquareScript>().rank == 5||
            tag == "BluePiece" && square.GetComponent<SquareScript>().rank == 0)
        {
            isKing = true;
            startcolor = tag == "RedPiece" ? Color.magenta : Color.cyan;
        }
    }

    public void toggleSelected()
    {
        selected = !selected;
    }

    public bool isSelected()
    {
        return selected;
    }

    public GameObject getSquare()
    {
        return square;
    }

    public bool canJump()
    {
        return square.GetComponent<SquareScript>().jumpableNeighbour();
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
