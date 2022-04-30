using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CinematicOpening : MonoBehaviour
{
    [SerializeField] List<string> openingLines;
    List<Image> storySprites;
    [SerializeField] List<Sprite> tempSpriteHolder;
    [SerializeField] TMP_Text textHolder;
    Image img;
    bool isTyping = false;
    bool cancelTyping = false;
    bool goToNext = false;
    public float typeSpeed = 0.3f;
    public int currentIndex = 1;
    [SerializeField] float Speed;
    [SerializeField] GameObject fadeGO;

    void Start()
    {
        img = GetComponent<Image>();
        storySprites = new List<Image>();
        /*
        for (int i = 0; i < tempSpriteHolder.Count; i++)
        {
            img.sprite = tempSpriteHolder[i];
            storySprites.Add(img);
            Debug.Log("For loop");
        }*/
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
                            if (fadeGO.GetComponent<Image>().color.a > 0.5f)
                            {
                                StartCoroutine(FadeOut(fadeGO));
                                img.color = Color.white;
                                //storySprites[0].sprite = tempSpriteHolder[0];
                                img.sprite = tempSpriteHolder[0];
                                
                            }
                            else
                                StartCoroutine(FadeIn(fadeGO));


                        }
                        else if (currentIndex >= 8 && currentIndex < 12)

                            if (fadeGO.GetComponent<Image>().color.a > 0.5f)
                            {
                                StartCoroutine(FadeOut(fadeGO));
                                img.sprite = tempSpriteHolder[1];
                              
                            }
                            else
                                StartCoroutine(FadeIn(fadeGO));
                        else if (currentIndex >= 12 && currentIndex < 16)
                        {
                            if (fadeGO.GetComponent<Image>().color.a > 0.5f)
                            {
                                StartCoroutine(FadeOut(fadeGO));
                                img.sprite = tempSpriteHolder[2];
                            }
                            else
                                StartCoroutine(FadeIn(fadeGO));
                        }
                        else if (currentIndex >= 16 && currentIndex < openingLines.Count - 1)
                        {
                            if (fadeGO.GetComponent<Image>().color.a > 0.5f)
                            {
                                StartCoroutine(FadeOut(fadeGO));
                                img.sprite = tempSpriteHolder[3];
                            }
                            else
                                StartCoroutine(FadeIn(fadeGO));
                        }
                        currentIndex += 1;
                    }

                }
            }
        }
    }

    IEnumerator FadeIn(GameObject fadeObj)
    {
        float alpha = fadeObj.GetComponent<Image>().color.a;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * Speed;
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }
        Debug.Log("Fading In");
    }

    IEnumerator FadeOut(GameObject fadeObj)
    {
        float alpha = fadeObj.GetComponent<Image>().color.a;
        while (alpha > 1)
        {
            alpha -= Time.deltaTime * Speed;
        
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }
        Debug.Log("Fading Out");

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

