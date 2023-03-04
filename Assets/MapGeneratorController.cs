using UnityEngine;
using MapGeneration;
using DelaunatorSharp;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

public class MapGeneratorController : MonoBehaviour
{
    struct UserAction
    {
        public UnityEngine.Events.UnityAction action;
        public string text;
        public KeyCode hotkey;
    }

    [SerializeField] GameObject buttonPrefab;

    private MapGenerator generator = null;

    private List<UserAction> userActions;

    void OnEnable()
    {
        generator = FindObjectOfType<MapGenerator>();

        InitializeUserActions();

        CreateActionButtons();
    }

    void Update()
    {
        foreach (var userAction in userActions)
        {
            if (Input.GetKeyDown(userAction.hotkey))
            {
                userAction.action();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            generator.points.Add(new Point(target.x, target.y));

            generator.GenerateAsync();
        }
    }

    private void InitializeUserActions()
    {
        userActions = new List<UserAction>
        {
            new UserAction
            {
                text = "[Space] Clear",
                action = () => generator.Clear(),
                hotkey = KeyCode.Space
            },
            new UserAction
            {
                text = "[Return] Generate",
                action = () =>
                {
                    generator.Clear();
                    generator.GenerateAsync();
                },
                hotkey = KeyCode.Return
            },
            new UserAction
            {
                text = "Relax points",
                action = () => generator.RelaxPoints(),
                hotkey = KeyCode.None
            }
        };
    }

    void CreateActionButtons()
    {
        var parentPanel = GameObject.Find("ActionPanel");

        foreach (var userAction in userActions)
        {
            var button = Instantiate(buttonPrefab, parentPanel.transform);
            button.GetComponent<Button>().onClick.AddListener(userAction.action);
            button.GetComponentInChildren<TMP_Text>().text = userAction.text;
        }
    }
}
