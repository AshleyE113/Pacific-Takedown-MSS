using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CinematicOpening : MonoBehaviour
{

    [SerializeField] List<string> openingLines;
    [SerializeField] TMP_Text textHolder;
    [SerializeField] Image goSprite;
    bool isTyping = false;
    bool cancelTyping = false;
    public float typeSpeed = 0.3f;
    int currentIndex = 0;

    //Other varis
    Color col = Color.white;
    float alphaVal = 255f;
    float lerpVal = 0.5f;
    Color objColor;

    void Start()
    {
        objColor = goSprite.color;
    }

    void Update()
    {
        RunThroughText();
    }

    void RunThroughText()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Allows the player to go through the script.
        {
            if (!isTyping)
            {
                if (currentIndex < openingLines.Count)
                {
                    Debug.Log(currentIndex);
                    StartCoroutine(TextScroll(openingLines[currentIndex]));
                    currentIndex += 1;
                }

            }

        }

        Debug.Log("Hmm");
        if (objColor.a > 0)
            objColor.a = 100f;
    }

    void FadeOut()
    {
        Color objColor = this.GetComponent<Image>().color;
        float fadeSpeed = 0.5f;
        float fadeVal = objColor.a - (fadeSpeed * Time.deltaTime);
        objColor = Color.white;
        this.GetComponent<Image>().color = objColor;
    }


    private IEnumerator TextScroll(string lineOfText) //Gives it that one character at a timeffect...
    {
        int letter = 0;
        textHolder.text = "";
        isTyping = true;
        cancelTyping = false;
        yield return new WaitForSeconds(typeSpeed);
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            textHolder.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        textHolder.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }
}

