using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWWManager : MonoBehaviour {
    public static WWWManager instance;
    private static GameObject container; // instance를 생성하기 위한 컨테이너
    private Result res;

    public string serverIp = "http://ec2-13-124-8-62.ap-northeast-2.compute.amazonaws.com";

    // 싱글톤 instance object 생성
    public static WWWManager getInstance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "WWWManager";
            instance = container.AddComponent(typeof(WWWManager)) as WWWManager;
        }
        return instance;
    }

    public WWW GET(string url)
    {
        WWW www = new WWW(url);
        StartCoroutine("WaitForRequest", www);
        return www;
    }

    public WWW POST(string url, IDictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);

        StartCoroutine("WaitForRequest", www);
        return www;
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
    }

    public int DoLogin(string userId, string password)
    {
        string url = serverIp  + "/users/" + userId + "/login";
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("password", password);

        WWW www = POST(url, data);
        while (!www.isDone) { }
        
        if (www.text == "")
        {
            return HTTPErrorCode.CONNECTION_ERROR;
        }

        res = JsonUtility.FromJson<Result>(www.text);

        if (res.error == HTTPErrorCode.SUCCESS)
        {
            Player.instance = JsonUtility.FromJson<Player>(res.data);
        }
        return res.error;
    }

    public int GetUserScore(string userId)
    {
        string url = serverIp + "/users/" + userId + "/scores";
        WWW www = GET(url);
        while (!www.isDone) { }

        if (www.text == "")
        {
            return HTTPErrorCode.CONNECTION_ERROR;
        }

        res = JsonUtility.FromJson<Result>(www.text);
        
        if (res.error == HTTPErrorCode.SUCCESS)
        {
            UserScore.list = JsonHelper.FromJson<UserScore>(res.data);
        }
        return res.error;
    }

    public int GetUserItem(string userId)
    {
        string url = serverIp + "/users/" + userId + "/items";
        WWW www = GET(url);
        while (!www.isDone) { }

        if (www.text == "")
        {
            return HTTPErrorCode.CONNECTION_ERROR;
        }

        res = JsonUtility.FromJson<Result>(www.text);

        if (res.error == HTTPErrorCode.SUCCESS)
        {
            UserItem.list = JsonHelper.FromJson<UserItem>(res.data);
        }
        return res.error;
    }

    public int IdMultipleCheck(string userId)
    {
        string url = serverIp + "/users/" + userId;
        WWW www = GET(url);
        while (!www.isDone) { }

        if (www.text == "")
        {
            return HTTPErrorCode.CONNECTION_ERROR;
        }

        res = JsonUtility.FromJson<Result>(www.text);

        if (res.error == HTTPErrorCode.SUCCESS)
        {
            if (res.data == "")
            {
                return HTTPErrorCode.SUCCESS;
            }
            else
            {
                return HTTPErrorCode.NULL_ERROR;
            }
        }
        return res.error;
    }

    public int DoSignUp(string userId, string password, string name, string email)
    {
        string url = serverIp + "/users/" + userId + "/join";
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("password", password);
        data.Add("name", name);
        data.Add("email", email);

        WWW www = POST(url, data);
        while (!www.isDone) { }

        if (www.text == "")
        {
            return HTTPErrorCode.CONNECTION_ERROR;
        }

        res = JsonUtility.FromJson<Result>(www.text);

        if (res.error == HTTPErrorCode.SUCCESS)
        {
            Player.instance = JsonUtility.FromJson<Player>(res.data);
            UserScore.list = new UserScore[0];
            UserItem.list = new UserItem[0];
        }
        return res.error;
    }

    public int DoItemUse(string userId, int itemId)
    {
        string url = serverIp + "/users/" + userId + "/use/item";
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("itemId", itemId.ToString());
        WWW www = POST(url, data);
        while (!www.isDone) { }

        if (www.text == "")
        {
            return HTTPErrorCode.CONNECTION_ERROR;
        }

        res = JsonUtility.FromJson<Result>(www.text);
        return res.error;
    }

    public int DoBuyItem(string userId, int itemId, int point, int itemCoset)
    {
        string url = serverIp + "/users/" + Player.instance.user_id + "/buy/item";
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("itemId", itemId.ToString());
        data.Add("point", point.ToString());
        data.Add("itemCost", itemCoset.ToString());

        WWW www = POST(url, data);
        while (!www.isDone) { }

        if (www.text == "")
        {
            return HTTPErrorCode.CONNECTION_ERROR;
        }

        res = JsonUtility.FromJson<Result>(www.text);
        
        if (res.error == HTTPErrorCode.SUCCESS)
        {
            // refresh
            Player.instance.star_point -= 10;

            bool hasItem = false;
            foreach (UserItem userItem in UserItem.list)
            {
                if (userItem.item_id == itemId)
                {
                    userItem.point = point;
                    hasItem = true;
                    break;
                }
            }

            if (!hasItem)
            {
                Array.Resize<UserItem>(ref UserItem.list, UserItem.list.Length + 1);
                UserItem.list[UserItem.list.Length - 1] = new UserItem();
                UserItem.list[UserItem.list.Length - 1].user_id = userId;
                UserItem.list[UserItem.list.Length - 1].item_id = itemId;
                UserItem.list[UserItem.list.Length - 1].point = point;
            }

        }
        return res.error;
    }

    public string GetRank(string userId, string gameName)
    {
        string url = serverIp + "/users/" + userId + "/rank";
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("gameName", gameName);

        WWW www = POST(url, data);
        while (!www.isDone) { }
        return www.text;
    }

    public string GetAllRank(string gameName)
    {
        string url = serverIp + "/leaderboard/" + gameName;

        WWW www = GET(url);
        while (!www.isDone) { }

        return www.text;

    }

    public int SendScore(string gameName, int score)
    {
        return SendScore(gameName, score, Player.instance.user_id, true);
    }
    public int SendScore(string gameName, int score, string userid)
    {
        return SendScore(gameName, score, userid, true);
    }
    public int SendScore(string gameName, int score, string userid, bool isRefresh)
    {
        string url = serverIp + "/users/" + userid + "/send/score";
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("gameName", gameName);
        data.Add("score", score.ToString());

        WWW www = POST(url, data);
        while (!www.isDone) { }

        res = JsonUtility.FromJson<Result>(www.text);
        if (res.error != HTTPErrorCode.SUCCESS)
            return res.error;

        if (isRefresh)
        {
            int beforeScore = 0;
            bool hasGameScore = false;
            foreach (UserScore us in UserScore.list)
            {
                if (us.game_name == gameName)
                {
                    beforeScore = us.score;
                    us.score = score;
                    hasGameScore = true;
                    break;
                }
            }

            Player.instance.total_score += (score - beforeScore);
            if (!hasGameScore)
            {
                Array.Resize<UserScore>(ref UserScore.list, UserScore.list.Length + 1);
                UserScore.list[UserScore.list.Length - 1] = new UserScore();
                UserScore.list[UserScore.list.Length - 1].user_id = userid;
                UserScore.list[UserScore.list.Length - 1].game_name = gameName;
                UserScore.list[UserScore.list.Length - 1].score = score;
            }
                
        }
        return res.error;
    }

    public int AddStarPoint(int point)
    {
        return AddStarPoint(point, Player.instance.user_id, true);
    }
    public int AddStarPoint(int point, string userid, bool isRefresh)
    {
        string url = serverIp + "/users/" + userid + "/add/StarPoint";
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("point", point.ToString());

        WWW www = POST(url, data);
        while (!www.isDone) { }

        res = JsonUtility.FromJson<Result>(www.text);
        if (res.error != HTTPErrorCode.SUCCESS)
            return res.error;

        if (isRefresh)
        {
            Player.instance.star_point += point;
        }

        return res.error;

    }

    void test()
    {
        WWWManager www = WWWManager.getInstance();

        // GET Test
        string url = serverIp + "/users/jinwoo";
        www.GET(url);

        // POST Test
        url = serverIp + "/users/test/rank";
        IDictionary<string, string> data = new Dictionary<string, string>();
        data.Add("gameName", "단어외우기");
        www.POST(url, data);
    }
}
