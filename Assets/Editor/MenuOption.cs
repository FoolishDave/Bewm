using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOption : MonoBehaviour
{
    public enum ActionTypes { LoadScene, PlayCameraAnimation, ReverseCameraAnimation, PlaySound }

    public Material OptionUnselected;
    public Material OptionSelected;
    public string SceneToLoad;
    public Dictionary<ActionTypes, Action> MenuActions;
    public ActionTypes MenuAction;
    public MenuController.MenuState StateOptions;
    public MenuController.MenuState UsableState;
    private Animation anim;
    private MenuController menuController;
    private MenuController.MenuState delayState = MenuController.MenuState.Other;

    void Awake()
    {
        menuController = GameObject.Find("MenuController").GetComponent<MenuController>();
        MenuActions = new Dictionary<ActionTypes, Action>
        {
            {ActionTypes.LoadScene, LoadScene},
            {ActionTypes.PlayCameraAnimation, CameraAnimation},
            {ActionTypes.ReverseCameraAnimation, ReverseAnimation},
            {ActionTypes.PlaySound, PlaySound}
        };
    }

    void Start()
    {
        anim = menuController.SceneCamera.GetComponent<Animation>();
    }

    void LoadScene()
    {
        Debug.Log("Loading scene: " + SceneToLoad);
        SceneManager.LoadScene(SceneToLoad);
    }

    void GoToOptions()
    {
        
    }

    void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }

    void CameraAnimation()
    {
        anim[StateOptions.ToString()].speed = 1;
        anim.Play(StateOptions.ToString());
        delayState = StateOptions;
    }

    void ReverseAnimation()
    {
        anim[StateOptions.ToString()].time = anim[StateOptions.ToString()].length;
        anim[StateOptions.ToString()].speed = -1;
        anim.Play(StateOptions.ToString());
        delayState = MenuController.MenuState.Main;
    }

    void OnMouseEnter()
    {
        if (menuController.CurrentState == UsableState)
            GetComponentInChildren<Renderer>().material = OptionSelected;
    }

    void OnMouseExit()
    {
           GetComponentInChildren<Renderer>().material = OptionUnselected;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && menuController.CurrentState == UsableState)
        {
            MenuActions[MenuAction].Invoke();
        }
    }

    void Update()
    {
        if (delayState != MenuController.MenuState.Other)
        {
            menuController.CurrentState = delayState;
            delayState = MenuController.MenuState.Other;
        }
        
    }
}


[CustomEditor(typeof(MenuOption))]
public class MenuOptionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MenuOption script = target as MenuOption;

        script.OptionUnselected = (Material)EditorGUILayout.ObjectField("Unselected Material:", script.OptionUnselected,
            typeof(Material), false);
        script.OptionSelected = (Material)EditorGUILayout.ObjectField("Selected Material:", script.OptionSelected,
            typeof(Material), false);

        script.UsableState = (MenuController.MenuState) EditorGUILayout.EnumPopup("Usable On:", script.UsableState);
        script.MenuAction = (MenuOption.ActionTypes) EditorGUILayout.EnumPopup("Action:", script.MenuAction);
        
        if (script.MenuAction == MenuOption.ActionTypes.LoadScene)
        {
            script.SceneToLoad = EditorGUILayout.TextField("Scene To Load:", script.SceneToLoad);
        }

        if (script.MenuAction == MenuOption.ActionTypes.PlayCameraAnimation || script.MenuAction == MenuOption.ActionTypes.ReverseCameraAnimation)
        {
            script.StateOptions = (MenuController.MenuState) EditorGUILayout.EnumPopup(script.StateOptions);
        }
    }
}
