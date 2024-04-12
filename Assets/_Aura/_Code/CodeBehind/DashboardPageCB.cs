using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class DashboardPageCB : MonoBehaviour
{
    [Header("REST URL")]
    [SerializeField] private string getPlayersUrl;

    [Header("UI Elements")]
    [SerializeField] private GameObject listItemObj;
    [SerializeField] private RectTransform itemParent;
    [SerializeField] private TMP_Text debugText;

    //cache of list items spawned on to the scene 
    private List<GameObject> itemsInScene = new List<GameObject>();

    //cache of all players retrieved from database
    private List<Player> retrievedPlayers = new List<Player>();
    private void Start()
    {
        GetPlayers();
    }


    /// <summary>
    /// Retrieves players from database
    /// </summary>
    private void GetPlayers()
    {
        PlayerStatsManager.Instance.GetPlayers((players) =>
        {
            SetUpLeaderBoard(players);

        });

    }

    /// <summary>
    /// Refreshes and repopulates leaderboard scroll list
    /// using retrieved players from database
    /// </summary>
    private void SetUpLeaderBoard(List<Player> _players)
    {
        //sort players by highest score
        var orderedPlayers = _players.OrderByDescending(p => p.HighScore);

        RefreshList();

        //single instance cache of a gameobject to conserve memory in the loop
        GameObject listObj;

        //instantiate a leaderboard list item for each player and 
        //call SetUpItem() on each list item to set it up
        foreach (var player in orderedPlayers)
        {
            listObj = Instantiate(listItemObj);
            listObj.transform.SetParent(itemParent, false);
            listObj.GetComponent<LeaderBoardItemCB>().SetUpItem(player);

            itemsInScene.Add(listObj);
        }
    }


    /// <summary>
    /// Refreshes leader board before
    /// repopulating
    /// </summary>
    private void RefreshList()
    {
        if (itemsInScene.Count > 0)
        {
            foreach (var item in itemsInScene)
            {
                Destroy(item);
            }
            itemsInScene.Clear();
        }
    }
}
