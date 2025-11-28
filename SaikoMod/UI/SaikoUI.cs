using RapidGUI;
using UnityEngine;
using SaikoMod.Mods;
using SaikoMod.Utils;

namespace SaikoMod.UI
{
    public class SaikoUI : BaseWindowUI
    {
        int page = 0;
        YandereController yand;

        public void Reload()
        {
            yand = Object.FindObjectOfType<YandereController>();
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
                    if (yand)
                    {
                        if (RGUI.Button(yand.inChase, "In Chase")) yand.inChase = !yand.inChase;
                        if (RGUI.Button(yand.shouldRun, "Should Run")) yand.shouldRun = !yand.shouldRun;
                        if (GUILayout.Button("Attempt Kidnap Player")) yand.AtemptKidnapPlayer();
                        if (GUILayout.Button("Fake Attack")) yand.FakeAttack();
                        if (GUILayout.Button("Increase Anger")) yand.mood.IncreaseAnger();
                    }
                    GUILayout.EndVertical();

                    if (yand)
                    {
                        yand.aI.currentState = RGUI.Field(yand.aI.currentState, "AI State");
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
                    if (yand)
                    {
                        GUILayout.BeginVertical("Box");
                        if (GUILayout.Button("Move To Certain")) yand.MoveToCertainLocation(PlayerUI.curPlayerPos);
                        if (GUILayout.Button("Walk To Destination")) yand.WalkToDestination(PlayerUI.curPlayerPos);
                        if (GUILayout.Button("Run To Destination")) yand.RunToDestination(PlayerUI.curPlayerPos);
                        if (GUILayout.Button("Patrol To Destination")) yand.PatrolToDestination(PlayerUI.curPlayerPos);
                        if (GUILayout.Button("Rotate Towards Object")) yand.RotateTowardsObject(PlayerUI.curPlayerPos);
                        GUILayout.EndVertical();
                    }
                    break;
            }
            page = RGUI.Page(page, 3, true);
        }

        public override string Title => "Saiko";
    }
}
