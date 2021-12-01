using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDraw : MonoBehaviour
{
    public GameObject linePrefab;

    LineRenderer lr;
    EdgeCollider2D col2D;
    List<Vector2> points = new List<Vector2>();


    public GameObject images;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = Instantiate(linePrefab);
            lr = go.GetComponent<LineRenderer>();
            col2D = go.GetComponent<EdgeCollider2D>();
          // points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
           points.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f))); //0.3f
            lr.positionCount = 1;

             lr.SetPosition(0, points[0]);  //list첫번째 포인트값을 1번째로 지정


            lr.transform.parent = images.transform;
            lr.transform.position = images.transform.position;

            //lr.transform.position = images.transform.position;
            //lr.startWidth = 0.01f;
            //lr.endWidth = 0.01f;
            //lr.numCornerVertices = 5;
            //lr.numCapVertices = 5;

        }
        else if(Input.GetMouseButton(0)) 
        {
            Vector2 pos=Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20f));
            points.Add(pos);

            lr.positionCount++;
            lr.SetPosition(lr.positionCount-1, pos);
            col2D.points = points.ToArray();


            lr.transform.parent = images.transform;
            lr.transform.position =  images.transform.position;
            //lr.startWidth = 0.01f;
            //lr.endWidth = 0.01f;
            //lr.numCornerVertices = 5;
            //lr.numCapVertices = 5;


        }

        else if (Input.GetMouseButtonUp(0))
        {
            points.Clear();
        }
    }
}





