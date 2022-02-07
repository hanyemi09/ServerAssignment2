using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLeaderboard : MonoBehaviour
{

    public GameObject m_LeaderBoardPanel;
    public GameObject m_ListingPrefab;
    public Transform m_ListingContainer;
    public void GetLeaderBoard()
    {
        var requestLeaderBoard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "PlayerHighScore", MaxResultsCount = 20 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderBoard, OnGetLeaderBoard, OnErrorLeaderBoard);
    }

    void OnGetLeaderBoard(GetLeaderboardResult result)
    {
        m_LeaderBoardPanel.SetActive(true);
        Debug.Log(result.Leaderboard[0].StatValue);
        foreach(PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject tempListing = Instantiate(m_ListingPrefab, m_ListingContainer);
            LeaderboardListing LL = tempListing.GetComponent<LeaderboardListing>();
            LL.playerName.text = player.DisplayName;
            LL.playerScore.text = player.StatValue.ToString();
            Debug.Log(player.DisplayName + ": " + player.StatValue);
        }
    }

    public void CloseLeaderboardPanel()
    {
        m_LeaderBoardPanel.SetActive(false);
        for(int i = m_ListingContainer.childCount - 1; i >=0;i--)
        {
            Destroy(m_ListingContainer.GetChild(i).gameObject);
        }
    }
    
    void OnErrorLeaderBoard(PlayFabError error)
    {
        Debug.Log("Unable to get Leaderboard ");
    }
}
