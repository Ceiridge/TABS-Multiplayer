using Landfall.TABS;
using Landfall.TABS.Budget;
using Landfall.TABS.UnitPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TABS_Multiplayer;
using UnityEngine;

#pragma warning disable CS0626
namespace TABS_Multiplayer
{
    class TickCoroutine
    {
        private static float waitTime = 0f, interval = 0.0166f;

        public static void Tick() // A thread safe repeating method
        {
            if (Time.time > waitTime) // Wait until the interval has passed (~60tps)
            {
                waitTime += interval;
                SocketConnection.SetCulture();

                if (SocketConnection.getUIClient().Connected)
                {
                    ScreenshareSender.SetWinHandle(); // Send the unity window handle for receiving images
                }

                if (SocketConnection.switchScene)
                {
                    SocketConnection.switchScene = false;
                    TABSSceneManager.LoadScene(SocketConnection.newScene, true); // Instantly load the new scene
                }

                if (SocketConnection.switchMap)
                {
                    //SocketConnection.WriteToUI("SHOWMSG|Loading a map!"); // Debug
                    SocketConnection.switchMap = false;
                    CampaignPlayerDataHolder.StartedPlayingSandbox(); // Change the game state
                    TABSSceneManager.LoadMap(GetMap(SocketConnection.newMap)); // Load the new map
                }

                if(SocketConnection.updateBudget) // Refresh the UI's budget
                {
                    SocketConnection.updateBudget = false;
                    UpdateUIBudget();
                }


                while (SocketConnection.tickCommands.TryDequeue(out string newData))
                {
                    if(newData.StartsWith("SPAWNUNIT"))
                    {
                        string[] split = newData.Split('|');
                        string entName = split[1];
                        Team team = (Team)Enum.Parse(typeof(Team), split[2]);
                        Vector3 pos = StrToVec3(split[3]);

                        UnitBlueprint blueprint = GetUnitBlueprint(entName);
                        GetBrushBehaviorOfUnitBrush(GameObject.FindObjectOfType<UnitBrush>()).Place(blueprint, team, pos); // Add the unit with the brush

                        ServiceLocator.GetService<BattleBudget>().SpendAmount(team, (int)blueprint.UnitCost); // Update the budget
                        UpdateUIBudget();
                    } else if(newData.StartsWith("REMOVEUNIT"))
                    {
                        string[] split = newData.Split('|');
                        Vector3 pos = StrToVec3(split[1]);
                        Team team = (Team)Enum.Parse(typeof(Team), split[2]);

                        Unit unit = FindClosestUnit(pos); // Get the nearest unit of the pos
                        if (unit != null && unit.Team == team)
                        {
                            GetBrushBehaviorOfUnitBrush(GameObject.FindObjectOfType<UnitBrush>()).Remove(unit, team); // Remove the unit with the brush

                            ServiceLocator.GetService<BattleBudget>().ReturnAmount(team, (int)unit.unitBlueprint.UnitCost); // Update the budget
                            UpdateUIBudget();
                        }
                    } else if(newData.StartsWith("CLEAR"))
                    {
                        bool red = bool.Parse(newData.Split('|')[1]);
                        Team team = red ? Team.Red : Team.Blue;
                        PlacementUI pui = GameObject.FindObjectOfType<PlacementUI>(); // Get the placement UI

                        // Clear the right area without triggering an echo
                        UnitLayoutManager.ClearTeam(team);
                        GetClearButtonDelegate(pui)(team);
                    }
                    else if (newData.StartsWith("AUDIO"))
                    {
                        string[] split = newData.Split('|');

                        string soundRef = split[1];
                        float volMulti = float.Parse(split[2]);
                        Vector3 relPos = StrToVec3(split[3]);
                        SoundEffectVariations.MaterialType matType = (SoundEffectVariations.MaterialType)Enum.
                            Parse(typeof(SoundEffectVariations.MaterialType), split[4]);
                        Vector3 worldPos = Camera.main.transform.position + relPos; // Get the world pos from the relative one

                        ServiceLocator.GetService<SoundPlayer>().PlaySoundEffect(soundRef, volMulti, worldPos, matType);
                    }
                }
            }
        }

        private static void UpdateUIBudget()
        {
            typeof(UnitBrush).GetMethod("UpdateTeamBudgetsInUI", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(GameObject.FindObjectOfType<UnitBrush>(), null);
        }

        private static PlacementUI.ClearButtonDelegate GetClearButtonDelegate(PlacementUI pui)
        {
            return (PlacementUI.ClearButtonDelegate)typeof(PlacementUI).GetMethod("GetOnClickedClear").Invoke(pui, null);
        }

        private static Unit FindClosestUnit(Vector3 pos)
        {
            return GameObject.FindObjectsOfType<Unit>().OrderBy(o => (o.transform.position - pos).sqrMagnitude).FirstOrDefault();
        }

        private static UnitBlueprint GetUnitBlueprint(string EntName) // Get the blueprint by the entity name
        {
            foreach(UnitBlueprint ub in LandfallUnitDatabase.GetDatabase().Units)
            {
                if (ub.Entity.ID == EntName)
                    return ub;
            }
            return LandfallUnitDatabase.GetDatabase().Units[0];  // Return the first one if none is found (illegal state)
        }

        private static BrushBehaviourBase GetBrushBehaviorOfUnitBrush(UnitBrush ub)
        {
            return (BrushBehaviourBase)typeof(UnitBrush).GetMethod("GetBrushBehaviour").Invoke(ub, null);
        }

        private static Vector3 StrToVec3(string str)
        {
            string[] split = str.Replace("(", "").Replace(")", "").Split(',');
            return new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
        }

        private static MapAsset GetMap(int index) // Get the map by the index id
        {
            foreach(MapAsset map in LandfallUnitDatabase.GetDatabase().Maps)
            {
                if (map.m_mapIndex == index)
                    return map;
            }
            return LandfallUnitDatabase.GetDatabase().Maps[0]; // Return the first one if none is found (illegal state)
        }
    }
}

// Several hooks for reliable ticking
class patch_GeneralInput : GeneralInput 
{
    public extern void orig_Update();
    public void Update()
    {
        TickCoroutine.Tick();
        orig_Update();
    }
}
class patch_SandboxGameModeHandler : SandboxGameModeHandler
{
    public extern void orig_Update();
    public void Update()
    {
        TickCoroutine.Tick();
        orig_Update();
    }
}
class patch_CameraAbilityPossess : CameraAbilityPossess
{
    public extern void orig_LateUpdate();
    public void LateUpdate()
    {
        TickCoroutine.Tick();
        orig_LateUpdate();
    }
}
class patch_PlayerInput : PlayerInput
{
    public extern void orig_Update();
    public void Update()
    {
        TickCoroutine.Tick();
        orig_Update();
    }
}
class patch_MainMenuStateHandler : MainMenuStateHandler
{
    public void Update()
    {
        TickCoroutine.Tick();
    }
}