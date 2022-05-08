using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCTalkScript : MonoBehaviour
{
    [SerializeField] string[] thingToSay;
    [SerializeField] bool repeat = false;
    private int currentSpokenStrings = 0;
    [SerializeField] TMP_Text NPCdialogue;
    [SerializeField] GameObject textBox, EIcon;
    bool isTyping = false;
    bool cancelTyping = false;
    bool goToNext = false;
    bool isTalking = false;
    public float typeSpeed = 0.3f;


    void Awake()
    {
        NPCdialogue = GameObject.Find("Dialogue").GetComponent<TMP_Text>();
        textBox.SetActive(false);
        EIcon.SetActive(false);
    }

    private void Update()
    {
        if (isTalking && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("EEEEEEEEEEEEE");
            EIcon.SetActive(false);
            if (currentSpokenStrings < thingToSay.Length)
            {
                StartCoroutine(TextScroll(Speak(thingToSay[currentSpokenStrings])));
                currentSpokenStrings++;
            }
            else
            {
                if (repeat)
                    currentSpokenStrings = 0;
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            EIcon.SetActive(true);
            isTalking = true;
            
            Debug.Log("In it");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            textBox.SetActive(false);
            isTalking = false;
            EIcon.SetActive(false);
        }
    }
    string Speak(string whatToSay)
    {
        textBox.SetActive(true);
        NPCdialogue.text = whatToSay;
        return NPCdialogue.text;
    }

    private IEnumerator TextScroll(string lineOfText) //Gives it that one character at a timeffect...
    {
        int letter = 0;
        NPCdialogue.text = "";
        isTyping = true;
        cancelTyping = false;
        yield return new WaitForSeconds(typeSpeed);
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            NPCdialogue.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        NPCdialogue.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }
}
