using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public int  redPieces;
    public int bluePieces;

    private GameObject selectedPiece;

    public bool redTurn;
    private bool jumped;

    public Text blueText;
    public Text redText;
    public Text winText;

    private bool gameOver = false;
    enum MoveType {NoMove, Move, Jump}

    // Use this for initialization
    void Start () {
        winText.enabled = false;
    }

    void endTurn()
    {
        redTurn = !redTurn;
        jumped = false;

        if (selectedPiece && selectedPiece.GetComponent<PiecesScript>().isSelected())
        {
            selectedPiece.GetComponent<PiecesScript>().toggleSelected();
            selectedPiece = null;
        }

    }

    // Processes the Move, returns the type of move committed.
    MoveType ProcessMove(GameObject piece, GameObject newSquare, bool isJump)
    {
        GameObject pieceSquare = piece.GetComponent<PiecesScript>().getSquare();
        int distance = pieceSquare.GetComponent<SquareScript>().rank - newSquare.GetComponent<SquareScript>().rank;
        if (!piece.GetComponent<PiecesScript>().isKing)
        {
            // if we are moving laterally, backwards, or too far away reject that move.
            if (piece.tag == "RedPiece" && (distance >= 0 || distance < -2) ||
                piece.tag == "BluePiece" && (distance < 1 || distance > 2))
            {
                Debug.Log("Invalid Move!");
                return MoveType.NoMove;
            }
        }

        // Normalizing our distance since we just checked we are going the right way.
        distance = distance < 0 ? distance * -1 : distance;

        // Checking to see if we can jump or not.
        if (isJump && distance != 2)
        {
            Debug.Log("Has to Jump Another Piece!");
            return MoveType.NoMove;
        }

        // If Square is open 
        if (newSquare.GetComponent<SquareScript>().occupiedBy() != "")
        {
            return MoveType.NoMove;
        }
        // If the distance is more then 1, check to make sure there is an enemy between us.
        if (distance == 1 && !pieceSquare.GetComponent<SquareScript>().isNeighbour(newSquare) ||
            distance == 2 && !pieceSquare.GetComponent<SquareScript>().attemptPieceJumpTo(newSquare))
        {
            Debug.Log("Invalid Jump!");
            return MoveType.NoMove;
        }
        
        piece.GetComponent<PiecesScript>().MoveTo(newSquare);

        //We have successfully Jumped, evaluate whether we go again.
        if (distance == 2)
        {
            return MoveType.Jump;
        }

        return MoveType.Move;
    }

    void updateUI()
    {
        if (gameOver)
        {
            redText.enabled = blueText.enabled = false;
            winText.enabled = true;
            if (redPieces <= 0)
            {
                winText.text = "Red Wins";
            }
            else
            {
                winText.text = "Blue Wins";
            }
        }
        else if (redTurn)
        {
            blueText.enabled = false;
            redText.enabled = true;

        }
        else
        {
            blueText.enabled = true;
            redText.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(redPieces <= 0 || bluePieces <= 0){
            gameOver = true;
            
        }
        // If we are playing, and the mouse is pressed
        if (!gameOver && Input.GetMouseButtonDown(0))
        {
            // Selection of pieces
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if(!jumped && (hitInfo.transform.gameObject.tag == "RedPiece" && redTurn || hitInfo.transform.gameObject.tag == "BluePiece" && !redTurn))
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
                    // If we have successfully moved, and cannot go again end turn.
                    MoveType result = ProcessMove(selectedPiece, hitInfo.transform.gameObject, jumped);
                    switch (result)
                    {
                        case MoveType.Jump:
                            if (selectedPiece.gameObject.tag == "RedPiece") redPieces--;
                            else bluePieces--;
                            if (selectedPiece.GetComponent<PiecesScript>().canJump())
                            {
                                jumped = true;
                                
                            }
                            else
                            {
                                endTurn();
                            }                            
                            break;
                        case MoveType.Move:
                            endTurn();
                            break;
                    }
                }
            }
        }

        updateUI();
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
