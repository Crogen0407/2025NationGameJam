#if  UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{
    private PoolManager _poolManager;
    private PoolCategorySO _currentSelectedPoolCategory;
    private static int _currentSelectedIndex;

    private void OnEnable()
    {
        _poolManager = target as PoolManager;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("PoolBase");

        if( _poolManager.poolBaseList != null )
        {
			for (int i = 0; i < _poolManager.poolBaseList.Count; ++i)
			{
				for (int j = i + 1; j < _poolManager.poolBaseList.Count; ++j)
				{
					if (_poolManager.poolBaseList[j] == null) continue;
					if (_poolManager.poolBaseList[i] == _poolManager.poolBaseList[j])
						Debug.LogWarning("같은 PoolBase를 리스트에 추가했습니다. PoolManager를 확인해주세요.");
				}

				GUILayout.BeginHorizontal();

				if (_currentSelectedIndex == i)
				{
					GUI.color = Color.green;
					_currentSelectedPoolCategory = _poolManager.poolBaseList[i];
				}
				if (GUILayout.Button("Select"))
				{
					SelectPoolBase(i);
				}

				_poolManager.poolBaseList[i] = EditorGUILayout.ObjectField(_poolManager.poolBaseList[i], typeof(PoolCategorySO), false) as PoolCategorySO;

				if (GUILayout.Button("New"))
				{
					var poolBase = ScriptableObject.CreateInstance<PoolCategorySO>();

					CreatePoolBaseAsset(poolBase);
					_poolManager.poolBaseList[i] = poolBase;
					SelectPoolBase(i);
				}

				if (_poolManager.poolBaseList[i] != null)
				{
					if (GUILayout.Button("Clone"))
					{
						var poolBase = Instantiate(_poolManager.poolBaseList[i]);

						CreatePoolBaseAsset(poolBase, poolBase.ToString());
						_poolManager.poolBaseList[i] = poolBase;
						SelectPoolBase(i);
					}
				}

				GUI.color = Color.white;

				GUILayout.EndHorizontal();
			}

		}

		if (GUILayout.Button("+"))
		{
            if(_poolManager.poolBaseList == null) _poolManager.poolBaseList = new List<PoolCategorySO>();
            _poolManager.poolBaseList.Add(null);
            SelectPoolBase(_poolManager.poolBaseList.Count - 1);
        }
        if (GUILayout.Button("-"))
        {
            _poolManager.poolBaseList.Remove(_currentSelectedPoolCategory);
            SelectPoolBase(_poolManager.poolBaseList.Count - 1);
        }
        GUILayout.Space(20);

        //PoolBase Name
        if (_currentSelectedPoolCategory != null)
        {
	        GUILayout.BeginHorizontal();
	        {
		        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(70));
		        string str = _currentSelectedPoolCategory.name.ToString();
		        string newName = EditorGUILayout.DelayedTextField(str);
		        
		        if (str.Equals(_currentSelectedPoolCategory.name) == false)
		        {
			        string assetPath =  AssetDatabase.GetAssetPath(_currentSelectedPoolCategory.GetInstanceID());
			        AssetDatabase.RenameAsset(assetPath, newName);
			        EditorUtility.SetDirty(_currentSelectedPoolCategory);
			        AssetDatabase.SaveAssets();
		        }
	        }
	        GUILayout.EndHorizontal();    
        }
		
        //PoolBase Serialize
        if (_currentSelectedPoolCategory != null)
        {
            _poolManager.poolingPairs = _currentSelectedPoolCategory.pairs;
            var poolBaseArrayObject = serializedObject.FindProperty("poolingPairs");
            EditorGUILayout.PropertyField(poolBaseArrayObject, true);
            serializedObject.ApplyModifiedProperties();
            _currentSelectedPoolCategory.pairs = _poolManager.poolingPairs;
            _currentSelectedPoolCategory.PairInit();

            serializedObject.Update();
        }

        if (GUILayout.Button("Generate Enum"))
        {
            GeneratePoolingEnumFile();
        }
    }

    private void SelectPoolBase(int index)
	{
        try
        {
            _currentSelectedPoolCategory = _poolManager.poolBaseList[index];
            _currentSelectedIndex = index;
        }
        catch (System.Exception) { }
    }

    private void GeneratePoolingEnumFile()
    {
        if (_currentSelectedPoolCategory == null) return;

		foreach (var pair in _currentSelectedPoolCategory.pairs)
		{
            if (pair.poolType == string.Empty)
			{
                Debug.LogWarning("Check your poolbase. May have an empty poolType.");
                return;
			}
            if (pair.poolType.Contains(' '))
			{
                Debug.LogWarning("Check your poolbase. Spaces are not allowed.");
                return;
			}
        }

        StringBuilder codeBuilder = new StringBuilder();
    
        foreach(var item in _currentSelectedPoolCategory.pairs)
        {
            codeBuilder.Append(item.poolType);
            codeBuilder.Append(", ");
        }

        string className = _currentSelectedPoolCategory.name;

        //함수로 묶을 수도 있긴 한데 함수까지 늘려가며 콜을 하고 싶진 않음.
        className = className.Replace(" ", string.Empty);
        className = className.Replace("(", "_");
        className = className.Replace(")", "_");
        className = className.Replace("[", "_");
        className = className.Replace("]", "_");
        className = className.Replace("{", "_");
        className = className.Replace("}", "_");


        string code = string.Format(CodeFormat.PoolingTypeFormat, className, codeBuilder.ToString());

        string path = $"{Application.dataPath}/Crogen/CrogenPooling";

        File.WriteAllText($"{path}/EnumTypes/{className}PoolType.cs", code);

        EditorUtility.SetDirty(_currentSelectedPoolCategory);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Success!");
    }

    private void CreatePoolBaseAsset(PoolCategorySO clonePoolCategorySo, string fileName = "New Pool Base")
    {
        var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath($"Assets/{fileName}.asset");
        AssetDatabase.CreateAsset(clonePoolCategorySo, uniqueFileName);
        _currentSelectedPoolCategory = clonePoolCategorySo;
        EditorUtility.SetDirty(_currentSelectedPoolCategory);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif