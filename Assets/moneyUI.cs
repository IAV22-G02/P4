using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace es.ucm.fdi.iav.rts.G02
{
    public class moneyUI : MonoBehaviour
    {
        public int team;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            this.gameObject.GetComponent<Text>().text = GameManager.Instance.GetMoney(team).ToString();
        }
    }
}
