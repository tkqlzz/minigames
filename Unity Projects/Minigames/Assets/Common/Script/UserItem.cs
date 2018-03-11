using UnityEngine;

[System.Serializable]
public class UserItem
{
    public string user_id;
    public int item_id;
    public int point;

    public static UserItem instance;
    public static UserItem getInstance()
    {
        if (instance == null)
        {
            instance = new UserItem();
        }
        return instance;
    }
    public static UserItem[] list;
}
