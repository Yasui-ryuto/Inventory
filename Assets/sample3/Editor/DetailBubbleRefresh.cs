using System;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DetailBubbleRefresh
{

    [MenuItem("Tools/DetailBubbleRefresh")]

    public static void Refresh()
    {
        string[] itemPathes = System.IO.Directory.GetFiles("Assets/sample3/Resources/ItemPrefab", "*.prefab"); //ItemPrefabからパスをすべて格納

        //ItemDetailBubbleフォルダがあるか確認
        string pathDetailFolder = $"Assets/sample3/Resources/ItemDetailBubble";
        if (AssetDatabase.IsValidFolder(pathDetailFolder))
        {
            AssetDatabase.DeleteAsset(pathDetailFolder);
            AssetDatabase.CreateFolder($"Assets/sample3/Resources", "ItemDetailBubble");
        }
        else
        {
            AssetDatabase.CreateFolder($"Assets/sample3/Resources", "ItemDetailBubble");
        }


        for (int i = 1; i < itemPathes.Length + 1; i++)
        {
            string pathDetail = $"Assets/sample3/Resources/ItemDetailBubble";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(itemPathes[i - 1]); //パスからプレハブを取得

            GameObject detail = PrefabUtility.SaveAsPrefabAsset(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/sample3/Prefab/ItemDetail.prefab"), $"{pathDetail}/{itemPathes[i - 1].Substring("Assets/sample3/Resources/ItemPrefab".Length + 1, 4)}_Detail.prefab"); //新しくプレハブを作成する

            //詳細のテキストを変更
            GameObject detailPrefab = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(detail)); //プレハブ専用の作業スペース(メモリ)を確保
            if (!prefab.GetComponent<ItemDetail>().GetDetail().Equals("")) //プレハブの中に詳細文が書かれているのかを判断
            {
                detailPrefab.transform.Find("Canvas/DetailText").GetComponent<TextMeshProUGUI>().text = prefab.GetComponent<ItemDetail>().GetDetail();
            }
            else
            {
                Debug.LogWarning($"{prefab.name}の詳細が入力されてません");
                detailPrefab.transform.Find("Canvas/DetailText").GetComponent<TextMeshProUGUI>().text = "notext";
            }

            PrefabUtility.SaveAsPrefabAsset(detailPrefab, AssetDatabase.GetAssetPath(detail)); //変更を保存
            PrefabUtility.UnloadPrefabContents(detailPrefab); //作業スペース(メモリ)を解放

            EditorUtility.SetDirty(detail);
        }

        //アセットを保存
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
