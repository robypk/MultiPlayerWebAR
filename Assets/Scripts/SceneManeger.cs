using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ROBY.SceneManeger
{
    public class SceneManeger : MonoBehaviour
    {
        public static SceneManeger Instance;


        private void Awake()
        {
            Instance = this;
        }
        public void onSceneChangeButtonClick(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }


    }
}
