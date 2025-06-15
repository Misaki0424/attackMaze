using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : MonoBehaviour
{
    public PlayerScript playerscript_BFS;
    public Director Director_BFS;
    public int[] dx = { 1, -1, 0, 0 };
    public int[] dz = { 0, 0, 1, -1 };
    Queue<(int x, int z)> queue = new Queue<(int x, int z)>(); //先に入ったものが先に出る
    public bool[,] visited = new bool[9,9];
    int xMove, zMove; //予測した移動後のマス
    public bool[,] BFS_wallVertical_bool = new bool[8, 9];
    public bool[,] BFS_wallHorizontal_bool = new bool[9, 8];

    public bool searchBFS(bool rotate, int wall_x, int wall_z){ ///ゴールに届くならtrue
        for(int i=0; i<9; i++){
            for(int j=0; j<9; j++){
                visited[i,j] = false;
            }
        }

        //仮のリストの初期化
        for(int i=0; i<8; i++){
            for(int j=0; j<9; j++){
                BFS_wallVertical_bool[i, j] = Director.wallVertical_bool[i,j];
            }
        }
        if(!rotate){
            BFS_wallVertical_bool[wall_x, wall_z] = true;
            BFS_wallVertical_bool[wall_x, wall_z+1] = true;
        }
        for(int i=0; i<9; i++){
            for(int j=0; j<8; j++){
                BFS_wallHorizontal_bool[i, j] = Director.wallHorizontal_bool[i,j];
            }
        }
        if(rotate){
            BFS_wallHorizontal_bool[wall_x, wall_z] = true;
            BFS_wallHorizontal_bool[wall_x+1, wall_z] = true;
        }

        queue.Clear(); //前のをクリアしておく
        queue.Enqueue((playerscript_BFS.x_, playerscript_BFS.z_)); //入れる
        visited[playerscript_BFS.x_, playerscript_BFS.z_] = true;

        while(queue.Count>0){
            (int x, int z) = queue.Dequeue(); //取り出す

            //ゴールついたらTrue
            if (this.gameObject.tag == "Player_white"){
                if(x == 0){
                    return true; 
                }
            }
            if (this.gameObject.tag == "Player_black"){
                if(x == 8){
                    return true; 
                }
            }
            for(int i = 0; i < 4; i++){
                xMove = x + dx[i];
                zMove = z + dz[i];
                if(xMove >= 0 && xMove < 9 && zMove >= 0 && zMove < 9 && !visited[xMove, zMove]){
                    if(!wallis(x, z, xMove, zMove)){ // 壁がなければ移動OK
                        queue.Enqueue((xMove, zMove)); // 次の位置を追加
                        visited[xMove, zMove] = true; // 訪問済みにマーク
                    }
                }
            }
        }
        return false;
    }

    bool wallis(int x, int z, int x_, int z_){
        if(x<x_) { //x+に動いたとき
            return BFS_wallVertical_bool[x,z];
        }
        else if(x>x_) { //x-に動いたとき
            return BFS_wallVertical_bool[x-1,z];
        }
        else if(z<z_) { //z+に動いたとき
            return BFS_wallHorizontal_bool[x,z];
        }
        else if(z>z_) { //z-に動いたとき
            return BFS_wallHorizontal_bool[x,z-1];
        }
        return false;
    }
}
