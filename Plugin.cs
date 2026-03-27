using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using KrokoshaCasualtiesMP;
using MossLib.Example;
using UnityEngine;

namespace MossLib;

[BepInPlugin(Guid, Name, "1.0.1")]
public class Plugin : BaseUnityPlugin
{
    // ReSharper disable once MemberCanBePrivate.Global
    internal new static ManualLogSource Logger;
    public const string Guid = "blackmoss.mosslib";
    public const string Name = "Moss Lib";
    private readonly Harmony _harmony = new(Guid);

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public static Plugin Instance { get; private set; } = null!;

    private static readonly bool Mutliplayer = Tools.IsMultiplayerActive();

    // General
    public static ConfigEntry<float> HealCountdown;
    public static ConfigEntry<bool> UndeadMode;
    public static ConfigEntry<bool> SwitchModeTip;
    public static ConfigEntry<List<string>> MutliplayerUndead;

    // Limb
    public static ConfigEntry<float> MuscleHeadth;
    public static ConfigEntry<float> SkinHealth;
    public static ConfigEntry<float> BoneHealTImer;
    public static ConfigEntry<float> DislocationTimer;
    public static ConfigEntry<float> InfectionAmount;
    public static ConfigEntry<float> BleedAmount;
    public static ConfigEntry<float> Painkillers;
    public static ConfigEntry<int> Shrapnel;
    public static ConfigEntry<bool> Infected;
    public static ConfigEntry<bool> Dismembered;
    public static ConfigEntry<bool> BlockedBleeding;

    // Body
    public static ConfigEntry<float> BrainHealth;
    public static ConfigEntry<float> BloodAmount;
    public static ConfigEntry<int> BloodViscous;
    public static ConfigEntry<float> OxygenAmount;
    public static ConfigEntry<float> Hunger;
    public static ConfigEntry<float> Thirst;
    public static ConfigEntry<float> Temperature;
    public static ConfigEntry<float> Consciousness;
    public static ConfigEntry<float> Stamina;
    public static ConfigEntry<float> Energy;
    public static ConfigEntry<float> SicknessAmount;
    public static ConfigEntry<float> SepticShock;
    public static ConfigEntry<float> RadiationSickness;
    public static ConfigEntry<float> TraumaAmount;
    public static ConfigEntry<float> InternalBleeding;
    public static ConfigEntry<float> Hemothorax;
    public static ConfigEntry<float> Dirtyness;
    public static ConfigEntry<float> Wetness;
    public static ConfigEntry<float> Happiness;
    public static ConfigEntry<float> AntidepressantHappiness;
    public static ConfigEntry<float> OpiateHappiness;
    public static ConfigEntry<float> HearingLoss;
    public static ConfigEntry<float> WeightOffset;
    public static ConfigEntry<float> BadSleepAmount;
    public static ConfigEntry<float> Adrenaline;
    public static ConfigEntry<float> CurAdrenaline;
    public static ConfigEntry<float> AntibioticImmunityTime;
    public static ConfigEntry<float> BrainGrowSickness;
    public static ConfigEntry<bool> Disfigured;
    public static ConfigEntry<bool> EyeGone;
    public static ConfigEntry<bool> TriedRollingLastStand;
    public static ConfigEntry<bool> SuccesfullyRolledLastStand;
    public static ConfigEntry<float> LastStandTime;

    public void Awake()
    {
        Logger = base.Logger;
        Instance = this;
        Logger.LogInfo(Mutliplayer);

        ModLocale.Initialize(Logger);
        ModCommand.Initialize(Logger);

        _harmony.PatchAll();

        Logger.LogInfo(ModLocale.GetFormat("log.welcome"));

        //  General
        HealCountdown = Config.Bind(
            "Undead Mode",
            "Heal Countdown",
            1f
        );
        UndeadMode = Config.Bind(
            "Undead Mode",
            "Undead Mode",
            false
        );
        SwitchModeTip = Config.Bind(
            "Undead Mode",
            "Switch Mode Tip",
            false
        );
        
        
        MutliplayerUndead = Config.Bind(
            "Undead Mode", 
            "Mutliplayer Undead", 
            new List<string>(), 
            "We are Undead!");
        

        // Limb
        MuscleHeadth = Config.Bind(
            "Undead Mode - Limb",
            "Muscle Health",
            100f
        );
        SkinHealth = Config.Bind(
            "Undead Mode - Limb",
            "Skin Health",
            100f
        );
        BoneHealTImer = Config.Bind(
            "Undead Mode - Limb",
            "Bone Heal TImer",
            0.0f
        );
        DislocationTimer = Config.Bind(
            "Undead Mode - Limb",
            "Dislocation Timer",
            0.0f
        );
        InfectionAmount = Config.Bind(
            "Undead Mode - Limb",
            "Infection Amount",
            0.0f
        );
        BleedAmount = Config.Bind(
            "Undead Mode - Limb",
            "Bleed Amount",
            0.0f
        );
        Painkillers = Config.Bind(
            "Undead Mode - Limb",
            "Painkillers",
            0.0f
        );
        Shrapnel = Config.Bind(
            "Undead Mode - Limb",
            "Shrapnel",
            0
        );
        Infected = Config.Bind(
            "Undead Mode - Limb",
            "Infected",
            false
        );
        Dismembered = Config.Bind(
            "Undead Mode - Limb",
            "Dismembered",
            false
        );
        BlockedBleeding = Config.Bind(
            "Undead Mode - Limb",
            "Blocked Bleeding",
            false
        );

        // Body
        BrainHealth = Config.Bind(
            "Undead Mode - Body",
            "Brain Health",
            100f
        );
        BloodAmount = Config.Bind(
            "Undead Mode - Body",
            "Blood Amount",
            100f
        );
        BloodViscous = Config.Bind(
            "Undead Mode - Body",
            "Blood Viscous",
            0
        );
        OxygenAmount = Config.Bind(
            "Undead Mode - Body",
            "Oxygen Amount",
            100f
        );
        Hunger = Config.Bind(
            "Undead Mode - Body",
            "Hunger",
            100f
        );
        Thirst = Config.Bind(
            "Undead Mode - Body",
            "Thirst",
            100f
        );
        Temperature = Config.Bind(
            "Undead Mode - Body",
            "Temperature",
            37f
        );
        Consciousness = Config.Bind(
            "Undead Mode - Body",
            "Consciousness",
            100f
        );
        Stamina = Config.Bind(
            "Undead Mode - Body",
            "Stamina",
            100f
        );
        Energy = Config.Bind(
            "Undead Mode - Body",
            "Energy",
            100f
        );
        SicknessAmount = Config.Bind(
            "Undead Mode - Body",
            "Sickness Amount",
            0.0f
        );
        SepticShock = Config.Bind(
            "Undead Mode - Body",
            "Septic Shock",
            0.0f
        );
        RadiationSickness = Config.Bind(
            "Undead Mode - Body",
            "Radiation Sickness",
            0.0f
        );
        TraumaAmount = Config.Bind(
            "Undead Mode - Body",
            "Trauma Amount",
            0.0f
        );
        InternalBleeding = Config.Bind(
            "Undead Mode - Body",
            "Internal Bleeding",
            0.0f
        );
        Hemothorax = Config.Bind(
            "Undead Mode - Body",
            "Hemothorax",
            0.0f
        );
        Dirtyness = Config.Bind(
            "Undead Mode - Body",
            "Dirtyness",
            0.0f
        );
        Wetness = Config.Bind(
            "Undead Mode - Body",
            "Wetness",
            0.0f
        );
        Happiness = Config.Bind(
            "Undead Mode - Body",
            "Happiness",
            0.0f
        );
        AntidepressantHappiness = Config.Bind(
            "Undead Mode - Body",
            "Antidepressant Happiness",
            0.0f
        );
        OpiateHappiness = Config.Bind(
            "Undead Mode - Body",
            "Opiate Happiness",
            0.0f
        );
        HearingLoss = Config.Bind(
            "Undead Mode - Body",
            "Hearing Loss",
            0.0f
        );
        WeightOffset = Config.Bind(
            "Undead Mode - Body",
            "Weight Offset",
            0.0f
        );
        BadSleepAmount = Config.Bind(
            "Undead Mode - Body",
            "Bad Sleep Amount",
            0.0f
        );
        Adrenaline = Config.Bind(
            "Undead Mode - Body",
            "Adrenaline",
            0.0f
        );
        CurAdrenaline = Config.Bind(
            "Undead Mode - Body",
            "Cur Adrenaline",
            0.0f
        );
        AntibioticImmunityTime = Config.Bind(
            "Undead Mode - Body",
            "Antibiotic Immunity Time",
            0.0f
        );
        BrainGrowSickness = Config.Bind(
            "Undead Mode - Body",
            "Brain Grow Sickness",
            0.0f
        );
        Disfigured = Config.Bind(
            "Undead Mode - Body",
            "Disfigured",
            false
        );
        EyeGone = Config.Bind(
            "Undead Mode - Body",
            "EyeGone",
            false
        );
        TriedRollingLastStand = Config.Bind(
            "Undead Mode - Body",
            "Tried Rolling Last Stand",
            false
        );
        SuccesfullyRolledLastStand = Config.Bind(
            "Undead Mode - Body",
            "Succesfully Rolled LastStand",
            false
        );
        LastStandTime = Config.Bind(
            "Undead Mode - Body",
            "Last Stand Time",
            -1000f
        );

        UndeadModeConfigs.Update();
    }

    [HarmonyPatch(typeof(Body), "Update")]
    public class BodyPatch
    {
        private static float _healTimer;

        // ReSharper disable once UnusedMember.Global
        public static void Postfix()
        {
            if (!UndeadModeConfigs.UndeadMode) return;

            _healTimer += Time.deltaTime;
            while (_healTimer >= UndeadModeConfigs.HealCountdown)
            {
                if (!Mutliplayer)
                {
                    Heal();
                }
                else
                {
                    MutliplayerHeal();
                }

                _healTimer -= UndeadModeConfigs.HealCountdown;
            }
        }
    }

    private static void Heal()
    {
        foreach (var limb in PlayerCamera.main.body.limbs)
        {
            limb.muscleHealth = UndeadModeConfigs.MuscleHealth;
            limb.skinHealth = UndeadModeConfigs.SkinHealth;
            limb.boneHealTimer = UndeadModeConfigs.BoneHealTImer;
            limb.dislocationTimer = UndeadModeConfigs.DislocationTimer;
            limb.infectionAmount = UndeadModeConfigs.InfectionAmount;
            limb.bleedAmount = UndeadModeConfigs.BleedAmount;
            limb.pain = UndeadModeConfigs.Painkillers;
            limb.shrapnel = UndeadModeConfigs.Shrapnel;
            limb.infected = UndeadModeConfigs.Infected;
            limb.dismembered = UndeadModeConfigs.Dismembered;
            limb.blockedBleeding = UndeadModeConfigs.BlockedBleeding;
        }

        var body = PlayerCamera.main.body;
        body.brainHealth = UndeadModeConfigs.BrainHealth;
        body.bloodAmount = UndeadModeConfigs.BloodAmount;
        body.bloodViscous = UndeadModeConfigs.BloodViscous;
        body.oxygenAmount = UndeadModeConfigs.OxygenAmount;
        body.hunger = UndeadModeConfigs.Hunger;
        body.thirst = UndeadModeConfigs.Thirst;
        body.temperature = UndeadModeConfigs.Temperature;
        body.consciousness = UndeadModeConfigs.Consciousness;
        body.stamina = UndeadModeConfigs.Stamina;
        body.energy = UndeadModeConfigs.Energy;
        body.sicknessAmount = UndeadModeConfigs.SicknessAmount;
        body.septicShock = UndeadModeConfigs.SepticShock;
        body.radiationSickness = UndeadModeConfigs.RadiationSickness;
        body.traumaAmount = UndeadModeConfigs.TraumaAmount;
        body.internalBleeding = UndeadModeConfigs.InternalBleeding;
        body.hemothorax = UndeadModeConfigs.Hemothorax;
        body.dirtyness = UndeadModeConfigs.Dirtyness;
        body.wetness = UndeadModeConfigs.Wetness;
        body.happiness = UndeadModeConfigs.Happiness;
        body.antidepressantHappiness = UndeadModeConfigs.AntidepressantHappiness;
        body.opiateHappiness = UndeadModeConfigs.OpiateHappiness;
        body.hearingLoss = UndeadModeConfigs.HearingLoss;
        body.weightOffset = UndeadModeConfigs.WeightOffset;
        body.badSleepAmount = UndeadModeConfigs.BadSleepAmount;
        body.adrenaline = UndeadModeConfigs.Adrenaline;
        body.curAdrenaline = UndeadModeConfigs.CurAdrenaline;
        body.antibioticImmunityTime = UndeadModeConfigs.AntibioticImmunityTime;
        body.brainGrowSickness = UndeadModeConfigs.BrainGrowSickness;
        body.disfigured = UndeadModeConfigs.Disfigured;
        body.eyeGone = UndeadModeConfigs.EyeGone;
        body.triedRollingLastStand = UndeadModeConfigs.TriedRollingLastStand;
        body.succesfullyRolledLastStand = UndeadModeConfigs.SuccesfullyRolledLastStand;
        body.lastStandTime = UndeadModeConfigs.LastStandTime;

        if (PlayerCamera.main.body.TryGetComponent(out Painkillers component1))
            Destroy(component1);
        if (PlayerCamera.main.body.TryGetComponent(out SleepingPills component2))
            Destroy(component2);
        if (PlayerCamera.main.body.TryGetComponent(out Antidepressants component3))
            Destroy(component3);
        CoUtils.instance.Stop("bleach");
        CoUtils.instance.Stop("mercury");
    }

    private static void MutliplayerHeal()
    {
        var whiteList = UndeadModeConfigs.MPUndeadMode;

        var allBodies = FindObjectsOfType<Body>();

        foreach (var body in allBodies)
        {
            if (!IsPlayerInWhiteList(body, whiteList)) continue;

            ApplyHealToBody(body);
        }
    }

    private static bool IsPlayerInWhiteList(Body body, List<string> whiteList)
    {
        try
        {
            var netPlayer = NetPlayer.GetNetPlayerFromBody(body);
            if (netPlayer == null) return false;

            var playerName = netPlayer.playername;

            return whiteList.Contains(playerName);
        }
        catch
        {
            return false;
        }
    }

    private static void ApplyHealToBody(Body targetBody)
    {
        foreach (var limb in targetBody.limbs)
        {
            limb.muscleHealth = UndeadModeConfigs.MuscleHealth;
            limb.skinHealth = UndeadModeConfigs.SkinHealth;
            limb.boneHealTimer = UndeadModeConfigs.BoneHealTImer;
            limb.dislocationTimer = UndeadModeConfigs.DislocationTimer;
            limb.infectionAmount = UndeadModeConfigs.InfectionAmount;
            limb.bleedAmount = UndeadModeConfigs.BleedAmount;
            limb.pain = UndeadModeConfigs.Painkillers;
            limb.shrapnel = UndeadModeConfigs.Shrapnel;
            limb.infected = UndeadModeConfigs.Infected;
            limb.dismembered = UndeadModeConfigs.Dismembered;
            limb.blockedBleeding = UndeadModeConfigs.BlockedBleeding;
        }

        targetBody.brainHealth = UndeadModeConfigs.BrainHealth;
        targetBody.bloodAmount = UndeadModeConfigs.BloodAmount;
        targetBody.bloodViscous = UndeadModeConfigs.BloodViscous;
        targetBody.oxygenAmount = UndeadModeConfigs.OxygenAmount;
        targetBody.hunger = UndeadModeConfigs.Hunger;
        targetBody.thirst = UndeadModeConfigs.Thirst;
        targetBody.temperature = UndeadModeConfigs.Temperature;
        targetBody.consciousness = UndeadModeConfigs.Consciousness;
        targetBody.stamina = UndeadModeConfigs.Stamina;
        targetBody.energy = UndeadModeConfigs.Energy;
        targetBody.sicknessAmount = UndeadModeConfigs.SicknessAmount;
        targetBody.septicShock = UndeadModeConfigs.SepticShock;
        targetBody.radiationSickness = UndeadModeConfigs.RadiationSickness;
        targetBody.traumaAmount = UndeadModeConfigs.TraumaAmount;
        targetBody.internalBleeding = UndeadModeConfigs.InternalBleeding;
        targetBody.hemothorax = UndeadModeConfigs.Hemothorax;
        targetBody.dirtyness = UndeadModeConfigs.Dirtyness;
        targetBody.wetness = UndeadModeConfigs.Wetness;
        targetBody.happiness = UndeadModeConfigs.Happiness;
        targetBody.antidepressantHappiness = UndeadModeConfigs.AntidepressantHappiness;
        targetBody.opiateHappiness = UndeadModeConfigs.OpiateHappiness;
        targetBody.hearingLoss = UndeadModeConfigs.HearingLoss;
        targetBody.weightOffset = UndeadModeConfigs.WeightOffset;
        targetBody.badSleepAmount = UndeadModeConfigs.BadSleepAmount;
        targetBody.adrenaline = UndeadModeConfigs.Adrenaline;
        targetBody.curAdrenaline = UndeadModeConfigs.CurAdrenaline;
        targetBody.antibioticImmunityTime = UndeadModeConfigs.AntibioticImmunityTime;
        targetBody.brainGrowSickness = UndeadModeConfigs.BrainGrowSickness;
        targetBody.disfigured = UndeadModeConfigs.Disfigured;
        targetBody.eyeGone = UndeadModeConfigs.EyeGone;
        targetBody.triedRollingLastStand = UndeadModeConfigs.TriedRollingLastStand;
        targetBody.succesfullyRolledLastStand = UndeadModeConfigs.SuccesfullyRolledLastStand;
        targetBody.lastStandTime = UndeadModeConfigs.LastStandTime;

        if (targetBody.TryGetComponent(out Painkillers component1))
            Destroy(component1);
        if (targetBody.TryGetComponent(out SleepingPills component2))
            Destroy(component2);
        if (targetBody.TryGetComponent(out Antidepressants component3))
            Destroy(component3);

        CoUtils.instance.Stop("bleach");
        CoUtils.instance.Stop("mercury");
    }
}
