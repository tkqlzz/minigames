using UnityEngine;

[System.Serializable]
public class Rank
{
    public int rank;
    public string userId;
    public int score;

    public static Rank instance;
    public static Rank getInstance()
    {
        if (instance == null)
        {
            instance = new Rank();
        }
        return instance;
    }
    public static Rank[] list;
}
