using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadText : MonoBehaviour
{

    static public List<Vector3> Positions = new List<Vector3>();
    static public List<Vector3Int> Polygons = new List<Vector3Int>();
    static public List<int> IndexList = new List<int>();
    static public List<Vector3> PNormals = new List<Vector3>();
    static public List<Vector3> VNormals = new List<Vector3>();

    public string[] textMessage; //テキストの加工前の一行を入れる変数
    public string[,] textWords; //テキストの複数列を入れる2次元は配列 

    private int rowLength; //テキスト内の行数を取得する変数
    private int columnLength; //テキスト内の列数を取得する変数

    private void Awake()
    {
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("Ball", typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる

        //Splitで一行づつを代入した1次配列を作成
        textMessage = TextLines.Split('\n'); //

        //行数と列数を取得
        rowLength = textMessage.Length;
        columnLength = 4;

        //2次配列を定義
        textWords = new string[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++)
        {
            columnLength = textMessage[0].Split(' ').Length;

            string[] tempWords = textMessage[i].Split(' '); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            switch (tempWords[0]) {

                case "v":
                    Positions.Add(2.0f * new Vector3(float.Parse(tempWords[1]), float.Parse(tempWords[2]), float.Parse(tempWords[3])));
                    break;

                case "f":
                    Vector3 V1, V2, V3;
                    if (tempWords[1].Split('/').Length == 1 )
                    {
                        Polygons.Add(new Vector3Int(int.Parse(tempWords[1]) - 1, int.Parse(tempWords[2]) - 1, int.Parse(tempWords[3]) - 1));
                        V1 = Positions[int.Parse(tempWords[1]) - 1];
                        V2 = Positions[int.Parse(tempWords[2]) - 1];
                        V3 = Positions[int.Parse(tempWords[3]) - 1];
                    } else
                    {
                        Polygons.Add(new Vector3Int(int.Parse(tempWords[1].Split('/')[0]) - 1, int.Parse(tempWords[2].Split('/')[0]) - 1, int.Parse(tempWords[3].Split('/')[0]) - 1));
                        V1 = Positions[int.Parse(tempWords[1].Split('/')[0]) - 1];
                        V2 = Positions[int.Parse(tempWords[2].Split('/')[0]) - 1];
                        V3 = Positions[int.Parse(tempWords[3].Split('/')[0]) - 1];
                    }

                    PNormals.Add(Vector3.Normalize(Vector3.Cross(V3 - V2, V2 - V1)));
                    break;
            }
            
        }

        for (int i = 0; i < Polygons.Count; i++)
        {
            IndexList.Add(Polygons[i].x);
            IndexList.Add(Polygons[i].y);
            IndexList.Add(Polygons[i].z);
        }

        //法線計算
        for (int i = 0; i < Positions.Count; i++)
        {
            int count = 0;
            Vector3 N = new Vector3(0, 0, 0);

            for (int j = 0; j < Polygons.Count; j++)
            {
                if (i == Polygons[j].x || i == Polygons[j].y || i == Polygons[j].z)
                {
                    N += PNormals[j];
                    count++;
                }
            }
            VNormals.Add(N / count);
        }
    }

}
