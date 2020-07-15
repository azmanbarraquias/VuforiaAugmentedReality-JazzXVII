using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public CardInfo cardInfo;

    public void SendLocation()
    {
        CardDetectionManager.Instance.AddCard(cardInfo);
    }

    public void RemoveLocation()
    {
        CardDetectionManager.Instance.RemoveCard(cardInfo);
    }
}
