using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LAN.UI {
    public class Charts : CustomBehaviour {
        [System.Serializable]
        public class DataChart {
            public string key, label;
            public ulong primaryValue, secondaryValue;
        }
    }
}
