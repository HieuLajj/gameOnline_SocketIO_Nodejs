using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class SceneManagementManager : MonoBehaviour
{
        public static void LoadLevel(string levelName) {
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }

        public static void LoadAddScene (string levelName){
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }

        public static void UnLoadScene(string levelName){
            SceneManager.UnloadSceneAsync(levelName);
        }
}

    public static class SceneList {
        public const string LOBBY_SCENE = "LobbyScene";
        public const string MENU_SCENE = "MenuScene";
        public const string PROFILE_SCENE = "ProfileScene";

        public const string Map1_Delta = "Map1_Delta";
        public const string Map2_Plateau = "Map2_Plateau";
        public const string Map3_Dungeon= "Map3_Dungeon";
    }
