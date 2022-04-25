using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CinematicOpening : MonoBehaviour
{
    [SerializeField] List<string> openingLines;
    [SerializeField] List<Sprite> storySprites;
    [SerializeField] TMP_Text textHolder;
    [SerializeField] Sprite BlackBG;
    Sprite goSprite;
    Image img;
    bool isTyping = false;
    bool cancelTyping = false;
    bool goToNext = false;
    public float typeSpeed = 0.3f;
    public int currentIndex = 1;
    int sceneIndex = 0;

    void Start()
    {
        img = GetComponent<Image>();
        
    }

    void Update()
    {
        if (goToNext == false)
        {
            img.color = Color.black;
            StartCoroutine(TextScroll(openingLines[0]));
            
            goToNext = true;
        }

        if (goToNext == true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) //Allows the player to go through the script.
            {
                if (!isTyping)
                {
                    if (currentIndex < openingLines.Count)
                    {
                        Debug.Log(currentIndex);
                        
                        StartCoroutine(TextScroll(openingLines[currentIndex]));

                        if (currentIndex >= 3 && currentIndex < 8)
                        {
                            //StartCoroutine(FadeTo(Mathf.Lerp(0f, 1f, 0.5f + Time.deltaTime), 1f));
                            img.color = Color.white;
                            
                            img.sprite = storySprites[0];
                            
                        }
                            
                        else if (currentIndex >= 8 && currentIndex < 12)
                        {
                            img.sprite = storySprites[1];

                        }
                        else if (currentIndex == 12)
                        {
                            img.sprite = storySprites[2];

                        }
                        else if (currentIndex > 12 && currentIndex < 16)
                        {
                            img.sprite = storySprites[3];

                        }
                        else if (currentIndex >= 16 && currentIndex < openingLines.Count - 1)
                        {
                            img.sprite = storySprites[4];

                        }
                        currentIndex += 1;

                    }
                    else
                    {
                        StartCoroutine(FadeTo(0.001f, 1f));
                        
                    }
                        


                }
            }
        }
        Debug.Log(goToNext);
    }

    void RunThroughText()
    {
        
       
    }

    private IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = GetComponent<Image>().color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            GetComponent<Image>().color = newColor;
            yield return null;
        }
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

