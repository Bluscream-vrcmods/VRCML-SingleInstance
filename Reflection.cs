using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using VRC.Core;

namespace SingleInstance
{
    internal static class Reflection
    {
        internal static void EnterWorld(string id)
        {
            try {
                // Reflection.enterWorld.Invoke(null, null, [id]);
            } catch (Exception ex){
                SingleInstance.Log($"Unable to enter World :/ {ex.Message}");
            }
        }
        private static MethodInfo enterWorld = typeof(PortalInternal).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.Name == "EnterWorld" && x.GetParameters().Length == 2);
        private static MethodInfo enterHomeWorld = typeof(PortalInternal).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.Name == "EnterWorld" && x.GetParameters().Length == 0);
        /*internal static PropertyInfo[] vrcFlowManager = typeof(VRCFlowManager).GetProperties(BindingFlags.Static | BindingFlags.Public);
        internal static MethodInfo Enterworld;
        internal static void Start()
        {
            foreach(var method in vrcFlowManager)
            {
                if (method.Name == "EnterWorld")
                    Enterworld = method.GetGetMethod();
            }
        }*/
        // internal static MethodInfo Enterworld = vrcFlowManager.First((Meth p) => p.getMethod().Name == "EnterWorld" && p.GetGetMethod().GetParameters().Length == 4)).getMethod();
    }
}