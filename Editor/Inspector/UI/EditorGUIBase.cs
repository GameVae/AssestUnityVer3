﻿#if UNITY_EDITOR

using UnityEditor;
using UI.Widget;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(CustomGUI), editorForChildClasses: true), CanEditMultipleObjects]
    public class EditorGUIBase : Editor
    {
        private CustomGUI BaseOwner;

        protected bool showMaskGrap;
        protected bool maskable;
        protected Sprite maskSprite;

        protected bool useBackgroud;
        protected Sprite backgroudSprite;

        protected bool interactable;
        protected bool isPlaceholder;
        protected float fontSize;
        protected string placeholder;
        protected Color placeholderColor;

        protected readonly GUILayoutOption[] sizeOption = new GUILayoutOption[]
        {
        };

        protected virtual void OnEnable()
        {
            BaseOwner = (CustomGUI)target;
            Undo.RecordObject(BaseOwner, BaseOwner.name);
            BaseOwner.SetChildrenDependence();

            // init 
            interactable = BaseOwner.Interactable;

            // backgroud sprite
            backgroudSprite = BaseOwner.BackgroudSprite;
            useBackgroud = BaseOwner.IsBackground;

            // mask field setup
            maskable = BaseOwner.Maskable;
            maskSprite = BaseOwner.MaskSprite;
            showMaskGrap = BaseOwner.Mask ? BaseOwner.Mask.showMaskGraphic : false;

            // placeholder field
            isPlaceholder = BaseOwner.IsPlaceholder;
            placeholder = BaseOwner.Placeholder?.text;
            placeholderColor = BaseOwner.PlaceholderColor;
            fontSize = BaseOwner.FontSize;
        }

        public override void OnInspectorGUI()
        {
            if (!BaseOwner.UIDependent && !Application.isPlaying)
            {
                InteractableGUI();
                MaskableGUI();
                BackgroudGUI();
                PlaceholderGUI();

                if (GUI.changed)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
                base.OnInspectorGUI();
            }
        }

        protected virtual void MaskableGUI()
        {
            maskable = EditorGUILayout.Toggle("__Maskable", maskable, sizeOption);
            if (maskable != BaseOwner.Maskable)
            {
                BaseOwner.MaskableChange(maskable);
            }

            using (new EditorGUI.IndentLevelScope())
            {
                if (maskable)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    {
                        EditorGUILayout.ObjectField("Readonly Target Graphic", BaseOwner.Mask.graphic, typeof(Graphic), false);
                    }
                    EditorGUI.EndDisabledGroup();

                    showMaskGrap = EditorGUILayout.Toggle("Show Mask Graphic", showMaskGrap);
                    BaseOwner.Mask.showMaskGraphic = showMaskGrap;

                    maskSprite = (Sprite)
                       EditorGUILayout.ObjectField("Mask Sprite", maskSprite, typeof(Sprite), false, sizeOption);

                    if (BaseOwner.MaskSprite != maskSprite)
                    {
                        BaseOwner.MaskSpriteChange(maskSprite);
                        if (BaseOwner.MaskSprite)
                            EditorUtility.SetDirty(BaseOwner.MaskSprite);
                    }
                }
            }
        }

        protected virtual void BackgroudGUI()
        {
            useBackgroud = EditorGUILayout.Toggle("__Is Use Backgroud", BaseOwner.IsBackground);

            if (useBackgroud != BaseOwner.IsBackground)
            {
                BaseOwner.IsBackgroudChange(useBackgroud);
            }
            using (new EditorGUI.IndentLevelScope())
            {
                if (useBackgroud)
                {
                    backgroudSprite = (Sprite)EditorGUILayout.ObjectField("Backgroud Sprite", backgroudSprite, typeof(Sprite), false, sizeOption);
                    if (BaseOwner.BackgroudSprite != backgroudSprite)
                    {
                        BaseOwner.BackgroundChange(backgroudSprite);
                        if (BaseOwner.BackgroudSprite)
                            EditorUtility.SetDirty(BaseOwner.BackgroudSprite);
                    }
                }
            }
        }

        protected virtual void InteractableGUI()
        {
            interactable = EditorGUILayout.Toggle("__Interactable", interactable);
            if (interactable != BaseOwner.Interactable)
            {
                BaseOwner.InteractableChange(interactable);
            }
        }

        protected virtual void PlaceholderGUI()
        {
            isPlaceholder = EditorGUILayout.Foldout(isPlaceholder, "__Is Use Placeholder");
            if (isPlaceholder != BaseOwner.IsPlaceholder)
                BaseOwner.IsPlaceholderChange(isPlaceholder);
            using (new EditorGUI.IndentLevelScope())
            {
                if (isPlaceholder)
                {
                    bool isChanged = false;
                    placeholder = EditorGUILayout.DelayedTextField("Placeholder", placeholder);
                    if (placeholder != BaseOwner.Placeholder?.text)
                    {
                        BaseOwner.PlaceholderValueChange(placeholder);
                        isChanged = true;
                    }

                    placeholderColor = EditorGUILayout.ColorField("Color", placeholderColor);
                    if (placeholderColor != BaseOwner.PlaceholderColor)
                    {
                        BaseOwner.PlaceholderColorChange(placeholderColor);
                        isChanged = true;
                    }

                    fontSize = EditorGUILayout.DelayedFloatField("Font Size", fontSize);
                    if (!Mathf.Approximately(fontSize, BaseOwner.FontSize))
                    {
                        BaseOwner.FontSizeChange(fontSize);
                        isChanged = true;
                    }
                    if (isChanged) EditorUtility.SetDirty(BaseOwner.Placeholder);
                }
            }
        }
    }
}
#endif
