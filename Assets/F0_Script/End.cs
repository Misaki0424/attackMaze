using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public static string winner;
    public string Whoiswinner;

    public GameObject clearCat_white;
    public GameObject clearCat_black;
    public GameObject clearImage_white;
    public GameObject clearImage_black;

    public TextMeshProUGUI ClearText;
    public Image sceneChangePanel;

    private bool bool_;

    // Start is called before the first frame update
    void Start() {
        clearCat_white.SetActive(false);
        clearCat_black.SetActive(false);
        bool_ = true;
        ClearText.rectTransform.localScale = Vector3.zero;
        
        Color c = sceneChangePanel.color;
        c.a = 1f;
        sceneChangePanel.DOFade(0f, 1f);

    } 

    void Update(){
        if(bool_){
            if(winner == "Player_white"){
                clearImage_white.SetActive(true);
                clearCat_black.SetActive(false);
                clearCat_white.SetActive(true);
                Whoiswinner = "しろ";
            }
            else if(winner == "Player_black"){
                clearImage_black.SetActive(true);
                clearCat_black.SetActive(true);
                clearCat_white.SetActive(false);
                Whoiswinner = "くろ";
            }
            
            ClearText.GetComponent<RectTransform>()
                    .DOScale(1.0f, 0.6f)
                    .SetEase(Ease.OutElastic);
            clearCat_white.transform.DOLocalRotate(new Vector3(0, 360f, 0), 6f, RotateMode.FastBeyond360)  
                            .SetEase(Ease.Linear)  
                            .SetLoops(-1, LoopType.Restart); 
            clearCat_black.transform.DOLocalRotate(new Vector3(0, 360f, 0), 6f, RotateMode.FastBeyond360)  
                            .SetEase(Ease.Linear)  
                            .SetLoops(-1, LoopType.Restart); 

            ClearText.text = Whoiswinner+"のかち";

            bool_ = false;
        }
        if(Input.GetKeyDown (KeyCode.Y)){
            sceneChangePanel.DOFade(1f, 0.5f).OnComplete(() => {
                SceneManager.LoadScene("start");
            });
        }
    }
}

    