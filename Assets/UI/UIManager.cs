using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public DataManagement dataManagement;
    public PlayerState state;

    public GameObject UIWindow;
    SkillWindow skillWindow; public bool isOnUIWindow; public Sprite[] skillSprite;

    public int windowNumber;
    private void Awake() {
        isOnUIWindow = UIWindow.activeSelf;
        windowNumber = 1;

        skillWindow = new SkillWindow(gameObject);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)){
            if(!state.isInteractive && !isOnUIWindow){
                UIWindow.SetActive(true);
                isOnUIWindow = true;
                state.isInteractive = true;
            }else if(isOnUIWindow){
                UIWindow.SetActive(false);
                isOnUIWindow = false;
                state.isInteractive = false;
            }
        }
        if(isOnUIWindow && windowNumber == 1)
            skillWindow.Update();
    }

    public void UILoad(){
        skillWindow.skillLoad();
    }

    class SkillWindow{
        UIManager uIManager;
        GameObject cursor;

        Image[,] slotImage; 

        int col, row;

        public SkillWindow(GameObject gameObject){
            uIManager = gameObject.GetComponent<UIManager>();
            cursor = gameObject.transform.Find("UIWindow").Find("board0").Find("cursor").gameObject;
            cursor.transform.localPosition = new Vector3(-180, 75, 0);
            slotImage = new Image[4, 15];
            col = 0; row = 0;

            Transform board0Trans = uIManager.UIWindow.transform.Find("board0");
            Transform tempTrans;
            for(int i = 0; i < 4; i++){
                int j = 0;
                tempTrans = board0Trans.Find("board0"+i);
                foreach(Transform trans in tempTrans){
                    slotImage[i,j++] = trans.Find("Image").GetComponent<Image>();
                }
            }
        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.RightArrow) && row < 7){
                if (row == 2 || row == 4) cursor.transform.localPosition += new Vector3(80, 0, 0);
                else cursor.transform.localPosition += new Vector3(40, 0, 0);
                row++;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && row > 0){
                if (row == 3 || row == 5) cursor.transform.localPosition += new Vector3(-80, 0, 0);
                else cursor.transform.localPosition += new Vector3(-40, 0, 0);
                row--;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && col > 0){
                cursor.transform.localPosition += new Vector3(0, 40, 0);
                col--;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && col < 4){
                cursor.transform.localPosition += new Vector3(0, -40, 0);
                col++;
            }
        }
    
        public void skillLoad(){
            for(int i = 0; i < 40; i++){
                int num0, num1;
                if(i < 15){
                    num0 = 0; num1 = i;
                }else if(i < 30){
                    num0 = 1; num1 = i-15;
                }else if(i < 35){
                    num0 = 2; num1 = i-30;
                }else{
                    num0 = 3; num1 = i-35;
                }

                if(uIManager.state.dataManagement.gameData.coreIsContain[i]){
                    slotImage[num0, num1].sprite = uIManager.skillSprite[uIManager.state.dataManagement.gameData.coreNumber[i]];
                    slotImage[num0, num1].enabled = true;
                }
                if(uIManager.state.dataManagement.gameData.coreIsTranslucent[i])
                    slotImage[num0, num1].color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
}


