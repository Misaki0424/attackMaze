using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public Director director;
    public BFS bfs;

    public int z;
    public int x; //移動前地点
    public int x_; 
    public int z_; //移動後地点
    
    public GameObject nikukyu;
    public PlayerScript enemyHasPS;

    public GameObject wall_prefab;
    private GameObject wall;
    public int CountWall;

    public PlayerState state; //現在の状態
    public bool rotate; //壁が回転しているか(falseでV, trueでH)
    public int wall_z;
    public int wall_x; //壁の地点

    public Material WallColor;
    public Material WallColor_Fade;

    public GameObject SmokeEffect;
    //public GameObject MoveEffect;
    public GameObject CEffect;

    bool[,] wallVertical = new bool[8, 9];
    bool[,] wallhorizontal = new bool[9, 8];

    public AudioSource click;
    public AudioSource win;

    private float time = 2f;

    public enum PlayerState {
        Move,   // 動く
        Wall,   // 壁を設置
        Wait,    // 待機
        Goal     //ゴール
    }

    public void StartSetting(bool isTurn){
        CountWall = 5;
        //初期スポーン地点
        if (this.gameObject.tag == "Player_white"){
            x = 8;
            z = 4;
        }
        else if (this.gameObject.tag == "Player_black"){
            x = 0;
            z = 4;  
        }

        if(isTurn){
            state = PlayerState.Move;
        }else{
            state = PlayerState.Wait;
        }

        x_ = x;
        z_ = z;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Director.turn);
        //Debug.Log(this.gameObject.tag + ": " + state.ToString());
        time += Time.deltaTime;

        if(!director.keyflag && !director.titlegoflag){
            //猫の移動
            switch (state){
                case PlayerState.Move:
                    if(x_==x && z == z_) nikukyu.SetActive(false);
                    else  nikukyu.SetActive(true);
                    
                    //white cat
                    if (this.gameObject.tag == "Player_white"){
                        nikukyu.transform.position = new Vector3(Director.coordinate[x_,z_].x , 1.42f, Director.coordinate[x_,z_].z-0.1f);
                        nikukyu.transform.rotation = Quaternion.Euler(0, 90, 0); 

                        //左移動
                        if(Input.GetKeyDown (KeyCode.DownArrow)){
                            click.Play();
                            if(z > 0) { 
                                x_ = x;
                                z_ = z - 1;
                            }
                        }
                        //右移動
                        else if(Input.GetKeyDown (KeyCode.UpArrow)){
                            click.Play();
                            if(z < 8) { 
                                x_ = x;
                                z_ = z + 1;
                            }
                        }
                        //上移動
                        else if (Input.GetKeyDown (KeyCode.LeftArrow)){
                            click.Play();
                            if(x > 0) { 
                                x_ = x - 1;
                                z_ = z;
                            }
                        }
                        //下移動
                        else if (Input.GetKeyDown (KeyCode.RightArrow)){
                            click.Play();
                            if(x < 8) { 
                                x_ = x + 1;
                                z_ = z;
                            }        
                        }
                        //動く
                        if(Input.GetKeyDown (KeyCode.Space) && time>1f){
                            bool move = true;
                            click.Play();
                            if(!(z_==z&&x_==x)) {
                                turnChange();
                            }
                        }
                        //壁設置モードに切り替え
                        if(Input.GetKeyDown (KeyCode.RightShift)){
                            click.Play();
                            state = PlayerState.Wall;
                            nikukyu.transform.position = new Vector3(200f, 1.42f, 200f);
                            if(z==8){
                                wall = Instantiate(wall_prefab, Director.wallVertical[x-1,z-1], Quaternion.Euler(90, 0, 0));
                                wall_x = x-1;
                                wall_z = z-1;
                            }
                            else{
                                wall = Instantiate(wall_prefab, Director.wallVertical[x-1,z], Quaternion.Euler(90, 0, 0));
                                wall_x = x-1;
                                wall_z = z;
                            }

                            wall.GetComponent<MeshRenderer>().material = WallColor_Fade;
                            time = 0.9f;
                            rotate = false;
                        }
                    }

                    //black cat
                    else if (this.gameObject.tag == "Player_black"){
                        nikukyu.transform.rotation = Quaternion.Euler(0, 270, 0); 
                        nikukyu.transform.position = new Vector3(Director.coordinate[x_,z_].x , 1.42f, Director.coordinate[x_,z_].z-0.1f);

                        //左移動
                        if(Input.GetKeyDown (KeyCode.W)){
                            click.Play();
                            if(z < 8) { 
                                x_ = x;
                                z_ = z + 1;
                            }
                        }
                        //右移動
                        else if(Input.GetKeyDown (KeyCode.S)){
                            click.Play();
                            if(z > 0) {
                                x_ = x;
                                z_ = z - 1;
                            }
                        }
                        //上移動
                        else if (Input.GetKeyDown (KeyCode.D)){
                            click.Play();
                            if(x < 8) {
                                x_ = x + 1;
                                z_ = z;
                            }
                        }
                        //下移動
                        else if (Input.GetKeyDown (KeyCode.A)){
                            click.Play();
                            if(x > 0) {
                                x_ = x - 1;
                                z_ = z;
                            }
                        }
                        //動く
                        if(Input.GetKeyDown (KeyCode.Space) && time>1f){
                            click.Play();
                            bool move = true;
                            if(!(z_==z&&x_==x)) {
                                //障害物があったら動かない
                                turnChange();
                            }
                        }
                        //壁設置モードに切り替え
                        if(Input.GetKeyDown (KeyCode.LeftShift)){
                            click.Play();
                            state = PlayerState.Wall;
                            if(z==8){
                                wall = Instantiate(wall_prefab, Director.wallVertical[x,z-1], Quaternion.Euler(90, 0, 0));
                                wall_x = x;
                                wall_z = z-1;
                            }
                            else{
                                wall = Instantiate(wall_prefab, Director.wallVertical[x,z], Quaternion.Euler(90, 0, 0));
                                wall_x = x;
                                wall_z = z;
                            }
                            wall.GetComponent<MeshRenderer>().material = WallColor_Fade;

                            nikukyu.transform.position = new Vector3(200f, 1.42f, 200f);
                            rotate = false;
                            time = 0.9f;
                        }
                    }
                    break;

            //壁設置
            case PlayerState.Wall:
                if (this.gameObject.tag == "Player_white"){
                    if(rotate){
                        wall.transform.position = Director.wallHorizontal[wall_x, wall_z];
                    }
                    else{
                        wall.transform.position = Director.wallVertical[wall_x, wall_z];
                    }

                    //回転
                    if(Input.GetKeyDown (KeyCode.Slash)){
                        click.Play();
                        time = 0.9f;
                        if (rotate) {
                            wall.transform.rotation = Quaternion.Euler(90, 0, 0); 
                            rotate = false;
                        }
                        else {
                            wall.transform.rotation = Quaternion.Euler(90, 90, 0);
                            rotate = true;
                        }
                    }
                    // 右移動
                    if(Input.GetKeyDown (KeyCode.DownArrow)){
                        click.Play();
                        time = 0.9f;
                        if(wall_z > 0) wall_z--;
                    }
                    // 左移動
                    else if(Input.GetKeyDown (KeyCode.UpArrow)){
                        click.Play();
                        time = 0.9f;
                        if(wall_z < 7) wall_z++;
                    }
                    // 下移動
                    else if(Input.GetKeyDown (KeyCode.LeftArrow)){
                        click.Play();
                        time = 0.9f;
                        if(wall_x > 0) wall_x--;
                    }
                    // 上移動
                    else if(Input.GetKeyDown (KeyCode.RightArrow)){
                        click.Play();
                        time = 0.9f;
                        if(wall_x < 7) wall_x++;
                    }
                    //決定
                    else if (Input.GetKeyDown (KeyCode.Space) && time>1f){
                        click.Play();
                        if (CountWall>0){
                            turnChange();
                        }
                        else{
                            StartCoroutine(hide_Alart());
                        }
                    }
                    //うごくモードに切り替え
                    else if(Input.GetKeyDown (KeyCode.RightShift)){
                        click.Play();
                        state = PlayerState.Move;
                        Destroy(wall);
                    }
                }

                if (this.gameObject.tag == "Player_black"){
                    if(rotate){
                        wall.transform.position = Director.wallHorizontal[wall_x, wall_z];
                    }
                    else{
                        wall.transform.position = Director.wallVertical[wall_x, wall_z];
                    }

                    //回転
                    if(Input.GetKeyDown (KeyCode.Z)){
                        click.Play();
                        time = 0.9f;
                        if (rotate) {
                            wall.transform.rotation = Quaternion.Euler(90, 0, 0); 
                            rotate = false;

                        }
                        else {
                            wall.transform.rotation = Quaternion.Euler(90, -90, 0);
                            rotate = true;
                        }
                    }
                    //右移動
                    if(Input.GetKeyDown (KeyCode.S)){
                        click.Play();
                        time = 0.9f;
                        if(wall_z > 0) wall_z--;
                    }
                    //左移動
                    else if(Input.GetKeyDown (KeyCode.W)){
                        click.Play();
                        time = 0.9f;
                        if(wall_z < 7) wall_z++;
                    }
                    //下移動
                    else if (Input.GetKeyDown (KeyCode.A)){
                        click.Play();
                        time = 0.9f;
                        if(wall_x > 0) wall_x--;
                    }
                    //上移動
                    else if (Input.GetKeyDown (KeyCode.D)){
                        click.Play();
                        time = 0.9f;
                        if(wall_x < 7) wall_x++;
                    }
                    //決定
                    else if (Input.GetKeyDown (KeyCode.Space) && time>1f){
                        click.Play();
                        if (CountWall>0){
                            turnChange();
                        }
                        else{
                            StartCoroutine(hide_Alart());
                        }
                    }
                    //うごくモードに切り替え
                    else if(Input.GetKeyDown (KeyCode.LeftShift)){
                        click.Play();
                        state = PlayerState.Move;
                        Destroy(wall);
                    }
                }
                break;

            case PlayerState.Wait:
                if(this.gameObject.tag == "Player_white"){
                    if(x == 0){
                        state = PlayerState.Goal;
                    }
                }  
                else if(this.gameObject.tag == "Player_black"){
                    if(x == 8){
                        state = PlayerState.Goal;
                    }
                }
                break;

            case PlayerState.Goal:
                End.winner = this.gameObject.tag;
                enemyHasPS.state = PlayerState.Wait;
                if(this.gameObject.tag == "Player_black" && !Director.isFinish){
                    win.Play();
                    Director.isFinish = true;
                    Instantiate(CEffect, Director.coordinate[x_,z_], Quaternion.identity);
                    StartCoroutine(winner_Nikukyu_black());  
                }
                else if(this.gameObject.tag == "Player_white" && !Director.isFinish){
                    win.Play();
                    Director.isFinish = true;
                    Instantiate(CEffect, Director.coordinate[x_,z_], Quaternion.identity);
                    StartCoroutine(winner_Nikukyu_white()); 
                }
                break;
            }
        }
    }


    public void skip(){
        if(time > 0.1f) {
            time = 0f;
            nikukyu.transform.position = new Vector3(200f, 1.42f, 200f);
            
            if(state == PlayerState.Wall && wall != null){
                Destroy(wall);
            }
            
            x_ = x;
            z_ = z;
            Director.turn++;
  
            state = PlayerState.Wait;
            enemyHasPS.state = PlayerState.Move;
        }
    }

    void turnChange(){
        switch(state){
            case PlayerState.Wall:
                if(isSet()){
                    if(!rotate){
                        Director.wallVertical_bool[wall_x, wall_z] = true;
                        Director.wallVertical_bool[wall_x, wall_z+1] = true;
                    }
                    else{
                        Director.wallHorizontal_bool[wall_x, wall_z] = true;
                        Director.wallHorizontal_bool[wall_x+1, wall_z] = true;
                    }
                    Director.wallCenter[wall_x, wall_z] = true;

                    time = 0f;
                    CountWall--;
                    x_ = x;
                    z_ = z;
                    wall.transform.DOMove(new Vector3(wall.transform.position.x, 1.7f, wall.transform.position.z), 0.6f)
                        .SetEase(Ease.InOutQuart); //移動にイージング
                    wall.GetComponent<MeshRenderer>().material = WallColor;
                    GameObject sm = Instantiate(SmokeEffect, wall.transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
                    Destroy(sm, 3.0f);

                    state = PlayerState.Wait;
                    enemyHasPS.state = PlayerState.Move;
                    x = x_;
                    z = z_; 
                    Director.turn++;
                }
                else{
                    StartCoroutine(hide_Alart());
                }
                break;

            case PlayerState.Move:
                if (isMove()){
                    time = 0f;
                    nikukyu.transform.position = new Vector3(200f, 1.42f, 200f);
                    this.transform.DOMove(Director.coordinate[x_, z_], 1f);
                    if((this.gameObject.tag == "Player_black" && x_!=8) || (this.gameObject.tag == "Player_white" && x_!=0)){
                        //GameObject ef = Instantiate(MoveEffect, Director.coordinate[x_, z_], Quaternion.identity);
                        //Destroy(ef, 3.0f);
                    }

                    state = PlayerState.Wait;
                    enemyHasPS.state = PlayerState.Move;
                    x = x_;
                    z = z_;
                    Director.turn++;
                }
                break;
        }
    }

    //移動できるかの判定
    bool isMove(){
        if(x_ == enemyHasPS.x && z_ == enemyHasPS.z){
            return false;
        }
        else if (x>x_ && Director.wallVertical_bool[x_, z]){
            return false;
        }
        else if(z>z_ && Director.wallHorizontal_bool[x, z_]){
            return false;
        }
        else if(x<x_ && Director.wallVertical_bool[x, z]){
            return false;
        }
        else if(z<z_ && Director.wallHorizontal_bool[x, z]){
            return false;
        }
        return true;
    }

    //設置できるかの判定
    bool isSet(){
        bool myOK = false;
        bool enemyOK = false;
        if(!rotate){
            if(Director.wallVertical_bool[wall_x, wall_z] == true || Director.wallVertical_bool[wall_x, wall_z+1] == true || 
                Director.wallCenter[wall_x, wall_z] == true){
                return false;
            }
            myOK = bfs.searchBFS(false, wall_x, wall_z);
            enemyOK = enemyHasPS.bfs.searchBFS(false, wall_x, wall_z);
        }else{
            if(Director.wallHorizontal_bool[wall_x, wall_z] == true || Director.wallHorizontal_bool[wall_x+1, wall_z] == true ||
                Director.wallCenter[wall_x, wall_z] == true){
                return false;
            }
            myOK = bfs.searchBFS(true, wall_x, wall_z);
            enemyOK = enemyHasPS.bfs.searchBFS(true, wall_x, wall_z);
        }

        if(myOK && enemyOK){
            return true;
        }
        return false;
    }

    //待つ操作
    IEnumerator winner_Nikukyu_white(){
        for (int i = 0; i < 9; i++){
            GameObject nikukyu_ = Instantiate(nikukyu, Director.coordinate[x, z], Quaternion.Euler(0, 90, 0));
            nikukyu_.SetActive(true);
            nikukyu_.transform.position = new Vector3(Director.coordinate[0, i].x , 1.42f, Director.coordinate[0, i].z - 0.1f);
            yield return new WaitForSeconds(0.3f); 
        }
        SceneManager.LoadScene("End");
    }

    IEnumerator winner_Nikukyu_black(){
        for (int i = 0; i < 9; i++){
            GameObject nikukyu_ = Instantiate(nikukyu, Director.coordinate[x, z], Quaternion.Euler(0, 270, 0));
            nikukyu_.SetActive(true);
            nikukyu_.transform.position = new Vector3(Director.coordinate[8, i].x , 1.42f, Director.coordinate[8, i].z - 0.1f);
            yield return new WaitForSeconds(0.3f); 
        }
        SceneManager.LoadScene("End");
    }

    IEnumerator hide_Alart() {
        director.alart.alpha = 1f;
        yield return new WaitForSeconds(1f);
        director.alart.DOFade(0f, 0.5f);
    }

    //StartCoroutineは途中で止まれる(WaitForSeconds)関数を動かすときに使う
}
