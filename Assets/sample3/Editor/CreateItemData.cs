using UnityEngine;
using UnityEditor;
using TMPro;

public class CreateItemData
{
    [MenuItem("Tools/CreateItemData")] //エディターにTools/CreateItemDataの機能を追加する

    public static void Create()
    {
        string[] itemPathes = System.IO.Directory.GetFiles("Assets/sample3/Prefab/ItemPrefab", "*.prefab"); //ItemPrefabからパスをすべて格納
        
        //ItemPrefabフォルダがあるか確認
        string pathPrefab = $"Assets/sample3/Resources/ItemPrefab";
        if (AssetDatabase.IsValidFolder(pathPrefab))
        {
            AssetDatabase.DeleteAsset(pathPrefab);
            AssetDatabase.CreateFolder("Assets/sample3/Resources", "ItemPrefab");
        }
        else
        {
            AssetDatabase.CreateFolder("Assets/sample3/Resources", "ItemPrefab");
        }

        //ItemDetailBubbleフォルダがあるか確認
        string pathDetail = $"Assets/sample3/Resources/ItemDetailBubble";
        if(AssetDatabase.IsValidFolder(pathDetail))
        {
            AssetDatabase.DeleteAsset (pathDetail);
            AssetDatabase.CreateFolder($"Assets/sample3/Resources", "ItemDetailBubble");
        }
        else
        {
            AssetDatabase.CreateFolder($"Assets/sample3/Resources", "ItemDetailBubble");
        }

        //ItemDataフォルダがあるか確認
        string pathData = $"Assets/sample3/Resources/ItemData";
        if (AssetDatabase.IsValidFolder(pathData))
        {
            AssetDatabase.DeleteAsset(pathData);
            AssetDatabase.CreateFolder("Assets/sample3/Resources", "ItemData");
        }
        else
        {
            AssetDatabase.CreateFolder("Assets/sample3/Resources", "ItemData");
        }
        
        //アイテムデータ作成
        for (int i = 1; i < itemPathes.Length + 1; i++)
        {
            string prefab = CreatePrefab(i, itemPathes[i - 1]); //アイテムのプレハブを作成、そのパスを格納
            string detail = CreateDetail(pathPrefab.Length + 1, prefab); //アイテムの詳細プレハブを作成
            CreateData(i, prefab, detail); //アイテムデータの作成


        }

        //アセットを保存
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static string CreatePrefab(int num, string path)
    {
        string pathPrefab = $"Assets/sample3/Resources/ItemPrefab";

        GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path); //パスからアイテムプレハブを取得
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(obj, $"{pathPrefab}/{obj.name.Insert(1, $"{num:000}")}.prefab"); //新しいプレハブを作成

        EditorUtility.SetDirty(obj);

        return AssetDatabase.GetAssetPath(prefab); //新しく作ったプレハブのパスを返す
    }

    static string CreateDetail(int index, string path)
    {
        string pathDetail = $"Assets/sample3/Resources/ItemDetailBubble";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path); //パスからプレハブを取得

        GameObject detail = PrefabUtility.SaveAsPrefabAsset(AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/sample3/Prefab/ItemDetail.prefab"), $"{pathDetail}/{path.Substring(index, 4)}_Detail.prefab"); //新しくプレハブを作成する

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

        return AssetDatabase.GetAssetPath(detail); //詳細プレハブのパスを返す
    }
    static void CreateData(int num, string prefabPath, string detailPath)
    {
        string pathData = $"Assets/sample3/Resources/ItemData";

        ItemData newItem = ScriptableObject.CreateInstance<ItemData>(); //新しくItemDataインスタンスを作る

        newItem.SetItemData(num, AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath), AssetDatabase.LoadAssetAtPath<GameObject>(detailPath)); //新しく作ったItemDataインスタンスに数値とプレハブを格納

        EditorUtility.SetDirty(newItem);

        AssetDatabase.CreateAsset(newItem, $"{pathData}/Item_{num:000}.asset"); //アセットを作成

    }
}
