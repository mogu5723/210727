using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public List<GameObject> tempObjList;
    public List<Coroutine> coroutineList;
    GameObject square;
    private void Awake() {
        tempObjList = new List<GameObject>();
        coroutineList = new List<Coroutine>();
        square = transform.Find("squareFragment").gameObject;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void squareFragmentEffect(int count, Color color, float power, Vector3 pos){
        coroutineList.Add(StartCoroutine(squareFragmentEffect_(count, color, power, pos)));
    }
    IEnumerator squareFragmentEffect_(int count, Color color, float power, Vector3 pos){
        GameObject[] fragmentArray = new GameObject[count];
        SpriteRenderer[] rend = new SpriteRenderer[count];

        for(int i = 0; i < count; i++){
            tempObjList.Add(fragmentArray[i] = Instantiate(square, pos, Quaternion.identity));
            rend[i] = fragmentArray[i].GetComponent<SpriteRenderer>();

            fragmentArray[i].SetActive(true);
            rend[i].color = color;
            fragmentArray[i].GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized*power;
            fragmentArray[i].GetComponent<Rigidbody2D>().rotation = Random.Range(0, 360);
        }

        yield return new WaitForSeconds(5f);

        Color newColor = color;
        while(newColor.a > 0){
            newColor.a -= Time.deltaTime;
            for(int i = 0; i < count; i++){
                rend[i].color = newColor;
            }
            yield return null;
        }

        for(int i = 0; i < count; i++){
            Destroy(fragmentArray[i]);
        }
    }

    public void deleteFrag(){
        int count = coroutineList.Count;
        for(int i = 0; i < count; i++){
            StopCoroutine(coroutineList[0]);
            coroutineList.RemoveAt(0);
        }
        count = tempObjList.Count;
        for(int i = 0; i < count; i++){
            Destroy(tempObjList[0]);
            tempObjList.RemoveAt(0);
        }
    }

}
