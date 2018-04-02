using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Practice : MonoBehaviour
{

    public GameObject RankingBtnItem;
    public Transform Content;
    public List<RankingItem> ItemList;
    
    void Start()
    {
        //AddListItem();
        this.Binding();
    }

    private void Binding()
    {
        GameObject btnItemTemp;
        RankingBtnItem itemobjectTemp;

        foreach (RankingItem item in ItemList)
        {
            btnItemTemp = Instantiate(RankingBtnItem) as GameObject;
            itemobjectTemp = btnItemTemp.GetComponent<RankingBtnItem>();

            itemobjectTemp.UserName = item.UserName;
            itemobjectTemp.UserRecord = item.UserRecord;
            itemobjectTemp.Item.onClick = item.OnItemClick;

            btnItemTemp.transform.SetParent(Content);
        }
    }


    public void ItemClick_Result(int i)
    {
        Debug.Log("아이템 클릭: " + i + " - 해당 유저 정보 불러오기");
    }



    private void AddListItem()
    {
        Sprite[] image = {
        Resources.Load("Image/Face_Balance", typeof(Sprite)) as Sprite,
        Resources.Load("Image/Face_Light", typeof(Sprite)) as Sprite,
        Resources.Load("Image/Face_Health", typeof(Sprite)) as Sprite,
        Resources.Load("Image/Face_Speed", typeof(Sprite)) as Sprite  };

        RankingItem itemTemp;

        for (int i = 0; i < 20; ++i)
        {
            itemTemp = new RankingItem();

            itemTemp.UserName.text = "1234";
            itemTemp.UserRecord.text = "1234";
            itemTemp.UserPurigon = image[Random.Range(0, 4)];
            itemTemp.OnItemClick = new Button.ButtonClickedEvent();
            itemTemp.OnItemClick.AddListener(delegate { ItemClick_Result(i); });


            //리스트에 추가
            this.ItemList.Add(itemTemp);
        }


    }
}