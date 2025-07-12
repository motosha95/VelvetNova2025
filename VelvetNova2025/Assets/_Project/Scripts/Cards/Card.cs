using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardID { get; private set; }

    [Header("Card Setup")]
    [SerializeField] Image frontImage;
    [SerializeField] GameObject frontFace;
    [SerializeField] GameObject backFace;

    [Header("Animation")]
    public Animator animator;

    private bool isFlipped = false;
    private bool isMatched = false;

    public void Initialize(int id, Sprite frontSprite)
    {
        cardID = id;
        frontImage.sprite = frontSprite;
        ResetCard();
    }

    public void OnClick()
    {
        if (isFlipped || isMatched || !GameManager.Instance.CanFlipCard()) return;
        Flip(true);
        GameManager.Instance.CardRevealed(this);
    }

    public void Flip(bool showFront)
    {
        isFlipped = showFront;
        animator.SetTrigger("Flip");
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public void ResetCard()
    {
        isFlipped = false;
        isMatched = false;
        frontFace.SetActive(false);
        backFace.SetActive(true);
    }

    public bool IsMatched => isMatched;
    public bool IsFlipped => isFlipped;
}
