using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;

public class Object3d : MonoBehaviour
{
    float x1, y1, z1, x2, y2, z2, x3, y3, z3;

    public InputField inputx1, inputy1, inputz1, inputx2, inputy2, inputz2, inputx3, inputy3, inputz3;
    public Matrix4x4 matrix;
    public Vector4 column0, column1, column2, column3;
    MatrixFirebase readDB;

    // Start is called before the first frame update
    void Start()
    {
        inputx1.text = "1";
        inputy1.text = "0";
        inputz1.text = "0";
        inputx2.text = "0";
        inputy2.text = "1";
        inputz2.text = "0";
        inputx3.text = "0";
        inputy3.text = "0";
        inputz3.text = "1";
    }

    // Update is called once per frame
    void Update()
    {

        x1 = float.Parse(inputx1.text);
        x2 = float.Parse(inputx2.text);
        x3 = float.Parse(inputx3.text);  
        y1 = float.Parse(inputy1.text);
        y2 = float.Parse(inputy2.text);
        y3 = float.Parse(inputy3.text);
        z1 = float.Parse(inputz1.text);
        z2 = float.Parse(inputz2.text);
        z3 = float.Parse(inputz3.text);

        column0 = new Vector4(x1, y1, z1, 0);
        column1 = new Vector4(x2, y2, z2, 0);
        column2 = new Vector4(x3, y3, z3, 0);
        column3 = new Vector4(0, 0, 0, 1);

        matrix = new Matrix4x4(column0, column1, column2, column3);

        transform.localScale = matrix.ExtractScale();
        transform.rotation = matrix.ExtractRotation();
        transform.position = matrix.ExtractPosition();

    }

    public void writeFirebase()
    {
        MatrixFirebase DBfire = new MatrixFirebase(x1, y1, z1, x2, y2, z2, x3, y3, z3);
        string json = JsonUtility.ToJson(DBfire);
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("matrix").SetRawJsonValueAsync(json);
    }
    public void readFirebase()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance
        .RootReference
        .Child("matrix")
        .GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                readDB = JsonUtility.FromJson<MatrixFirebase>(snapshot.GetRawJsonValue());
                inputx1.text = readDB.x1.ToString();
                inputy1.text = readDB.y1.ToString();
                inputz1.text = readDB.z1.ToString();
                inputx2.text = readDB.x2.ToString();
                inputy2.text = readDB.y2.ToString();
                inputz2.text = readDB.z2.ToString();
                inputx3.text = readDB.x3.ToString();
                inputy3.text = readDB.y3.ToString();
                inputz3.text = readDB.z3.ToString();
            }
        });
    }


}

 
public static class MatrixExtensions
{
    public static Quaternion ExtractRotation(this Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    public static Vector3 ExtractPosition(this Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }

    public static Vector3 ExtractScale(this Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }
}

public class MatrixFirebase
{
    public float x1, y1, z1, x2, y2, z2, x3, y3, z3;

    public MatrixFirebase(float x1,float y1,float z1, float x2, float y2, float z2, float x3, float y3, float z3)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;
        this.x2 = x2;
        this.y2 = y2;
        this.z2 = z2;
        this.x3 = x3;
        this.y3 = y3;
        this.z3 = z3;
    }
}