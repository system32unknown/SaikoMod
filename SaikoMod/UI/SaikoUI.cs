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

        AssetBundle EmoteAsset;

        Material[][] originalMat = new Material[3][];
        bool eyeMatColor = false;
        Material eyeMat;
        Shader eyeShader;
        Shader originalShader;

        Texture2D custom_eye;
        Texture original_eye;

        public YandereController yand;
        YandereAI ai;
        YandereGraphicQualityManager graphic;
        YandereMoodController mood;

        List<string> EmoteNames = new List<string>();
        List<string> EmoteFilenames = new List<string>();
        const string emotefilePath = "mods/emotes/saiko/";

        bool animAdded = false;
        Animation EmoteAnimation;
        List<AnimationClip> animationClips = new List<AnimationClip>();
        AnimationClip curClip;
        int animIdx = 0;

        public SaikoUI()
        {
            AssetBundleHelper.InitBundle(emotefilePath, ".emotes", (string ename, string filename) =>
            {
                EmoteNames.Add(ename);
                EmoteFilenames.Add(filename);
            });
        }

        public void OnLoad()
        {
            if (yand = Object.FindObjectOfType<YandereController>()) {
                ai = yand.aI;
                mood = yand.mood;
                graphic = yand.GetComponent<YandereGraphicQualityManager>();

                for (int i = 0; i < graphic.meshToChangeMat.Length; i++) {
                    originalMat[i] = graphic.meshToChangeMat[i].materials;
                }
            }

            custom_eye = PathUtils.TextureFromFile("mods/textures/saiko/custom_eye.png", TextureFormat.RGBA32);
            original_eye = Resources.FindObjectsOfTypeAll<Texture2D>().Where(x => x.name == "eye").First();

            eyeMat = graphic.meshToChangeMat[0].materials[1];
            originalShader = eyeMat.shader;
            eyeShader = Shader.Find("Legacy Shaders/Transparent/Diffuse");

            foreach (string animName in EmoteFilenames) {
                EmoteAsset = AssetBundle.LoadFromFile(emotefilePath + animName);
                animationClips.Add(EmoteAsset.LoadAllAssets<AnimationClip>()[0]);
            }
        }
        public void OnUnload()
        {
            animAdded = false;
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
                        mood.saikoState = RGUI.Field(mood.saikoState, "Saiko State");
                        mood.angerLevel = RGUI.SliderInt(mood.angerLevel, 0, 10, 0, "Anger Level");
                        mood.damagePerSwing = RGUI.SliderFloat(mood.damagePerSwing, 0f, 999f, 10f, "Damage Per Swing");
                        if (RGUI.Button(yand.isActive, "Is Active")) yand.isActive = !yand.isActive;

                        GUILayout.BeginVertical("Box");
                        yand.runSpeed = RGUI.SliderFloat(yand.runSpeed, 0f, 999f, 9f, "Run Speed");
                        yand.walkSpeed = RGUI.SliderFloat(yand.walkSpeed, 0f, 999f, 5f, "Walk Speed");
                        yand.patrolSpeed = RGUI.SliderFloat(yand.patrolSpeed, 0f, 999f, 3f, "Patrol Speed");
                        yand.chaseSpeed = RGUI.SliderFloat(yand.chaseSpeed, 0f, 999f, 5f, "Chase Speed");
                        yand.seeDistance = RGUI.SliderFloat(yand.seeDistance, 0f, 999f, 5f, "See Distance");
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
                                case SaikoSkins.What:
                                    mats[0] = MaterialUtils.CorruptMaterial();
                                    mats[1] = MaterialUtils.CorruptMaterial();
                                    mats[2] = MaterialUtils.CorruptMaterial();
                                    break;
                            }
                            foreach (SkinnedMeshRenderer skin in graphic.meshToChangeMat) skin.materials = mats;
                        }

                        if (RGUI.Button(YandModAI.customEye, "Custom Eye")) {
                            YandModAI.customEye = !YandModAI.customEye;
                            ai.eyeShader.SetTexture("_MainTex", YandModAI.customEye ? custom_eye : original_eye);
                        }
                        if (YandModAI.customEye) ai.eyeShader.color = RGUI.ColorPicker(ai.eyeShader.color, "Glow Eye Color");
                        if (RGUI.Button(eyeMatColor, "Eye Color")) {
                            eyeMatColor = !eyeMatColor;
                            eyeMat.shader = eyeMatColor ? eyeShader : originalShader;
                        }
                        if (eyeMatColor) eyeMat.color = RGUI.ColorPicker(eyeMat.color, "Eye Color");
                        GUILayout.EndVertical();
                    }
                    break;
                case 2:
                    if (EmoteFilenames.Count < 0) return;

                    GUILayout.BeginVertical("Box");
                    if (!animAdded && GUILayout.Button("Add Custom Anim")) {
                        animAdded = true;
                        yand.gameObject.GetComponent<Animator>().enabled = false;
                        if (!EmoteAnimation) EmoteAnimation = yand.gameObject.AddComponent<Animation>();
                    }
                    if (animAdded && GUILayout.Button("Remove Custom Anim")) {
                        animAdded = false;
                        yand.gameObject.GetComponent<Animator>().enabled = true;
                        if (EmoteAnimation) Object.Destroy(EmoteAnimation);
                    }
                    if (animAdded && EmoteAnimation)
                    {
                        if (RGUI.ArrayNavigatorButton<AnimationClip>(ref animIdx, animationClips, "Animation")) {
                            if (EmoteAnimation.GetClip(EmoteNames[animIdx]) == null) EmoteAnimation.AddClip(curClip, EmoteNames[animIdx]);
                            EmoteAnimation.Play(EmoteNames[animIdx]);
                        }
                        if (EmoteAnimation.isPlaying && GUILayout.Button("Stop")) EmoteAnimation.Stop();
                        curClip = animationClips[animIdx];
                        curClip.wrapMode = RGUI.Field(curClip.wrapMode, "Wrap Mode");
                    }
                    GUILayout.EndVertical();
                    break;
            }
            page = RGUI.Page(page, 3, true);
        }

        public override string Title => "Saiko";
    }
}
