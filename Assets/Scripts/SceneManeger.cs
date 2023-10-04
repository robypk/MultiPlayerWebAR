using Photon.Pun;
using Photon.Realtime;
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
            PhotonNetwork.LoadLevel(sceneIndex);
        }


    }
}
