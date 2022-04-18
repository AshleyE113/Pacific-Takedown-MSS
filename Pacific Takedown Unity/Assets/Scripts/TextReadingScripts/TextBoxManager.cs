using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour{

    public GameObject textBox;
    public Text theText;
    public TextAsset textFile;
    public string[] textLines;
    public int currentLine;
    public int endAtLine;
    public PlayerController player;
    public bool isActive;
    public bool stopPlayerMovement;
    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typeSpeed;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if (textFile != null) //if there is a text file...
        {
            textLines = (textFile.text.Split('\n')); //split the text onto new lines.
        }

        if(endAtLine == 0)
        {
            endAtLine = textLines.Length - 1; //Sets the endline to the length of the array
        }

        if (isActive)
        {
            EnableTextBox(); //Shows the textbox
        }
        else
        {
            DisableTextBox(); //makes the box disappear
        }

    }

    void Update()
    {
        if (!isActive) //Don't do anything if the box isn't there
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) //Allows the player to go through the script.
        {
            if (!isTyping)
            {
                currentLine += 1;

                if (currentLine > endAtLine)
                {
                    DisableTextBox(); //When it finishes, turn off the box.
                }
                else
                {
                    StartCoroutine(TextScroll(textLines[currentLine])); //run through the lines and display the text
                }

            }
            else if(isTyping && !cancelTyping) //Makes it all appear at once
            {
                cancelTyping = true;
            }
        }
    }

    private IEnumerator TextScroll (string lineOfText) //Gives it that one character at a timeffect...
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            theText.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }

    public void EnableTextBox()
    {
        textBox.SetActive(true);
        isActive = true;

        if(stopPlayerMovement)
        {
            player.canMove = false;
        }
        StartCoroutine(TextScroll(textLines[currentLine]));
    }

    public void DisableTextBox() //if the textbox is gone, the player can move
    {
        textBox.SetActive(false);
        isActive = false;
        player.canMove = true;
    }

    public void ReloadScript(TextAsset theText) 
    {
        if(theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }
}
