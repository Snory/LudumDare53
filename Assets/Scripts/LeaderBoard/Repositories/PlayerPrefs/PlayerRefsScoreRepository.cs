using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRefsScoreRepository : RepositoryBase
{

    private List<ScoreEventData> _results;

    public override void Add(ScoreEventData item)
    {
        _results.Add(item);

    }

    public override IEnumerable<ScoreEventData> FindAll()
    {
        return _results.Select(item => new ScoreEventData(item.ScoreData));
    }

    public override void Load()
    {
        List<ScoreData> dataList = new List<ScoreData>();
        if (PlayerPrefs.HasKey("Score"))
        {
            //rozparsovat json
            string data = PlayerPrefs.GetString("Score");

            //narvat do results   
            dataList = JsonUtility.FromJson<SerializableList<ScoreData>>(data).WrappedList;
            _results = dataList.Select(item => new ScoreEventData(item)).ToList();
        } else
        {
            _results = new List<ScoreEventData>();
        }                
    }

    public override void Save()
    {
        SerializableList<ScoreData> dataList = new SerializableList<ScoreData>();
        dataList.WrappedList = _results.Select(item => item.ScoreData).ToList();

        if(dataList.WrappedList != null)
        {
            string data = JsonUtility.ToJson(dataList);
            Debug.Log(data);
            PlayerPrefs.SetString("Score", data);
        }
        PlayerPrefs.Save(); //to be sure, but it should be called on quit anyway
    }
}
