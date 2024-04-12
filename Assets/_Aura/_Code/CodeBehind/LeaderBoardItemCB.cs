using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardItemCB : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private TMP_Text highScoreText;

    public void SetUpItem(Player _player)
    {
        userNameText.text = _player.username;
        highScoreText.text = _player.HighScore.ToString();
    }
}
