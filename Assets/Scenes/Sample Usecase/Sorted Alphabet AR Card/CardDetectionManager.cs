using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class CardDetectionManager : MonoBehaviour
{
    public TextMeshProUGUI cardsTMP;
    public GameObject cardUI;

    [Space]

    public List<CardInfo> cards;


    #region Card Detection Manager  Singleton
    private static CardDetectionManager instance;

    public static CardDetectionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CardDetectionManager>();
            }
            return instance;
        }
    }
    #endregion Card Detection Manager Singleton

    // Update is called once per frame
    void Update()
    {
        SortedCard();
    }

    public void SortedCard()
    {
        cardsTMP.text = "";

        var sortedCard = cards.OrderBy(c => c.location.position.x);

        foreach (var card in sortedCard)
        {
            cardsTMP.text += card.letter;
        }

        if (FindObjectOfType<AchievementManager>() != null)
        {
            AchievementManager.Instance.EarnAchievement(cardsTMP.text);
        }
        // Check database
        // Check If This Letter Contain

    }

    public void CheckLetter()
    {
        var sortedLetter = cards.OrderBy(c => c.location.position.x);

        string allLetter = "";

        foreach (var letter in sortedLetter)
        {
            allLetter += letter.letter.ToString();
        }

        if (FindObjectOfType<AchievementManager>() != null)
        {
            AchievementManager.Instance.EarnAchievement(allLetter);
        }

        Debug.Log(allLetter);
    }

    public void AddCard(CardInfo cardInfo)
    {
        Debug.Log(cardInfo.letter + " added");
        cards.Add(cardInfo);
    }

    public void RemoveCard(CardInfo cardInfo)
    {
        Debug.Log(cardInfo.letter + " removed");
        cards.Remove(cardInfo);
    }

    public void OpenCloseUI()
    {
        cardUI.SetActive(!cardUI.activeSelf);
    }
}

[System.Serializable]
public class CardInfo
{
    public string letter;

    public Transform location;
}
