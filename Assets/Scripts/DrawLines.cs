using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour
{
    Vector3 topLeft, topRight, bottomLeft, bottomRight;
    [SerializeField] Material mat;

    private void Start()
    {
        topLeft = Vector2.zero;
        topRight = Vector2.right;
        bottomLeft = Vector2.down;
        bottomRight = new Vector2(1,-1);
    }

    public void DrawRect(float xMin, float xMax, float yMin, float yMax)
    {
        topLeft = new Vector2(xMin, yMin);
        topRight = new Vector2(xMax, yMin);
        bottomLeft = new Vector2(xMin, yMax);
        bottomRight = new Vector2(xMax, yMax);
    }

    private void OnPostRender()
    {
        if (!mat)
        {
            //Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();

        GL.Begin(GL.LINE_STRIP);
        GL.Color(Color.white);
        GL.Vertex(topLeft);
        GL.Vertex(topRight);
        GL.Vertex(bottomRight);
        GL.Vertex(bottomLeft);
        GL.End();

        /*GL.Begin(GL.LINE_STRIP);
        GL.Color(Color.white);
        GL.Vertex(bottomLeft);
        GL.Vertex(bottomRight);
        GL.End();

        GL.Begin(GL.LINE_STRIP);
        GL.Color(Color.white);
        GL.Vertex(topLeft);
        GL.Vertex(bottomLeft);
        GL.End();

        GL.Begin(GL.LINE_STRIP);
        GL.Color(Color.white);
        GL.Vertex(topRight);
        GL.Vertex(bottomRight);
        GL.End();*/

        GL.PopMatrix();
    }
}
