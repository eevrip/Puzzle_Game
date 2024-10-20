using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextAnimation : MonoBehaviour
{
    [SerializeField] private Sprite img1;
    [SerializeField] private Sprite img2;
    [SerializeField] private Sprite img3;
    private Image currentImg;

    public void Start()
    {
        currentImg = GetComponent<Image>();
    }
   
    public void SetImage(bool tilted)
    {
        if (tilted)
        {
            currentImg.sprite = img2;
        }
        else
            currentImg.sprite = img1;
    }
    public void SetImageTriple(float num) 
    {
        if(num <= 0.25)
        {
            currentImg.sprite = img1;
        }
        else if(num >=0.75) 
        {
             currentImg.sprite=img3;
        }
        else
        {
            currentImg.sprite = img2;
        }
    }
}
