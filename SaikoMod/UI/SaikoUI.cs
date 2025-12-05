using System.Collections.Generic;
using System.Linq;
using RapidGUI;
using UnityEngine;
using SaikoMod.Core.Enums;
using SaikoMod.Mods;
using SaikoMod.Utils;
using SaikoMod.Helper;

namespace SaikoMod.UI
{
    public class SaikoUI : BaseWindowUI
    {
        int page = 0;

        SaikoSkins skins = SaikoSkins.Normal;

        Shader toonShader;
        Shader diffuseShader;
        bool shaderToggled = false;

        Material[][] originalMat = new Material[3][];
        Color eyeColor = Color.white;

        public YandereController yand;
        YandereAI ai;
        YandereGraphicQualityManager graphic;

        List<string> EmoteNames = new List<string>();
        List<string> EmoteFilesNames = new List<string>();

        Animation EmoteAnimation;
        List<AnimationClip> animationClips = new List<AnimationClip>();
        AssetBundle EmoteAsset;
        int animIdx = 0;

        public SaikoUI()
        {
            AssetBundleHelper.InitBundle("mods/emotes/saiko/", ".emotes", (string ename, string filename) =>
            {
                EmoteNames.Add(ename);
                EmoteFilesNames.Add(filename);
            });
        }

        public void OnLoad()
        {
            if (yand = Object.FindObjectOfType<YandereController>()) {
                ai = yand.aI;

                graphic = yand.GetComponent<YandereGraphicQualityManager>();
                toonShader = ai.eyeMat.shader;
                diffuseShader = Shader.Find("Legacy Shaders/Transparent/Diffuse");

                for (int i = 0; i < graphic.meshToChangeMat.Length; i++) {
                    originalMat[i] = graphic.meshToChangeMat[i].materials;
                }
            }

            foreach (string animName in EmoteFilesNames)
            {
                EmoteAsset = AssetBundle.LoadFromFile("mods/emotes/saiko/" + animName);
                animationClips.Add(EmoteAsset.LoadAllAssets<AnimationClip>()[0]);
            }
        }
        public void OnUnload()
        {
            animationClips.Clear();
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
                        yand.mood.saikoState = RGUI.Field(yand.mood.saikoState, "Saiko State");
                        yand.mood.angerLevel = RGUI.SliderInt(yand.mood.angerLevel, 0, 10, 0, "Anger Level");
                        if (RGUI.Button(yand.isActive, "Is Active")) yand.isActive = !yand.isActive;

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
                            if (skins == SaikoSkins.Normal) {
                                for (int i = 0; i < graphic.meshToChangeMat.Length; i++)
                                {
                                    graphic.meshToChangeMat[i].materials = originalMat[i];
                                }
                                return;
                            }

                            Material[] mats = new Material[3];
                            switch (skins) {
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
                            if (RGUI.Button(shaderToggled, "Diffuse Toggle"))
                            {
                                if (shaderToggled = !shaderToggled)
                                {
                                    ai.eyeShader.shader = diffuseShader;
                                    ai.eyeShader.color = eyeColor;
                                } else {
                                    ai.eyeShader.shader = toonShader;
                                    ai.eyeShader.color = eyeColor;
                                }
                            }
                            eyeColor = RGUI.ColorPicker(eyeColor, "Eye Shader Color");
                            ai.eyeMat.color = RGUI.ColorPicker(ai.eyeMat.color, "Eye Material Color");
                        }
                        GUILayout.EndVertical();
                    }
                    break;
                case 2:
                    if (RGUI.ArrayNavigatorButton<AnimationClip>(ref animIdx, animationClips, "Animation"))
                    {
                        if (!EmoteAnimation) EmoteAnimation = yand.gameObject.AddComponent<Animation>();
                        if (EmoteAnimation.GetClip(EmoteNames[animIdx]) == null) EmoteAnimation.AddClip(animationClips[animIdx], EmoteNames[animIdx]);
                        EmoteAnimation.Play(EmoteNames[animIdx]);
                    }
                    break;
            }
            page = RGUI.Page(page, 3, true);
        }

        public override string Title => "Saiko";
    }
}
