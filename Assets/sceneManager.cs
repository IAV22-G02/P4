using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace es.ucm.fdi.iav.rts
{
    public class sceneManager : MonoBehaviour
    {
        public void changeScene1()
        {
            SceneManager.LoadScene("RTSScenario 2");
        }
        public void changeScene2()
        {
            SceneManager.LoadScene("RTSScenario 3");
        }
    }
}