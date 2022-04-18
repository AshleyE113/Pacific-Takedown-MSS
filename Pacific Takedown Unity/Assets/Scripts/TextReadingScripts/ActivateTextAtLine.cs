using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour
{

    public TextAsset theText;
    public int startLine;
    public int endLine;
    public TextBoxManager theTextBox;
    public bool destroyWhenActivated;

    void Start()
    {
        theTextBox = FindObjectOfType<TextBoxManager>();  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "player")
        {
            theTextBox.ReloadScript(theText);
            theTextBox.currentLine = startLine;
            theTextBox.endAtLine = endLine;
            theTextBox.EnableTextBox();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }

        }
    }
}
