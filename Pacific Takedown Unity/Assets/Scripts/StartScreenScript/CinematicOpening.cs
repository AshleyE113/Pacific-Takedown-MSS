using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CinematicOpening : MonoBehaviour
{
    [SerializeField] List<string> openingLines;
    [SerializeField] List<Sprite> tempSpriteHolder;
    [SerializeField] TMP_Text textHolder;
    Image img;
    Color imgColor;
    Color BaseColor;
    bool isTyping = false;
    bool cancelTyping = false;
    bool goToNext = false;
    public float typeSpeed = 0.3f;
    public int currentIndex = 1;
    [SerializeField] float Speed;
    [SerializeField] GameObject fadeGO;
    bool faded = false;

    void Start()
    {
        img = GetComponent<Image>();
        imgColor = GetComponent<Image>().color;
        BaseColor = GetComponent<Image>().color;
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
                        StartCoroutine(TextScroll(openingLines[currentIndex]));
                        if (currentIndex == openingLines.Count - 1 || currentIndex > openingLines.Count)
                        {
                            for (float i = 1f; i >= 0; i -= Time.deltaTime)
                            {
                                // set color with i as alpha
                                img.color = new Color(0, 0, 0, i);
                   
                            }
                            imgColor.a -= Time.deltaTime;
                            Debug.Log("DONE");
                        }

                        if (currentIndex == 2 || currentIndex == 7 || currentIndex == 12 || currentIndex == 15)
                        {
                            StartCoroutine(FadeImage(fadeGO, false, 1));
                        }
                        else if (currentIndex == 3 || currentIndex == 8 || currentIndex == 13 || currentIndex == 16)
                        {
                            StartCoroutine(FadeImage(fadeGO, true, 1));
                        }

                        if (currentIndex == openingLines.Count - 1)
                        {
                            StartCoroutine(RemovePanel(fadeGO));
                        }

                        if (currentIndex >= 3 && currentIndex < 8)
                        {
                            img.color = Color.white;
                            img.sprite = tempSpriteHolder[0];

                        }
                        else if (currentIndex >= 8 && currentIndex < 12)
                        {
                          img.sprite = tempSpriteHolder[1];

                        }
                        else if (currentIndex >= 12 && currentIndex < 16)
                        {
                         
                          img.sprite = tempSpriteHolder[2];
                        }
                        else if (currentIndex >= 16)
                        {
                                img.sprite = tempSpriteHolder[3];
                        
                        }
                        currentIndex += 1;
                    }

                }
            }
        }
    }

    IEnumerator FadeImage(GameObject fadeGO, bool fadeAway, float timeVal)
    {
        Image fade = fadeGO.GetComponent<Image>();
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (timeVal = 1f; timeVal >= 0; timeVal -= Time.deltaTime)
            {
                // set color with i as alpha
                fade.color = new Color(0, 0, 0, timeVal);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (timeVal = 1f; timeVal <= 1; timeVal += Time.deltaTime)
            {
                // set color with i as alpha
                fade.color = new Color(0, 0, 0, timeVal);
                yield return null;
            }
        }
    }
    IEnumerator RemovePanel(GameObject fadeGO)
    {
        StartCoroutine(FadeImage(fadeGO, true, 1));
        yield return new WaitForSeconds(1.5f);
        Destroy(fadeGO);
        this.gameObject.SetActive(false);
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

