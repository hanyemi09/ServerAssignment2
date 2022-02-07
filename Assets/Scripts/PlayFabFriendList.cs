using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayFab;
using PlayFab.ClientModels;

public class PlayFabFriendList : MonoBehaviour
{
    List<FriendInfo> _friends = null;
    public GameObject m_FriendListPanel;
    public GameObject m_ListingPrefab;
    public Transform m_ListingContainer;
    public InputField m_EmailInputFriend;
    void GetFriends()
    {
        m_FriendListPanel.gameObject.SetActive(true);
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        }, result => {
            _friends = result.Friends;
            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);
    }
    void DisplayFriends(List<FriendInfo> friendsCache)
    {
        Debug.Log(string.Format("There are {0} friend(s)", friendsCache.Count));
        friendsCache.ForEach(f => Debug.Log(f.FriendPlayFabId));
        foreach(FriendInfo f in friendsCache)
        {
            GameObject listing = Instantiate(m_ListingPrefab, m_ListingContainer);
            LeaderboardListing LL = listing.GetComponent<LeaderboardListing>();
            LL.playerName.text = f.TitleDisplayName;
        }
    }

    public void CloseFriendListPanel()
    {
        m_FriendListPanel.SetActive(false);
        for (int i = m_ListingContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(m_ListingContainer.GetChild(i).gameObject);
        }
    }

    void DisplayPlayFabError(PlayFabError error) { Debug.Log(error.GenerateErrorReport()); }

    public void OnGetFriendsListClicked()
    {
        GetFriends();
    }

    enum FriendIdType { PlayFabId, Username, Email, DisplayName };
    void AddFriend(FriendIdType idType, string friendId)
    {
        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;
                break;
        }
        // Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, result => {
            Debug.Log("Friend added successfully!");
        }, DisplayPlayFabError);
    }

    public void OnAddFriendClicked()
    {
        // second parameter should be another player account in your title
        AddFriend(FriendIdType.Email, m_EmailInputFriend.text);
    }
}
