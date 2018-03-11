using UnityEngine;
using System.Collections;

public class movemap : MonoBehaviour
{
    public GameObject tile;//바닥에 사용할 GameObject(프리팹).
    //private GameObject[] tiles;
    private int tileNum = 3;

    struct TileStruct
    {//타일 사용에 필요한 정보들을 담고있는 구조체.
        public GameObject obj;
        public Transform tf;
        public bool active;
        public Vector3 pos;
    }
    private TileStruct[] tiles;


    private Vector3 tileCenterVec;//기준점.
    private float tileGap = 14.9f;//블록 길이 차이.
    private float tileEndPoint = -19f;

    private Vector3 tempVec;//임시벡터.

    private float speed = 0.2f;//배경이나 오브젝트 이동속도.
    private int lastTileNum = 0;//현재 가장 뒤에 위치한 블록의 번호.

    //장애물추가
    public GameObject obstacle;
    private int obsNum = 10;
    struct ObstacleStruct
    {
        public GameObject obj;
        public bool active;
        public int parentTileNum;
        public obstacleInfo info;//obs 정보 스크립트.
    }
    private ObstacleStruct[] obss;

    //Star 추가.
    public GameObject Star;
    private int StarNum = 5;
    struct StarStruct
    {
        public GameObject obj;
        public bool active;
        public int parentTileNum;
    }
    private StarStruct[] starSets;
    void Awake()
    {
        tileCenterVec = new Vector3(0, -5.22f, 0);
        CreateTiles();
        CreateObss();//장애물추가
        CreateStar();
    }

    void FixedUpdate()
    {//블록이 계속하여 일정한 (주어진)속도로 이동하도록 만듭니다.
        for (int i = 0; i < tileNum; i++)
        {
            tiles[i].pos.x -= speed;
            if (tiles[i].pos.x > tileEndPoint)
            {//화면의 보이지 않는 지점으로 정해준 곳보다 더 가지 않았으면. (일반적)
                tiles[i].tf.position = tiles[i].pos;
            }
            else
            {//endPoint 넘어감 -> 가장 마지막 블록으로 위치시킴.
                DeleteObs(i);//넘어간 블록에 있던 장애물들 제거 처리.
                tiles[i].pos = tiles[lastTileNum].pos;
                tiles[i].pos.x += tileGap;
                if (lastTileNum > i)//i보다 크다면 아직 0.2f 감소가 안 된 상황이니 추가로 0.2 감소해줌.
                {
                    tiles[i].pos.x -= 0.2f;
                }

                tiles[i].tf.position = tiles[i].pos;//실제 위치 변경.
                AddedObs(i, 1);//일단 하나만 고정. 
                AddStar(i, 1);
                lastTileNum = i;//다음에 바꿀 때를 위해 마지막 블록 번호 바꿔줌.
            }
        }

    }
    void AddedObs(int tileN, int obsN)//타일 번호, 장애물수
    {//해당되는 타일에 장애물 추가해줌.
        tempVec.x = tiles[tileN].pos.x;//블록 중앙 테스트.
        tempVec.y = (float)-4.71;
        tempVec.z = 0;

        for (int i = 0; i < obsNum; i++)
        {
            if (!obss[i].active)
            {//비활성화 상태인 장애물을 만나면
                obss[i].obj.SetActive(true);//활성화해줌
                obss[i].active = true;
                obss[i].obj.transform.position = tempVec;

                obss[i].obj.transform.SetParent(tiles[tileN].tf);//부모 바꿔줌
                obss[i].parentTileNum = tileN;
                break;//하나 생성하면 for문 더이상 불필요
            }
        }
    }
    void DeleteObs(int tileN)//타일 제거할 때 호출.
    {
        for (int i = 0; i < obsNum; i++)
        {
            if (obss[i].active)
            {
                if (obss[i].parentTileNum == tileN)
                {
                    obss[i].obj.transform.parent = null;//부모없앰. -> 다시 obss오브젝트 하위로.
                    obss[i].parentTileNum = -1;
                    obss[i].obj.SetActive(false);//비활성화.
                    obss[i].active = false;
                }
            }
        }
    }

    void CreateTiles()
    {//반복 사용할 타일들을 생성합니다. (오브젝트풀)
        tempVec = tileCenterVec;//생성 위치 지정을 위한 최초 기준점.

        tiles = new TileStruct[tileNum];//총 3개 반복 사용.
        for (int i = 0; i < tileNum; i++)
        {//기본정보와 위치도 셋팅해줍니다.
            tiles[i].obj = Instantiate(tile, tempVec, Quaternion.identity) as GameObject;//생성.
            tiles[i].tf = tiles[i].obj.transform;
            tiles[i].pos = tiles[i].tf.position;
            tiles[i].active = true;

            tempVec.x += tileGap;//다음 블록은 tileGap만큼 플러한 위치에 만듦.
        }
        lastTileNum = 2;//처음엔 012순으로 위치.
    }
    void CreateObss()
    {//반복 사용할 장애물들을 생성합니다.
        obss = new ObstacleStruct[obsNum];
        for (int i = 0; i < obsNum; i++)
        {
            obss[i].obj = Instantiate(obstacle, new Vector3(-4, -5, 0), Quaternion.identity) as GameObject;
            obss[i].active = false;
            obss[i].parentTileNum = -1;
            obss[i].info = obss[i].obj.GetComponent<obstacleInfo>();//컴포넌트(스크립트)열어옴.
            obss[i].obj.SetActive(false);
        }

    }
    //별추가
    void CreateStar()
    {
        starSets = new StarStruct[StarNum];
        for (int i = 0; i < StarNum; i++)
        {//기본정보와 위치 셋팅
            starSets[i].obj = Instantiate(Star, new Vector3(-5, -2, 0), Quaternion.identity) as GameObject;//생성.
            starSets[i].active = false;
            starSets[i].parentTileNum = -1;
            starSets[i].obj.SetActive(false);
        }
    }
    void AddStar(int tileN, int level)
    {//해당되는 타일에 장애물 추가해줌.
        tempVec.x = tiles[tileN].pos.x;//블록 중앙 테스트.
        tempVec.y = -1f + 1.25f * level;
        tempVec.z = 0;

        for (int i = 0; i < StarNum; i++)
        {
            starSets[i].obj.SetActive(true);
            if (!starSets[i].active)
            {//비활성화 상태인 장애물을 만나면.
                starSets[i].obj.SetActive(true);//활성화 해주고
                starSets[i].active = true;
                starSets[i].obj.transform.position = tempVec;
                starSets[i].obj.transform.SetParent(tiles[tileN].tf);
                starSets[i].parentTileNum = tileN;
                break;
            }
        }

    }
}
