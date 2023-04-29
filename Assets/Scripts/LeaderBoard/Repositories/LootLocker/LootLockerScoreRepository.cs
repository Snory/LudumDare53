using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootLockerScoreRepository : RepositoryBase
{
    int _leaderBoardId = 3195;
    string _memberId;

    public override void Add(ScoreEventData item)
    {
        StartCoroutine(AddScoreRoutine(item));    
    }


    private IEnumerator AddScoreRoutine(ScoreEventData item)
    {
        bool done = false;

        LootLockerSDKManager.SubmitScore(_memberId, item.ScoreData.Score, _leaderBoardId, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Couldn not upload score: " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    private void SetName()
    {
        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            return;
        }

        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                if (string.IsNullOrEmpty(response.name))
                {
                    LootLockerSDKManager.SetPlayerName(PlayerPrefs.GetString("PlayerName"), (response) =>
                    {
                        if (!response.success)
                        {
                            Debug.LogError("Could not set name " + response.Error);
                        }
                    });
                }
            }
            else
            {
                Debug.LogError("Could not retrieve player name " + response.Error);
            }
        });
    }

    public override IEnumerable<ScoreEventData> FindAll()
    {
        List<ScoreEventData> data = new List<ScoreEventData>();

          LootLockerSDKManager.GetScoreList(_leaderBoardId, 10, (response) =>
          {
              if (!response.success)
              {
                  Debug.LogError("Could not get score list " + response.Error);

              } else
              {
                  foreach(var item in response.items)
                  {
                      data.Add(new ScoreEventData(new ScoreData(item.score, item.player.name)));
                  }                  
              }

          });

        return data;
    }

    public override void Load()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.LogError("error starting LootLocker session " + response.Error);

                return;
            } else
            {
                Debug.Log("Lootlocker repository loaded");
            }

            _memberId = response.player_id.ToString();
        });

        SetName();
    }

    public override void Save()
    {
        LootLockerSDKManager.EndSession((response) =>
        {
            if (!response.success)
            {
                Debug.LogError("error ending LootLocker session " + response.Error);

                return;
            }
        });
    }


}
