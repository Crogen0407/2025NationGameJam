#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Crogen.CrogenPooling.SoundPlayer
{
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {
        private SoundManager _soundManager;
        private static SoundDataSO _currentSelectedSoundDataSO;
        private static int _currentSelectedIndex;
        
        private void OnEnable()
        {
            _soundManager = target as SoundManager;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Sound Data");
            if (_soundManager.soundDataSOList != null)
            {
                for (int i = 0; i < _soundManager.soundDataSOList.Count; ++i)
                {
                    for (int j = i + 1; j < _soundManager.soundDataSOList.Count; ++j)
                    {
                        if (_soundManager.soundDataSOList[j] == null) continue;
                        if (_soundManager.soundDataSOList[i] == _soundManager.soundDataSOList[j])
                            Debug.LogWarning("같은 SoundData를 리스트에 추가했습니다. SoundManager를 확인해주세요.");
                    }

                    GUILayout.BeginHorizontal();

                    if (_currentSelectedIndex == i)
                    {
                        GUI.color = Color.green;
                        _currentSelectedSoundDataSO = _soundManager.soundDataSOList[i];
                    }

                    if (GUILayout.Button("Select"))
                    {
                        SelectSoundDataSO(i);
                    }

                    _soundManager.soundDataSOList[i] = EditorGUILayout.ObjectField(_soundManager.soundDataSOList[i], typeof(SoundDataSO), false) as SoundDataSO;

                    if (GUILayout.Button("New"))
                    {
                        var soundDataSO = ScriptableObject.CreateInstance<SoundDataSO>();

                        CreateSoundDataSOAsset(soundDataSO);
                        _soundManager.soundDataSOList[i] = soundDataSO;
                        SelectSoundDataSO(i);
                    }

                    if (_soundManager.soundDataSOList[i] != null)
                    {
                        if (GUILayout.Button("Clone"))
                        {
                            var poolBase = Instantiate(_soundManager.soundDataSOList[i]);

                            CreateSoundDataSOAsset(poolBase, poolBase.ToString());
                            _soundManager.soundDataSOList[i] = poolBase;
                            SelectSoundDataSO(i);
                        }
                    }

                    GUI.color = Color.white;

                    GUILayout.EndHorizontal();
                }
            }

            if (GUILayout.Button("+"))
            {
                if(_soundManager.soundDataSOList == null) _soundManager.soundDataSOList = new List<SoundDataSO>();
                _soundManager.soundDataSOList.Add(null);
                EditorUtility.SetDirty(_soundManager);
                SelectSoundDataSO(_soundManager.soundDataSOList.Count - 1);
            }

            if (GUILayout.Button("-"))
            {
                _soundManager.soundDataSOList.Remove(_currentSelectedSoundDataSO);
                EditorUtility.SetDirty(_soundManager);
                SelectSoundDataSO(_soundManager.soundDataList.Count - 1);
            }

            GUILayout.Space(20);

            //PoolBase Serialize
            if (_currentSelectedSoundDataSO != null)
            {
                _soundManager.soundDataList = _currentSelectedSoundDataSO.soundDataList;
                var soundDataSOArrayObject = serializedObject.FindProperty("soundDataList");
                EditorGUILayout.PropertyField(soundDataSOArrayObject, true);
                serializedObject.ApplyModifiedProperties();
                _currentSelectedSoundDataSO.soundDataList = _soundManager.soundDataList;
                EditorUtility.SetDirty(_currentSelectedSoundDataSO);
                _currentSelectedSoundDataSO.PairInit();

                serializedObject.Update();
            }
        }

        private void SelectSoundDataSO(int index)
        {
            try
            {
                _currentSelectedSoundDataSO = _soundManager.soundDataSOList[index];
                _currentSelectedIndex = index;
            }
            catch (Exception) { }
        }
        
        private void CreateSoundDataSOAsset(SoundDataSO cloneSoundDataSO, string fileName = "New Sound Data SO")
        {
            var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath($"Assets/{fileName}.asset");
            AssetDatabase.CreateAsset(cloneSoundDataSO, uniqueFileName);
            _currentSelectedSoundDataSO = cloneSoundDataSO;
            EditorUtility.SetDirty(_currentSelectedSoundDataSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
#endif