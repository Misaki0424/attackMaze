using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartDirector : MonoBehaviour
{
    public int step;
    public GameObject[] image;
    public GameObject[] text;
    public Image sceneChangePanel;
    public AudioSource click;

    public GameObject camera;
    public Button startButton;
    public Button trButton;
    public Image sceneChangePanel2;

    public GameObject canvas1;
    public GameObject canvas2;

    // Start is called before the first frame update
    void Start()
    {
        camera.transform.DOLocalRotate(new Vector3(0, 360f, 0), 240f, RotateMode.FastBeyond360)  
                 .SetEase(Ease.Linear)
                 .SetLoops(-1, LoopType.Restart);  

        startButton.onClick.AddListener(() => OnButtonClick("start"));
        trButton.onClick.AddListener(() => OnButtonClick("tr"));

        canvas1.SetActive(true);
        canvas2.SetActive(false);
        
        step = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(step>0 && (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A))){
            click.Play();
            step--;
        }
        if(step<7 && (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D))){
            step++;
            click.Play();
        }

        switch (step){
            case 0:
                image[5].SetActive(false);//A
                image[1].SetActive(false);
                image[0].SetActive(true);
                text[1].SetActive(false);
                text[0].SetActive(true);
                break;
            case 1: 
                image[4].SetActive(false);//にくきゅう
                image[5].SetActive(true); 
                image[0].SetActive(false);
                image[2].SetActive(false);
                image[1].SetActive(true);
                text[0].SetActive(false);
                text[2].SetActive(false);
                text[1].SetActive(true);
                break;
            case 2: 
                image[4].SetActive(true); 
                image[2].SetActive(false);
                image[1].SetActive(true);
                text[1].SetActive(false);
                text[3].SetActive(false);
                text[2].SetActive(true);
                break;
            case 3: 
                image[4].SetActive(false);
                image[1].SetActive(false);
                image[3].SetActive(false);
                image[2].SetActive(true);
                text[2].SetActive(false);
                text[4].SetActive(false);
                text[3].SetActive(true);
                break;
            case 4:
                text[3].SetActive(false);
                text[5].SetActive(false);
                text[4].SetActive(true);
                break;
            case 5:
                image[3].SetActive(false);
                image[2].SetActive(true);
                text[4].SetActive(false);
                text[6].SetActive(false);
                text[5].SetActive(true);
                break;
            case 6:
                image[6].SetActive(true);
                image[2].SetActive(true);
                image[7].SetActive(false);
                text[5].SetActive(false);
                text[7].SetActive(false);
                text[6].SetActive(true);
                break;
            case 7:
                image[6].SetActive(false);//D
                image[2].SetActive(false);
                image[3].SetActive(true);
                image[7].SetActive(true);
                text[6].SetActive(false);
                text[7].SetActive(true);

                if(Input.GetKeyDown (KeyCode.Space)){
                    sceneChangePanel2.DOFade(1f, 0.5f).OnComplete(() => {
                        SceneManager.LoadScene("main");
                    });
                }

                break;
        }
    }

    public void OnButtonClick(string buttonName) {
        if (buttonName == "start") {
            click.Play();
            sceneChangePanel.DOFade(1f, 0.5f).OnComplete(() => {
                SceneManager.LoadScene("main");
            });
        }
        else if (buttonName == "tr") {
            click.Play();
            sceneChangePanel.DOFade(1f, 0.5f).OnComplete(() => {
                Debug.Log("sceneChangePanel active = " + sceneChangePanel.gameObject.activeInHierarchy);
                canvas1.SetActive(false);
                canvas2.SetActive(true);
            });
        }
    }
}
