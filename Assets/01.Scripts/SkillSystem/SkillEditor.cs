#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace _01.Scripts.SkillSystem
{
    [CustomEditor(typeof(SkillInfoManager))]
    public class SkillEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        { 
            base.OnInspectorGUI();
            
            var itemsEnum = Enum.GetValues(typeof(SkillEnum));
            
            var commonItemArray = (SkillInfoManager)target;
            if (commonItemArray.skills is not { Length: > 0 } || commonItemArray.skills.Length != itemsEnum.Length)
            {
                var newArray = new Skill[itemsEnum.Length];
                
                for (var i = 0; i < (commonItemArray.skills?.Length ?? 0); i++) newArray[i] = commonItemArray.skills![i];
                for (var i = (commonItemArray.skills?.Length ?? 0) - 1; i >= 0 && i < newArray.Length; i++) newArray[i] = new Skill();

                commonItemArray.skills = newArray;
            }

            if (commonItemArray.toggled == null || commonItemArray.toggled.Length != itemsEnum.Length)
                commonItemArray.toggled = new bool[itemsEnum.Length];
            
            GUILayout.Space(20);

            foreach (var type in itemsEnum)
            {
                var idx = (int)type;
                var commonItem = commonItemArray.skills[idx];
                if (commonItemArray.toggled[idx] == EditorGUILayout.BeginFoldoutHeaderGroup(commonItemArray.toggled[idx], $"{type}: {commonItem.skillName}"))
                {
                    commonItem.skillType = (SkillEnum)type;
                    commonItem.skillName = EditorGUILayout.TextField("Name", commonItem.skillName);
                    EditorGUILayout.LabelField("Description");
                    commonItem.skillSprite =
                        (Sprite)EditorGUILayout.ObjectField("InvSprite", commonItem.skillSprite, typeof(Sprite), false);
                    EditorGUILayout.Space(14);
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }
    }
}
#endif
