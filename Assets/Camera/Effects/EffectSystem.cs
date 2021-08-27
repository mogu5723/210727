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

    public void collectingEffect(Vector3 pos, Color color, float range, float time, float endAlpha){
        coroutineList.Add(StartCoroutine(collectingEffect_(pos, color, range, time, endAlpha)));
    }

    IEnumerator collectingEffect_(Vector3 pos, Color color, float range, float time, float endAlpha){
        GameObject obj;
        SpriteRenderer rend;
        Rigidbody2D rigid;
        float angle = Random.Range(0, Mathf.PI*2);

        tempObjList.Add(obj = Instantiate(square, pos + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)))*range, Quaternion.identity));
        rend = obj.GetComponent<SpriteRenderer>();
        rigid = obj.GetComponent<Rigidbody2D>();
        obj.SetActive(true);

        Color tempColor = color;
        rend.color = tempColor;
        rigid.gravityScale = 0;
        Vector3 velocity = (pos - obj.transform.position)/time;
        rigid.velocity = velocity;

        int repeat = (int)(time*50);
        float alphaChangeRate = (endAlpha-tempColor.a)/repeat;
        Vector3 dir = (pos - obj.transform.position)/repeat;
        for(int i = 0; i < repeat; i++){
            yield return new WaitForFixedUpdate();
            rend.color += new Color(0, 0, 0, alphaChangeRate);
            rigid.velocity = velocity;
        }

        Destroy(obj);
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
