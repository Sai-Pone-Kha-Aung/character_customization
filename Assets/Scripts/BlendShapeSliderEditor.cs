using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CC.Blendshapes
{
    [CustomEditor(typeof(BlendShapeSlider))]
    public class BlendShapeSliderEditor : Editor
    {
        public enum State { auto, manuel }
        public State state;
        private BlendShapeSlider blendShapeSlider;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));

            if (GUILayout.Button("Auto")) state = State.auto;
            if (GUILayout.Button("Manuel")) state = State.manuel;

            EditorGUILayout.EndHorizontal();

            blendShapeSlider = (BlendShapeSlider)target;

            switch (state)
            {
                case State.auto: GUI_Auto(); break;
                case State.manuel: GUI_Manuel(); break;
                default: GUI_Auto(); break;
            }
        }
        private void GUI_Manuel()
        {
            base.OnInspectorGUI();
        }

        private void GUI_Auto()
        {
            CharacterC characterC = GameObject.FindObjectOfType<CharacterC>();

            if (characterC == null)
            {   
                EditorGUILayout.LabelField("Have the character in scene");
                throw new System.Exception("Have the character in scene");

            }

            if(characterC.GetNumberOfEntries() <= 0)
            characterC.Initialize ();

            string[] blendShapeName = characterC.GetBlendShapeNames();

            if (blendShapeName.Length <=0 )
             throw new System.Exception("Dictionary Amount is 0 !?");

             int blendShapeID = 0;
             for (int i = 0; i < blendShapeName.Length; i++)
             if (blendShapeSlider.blendShapeName == blendShapeName[i])
             blendShapeID = i;

             blendShapeID = EditorGUILayout.Popup("BlendShapeName", blendShapeID, blendShapeName);
             blendShapeSlider.blendShapeName = blendShapeName[blendShapeID];
        }
    }



}

