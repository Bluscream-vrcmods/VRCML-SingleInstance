using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using VRC.Core;
using VRCModLoader;
using VRCTools;
using UnityEngine;

namespace SingleInstance
{
    [VRCModInfo("Single Instance", "2.1.2.1", "Bluscream")]
    public class SingleInstance : VRCMod
    {
        string world_pattern = @"(wrld_[a-z0-9]{8}(?:-[a-z0-9]{4}){3}-[a-z0-9]{12}):(\d+)";
        string world_pattern_private = @"(wrld_[a-z0-9]{8}(?:-[a-z0-9]{4}){3}-[a-z0-9]{12}):((.\d+)~hidden\((usr_[a-z0-9]{8}(?:-[a-z0-9]{4}){3}-[a-z0-9]{12})\)~nonce\(([A-z0-9]{64})\))";
        string lastClipboard = ""; //Type VRUIHelper;MethodInfo lastMarcoInvite;
        // VRCFlowManager VRCFlowManager;

        void OnApplicationStart() {
            Log($"OnApplicationStart");
            Log($"HWID: {VRC.Core.API.DeviceID}");
            System.Threading.Mutex m = new System.Threading.Mutex(false, "VRChat");
            if (m.WaitOne(1, false) == false)
            {
                Log($"ALREADY RUNNING!");
                string lp = "";
                bool first = true;
                foreach (var lp2 in Environment.GetCommandLineArgs())
                {
                    if (first) first = false;
                    else lp += " " + lp2;
                }
                string file = "commandline.txt";
                File.WriteAllText(file, lp);
                System.Diagnostics.Process.Start("notepad.exe", file);
                Application.Quit();
                return;
            }
            Log($"NOT ALREADY RUNNING!");
            lastClipboard = GetClipboardContent();
            /*try {
                VRUIHelper = Type.GetType("marcoclient.UI.VRUIHelper");
                lastMarcoInvite = VRUIHelper.GetMethod("lastInvite", BindingFlags.Public | BindingFlags.Static);
            } catch (Exception ex) { Log(ex.ToString()); }*/
            // VRCFlowManager = Resources.FindObjectsOfTypeAll<VRCFlowManager>()[0];
        }
        void OnUpdate() {
            // VRCModLogger.Log("SingleInstance > OnUpdate");
            var clipboard = GetClipboardContent();
            if (clipboard == lastClipboard) return;
            /*try {
                string result = (string)lastMarcoInvite.Invoke(null, null);
                if (clipboard == result) return;
            } catch (Exception) { }*/
            Log($"Clipboard changed to: \"{clipboard}\"");
            lastClipboard = clipboard;
            clipboard = clipboard.Trim().ToLower();
            if (!clipboard.Contains("wrld_")) return;
           var match = Regex.Match(clipboard, world_pattern_private);
           if (!match.Success)
            {
                match = Regex.Match(clipboard, world_pattern);
                if (!match.Success) return;
            }
           string world_id = match.Groups[1].Value;string instance_id = match.Groups[2].Value;bool is_private = match.Groups.Count > 3;
           string creator = ""; string nonce = ""; string instance_id_only = "";bool force = clipboard.Contains("force=true");
            if (is_private)
            {
                instance_id_only = match.Groups[3].Value;
                creator = match.Groups[4].Value;
                nonce = match.Groups[5].Value;
            }
            Log($"Catched World: {match.Groups[0].Value}");
            Log($"Catched World ID: {world_id}");
            Log($"Catched Instance: {instance_id}");
            Log($"Catched Instance ID: {instance_id_only}");
            Log($"Catched Instance Creator: {creator}");
            Log($"Catched Instance Nonce: {nonce}");
            string full_id = $"{world_id}:{instance_id}";
            ApiWorldInstance instance = null;
            ApiWorld apiWorld = new ApiWorld();
            var message = "";
            /*try {
                apiWorld.FetchInstance(world_id, (worldinstance) =>
                {
                    Log("Instance: " + worldinstance);
                    instance = worldinstance;
                });
                message = $"{instance.instanceWorld.name}\n Instance: {instance.idOnly} | Private: {(instance.isPublic ? "No" : "Yes")}";
            } catch(Exception ex) {
                Log($"Exception: {ex.Message}\n{ex.StackTrace}\n{ex}");
                message = $"{world_id}\n Instance: {(is_private ? instance_id_only : instance_id)} | Private: {(is_private ? "Yes" : "No")}"; // .Replace("wrld_", "")
            }*/
            if (force) {
                EnterWorld(full_id);
                return;
            }
            message = $"{world_id}\n Instance: {(is_private ? instance_id_only : instance_id)} | Private: {(is_private ? "Yes" : "No")}"; // .Replace("wrld_", "")
            VRCUiPopupManagerUtils.GetVRCUiPopupManager().ShowStandardPopup("URL Found", message,
                "Yes", () => {
                    VRCUiPopupManagerUtils.GetVRCUiPopupManager().HideCurrentPopup();
                    EnterWorld(full_id);
                },
                "Cancel", () =>
                {
                    VRCUiPopupManagerUtils.GetVRCUiPopupManager().HideCurrentPopup();
                }
            );
        }

        internal static void EnterWorld(string full_id)
        {
            Resources.FindObjectsOfTypeAll<VRCFlowManager>()[0].EnterWorld(full_id);
        }

        public static void Log(string message)
        {
            VRCModLogger.Log($"SingleInstance > {message}");
        }

        private string GetClipboardContent()
        {
            return GUIUtility.systemCopyBuffer.Trim().Replace("\n", "").Replace("\r", "");
        }
    }
}