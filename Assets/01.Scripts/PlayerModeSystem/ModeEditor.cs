using System;
using _01.Scripts.SkillSystem;
using UnityEditor;
using UnityEngine;

namespace _01.Scripts.PlayerModeSystem
{
    [CustomEditor(typeof(ModeManager))]
    public class ModeEditor : Editor
    {
        public override void OnInspectorGUI()
        { 
            base.OnInspectorGUI();
            
            var itemsEnum = Enum.GetValues(typeof(ModeEnum));
            
            var commonItemArray = (ModeManager)target;
            if (commonItemArray.PlayerModes is not { Length: > 0 } || commonItemArray.PlayerModes.Length != itemsEnum.Length)
            {
                var newArray = new PlayerMode[itemsEnum.Length];
                
                for (var i = 0; i < (commonItemArray.PlayerModes?.Length ?? 0); i++) newArray[i] = commonItemArray.PlayerModes![i];
                for (var i = (commonItemArray.PlayerModes?.Length ?? 0) - 1; i >= 0 && i < newArray.Length; i++) newArray[i] = new PlayerMode();

                commonItemArray.PlayerModes = newArray;
            }

            if (commonItemArray.toggled == null || commonItemArray.toggled.Length != itemsEnum.Length)
                commonItemArray.toggled = new bool[itemsEnum.Length];
            
            GUILayout.Space(20);

            foreach (var type in itemsEnum)
            {
                var idx = (int)type;
                var commonItem = commonItemArray.PlayerModes[idx];
                if (commonItemArray.toggled[idx] == EditorGUILayout.BeginFoldoutHeaderGroup(commonItemArray.toggled[idx], $"{type}"))
                {
                    commonItem.modeInfo = EditorGUILayout.ObjectField("object Info Serial", commonItem.modeInfo, typeof(ModeInfo), false) as ModeInfo;
                    if (commonItem.modeInfo) commonItem.modeInfo.mode = (ModeEnum)type;
                    EditorGUILayout.Space(14);
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }
    }
}
