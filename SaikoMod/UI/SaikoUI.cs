using System.Collections.Generic;
using System.Linq;
using System.IO;
using RapidGUI;
using UnityEngine;
using SaikoMod.Core.Enums;
using SaikoMod.Mods;
using SaikoMod.Utils;

namespace SaikoMod.UI
{
    public class SaikoUI : BaseWindowUI
    {
        int page = 0;

        SaikoSkins skins = SaikoSkins.Black;
        Material[] toonShaders;
        float litIntensity = 0f;

        YandereController yand;
        YandereAI ai;
        YandereGraphicQualityManager graphic;

        string animName = "";
        List<string> EmoteFilesNames = new List<string>();
        Animation EmoteAnimation;
        AnimationClip animationClip;
        AssetBundle EmoteAsset;

        public SaikoUI()
        {
            string text = "mods/emotes/";
            if (Directory.Exists(text))
            {
                foreach (string text2 in Directory.GetFiles(text))
                {
                    if (text2.Length > 0 && text2.EndsWith(".emotes"))
                    {
                        try
                        {
                            animName = Path.GetFileName(text2).Substring(0, Path.GetFileName(text2).Length - 7);
                            EmoteFilesNames.Add(Path.GetFileName(text2));
                        }
                        catch { }
                    }
                }
            }
            else Directory.CreateDirectory(text);
        }

        public void OnLoad()
        {
            if (yand = Object.FindObjectOfType<YandereController>()) {
                ai = yand.aI;
                toonShaders = ai.toonShader;

                graphic = yand.GetComponent<YandereGraphicQualityManager>();
            }

            for (int i = 0; i < EmoteFilesNames.Count; i++)
            {
                EmoteAsset = AssetBundle.LoadFromFile("mods/emotes/" + EmoteFilesNames[i]);
                Debug.Log(EmoteAsset);
                animationClip = EmoteAsset.LoadAllAssets<AnimationClip>()[0];
            }
        }
        public void OnUnload()
        {
            AssetBundle.UnloadAllAssetBundles(true);
        }

        public override void Draw()
        {
            switch (page)
            {
                case 0:
                    GUILayout.BeginVertical("Box");
                    if (RGUI.Button(YandModController.noDetect, "No Detect")) YandModController.noDetect = !YandModController.noDetect;
                    if (RGUI.Button(YandModAI.notAttacted, "No Attact")) YandModAI.notAttacted = !YandModAI.notAttacted;
                    if (RGUI.Button(YandModAI.noDistanceCheck, "No Distance Check")) YandModAI.noDistanceCheck = !YandModAI.noDistanceCheck;
                    if (RGUI.Button(YandModController.noAlert, "No Alerted")) YandModController.noAlert = !YandModController.noAlert;
                    if (RGUI.Button(YandModController.noPushing, "No Push")) YandModController.noPushing = !YandModController.noPushing;
                    YandModController.lookMode = RGUI.Field(YandModController.lookMode, "Look Mode");
                    if (yand) {
                        if (GUILayout.Button("Attempt Kidnap Player")) yand.AtemptKidnapPlayer();
                        if (GUILayout.Button("Fake Attack")) yand.FakeAttack();
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Increase Anger")) yand.mood.IncreaseAnger();
                        if (GUILayout.Button("Give Gift")) yand.GiveGiftToSenpai();
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("TP Saiko To Player")) {
                            yand.playerAgent.enabled = false;
                            yand.transform.position = yand.playerChar.transform.position;
                            yand.playerAgent.enabled = true;
                        }
                        if (GUILayout.Button("TP Player To Saiko")) yand.playerChar.transform.position = yand.transform.position;
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();

                    if (yand)
                    {
                        ai.currentState = RGUI.Field(ai.currentState, "AI State");
                        yand.mood.mood = RGUI.Field(yand.mood.mood, "AI Mood");
                        yand.mood.saikoState = RGUI.Field(yand.mood.saikoState, "Saiko State");
                        yand.slowDownType = RGUI.Field(yand.slowDownType, "Slowdown Type");
                        yand.mood.angerLevel = RGUI.SliderInt(yand.mood.angerLevel, 0, 10, 0, "Anger Level");
                        if (RGUI.Button(yand.isActive, "Is Active")) yand.isActive = !yand.isActive;

                        if (GUILayout.Button("RNG Voice"))
                        {
                            foreach (LipSyncVoice[] voices in ReflectionHelpers.GetPublicFieldsOfType<LipSyncVoice[]>(yand.facial)) LipSyncUtils.Shufflevoices(voices);
                            foreach (LipSyncVoice voices in ReflectionHelpers.GetPublicFieldsOfType<LipSyncVoice>(yand.facial)) LipSyncUtils.Shufflevoice(voices);
                        }
                        if (GUILayout.Button("TEST ANIM"))
                        {
                            EmoteAnimation = yand.gameObject.AddComponent<Animation>();
                            EmoteAnimation.AddClip(animationClip, animName);
                            EmoteAnimation.Play(animName);
                        }

                        GUILayout.BeginVertical("Box");
                        yand.runSpeed = RGUI.SliderFloat(yand.runSpeed, 0f, 999f, 9f, "Run Speed");
                        yand.walkSpeed = RGUI.SliderFloat(yand.walkSpeed, 0f, 999f, 5f, "Walk Speed");
                        yand.patrolSpeed = RGUI.SliderFloat(yand.patrolSpeed, 0f, 999f, 3f, "Patrol Speed");
                        yand.chaseSpeed = RGUI.SliderFloat(yand.chaseSpeed, 0f, 999f, 5f, "Chase Speed");
                        GUILayout.EndVertical();
                    }
                    break;
                case 1:
                    if (GameUI.curRoom) {
                        GUILayout.BeginVertical("Box");
                        if (GUILayout.Button("Look Window")) yand.LookThroughWindow(GameUI.curRoom);
                        if (GUILayout.Button("Hide Behind Desk")) yand.HideBehindDesk(GameUI.curRoom);
                        if (GUILayout.Button("Harrass Player")) yand.HarrassPlayer(GameUI.curRoom);
                        if (GUILayout.Button("Come Here")) yand.GoToComeHerePosition(GameUI.curRoom);
                        if (GUILayout.Button("Drag Player To Room")) yand.DragPlayerToTheRoom(GameUI.curRoom);
                        GUILayout.EndVertical();
                    }
                    if (yand) {
                        GUILayout.BeginVertical("Box");
                        if (GUILayout.Button("Move To Certain")) yand.MoveToCertainLocation(PlayerUI.curPlayerPos);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Walk To Destination")) yand.WalkToDestination(PlayerUI.curPlayerPos);
                        if (GUILayout.Button("Run To Destination")) yand.RunToDestination(PlayerUI.curPlayerPos);
                        if (GUILayout.Button("Patrol To Destination")) yand.PatrolToDestination(PlayerUI.curPlayerPos);
                        GUILayout.EndHorizontal();
                        if (GUILayout.Button("Rotate Towards Object")) yand.RotateTowardsObject(PlayerUI.curPlayerPos);
                        GUILayout.EndVertical();
                    }
                    if (graphic && yand)
                    {
                        GUILayout.BeginVertical("Box");
                        skins = RGUI.Field(skins, "Saiko Skins");
                        if (GUILayout.Button("Change"))
                        {
                            Material[] mats = new Material[3];
                            switch (skins)
                            {
                                case SaikoSkins.Black: mats = Enumerable.Repeat(MaterialUtils.black, 3).ToArray(); break;
                                case SaikoSkins.Shadow: mats = Enumerable.Repeat(MaterialUtils.CreateTransparent(new Color(0f, 0f, 0f, .3f)), 3).ToArray(); break;
                                case SaikoSkins.Ghost: mats = Enumerable.Repeat(MaterialUtils.CreateTransparent(new Color(115f, 169f, 255f, .2f)), 3).ToArray(); break;
                                case SaikoSkins.Invisible: mats = Enumerable.Repeat(MaterialUtils.CreateTransparent(new Color(0f, 0f, 0f, 0f)), 3).ToArray(); break;
                            }
                            foreach (SkinnedMeshRenderer skin in graphic.meshToChangeMat) skin.materials = mats;
                        }
                        if (RGUI.Button(YandModAI.customEye, "Custom Eye")) YandModAI.customEye = !YandModAI.customEye;
                        if (YandModAI.customEye)
                        {
                            ai.eyeShader.color = RGUI.ColorPickerOne(ai.eyeShader.color, "Eye Shader Color");
                            ai.eyeMat.color = RGUI.ColorPicker(ai.eyeMat.color, "Eye Material Color");
                            litIntensity = RGUI.SliderFloat(litIntensity, 0f, 1f, 0f, "Lit Intensity");
                            if (GUILayout.Button("Set Intensity"))
                            {
                                foreach (Material toon in toonShaders)
                                {
                                    toon.SetFloat("_SelfLitIntensity", litIntensity);
                                }
                            }
                        }
                        GUILayout.EndVertical();
                    }
                    break;
            }
            page = RGUI.Page(page, 3, true);
        }

        public override string Title => "Saiko";
    }
}
