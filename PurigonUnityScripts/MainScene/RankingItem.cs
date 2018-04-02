using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


[Serializable]
public class RankingItem
{
    public Text UserName;
    public Text UserRecord;
    public Sprite UserPurigon;

    public Button.ButtonClickedEvent OnItemClick;
}