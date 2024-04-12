using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;


/// <summary>
/// This singleton class acts as a service class between the REST API calls and the 
/// Game logic.
/// It contains public methods that you can call from any script in any scene to 
/// 1.) Get all players
/// 2.)create a new player
/// 3.)update the current player
/// 
/// </summary>
public class PlayerStatsManager : MonoBehaviour
{
    #region REST URL's
    private const string CreateNewPlayerUrl = "https://polycrestinfin8vs.azurewebsites.net/api/CreateNewPlayer";
    private const string UpdatePlayerUrl = "https://polycrestinfin8vs.azurewebsites.net/api/UpdatePlayer";
    private const string GetAllPlayersURL = "https://polycrestinfin8vs.azurewebsites.net/api/GetAllPlayers";
    #endregion

    #region Singleton Properties
    private static PlayerStatsManager instance;
    public static PlayerStatsManager Instance
    {
        get => instance;
    }
    #endregion

    private Player currentPlayer;
    private List<Player> retrievedPlayers;
    private void Awake()
    {
        SetupSingleton();
    }

    private void Start()
    {
        GetPlayers();
    }
    #region Public Leaderboard API

    public void GetCurrentPlayer(string _email, Action _gotPlayerSuccess, Action _gotPlayerError)
    {
       
        //foreach (var player in retrievedPlayers)
        //{
        //    if(player.email == _email)
        //    {
        //        currentPlayer = player;
        //        _gotPlayerSuccess?.Invoke();
        //        break;
        //    }
        //}

        currentPlayer = retrievedPlayers.FirstOrDefault<Player>(p=>p.email == _email);
        if(currentPlayer == null)
        {
            _gotPlayerError?.Invoke();
        }
        else
        {
            _gotPlayerSuccess?.Invoke();
        }
    }

    /// <summary>
    /// Retrieves all players from Database
    /// </summary>
    /// <param name="_errorCallback"></param>
    /// <param name="_successCallback"></param>
    /// <returns></returns>
    public List<Player> GetPlayers(Action<List<Player>> _successCallback = null, Action<string> _errorCallback = null)
    {
        retrievedPlayers = new List<Player>();

        WebRequests.Get(GetAllPlayersURL, (_error) =>
        {
            _errorCallback?.Invoke(_error.ToString());
        },
        (_result) =>
        {
            retrievedPlayers = JsonConvert.DeserializeObject<List<Player>>(_result);
            _successCallback?.Invoke(retrievedPlayers);
        });

        return retrievedPlayers;
    }

    /// <summary>
    /// Receives new player data compiled on 
    /// sign up in the sign up screen and uploads 
    /// to online database
    /// </summary>
    /// <param name="_player"></param>
    public void CreateNewPlayer(Player _player, Action _createNewPlayerErrorCallback, Action _createNewPlayerSuccessCallback)
    {
        var newPlayerJson = JsonConvert.SerializeObject(_player);

        WebRequests.PostJson(CreateNewPlayerUrl, newPlayerJson,
          (error) =>
          {
              _createNewPlayerErrorCallback?.Invoke();
          }
        , (success) =>
        {
            _createNewPlayerSuccessCallback?.Invoke();
        });
    }

    /// <summary>
    /// Receives the logged in players score 
    /// and uploads to database
    /// </summary>
    /// <param name="_player"></param>
    public void UpdatePlayer(int _playerScore)
    {
        Player updatePlayer = new Player()
        {
            id = currentPlayer.id,
            email = currentPlayer.email,
            username = currentPlayer.username,
            HighScore = _playerScore
        };

        var updatePlayerJson = JsonConvert.SerializeObject(updatePlayer);
        WebRequests.Put(UpdatePlayerUrl, updatePlayerJson, (error) => { }, (success) => { });
    }

    #endregion
    #region Initialization Utilites
    private void SetupSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
