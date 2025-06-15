using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    public static int turn; // turn%2 = 1(白), 0(黒)
    public static int first; // 先攻
    public static bool isFinish; // ゴール判定

    public static Vector3[,] coordinate = new Vector3[9,9];
    public static Vector3[,] wallVertical = new Vector3[8, 9];
    public static Vector3[,] wallHorizontal = new Vector3[9, 8];

    public static bool[,] wallVertical_bool = new bool[8, 9];
    public static bool[,] wallHorizontal_bool = new bool[9, 8];
    public static bool[,] wallCenter = new bool[8, 8];

    string[] turnStr = new string[]{"くろねこのばん","しろねこのばん"};
    
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI CountWall_black;
    public TextMeshProUGUI CountWall_white;

    public CanvasGroup alart; //GameObject型だと, DoFadeが使えない
    public Image sceneChangePanel;

    public PlayerScript[] players;

    public CanvasGroup key;
    public bool keyflag; 

    public CanvasGroup titlego;
    public bool titlegoflag; 

    // Start is called before the first frame update
    void Start() {
        Director.first = Random.Range(0, 1000);
        alart.alpha = 0f;
        key.alpha = 0f;
        titlego.alpha = 0f;
        
        Debug.Log("先行: "+turnStr[Director.first%2]);
        
        for(int i=0; i<9; i++){
            for(int j=0; j<9; j++){
                Director.coordinate[i,j] = new Vector3(18.08f+i*1.19f, 1.7f, 22.6f+j*1.19f);
            }
        }
        for(int i=0; i<8; i++){
            for(int j=0; j<9; j++){
                Director.wallVertical[i,j] = new Vector3(18.684f+i*1.19f, 2.2f, 23.1f+j*1.19f);
                Director.wallVertical_bool[i,j] = false;
            }
        }
        for(int i=0; i<9; i++){
            for(int j=0; j<8; j++){
                Director.wallHorizontal[i,j] = new Vector3(18.7f+i*1.19f, 2.2f, 23.13f+j*1.19f);
                Director.wallHorizontal_bool[i,j] = false;
            }
        }
        for(int i=0; i<8; i++){
            for(int j=0; j<8; j++){
                Director.wallCenter[i,j] = false;
            }
        }

        Director.turn = Director.first;

        Director.isFinish = false;
        keyflag = false;
        titlegoflag = false;

        players[Director.first%2].StartSetting(true);
        players[(Director.first+1)%2].StartSetting(false);
    } 

    void Update(){
        turnText.text = turnStr[Director.turn%2];
        CountWall_black.text = "のこりのかべ : " + players[0].CountWall + "こ";
        CountWall_white.text = "のこりのかべ : " + players[1].CountWall + "こ";

        if(Input.GetKeyDown (KeyCode.T)){
            if(keyflag){
                key.DOFade(0f, 0.5f);
            }else{
                key.DOFade(1f, 0.5f);
            }
            keyflag = !keyflag;
        }

        if(Input.GetKeyDown(KeyCode.Y) && !keyflag){
            if(players[turn % 2].state == PlayerScript.PlayerState.Move || 
               players[turn % 2].state == PlayerScript.PlayerState.Wall){
                players[turn % 2].skip();
            }
        }

        if(!titlegoflag){
            if(Input.GetKeyDown (KeyCode.R)){
                titlego.DOFade(1f, 0.5f);
                titlegoflag = !titlegoflag; 
            }
        }

        if(titlegoflag){
            if(Input.GetKeyDown (KeyCode.Y)){
                sceneChangePanel.DOFade(1f, 0.5f).OnComplete(() => {
                    SceneManager.LoadScene("start");
                });
            }
            if(Input.GetKeyDown (KeyCode.N)){
                titlego.DOFade(0f, 0.5f);
                titlegoflag = !titlegoflag; 
            }
        }
    }
}

    