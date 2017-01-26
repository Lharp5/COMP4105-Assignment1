using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public GameObject[] redPieces;
    public GameObject[] bluePieces;

    private GameObject selectedPiece;

    public bool redTurn;

    // Use this for initialization
    void Start () {
    }

    void endTurn()
    {
        redTurn = !redTurn;
        if (selectedPiece && selectedPiece.GetComponent<PiecesScript>().isSelected())
        {
            selectedPiece.GetComponent<PiecesScript>().toggleSelected();
            selectedPiece = null;
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            // Selection of pieces
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if(hitInfo.transform.gameObject.tag == "RedPiece" && redTurn || hitInfo.transform.gameObject.tag == "BluePiece" && !redTurn)
                {
                    if(!selectedPiece || selectedPiece.GetInstanceID() == hitInfo.transform.gameObject.GetInstanceID())
                    {
                        hitInfo.transform.gameObject.GetComponent<PiecesScript>().toggleSelected();
                        if (hitInfo.transform.gameObject.GetComponent<PiecesScript>().isSelected())
                        {
                            selectedPiece = hitInfo.transform.gameObject;
                        }
                        else
                        {
                            selectedPiece = null;
                        }
                    }                    
                } // Selection of moves
                else if(selectedPiece && hitInfo.transform.gameObject.tag == "Square")
                {
                    if (selectedPiece.GetComponent<PiecesScript>().MoveTo(hitInfo.transform.gameObject))
                    {
                        endTurn();
                    }
                }
            }
        }
        
    }

    //called when dealing with rigidBody, like adding force.
    void FixedUpdate()
    {

    }

    //called after all update functions have been called.
    void LateUpdate()
    {

    }
}
