using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Setup")]
    public GameObject cardPrefab;
    public Transform cardParent;
    public GridLayoutGroup gridLayout;
    public List<Sprite> cardSprites;
    public Vector2Int gridSize = new Vector2Int(4, 3);

  

    private List<Card> cards = new();
    private List<Card> revealedCards = new();
    private int score = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        SetupGrid();
        GenerateCards();
        UpdateScore(0);
    }

    public void SetupGrid()
    {
        float width = cardParent.GetComponent<RectTransform>().rect.width;
        float height = cardParent.GetComponent<RectTransform>().rect.height;
        float cellWidth = width / gridSize.x;
        float cellHeight = height / gridSize.y;
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gridSize.x;
        gridLayout.cellSize = new Vector2(cellWidth - 10, cellHeight - 10);
    }

    public void GenerateCards()
    {
        List<int> cardIDs = new();
        for (int i = 0; i < (gridSize.x * gridSize.y) / 2; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }
        Shuffle(cardIDs);

        foreach (var id in cardIDs)
        {
            GameObject cardGO = Instantiate(cardPrefab, cardParent);
            Card card = cardGO.GetComponent<Card>();
            card.Initialize(id, cardSprites[id]);
            cards.Add(card);
        }
    }

    public void CardRevealed(Card card)
    {
        revealedCards.Add(card);

        if (revealedCards.Count == 2)
        {
            if (revealedCards[0].cardID == revealedCards[1].cardID)
            {
                revealedCards[0].SetMatched();
                revealedCards[1].SetMatched();
                UpdateScore(100);
                CheckGameOver();
            }
            else
            {
                UpdateScore(-10);
                StartCoroutine(ResetCardsAfterDelay(1f));
            }
            revealedCards.Clear();
        }
    }

    private IEnumerator<WaitForSeconds> ResetCardsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var card in cards)
        {
            if (!card.IsMatched)
                card.ResetCard();
        }
    }

    private void UpdateScore(int delta)
    {
        score += delta;
        UIManager.Instance.UpdateScore(score);
    }

    private void CheckGameOver()
    {
        foreach (var card in cards)
        {
            if (!card.IsMatched)
                return;
        }
        UIManager.Instance.ShowGameOver();
    }

    public bool CanFlipCard() => revealedCards.Count < 2;

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(0, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}
