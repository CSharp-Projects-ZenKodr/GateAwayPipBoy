﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoPanel : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TextMeshProUGUI _teamNameText;
    [SerializeField] private TextMeshProUGUI _playerTypeText;
#pragma warning restore 0649

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        _nameInputField.text = player.playerName;
        _nameInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    private void Update()
    {
        _playerTypeText.text = player.deviceType.ToString();
        if (player.teamIdentity != null)
        {
            _teamNameText.text = player.teamIdentity.GetComponent<Team>().teamName;
        }
    }

    private void OnInputFieldEndEdit(string text)
    {
        player.CmdSetPlayerName(text);
    }
}
