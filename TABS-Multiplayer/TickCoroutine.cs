using Landfall.TABS;
using Landfall.TABS.UnitPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
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

                while (SocketConnection.tickCommands.TryDequeue(out string newData))
                {
                    if(newData.StartsWith("SPAWNUNIT"))
                    {
                        string[] split = newData.Split('|');
                        string entName = split[1];
                        Team team = (Team)Enum.Parse(typeof(Team), split[2]);
                        Vector3 pos = StrToVec3(split[3]);

                        GetBrushBehaviorOfUnitBrush(GameObject.FindObjectOfType<UnitBrush>()).Place(GetUnitBlueprint(entName), team, pos);
                    } else if(newData.StartsWith("REMOVEUNIT"))
                    {
                        string[] split = newData.Split('|');
                        Vector3 pos = StrToVec3(split[1]);
                        Team team = (Team)Enum.Parse(typeof(Team), split[2]);

                        Unit unit = FindClosestUnit(pos);
                        if (unit != null && unit.Team == team)
                            GetBrushBehaviorOfUnitBrush(GameObject.FindObjectOfType<UnitBrush>()).Remove(unit, team);
                    }
                }
            }
        }

        private static Unit FindClosestUnit(Vector3 pos)
        {
            return GameObject.FindObjectsOfType<Unit>().OrderBy(o => (o.transform.position - pos).sqrMagnitude).FirstOrDefault();
        }

        private static UnitBlueprint GetUnitBlueprint(string EntName) // Get the blueprint by the entity name
        {
            foreach(UnitBlueprint ub in LandfallUnitDatabase.GetDatabase().Units)
            {
                if (ub.Entity.Name == EntName)
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