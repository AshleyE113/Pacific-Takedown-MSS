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

    void Start()
    {
        img = GetComponent<Image>();
        
    }

    void Update()
    {
        for (int i = 0; i < tempSpriteHolder.Count; i++)
        {
            storySprites[i].sprite = tempSpriteHolder[i];
            Debug.Log("For loop");
        }

        if (goToNext == false)
        {
            img.color = Color.black;
            StartCoroutine(TextScroll(openingLines[0]));
            goToNext = true;
        }
        else
            img.color = Color.white;

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
                            if (img.color.a > 0.5f)
                            {

                                StartCoroutine(FadeOut());
                                //storySprites[0].sprite = tempSpriteHolder[0];
                                img.sprite = storySprites[0].sprite;
                            }
                            else
                                StartCoroutine(FadeIn());
                        }
                        else if (currentIndex >= 8 && currentIndex < 12)

                            if (img.color.a > 0.5f)
                            {
                                StartCoroutine(FadeOut());
                                //storySprites[1].sprite = tempSpriteHolder[1];
                                img.sprite = storySprites[1].sprite;
                            }
                            else
                                StartCoroutine(FadeIn());
                        else if (currentIndex >= 12 && currentIndex < 16)
                        {
                            if (img.color.a > 0.5f)
                            {
                                StartCoroutine(FadeOut());
                                //storySprites[2].sprite = tempSpriteHolder[2];
                                img.sprite = storySprites[2].sprite;
                            }
                            else
                                StartCoroutine(FadeIn());
                        }
                        else if (currentIndex >= 16 && currentIndex < openingLines.Count - 1)
                        {
                            if (img.color.a > 0.5f)
                            {
                                StartCoroutine(FadeOut());
                               // storySprites[3].sprite = tempSpriteHolder[3];
                                img.sprite = storySprites[3].sprite;
                            }
                            else
                                StartCoroutine(FadeIn());
                        }
                        currentIndex += 1;
                    }

                }
            }
        }
    }

    IEnumerator FadeIn()
    {
        float alpha = GetComponent<Image>().color.a;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * Speed;
            for (int i = 0; i < storySprites.Count; i++)
            {
                storySprites[i].color = new Color(storySprites[i].color.r, storySprites[i].color.g, storySprites[i].color.b, alpha);
            }
            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        float alpha = GetComponent<Image>().color.a;
        while (alpha > 1)
        {
            alpha -= Time.deltaTime * Speed;
            for (int i = 0; i < storySprites.Count; i++)
            {
                storySprites[i].color = new Color(storySprites[i].color.r, storySprites[i].color.g, storySprites[i].color.b, alpha);
            }
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

