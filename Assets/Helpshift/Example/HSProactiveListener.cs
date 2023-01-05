using System.Collections.Generic;
using Helpshift;
using UnityEngine;

namespace HelpshiftExample
{
    public class ProactiveConfigCollector : IHelpshiftProactiveAPIConfigCollector
    {
        public Dictionary<string, object> getLocalApiConfig()
        {
            Debug.Log("Helpshift - event_ getApiConfig");
            Dictionary<string, object> proactiveConfig = new Dictionary<string, object>();
            proactiveConfig.Add("initialUserMessage", "Hi there!");
            proactiveConfig.Add("fullPrivacy", true);
            proactiveConfig.Add("contactUsVisibility", 1);
            proactiveConfig.Add("tags", new string[] { "vip", "payment", "blocked", "renewal" });
            return proactiveConfig;

        }
    }
}



