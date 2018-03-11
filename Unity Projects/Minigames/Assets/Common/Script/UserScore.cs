using UnityEngine;

[System.Serializable]
public class UserScore
{
    public string user_id;
    public string game_name;
    public int score;

    public static UserScore instance;
    public static UserScore getInstance()
    {
        if (instance == null)
        {
            instance = new UserScore();
        }
        return instance;
    }
    public static UserScore[] list;
}
