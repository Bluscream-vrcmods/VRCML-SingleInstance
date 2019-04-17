# VRCML-SingleInstance ![](https://img.shields.io/github/downloads/Bluscream/VRCML-SingleInstance/total.svg)
SingleInstance Mod for VRCModLoader


## Description

This module allows you to use `world_id:instance_id` combinations in an already existing VRChat window.
Just copy text containing such a string to your clipboard and your game will ask you if you want to switch to it.
You can use `force=true` anywhere before the actual world_id to skip the confirmation dialog in case your vrchat is bugged in a way that you can't confirm it (example below)

![Screenshot](https://i.imgur.com/152DBMA.png)

Examples in clipboard:
```
wrld_6caf5200-70e1-46c2-b043-e3c4abe69e0f:14195
vrchat://launch?id=wrld_6caf5200-70e1-46c2-b043-e3c4abe69e0f:14195
vrchat://launch?force=true&id=wrld_6caf5200-70e1-46c2-b043-e3c4abe69e0f:14195
vrchat://launch?id=wrld_6caf5200-70e1-46c2-b043-e3c4abe69e0f:14195~hidden(usr_e0c03050-d290-44b9-b643-8fbd28158b50)~nonce(02A612A550AEDA9464DFB0B864441EEFE0A0D3437A28E7E6A9686B775F507718)
```
