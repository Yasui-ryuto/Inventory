using UnityEditor;
using UnityEngine;

public class ListItemDetail : EditorWindow
{
    Vector2 scrollPos = Vector2.zero;

    [MenuItem("Tools/DetilList")]
    public static void Window()
    {
        ListItemDetail window = GetWindow<ListItemDetail>("詳細説明変更"); //「詳細説明変更」という名前でウィンドウを作成
        window.position = new Rect(400, 100, 400, 500); //ウィンドウの大きさを設定
    }

    void OnGUI()
    {
        if (AssetDatabase.IsValidFolder("Assets/sample3/Resources/ItemPrefab"))
        {
            string[] itemPathes = System.IO.Directory.GetFiles("Assets/sample3/Resources/ItemPrefab", "*.prefab"); //アイテムプレハブのパスをすべて取得

            if (itemPathes.Length > 0)
            {
                string[] detailText = new string[itemPathes.Length]; //詳細テキストを入れる配列

                scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(475)); //ウィンドウをスクロールできるようにする

                for (int i = 0; i < itemPathes.Length; i++)
                {
                    GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(itemPathes[i]); //アイテムプレハブをロード
                    GUILayout.BeginHorizontal(); //ここからウィンドウの配置を横にする
                    GUILayout.Label(obj.name); //プレハブの名前
                    GUILayout.FlexibleSpace(); //空白をいい感じに入れる
                    detailText[i] = EditorGUILayout.TextArea(obj.GetComponent<ItemDetail>().GetDetail(), EditorStyles.textArea, GUILayout.Height(Mathf.Max(16, obj.GetComponent<ItemDetail>().GetDetail().Split('\n').Length * 16)), GUILayout.Width(200)); //テキストエリアを作成
                    //改行が入るたびにテキストエリアを多くする
                    GUILayout.EndHorizontal(); //ここまでウィンドウの配置を横にする

                    obj.GetComponent<ItemDetail>().SetDetail(detailText[i]); //アイテムプレハブに詳細テキストの変更を保存
                }
                GUILayout.EndScrollView(); //ここまでをスクロールできるようにする

                if (GUILayout.Button("元のプレハブに保存")) //元のプレハブにテキストの変更を保存する
                {
                    if (AssetDatabase.IsValidFolder("Assets/sample3/Prefab/ItemPrefab")) //フォルダがあるか確認
                    {
                        string[] originPrefabs = System.IO.Directory.GetFiles("Assets/sample3/Prefab/ItemPrefab", "*.prefab"); //元のプレハブのパスをすべて保存

                        if (originPrefabs.Length == detailText.Length) //プレハブの個数とテキストの個数が一致するか確認
                        {
                            for (int i = 0; i < itemPathes.Length; i++)
                            {
                                AssetDatabase.LoadAssetAtPath<GameObject>(originPrefabs[i]).GetComponent<ItemDetail>().SetDetail(detailText[i]); //元のプレハブの詳細テキストを変更
                                EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<GameObject>(originPrefabs[i]));
                            }

                            Close();
                        }
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                GUILayout.Label("アイテムプレハブがありません");
            }
        }
        else
        {
            GUILayout.Label("ItemPrefabフォルダがありません");
        }
         
    }

    //void OnGUI()
    //{
    //    //int inputAmount = 0;

    //    int cupsulAmount = 10;
    //    int circleAmount = 10;
    //    int triangleAmount = 10;

    //    GUILayout.Label("作成する個数を入力してください", EditorStyles.boldLabel);
    //    //inputAmount = EditorGUILayout.IntField("全体個数", inputAmount);
    //    //cupsulAmount = EditorGUILayout.IntField("カプセルの個数", cupsulAmount);
    //    //circleAmount = EditorGUILayout.IntField("カプセルの個数", circleAmount);
    //    //triangleAmount = EditorGUILayout.IntField("カプセルの個数", triangleAmount);

    //    if (GUILayout.Button("作成"))
    //    {
    //        Create(cupsulAmount, circleAmount, triangleAmount);
    //        Close();
    //    }
    //}

    //private void Create(int cupsul, int circle, int triangle)
    //{
    //    string path = $"Assets/sample3/Resources/ItemDetail";

    //    GameObject ItemDetail = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/sample3/Prefab/ItemDetail");


    //    if (!AssetDatabase.IsValidFolder(path))
    //    {
    //        AssetDatabase.CreateFolder("Assets/sample3/Resources", "ItemDetail");
    //    }
    //    else
    //    {
    //        AssetDatabase.DeleteAsset(path);
    //        AssetDatabase.CreateFolder("Assets/sample3/Resources", "ItemDetail");
    //    }

    //    for(int i = 1; i < cupsul + circle + triangle + 1; i++)
    //    {    
    //        if(i <  cupsul + 1)
    //        {
    //            PrefabUtility.SaveAsPrefabAsset(ItemDetail, $"{path}/N{i:000}_Detail.prefab");
    //        }
    //        else if(i < cupsul + circle + 1)
    //        {
    //            PrefabUtility.SaveAsPrefabAsset(ItemDetail, $"{path}/R{i:000}_Detail.prefab");
    //        }
    //        else
    //        {
    //            PrefabUtility.SaveAsPrefabAsset(ItemDetail, $"{path}/S{i:000}_Detail.prefab");
    //        }
    //    }

    //}
}
