using es.ucm.fdi.iav.rts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace es.ucm.fdi.iav.rts
{
    public class RTSControllerID : MonoBehaviour
    {
        [SerializeField]
        [Header("Necessary to know which controller is using. Whether is IA or Human")]
        private RTSController controller;

        public RTSController getController() => controller;
    }
}

