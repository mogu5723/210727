using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataManagement : MonoBehaviour
{
    public PlayerState playerState;
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
    public string GameDataFileName = "21412812Data.json";

    private void Start() {
        
    }

    public GameData _gameData;
    public GameData gameData{
        get{
            if(_gameData == null){
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    public void LoadGameData(){
        string filePath = Application.persistentDataPath + GameDataFileName;

        if(File.Exists(filePath)){
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }else{
            _gameData = new GameData();

            _gameData.mapCode0 = 0;
            _gameData.mapCode1 = 0;
            _gameData.spawnX = -34.5f;
            _gameData.spawnY = -1f;
        }

        invenManager.invenLoad();
        playerState.respawn();
    }

    public void SaveGameData(){
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
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }
}
