using UnityEngine;

[System.Serializable]
public class Player {
    public string user_id;
    public string password;
    public string name;
    public string email;
    public int total_score;
    public int star_point;

    public static Player instance;
    public static Player getInstance()
    {
        if (instance == null)
        {
            instance = new Player();
        }
        return instance;
    }
}
