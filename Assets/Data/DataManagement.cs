using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DataManagement : MonoBehaviour
{
    public UnityEvent loadEvent;
    public string saveName;

    public PlayerState playerState;
    public DropSystem dropSystem;
    public InventoryManager invenManager; public GameObject inventoryObj;

    static GameObject _container;
    static GameObject Container{
        get{
            return _container;
        }
    }
    static DataManagement _instance;
    public static DataManagement Instance{
        get{
            if(!_instance){
                _container = new GameObject();
                _container.name = "DataManagement";
                _instance = _container.AddComponent(typeof(DataManagement)) as DataManagement;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    public string DataResetFIleName = "121220812.json";
    public string GameDataFile = "21412812Data";

    private void Start() {
        LoadGameData(saveName);
    }

    public GameData _gameData;
    public GameData gameData{
        get{
            if(_gameData == null){
                LoadGameData("0");
                SaveGameData("0");
            }
            return _gameData;
        }
    }

    public void LoadGameData(string saveName_){
        string filePath = Application.persistentDataPath + GameDataFile + saveName_ + ".json";

        if(File.Exists(filePath)){
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }else{
            //처음 게임 시작시 설정
            _gameData = new GameData();

            _gameData.mapCode0 = 0;
            _gameData.mapCode1 = 0;
            _gameData.spawnX = -34.5f;
            _gameData.spawnY = -1f;

            _gameData.invenCount = 5; _gameData.invenItemCode = new int[5]; _gameData.invenItemCount = new int[5];
            for(int i = 0; i < 5; i++){
                _gameData.invenItemCode[i] = 0;
                _gameData.invenItemCount[i] = 0;
            }

            _gameData.coreIsContain = new bool[40]; _gameData.coreIsTranslucent = new bool[40]; _gameData.coreNumber = new int[40];
            for(int i = 0; i < 40; i++){
                _gameData.coreIsContain[i] = false;
                _gameData.coreIsTranslucent[i] = false;
                _gameData.coreNumber[i] = 0;
            }
        }

        dropSystem.getGold(0);
        invenManager.invenLoad();
        playerState.respawn();
        loadEvent.Invoke();
        Debug.Log(filePath);
    }

    public Text text;

    public void SaveGameData(string saveName_){
        _gameData.mapCode0 = playerState.CCtrl.mapCode[0]; _gameData.mapCode1 = playerState.CCtrl.mapCode[1];
        _gameData.spawnX = playerState.transform.position.x; _gameData.spawnY = playerState.transform.position.y;

        gameData.invenCount = invenManager.inven.Count;
        gameData.invenItemCode = new int[gameData.invenCount];
        gameData.invenItemCount = new int[gameData.invenCount];
        int i = 0;
        foreach(Inventory slot in invenManager.inven){
            gameData.invenItemCode[i] = slot.itemCode;
            gameData.invenItemCount[i++] = slot.count;
        }

        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFile + saveName_ + ".json";

        File.WriteAllText(filePath, ToJsonData);
        
    }
}
