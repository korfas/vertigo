using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameGrid))]
public class GameGridEditor : Editor {

    private GameGrid _gameGrid;

    SerializedProperty _gridObjectProp;
    SerializedProperty _hexagonsParentProp;
    SerializedProperty _hexagonCoordinatesParentProp;
    SerializedProperty _buttonsParentProp;
    SerializedProperty _hexagonPrefabProp;
    SerializedProperty _hexagonCoordinatePrefabProp;
    SerializedProperty _buttonPrefabProp;

    SerializedProperty _gridSizeProp;

    //SerializedProperty _gridSizeXProp;
    //SerializedProperty _gridSizeYProp;

    SerializedProperty _hexagonPaddingPercentProp;


    readonly GUIContent _gameObjectReferencesContent = new GUIContent("Game Object References");
    readonly GUIContent _gridObjectContent = new GUIContent("Grid Object");
    readonly GUIContent _hexagonsParentContent = new GUIContent("Hexagons' Parent");
    readonly GUIContent _hexagonCoordinatesParentContent = new GUIContent("Hexagon Coordinates' Parent");
    readonly GUIContent _buttonsParentContent = new GUIContent("Buttons' Parent");
    readonly GUIContent _hexagonPrefabContent = new GUIContent("Hexagon Prefab");
    readonly GUIContent _hexagonCoordinatePrefabContent = new GUIContent("Hexagon Coordinates Prefab");
    readonly GUIContent _buttonPrefabContent = new GUIContent("Button Prefab");

    bool _gameObjectReferencesFoldout;

    private void OnEnable() {

        _gameGrid = target as GameGrid;

        _gridObjectProp = serializedObject.FindProperty("_gridObject");
        _hexagonsParentProp = serializedObject.FindProperty("_hexagonsParent");
        _hexagonCoordinatesParentProp = serializedObject.FindProperty("_hexagonCoordinatesParent");
        _buttonsParentProp = serializedObject.FindProperty("_buttonsParent");
        _hexagonPrefabProp = serializedObject.FindProperty("_hexagonPrefab");
        _hexagonCoordinatePrefabProp = serializedObject.FindProperty("_hexagonCoordinatePrefab");
        _buttonPrefabProp = serializedObject.FindProperty("_buttonPrefab");

        _gridSizeProp = serializedObject.FindProperty("_gridSize");

        //_gridSizeXProp = serializedObject.FindProperty("_gridSizeX");
        //_gridSizeYProp = serializedObject.FindProperty("_gridSizeY");

        _hexagonPaddingPercentProp = serializedObject.FindProperty("_hexagonPaddingPercent");


    }

    public override void OnInspectorGUI() {

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        _gameObjectReferencesFoldout = EditorGUILayout.Foldout(_gameObjectReferencesFoldout, _gameObjectReferencesContent);

        if (_gameObjectReferencesFoldout) {
            EditorGUILayout.PropertyField(_gridObjectProp, _gridObjectContent);
            EditorGUILayout.PropertyField(_hexagonsParentProp, _hexagonsParentContent);
            EditorGUILayout.PropertyField(_hexagonCoordinatesParentProp, _hexagonCoordinatesParentContent);
            EditorGUILayout.PropertyField(_buttonsParentProp, _buttonsParentContent);
            EditorGUILayout.PropertyField(_hexagonPrefabProp, _hexagonPrefabContent);
            EditorGUILayout.PropertyField(_hexagonCoordinatePrefabProp, _hexagonCoordinatePrefabContent);
            EditorGUILayout.PropertyField(_buttonPrefabProp, _buttonPrefabContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField("Grid Size");
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        _gridSizeProp.vector2IntValue = EditorGUILayout.Vector2IntField("Grid Size", _gridSizeProp.vector2IntValue);

        //gridSizeXProp.intValue = EditorGUILayout.IntField("Width", _gridSizeXProp.intValue);
        //_gridSizeYProp.intValue = EditorGUILayout.IntField("Heght", _gridSizeYProp.intValue);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField("Design");
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.IntSlider(_hexagonPaddingPercentProp, 0, 50, "Hexagon Padding (%)");

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Apply")) {

            _gameGrid.Apply();
        }

    }
}
