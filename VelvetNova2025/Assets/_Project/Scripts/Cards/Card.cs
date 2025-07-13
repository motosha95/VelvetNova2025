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

    private bool isFlipped = true;
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
        AudioManager.Instance.PlayFlip();

    }

    public void SetMatched()
    {
        isMatched = true;

        StartCoroutine(HideCardAfterDelay(1f));

    }
    private IEnumerator<WaitForSeconds> HideCardAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        AudioManager.Instance.PlayMatch();

        frontFace.SetActive(false);
        backFace.SetActive(false);
       

    }
    public void ResetCard()
    {
      if(IsFlipped)
            animator.SetTrigger("Flip");

        isFlipped = false;
        isMatched = false;

    }

    public bool IsMatched => isMatched;
    public bool IsFlipped => isFlipped;
}
