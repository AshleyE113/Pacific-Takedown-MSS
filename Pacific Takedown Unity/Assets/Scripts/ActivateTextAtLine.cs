using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour{

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextBox;

    public bool destroyWhenActivated;

     // Start is called before the first frame update
    void Start(){
        theTextBox = FindObjectOfType<TextBoxManager>();
        theText = gameObject.GetComponent<ActivateTextAtLine>().theText;
        
    }

    // Update is called once per frame
    void Update(){
    }



    private void FixedUpdate()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "player")
        {
            print("hi");
            if (!Input.GetKeyDown(KeyCode.E)) return;
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
