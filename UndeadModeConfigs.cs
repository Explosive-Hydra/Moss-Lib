using System.Collections.Generic;

namespace MossLib;

public static class UndeadModeConfigs
{
    // General
    public static bool  UndeadMode;
    public static float HealCountdown;
    public static bool  SwitchModeTip;
    public static List<string> MPUndeadMode;
    
    // Limb
    public static float MuscleHealth;
    public static float SkinHealth;
    public static float BoneHealTImer;
    public static float DislocationTimer;
    public static float InfectionAmount;
    public static float BleedAmount;
    public static float Painkillers;
    public static int   Shrapnel;
    public static bool  Infected;
    public static bool  Dismembered;
    public static bool  BlockedBleeding;
    
    // Body
    public static float BrainHealth;
    public static float BloodAmount;
    public static int   BloodViscous;
    public static float OxygenAmount;
    public static float Hunger;
    public static float Thirst;
    public static float Temperature;
    public static float Consciousness;
    public static float Stamina;
    public static float Energy;
    public static float SicknessAmount;
    public static float SepticShock;
    public static float RadiationSickness;
    public static float TraumaAmount;
    public static float InternalBleeding;
    public static float Hemothorax;
    public static float Dirtyness;
    public static float Wetness;
    public static float Happiness;
    public static float AntidepressantHappiness;
    public static float OpiateHappiness;
    public static float HearingLoss;
    public static float WeightOffset;
    public static float BadSleepAmount;
    public static float Adrenaline;
    public static float CurAdrenaline;
    public static float AntibioticImmunityTime;
    public static float BrainGrowSickness;
    public static bool  Disfigured;
    public static bool  EyeGone;
    public static bool  TriedRollingLastStand;
    public static bool  SuccesfullyRolledLastStand;
    public static float LastStandTime;
    
    public static void Update()
    {
        // General
        UndeadMode    = Plugin.UndeadMode.Value;
        HealCountdown = Plugin.HealCountdown.Value;
        SwitchModeTip = Plugin.SwitchModeTip.Value;
        MPUndeadMode = Plugin.MutliplayerUndead.Value;
        
        // Limb
        MuscleHealth     = Plugin.MuscleHeadth.Value;
        SkinHealth       = Plugin.SkinHealth.Value;
        BoneHealTImer    = Plugin.BoneHealTImer.Value;
        DislocationTimer = Plugin.DislocationTimer.Value;
        InfectionAmount  = Plugin.InfectionAmount.Value;
        BleedAmount      = Plugin.BleedAmount.Value;
        Painkillers      = Plugin.Painkillers.Value;
        Shrapnel         = Plugin.Shrapnel.Value;
        Infected         = Plugin.Infected.Value;
        Dismembered      = Plugin.Dismembered.Value;
        BlockedBleeding  = Plugin.BlockedBleeding.Value;
        
        // Body
        BrainHealth                = Plugin.BrainHealth.Value;
        BloodAmount                = Plugin.BloodAmount.Value;
        BloodViscous               = Plugin.BloodViscous.Value;
        OxygenAmount               = Plugin.OxygenAmount.Value;
        Hunger                     = Plugin.Hunger.Value;
        Thirst                     = Plugin.Thirst.Value;
        Temperature                = Plugin.Temperature.Value;
        Consciousness              = Plugin.Consciousness.Value;
        Stamina                    = Plugin.Stamina.Value;
        Energy                     = Plugin.Energy.Value;
        SicknessAmount             = Plugin.SicknessAmount.Value;
        SepticShock                = Plugin.SepticShock.Value;
        RadiationSickness          = Plugin.RadiationSickness.Value;
        TraumaAmount               = Plugin.TraumaAmount.Value;
        InternalBleeding           = Plugin.InternalBleeding.Value;
        Hemothorax                 = Plugin.Hemothorax.Value;
        Dirtyness                  = Plugin.Dirtyness.Value;
        Wetness                    = Plugin.Wetness.Value;
        Happiness                  = Plugin.Happiness.Value;
        AntidepressantHappiness    = Plugin.AntidepressantHappiness.Value;
        OpiateHappiness            = Plugin.OpiateHappiness.Value;
        HearingLoss                = Plugin.HearingLoss.Value;
        WeightOffset               = Plugin.WeightOffset.Value;
        BadSleepAmount             = Plugin.BadSleepAmount.Value;
        Adrenaline                 = Plugin.Adrenaline.Value;
        CurAdrenaline              = Plugin.CurAdrenaline.Value;
        AntibioticImmunityTime     = Plugin.AntibioticImmunityTime.Value;
        BrainGrowSickness          = Plugin.BrainGrowSickness.Value;
        Disfigured                 = Plugin.Disfigured.Value;
        EyeGone                    = Plugin.EyeGone.Value;
        TriedRollingLastStand      = Plugin.TriedRollingLastStand.Value;
        SuccesfullyRolledLastStand = Plugin.SuccesfullyRolledLastStand.Value;
        LastStandTime              = Plugin.LastStandTime.Value;
    }
}