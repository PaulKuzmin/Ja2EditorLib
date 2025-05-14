// ReSharper disable InconsistentNaming

using System.ComponentModel;
using Ja2StracSaveEditorLib.Managers;
using Ja2StracSaveEditorLib.Structures;
using Ja2StracSaveEditorLib.Utilities;

namespace Ja2StracSaveEditorLib;

public class PathSt : INotifyPropertyChanged
{
#pragma warning disable CS0067 // The event is never used
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

    public uint uiSectorId;
    public PathSt pNext;
    public PathSt pPrev;
}

public class Soldier : INotifyPropertyChanged
{
    public const int MAX_NUM_SOLDIERS = 148;
    public const int NUM_PLANNING_MERCS = 8; // XXX this is a remnant of the planning mode, see issue #902
    public const int TOTAL_SOLDIERS = NUM_PLANNING_MERCS + MAX_NUM_SOLDIERS;
    public const int SOLDIER_SIZE = 2328;
    public const int NUM_INV_SLOTS = 19;
    public const int NUM_SOLDIER_SHADES = 48;
    public const int MAX_PATH_LIST_SIZE = 30;
    public const int MAXPATROLGRIDS = 10; // *** THIS IS A DUPLICATION - MUST BE MOVED !
    public const int SOLDIERTYPE_NAME_LENGTH = 10;
    private const int PaletteRepID_LENGTH = 30;

#pragma warning disable CS0067 // The event is never used
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

    public byte ubID { get; set; }

    // DESCRIPTION / STATS, ETC
    public byte ubBodyType { get; set; }
    public sbyte bActionPoints { get; set; }
    public sbyte bInitialActionPoints { get; set; }

    public uint uiStatusFlags { get; set; }

    public ObjectType[] inv { get; set; } = new ObjectType[NUM_INV_SLOTS];

    public object pTempObject { get; set; }

    //public object pKeyRing { get; set; }
    public byte[] pKeyRing { get; set; } = new byte[4];

    public sbyte bOldLife { get; set; } // life at end of last turn, recorded for monster AI

    // attributes
    public byte bInSector { get; set; }
    public sbyte bFlashPortraitFrame { get; set; }
    public short sFractLife { get; set; } // fraction of life pts (in hundreths)
    public sbyte bBleeding { get; set; } // blood loss control variable
    public sbyte bBreath { get; set; } // current breath value
    public sbyte bBreathMax { get; set; } // max breath, affected by fatigue/sleep
    public sbyte bStealthMode { get; set; }

    public short sBreathRed { get; set; } // current breath value
    public bool fDelayedMovement { get; set; }

    public byte ubWaitActionToDo { get; set; }

    public sbyte ubInsertionDirection { get; set; }

    // skills
    public byte opponent { get; set; }
    public sbyte bLastRenderVisibleValue { get; set; }

    public byte ubAttackingHand { get; set; }

// traits
    public short sWeightCarriedAtTurnStart { get; set; }
    public string name { get; set; }

    public sbyte bVisible { get; set; } // to render or not to render...


    public sbyte bActive { get; set; }

    public sbyte bTeam { get; set; } // Team identifier

//NEW MOVEMENT INFORMATION for Strategic Movement
    public byte ubGroupID { get; set; } //the movement group the merc is currently part of.

    public bool fBetweenSectors { get; set; } //set when the group isn't actually in a sector.
    //sSectorX and sSectorY will reflect the sector the
    //merc was at last.

    public byte ubMovementNoiseHeard { get; set; } // 8 flags by direction

// WORLD POSITION STUFF
    public float dXPos { get; set; }
    public float dYPos { get; set; }
    public short sInitialGridNo { get; set; }
    public short sGridNo { get; set; }
    public byte bDirection { get; set; }
    public short sHeightAdjustment { get; set; }
    public short sDesiredHeight { get; set; }
    public short sTempNewGridNo { get; set; } // New grid no for advanced animations
    public sbyte bOverTerrainType { get; set; }

    public sbyte bCollapsed { get; set; } // collapsed due to being out of APs
    public sbyte bBreathCollapsed { get; set; } // collapsed due to being out of APs

    public byte ubDesiredHeight { get; set; }
    public ushort usPendingAnimation { get; set; }
    public byte ubPendingStanceChange { get; set; }
    public ushort usAnimState { get; set; }
    public bool fNoAPToFinishMove { get; set; }
    public bool fPausedMove { get; set; }
    public bool fUIdeadMerc { get; set; } // UI Flags for removing a newly dead merc
    public bool fUICloseMerc { get; set; } // UI Flags for closing panels

    public DateTime? UpdateCounter { get; set; }
    public DateTime? DamageCounter { get; set; }
    public DateTime? AICounter { get; set; }
    public DateTime? FadeCounter { get; set; }

    public byte ubSkillTrait1 { get; set; }
    public byte ubSkillTrait2 { get; set; }

    public sbyte bDexterity { get; set; } // dexterity (hand coord) value
    public sbyte bWisdom { get; set; }
    public byte attacker { get; set; }
    public byte previous_attacker { get; set; }
    public byte next_to_previous_attacker { get; set; }
    public bool fTurnInProgress { get; set; }

    public bool fIntendedTarget { get; set; } // intentionally shot?
    public bool fPauseAllAnimation { get; set; }

    public sbyte bExpLevel { get; set; } // general experience level
    public short sInsertionGridNo { get; set; }

    public bool fContinueMoveAfterStanceChange { get; set; }

    //public AnimationSurfaceCacheType AnimCache { get; set; }

    public sbyte bLife { get; set; } // current life (hit points or health)
    public byte bSide { get; set; }
    public sbyte bNewOppCnt { get; set; }

    public ushort usAniCode { get; set; }
    public ushort usAniFrame { get; set; }
    public short sAniDelay { get; set; }

// MOVEMENT TO NEXT TILE HANDLING STUFF
    public sbyte bAgility { get; set; } // agility (speed) value
    public short sDelayedMovementCauseGridNo { get; set; }
    public short sReservedMovementGridNo { get; set; }

    public sbyte bStrength { get; set; }

// Weapon Stuff
    public short sTargetGridNo { get; set; }
    public sbyte bTargetLevel { get; set; }
    public sbyte bTargetCubeLevel { get; set; }
    public short sLastTarget { get; set; }
    public sbyte bTilesMoved { get; set; }
    public sbyte bLeadership { get; set; }
    public float dNextBleed { get; set; }
    public bool fWarnedAboutBleeding { get; set; }
    public bool fDyingComment { get; set; }

    public byte ubTilesMovedPerRTBreathUpdate { get; set; }
    public ushort usLastMovementAnimPerRTBreathUpdate { get; set; }

    public bool fTurningToShoot { get; set; }
    public bool fTurningUntilDone { get; set; }
    public bool fGettingHit { get; set; }
    public bool fInNonintAnim { get; set; }
    public bool fFlashLocator { get; set; }
    public short sLocatorFrame { get; set; }
    public bool fShowLocator { get; set; }
    public bool fFlashPortrait { get; set; }
    public sbyte bMechanical { get; set; }
    public sbyte bLifeMax { get; set; } // maximum life for this merc

    public object face { get; set; } // FACETYPE


    // PALETTE MANAGEMENT STUFF
    public string HeadPal { get; set; }
    public string PantsPal { get; set; }
    public string VestPal { get; set; }
    public string SkinPal { get; set; }

    public ushort[] pShades { get; set; } = new ushort[NUM_SOLDIER_SHADES]; // Shading tables
    public ushort[] pGlowShades { get; set; } = new ushort[20];
    public sbyte bMedical { get; set; }
    public bool fBeginFade { get; set; }
    public byte ubFadeLevel { get; set; }
    public byte ubServiceCount { get; set; }
    public byte service_partner { get; set; }
    public sbyte bMarksmanship { get; set; }
    public sbyte bExplosive { get; set; }
    public object pThrowParams { get; set; } // THROW_PARAMS*
    public bool fTurningFromPronePosition { get; set; }
    public sbyte bReverse { get; set; }
    public object pLevelNode { get; set; } // LEVELNODE* 

    // WALKING STUFF
    public sbyte bDesiredDirection { get; set; }
    public short sDestXPos { get; set; }
    public short sDestYPos { get; set; }
    public short sDestination { get; set; }
    public short sFinalDestination { get; set; }
    public sbyte bLevel { get; set; }

// PATH STUFF
    public byte[] ubPathingData { get; set; } = new byte[MAX_PATH_LIST_SIZE];
    public byte ubPathDataSize { get; set; }
    public byte ubPathIndex { get; set; }
    public short sBlackList { get; set; }
    public sbyte bAimTime { get; set; }
    public sbyte bShownAimTime { get; set; }
    public sbyte bPathStored { get; set; } // good for AI to reduct redundancy
    public sbyte bHasKeys { get; set; } // allows AI controlled dudes to open locked doors

    public byte ubStrategicInsertionCode { get; set; }
    public ushort usStrategicInsertionData { get; set; }

    public object light { get; set; } //LIGHT_SPRITE*
    public object muzzle_flash { get; set; } //LIGHT_SPRITE*
    public sbyte bMuzFlashCount { get; set; }

    public short sX { get; set; }
    public short sY { get; set; }

    public ushort usOldAniState { get; set; }
    public short sOldAniCode { get; set; }

    public sbyte bBulletsLeft { get; set; }
    public byte ubSuppressionPoints { get; set; }

// STUFF FOR RANDOM ANIMATIONS
    public uint uiTimeOfLastRandomAction { get; set; }

// AI STUFF
    public sbyte[] bOppList { get; set; } = new sbyte[MAX_NUM_SOLDIERS]; // AI knowledge database
    public sbyte bLastAction { get; set; }
    public sbyte bAction { get; set; }
    public ushort usActionData { get; set; }
    public sbyte bNextAction { get; set; }
    public ushort usNextActionData { get; set; }
    public sbyte bActionInProgress { get; set; }
    public sbyte bAlertStatus { get; set; }
    public sbyte bOppCnt { get; set; }
    public sbyte bNeutral { get; set; }
    public sbyte bNewSituation { get; set; }
    public sbyte bNextTargetLevel { get; set; }
    public sbyte bOrders { get; set; }
    public sbyte bAttitude { get; set; }
    public sbyte bUnderFire { get; set; }
    public sbyte bShock { get; set; }
    public sbyte bBypassToGreen { get; set; }
    public sbyte bDominantDir { get; set; } // AI main direction to face...
    public sbyte bPatrolCnt { get; set; } // number of patrol gridnos
    public sbyte bNextPatrolPnt { get; set; } // index to next patrol gridno
    public short[] usPatrolGrid { get; set; } = new short[MAXPATROLGRIDS]; // AI list for ptr->orders==PATROL
    public short sNoiseGridno { get; set; }
    public byte ubNoiseVolume { get; set; }
    public sbyte bLastAttackHit { get; set; }
    public byte xrayed_by { get; set; }
    public float dHeightAdjustment { get; set; }
    public sbyte bMorale { get; set; }
    public sbyte bTeamMoraleMod { get; set; }
    public sbyte bTacticalMoraleMod { get; set; }
    public sbyte bStrategicMoraleMod { get; set; }
    public sbyte bAIMorale { get; set; }
    public byte ubPendingAction { get; set; }
    public byte ubPendingActionAnimCount { get; set; }
    public uint uiPendingActionData1 { get; set; }
    public short sPendingActionData2 { get; set; }
    public sbyte bPendingActionData3 { get; set; }
    public sbyte ubDoorHandleCode { get; set; }
    public uint uiPendingActionData4 { get; set; }
    public sbyte bInterruptDuelPts { get; set; }
    public sbyte bPassedLastInterrupt { get; set; }
    public sbyte bIntStartAPs { get; set; }
    public sbyte bMoved { get; set; }
    public sbyte bHunting { get; set; }
    public byte ubCaller { get; set; }
    public short sCallerGridNo { get; set; }
    public byte bCallPriority { get; set; }
    public sbyte bCallActedUpon { get; set; }
    public sbyte bFrenzied { get; set; }
    public sbyte bNormalSmell { get; set; }
    public sbyte bMonsterSmell { get; set; }
    public sbyte bMobility { get; set; }
    public sbyte fAIFlags { get; set; }

    public bool fDontChargeReadyAPs { get; set; }
    public ushort usAnimSurface { get; set; }
    public ushort sZLevel { get; set; }
    public bool fPrevInWater { get; set; }
    public bool fGoBackToAimAfterHit { get; set; }

    public short sWalkToAttackGridNo { get; set; }
    public short sWalkToAttackWalkToCost { get; set; }

    public bool fForceShade { get; set; }
    public uint pForcedShade { get; set; }

    public sbyte bDisplayDamageCount { get; set; }
    public sbyte fDisplayDamage { get; set; }
    public short sDamage { get; set; }
    public short sDamageX { get; set; }
    public short sDamageY { get; set; }
    public sbyte bDoBurst { get; set; }
    public short usUIMovementMode { get; set; }
    public bool fUIMovementFast { get; set; }

    public DateTime? BlinkSelCounter { get; set; }
    public DateTime? PortraitFlashCounter { get; set; }
    public bool fDeadSoundPlayed { get; set; }
    public byte ubProfile { get; set; }
    public byte ubQuoteRecord { get; set; }
    public byte ubQuoteActionID { get; set; }
    public byte ubBattleSoundID { get; set; }

    public bool fClosePanel { get; set; }
    public bool fClosePanelToDie { get; set; }
    public byte ubClosePanelFrame { get; set; }
    public bool fDeadPanel { get; set; }
    public byte ubDeadPanelFrame { get; set; }

    public short sPanelFaceX { get; set; }
    public short sPanelFaceY { get; set; }

// QUOTE STUFF
    public sbyte bNumHitsThisTurn { get; set; }
    public ushort usQuoteSaidFlags { get; set; }
    public sbyte fCloseCall { get; set; }
    public sbyte bLastSkillCheck { get; set; }
    public sbyte ubSkillCheckAttempts { get; set; }

    public sbyte bStartFallDir { get; set; }
    public sbyte fTryingToFall { get; set; }

    public byte ubPendingDirection { get; set; }
    public uint uiAnimSubFlags { get; set; }

    public byte bAimShotLocation { get; set; }
    public byte ubHitLocation { get; set; }

    //ushort? effect_shade { get; set; } // Shading table for effects

    public short[] sSpreadLocations { get; set; } = new short[6];
    public bool fDoSpread { get; set; }
    public short sStartGridNo { get; set; }
    public short sEndGridNo { get; set; }
    public short sForcastGridno { get; set; }
    public short sZLevelOverride { get; set; }
    public sbyte bMovedPriorToInterrupt { get; set; }

    public int
        iEndofContractTime
    {
        get;
        set;
    } // time, in global time(resolution, minutes) that merc will leave, or if its a M.E.R.C. merc it will be set to -1. -2 for NPC and player generated

    public int iStartContractTime { get; set; }

    public int
        iTotalContractLength
    {
        get;
        set;
    } // total time of AIM mercs contract or the time since last paid for a M.E.R.C. merc

    public int iNextActionSpecialData { get; set; } // AI special action data record for the next action
    public byte ubWhatKindOfMercAmI { get; set; } //Set to the type of character it is
    public sbyte bAssignment { get; set; } // soldiers current assignment

    public bool
        fForcedToStayAwake
    {
        get;
        set;
    } // forced by player to stay awake, reset to false, the moment they are set to rest or sleep

    public sbyte bTrainStat { get; set; } // current stat soldier is training
    public SGPSector sSector { get; set; } // position on the Stategic Map
    public int iVehicleId { get; set; } // the id of the vehicle the char is in
    public PathSt pMercPath { get; set; } // Path Structure
    public byte fHitByGasFlags { get; set; } // flags
    public ushort usMedicalDeposit { get; set; } // is there a medical deposit on merc
    public ushort usLifeInsurance { get; set; } // is there life insurance taken out on merc

    public int iStartOfInsuranceContract { get; set; }
    public uint uiLastAssignmentChangeMin { get; set; } // timestamp of last assignment change in minutes
    public int iTotalLengthOfInsuranceContract { get; set; }

    public byte ubSoldierClass { get; set; } //admin, elite, troop (creature types?)
    public byte ubAPsLostToSuppression { get; set; }
    public bool fChangingStanceDueToSuppression { get; set; }
    public byte suppressor { get; set; }

    public byte ubCivilianGroup { get; set; }

// time changes...when a stat was changed according to GetJA2Clock() {get; set;}
    public uint uiChangeLevelTime { get; set; }
    public uint uiChangeHealthTime { get; set; }
    public uint uiChangeStrengthTime { get; set; }
    public uint uiChangeDexterityTime { get; set; }
    public uint uiChangeAgilityTime { get; set; }
    public uint uiChangeWisdomTime { get; set; }
    public uint uiChangeLeadershipTime { get; set; }
    public uint uiChangeMarksmanshipTime { get; set; }
    public uint uiChangeExplosivesTime { get; set; }
    public uint uiChangeMedicalTime { get; set; }
    public uint uiChangeMechanicalTime { get; set; }

    public uint
        uiUniqueSoldierIdValue
    {
        get;
        set;
    } // the unique value every instance of a soldier gets - 1 is the first valid value

    public sbyte bBeingAttackedCount { get; set; } // Being attacked counter

    public sbyte[] bNewItemCount { get; set; } = new sbyte[NUM_INV_SLOTS];
    public sbyte[] bNewItemCycleCount { get; set; } = new sbyte[NUM_INV_SLOTS];
    public bool fCheckForNewlyAddedItems { get; set; }
    public sbyte bEndDoorOpenCode { get; set; }

    public byte ubScheduleID { get; set; }
    public short sEndDoorOpenCodeData { get; set; }
    public DateTime? NextTileCounter { get; set; }
    public bool fBlockedByAnotherMerc { get; set; }
    public sbyte bBlockedByAnotherMercDirection { get; set; }
    public ushort usAttackingWeapon { get; set; }
    public byte target { get; set; }
    public byte bWeaponMode { get; set; }
    public sbyte bAIScheduleProgress { get; set; }
    public short sOffWorldGridNo { get; set; }
    public object pAniTile { get; set; }
    public sbyte bCamo { get; set; }
    public short sAbsoluteFinalDestination { get; set; }
    public byte ubHiResDirection { get; set; }
    public byte ubLastFootPrintSound { get; set; }
    public sbyte bVehicleID { get; set; }
    public sbyte fPastXDest { get; set; }
    public sbyte fPastYDest { get; set; }
    public sbyte bMovementDirection { get; set; }
    public short sOldGridNo { get; set; }
    public ushort usDontUpdateNewGridNoOnMoveAnimChange { get; set; }
    public short sBoundingBoxWidth { get; set; }
    public short sBoundingBoxHeight { get; set; }
    public short sBoundingBoxOffsetX { get; set; }
    public short sBoundingBoxOffsetY { get; set; }
    public uint uiTimeSameBattleSndDone { get; set; }
    public sbyte bOldBattleSnd { get; set; }
    public bool fContractPriceHasIncreased { get; set; }
    public uint uiBurstSoundID { get; set; }
    public bool fFixingSAMSite { get; set; }
    public bool fFixingRobot { get; set; }
    public sbyte bSlotItemTakenFrom { get; set; }
    public bool fSignedAnotherContract { get; set; }
    public bool fDontChargeTurningAPs { get; set; }
    public byte auto_bandaging_medic { get; set; }
    public byte robot_remote_holder { get; set; }
    public uint uiTimeOfLastContractUpdate { get; set; }
    public sbyte bTypeOfLastContract { get; set; }
    public sbyte bTurnsCollapsed { get; set; }
    public sbyte bSleepDrugCounter { get; set; }
    public byte ubMilitiaKills { get; set; }

    public sbyte[] bFutureDrugEffect { get; set; } = new sbyte[2]; // value to represent effect of a needle
    public sbyte[] bDrugEffectRate { get; set; } = new sbyte[2]; // represents rate of increase and decrease of effect
    public sbyte[] bDrugEffect { get; set; } = new sbyte[2]; // value that affects AP & morale calc ( -ve is poorly )
    public sbyte[] bDrugSideEffectRate { get; set; } = new sbyte[2]; // duration of negative AP and morale effect
    public sbyte[] bDrugSideEffect { get; set; } = new sbyte[2]; // duration of negative AP and morale effect

    public sbyte bBlindedCounter { get; set; }
    public bool fMercCollapsedFlag { get; set; }
    public bool fDoneAssignmentAndNothingToDoFlag { get; set; }
    public bool fMercAsleep { get; set; }
    public bool fDontChargeAPsForStanceChange { get; set; }

    public byte ubTurnsUntilCanSayHeardNoise { get; set; }
    public ushort usQuoteSaidExtFlags { get; set; }

    public ushort sContPathLocation { get; set; }
    public sbyte bGoodContPath { get; set; }
    public sbyte bNoiseLevel { get; set; }
    public sbyte bRegenerationCounter { get; set; }
    public sbyte bRegenBoostersUsedToday { get; set; }
    public sbyte bNumPelletsHitBy { get; set; }
    public short sSkillCheckGridNo { get; set; }
    public byte ubLastEnemyCycledID { get; set; }

    public byte ubPrevSectorID { get; set; }
    public byte ubNumTilesMovesSinceLastForget { get; set; }
    public sbyte bTurningIncrement { get; set; }
    public uint uiBattleSoundID { get; set; }

    public bool fSoldierWasMoving { get; set; }
    public bool fSayAmmoQuotePending { get; set; }
    public ushort usValueGoneUp { get; set; }

    public byte ubNumLocateCycles { get; set; }
    public byte ubDelayedMovementFlags { get; set; }
    public bool fMuzzleFlash { get; set; }
    public byte CTGTTarget { get; set; }

    public DateTime? PanelAnimateCounter { get; set; }

    public sbyte bCurrentCivQuote { get; set; }
    public sbyte bCurrentCivQuoteDelta { get; set; }
    public byte ubMiscSoldierFlags { get; set; }
    public byte ubReasonCantFinishMove { get; set; }

    public short sLocationOfFadeStart { get; set; }
    public byte bUseExitGridForReentryDirection { get; set; }

    public uint uiTimeSinceLastSpoke { get; set; }
    public byte ubContractRenewalQuoteCode { get; set; }
    public short sPreTraversalGridNo { get; set; }
    public uint uiXRayActivatedTime { get; set; }
    public sbyte bTurningFromUI { get; set; }
    public sbyte bPendingActionData5 { get; set; }

    public sbyte bDelayedStrategicMoraleMod { get; set; }
    public byte ubDoorOpeningNoise { get; set; }

    public byte ubLeaveHistoryCode { get; set; }
    public bool fDontUnsetLastTargetFromTurn { get; set; }
    public sbyte bOverrideMoveSpeed { get; set; }
    public bool fUseMoverrideMoveSpeed { get; set; }

    public uint uiTimeSoldierWillArrive { get; set; }
    public bool fUseLandingZoneForArrival { get; set; }
    public bool fFallClockwise { get; set; }
    public sbyte bVehicleUnderRepairID { get; set; }
    public int iTimeCanSignElsewhere { get; set; }
    public sbyte bHospitalPriceModifier { get; set; }
    public uint uiStartTimeOfInsuranceContract { get; set; }
    public bool fRTInNonintAnim { get; set; }
    public bool fDoingExternalDeath { get; set; }
    public sbyte bCorpseQuoteTolerance { get; set; }
    public int iPositionSndID { get; set; }
    public uint uiTuringSoundID { get; set; }
    public byte ubLastDamageReason { get; set; }
    public bool fComplainedThatTired { get; set; }
    public short[] sLastTwoLocations { get; set; } = new short[2];
    public int uiTimeSinceLastBleedGrunt { get; set; }

//

    public uint CheckSum { get; set; }

    private byte[] Data;

    public BinaryReaderOffsetItem SaveFileOffset { get; private init; }

    public Dictionary<string, BinaryReaderOffsetItem> Offsets { get; private set; }

    public static Soldier Deserialize(byte[] data, ContentDataManager itemsManager, BinaryReaderOffsetItem offset)
    {
        //File.WriteAllBytes("soldier.bin", data);
        using var memoryStream = new MemoryStream(data);
        using var reader = new BinaryReaderOffset(memoryStream);

        var s = new Soldier
        {
            Data = data,
            SaveFileOffset = offset,
            ubID = reader.ReadByte()
        };

        reader.Skip(1);

        s.ubBodyType = reader.ReadByte();
        s.bActionPoints = reader.ReadSByte();
        s.bInitialActionPoints = reader.ReadSByte();

        reader.Skip(3);

        s.uiStatusFlags = reader.ReadUInt32();

        for (var i = 0; i < NUM_INV_SLOTS; i++)
        {
            s.inv[i] = ExtractObject(reader, itemsManager);
        }

        s.pTempObject = reader.Ptr();
        //s.pKeyRing = reader.Ptr();
        s.pKeyRing = reader.ReadBytes(4);

        s.bOldLife = reader.ReadSByte();
        s.bInSector = reader.ReadByte();
        s.bFlashPortraitFrame = reader.ReadSByte();

        reader.Skip(1);

        s.sFractLife = reader.ReadInt16();
        s.bBleeding = reader.ReadSByte();
        s.bBreath = reader.ReadSByte(nameof(bBreath));
        s.bBreathMax = reader.ReadSByte(nameof(bBreathMax));
        s.bStealthMode = reader.ReadSByte();
        s.sBreathRed = reader.ReadInt16(nameof(sBreathRed));
        s.fDelayedMovement = reader.ReadBoolean();

        reader.Skip(1);
        s.ubWaitActionToDo = reader.ReadByte();

        reader.Skip(1);

        s.ubInsertionDirection = reader.ReadSByte(); // хотя ub обычно => ReadByte()

        reader.Skip(1);

        s.opponent = reader.ReadByte();
        //EXTR_SOLDIER(d, s.opponent)

        s.bLastRenderVisibleValue = reader.ReadSByte();
        reader.Skip(1);
        s.ubAttackingHand = reader.ReadByte();
        reader.Skip(2);
        s.sWeightCarriedAtTurnStart = reader.ReadInt16();

        /*
        if (stracLinuxFormat)
    {
        reader.Skip(2);

        s->name = d.readUTF32(SOLDIERTYPE_NAME_LENGTH);
        }
        else
        {
            s->name = d.readUTF16(SOLDIERTYPE_NAME_LENGTH);
        }
        */

        s.name = reader.ReadUtf16Le(SOLDIERTYPE_NAME_LENGTH);

        s.bVisible = reader.ReadSByte();
        s.bActive = reader.ReadSByte();
        s.bTeam = reader.ReadSByte();
        s.ubGroupID = reader.ReadByte();
        s.fBetweenSectors = reader.ReadBoolean();
        s.ubMovementNoiseHeard = reader.ReadByte();

        // stracLinuxFormat блок в оригинале пропущен (условный skip(2))
        // reader.Skip(2); // если stracLinuxFormat == true

        s.dXPos = reader.ReadSingle(); // EXTR_FLOAT
        s.dYPos = reader.ReadSingle(); // EXTR_FLOAT


        reader.Skip(8);

        s.sInitialGridNo = reader.ReadInt16();
        s.sGridNo = reader.ReadInt16();
        s.bDirection = reader.ReadByte();

        reader.Skip(1);

        s.sHeightAdjustment = reader.ReadInt16();
        s.sDesiredHeight = reader.ReadInt16();
        s.sTempNewGridNo = reader.ReadInt16();

        reader.Skip(2);

        s.bOverTerrainType = reader.ReadSByte();

        reader.Skip(1);

        s.bCollapsed = reader.ReadSByte();
        s.bBreathCollapsed = reader.ReadSByte();
        s.ubDesiredHeight = reader.ReadByte();

        reader.Skip(1);


        s.usPendingAnimation = reader.ReadUInt16();
        s.ubPendingStanceChange = reader.ReadByte();


        reader.Skip(1);

        s.usAnimState = reader.ReadUInt16();
        s.fNoAPToFinishMove = reader.ReadBoolean();
        s.fPausedMove = reader.ReadBoolean();
        s.fUIdeadMerc = reader.ReadBoolean();


        reader.Skip(1);

        s.fUICloseMerc = reader.ReadBoolean();

        reader.Skip(13);

        s.UpdateCounter = null;
        s.DamageCounter = null;
        reader.Skip(4);

        reader.Skip(12);

        s.AICounter = null;
        s.FadeCounter = null;

        s.ubSkillTrait1 = reader.ReadByte(nameof(ubSkillTrait1));
        s.ubSkillTrait2 = reader.ReadByte(nameof(ubSkillTrait2));


        reader.Skip(6);

        s.bDexterity = reader.ReadSByte(nameof(bDexterity));
        s.bWisdom = reader.ReadSByte(nameof(bWisdom));
        reader.Skip(2); // EXTR_SKIP_I16

        s.attacker = reader.ReadByte(); // EXTR_SOLDIER
        s.previous_attacker = reader.ReadByte(); // EXTR_SOLDIER

        s.fTurnInProgress = reader.ReadBoolean();
        s.fIntendedTarget = reader.ReadBoolean();
        s.fPauseAllAnimation = reader.ReadBoolean();

        s.bExpLevel = reader.ReadSByte(nameof(bExpLevel));
        s.sInsertionGridNo = reader.ReadInt16();
        s.fContinueMoveAfterStanceChange = reader.ReadBoolean();


        reader.Skip(15);

        s.bLife = reader.ReadSByte(nameof(bLife));
        s.bSide = reader.ReadByte();
        reader.Skip(1);

        s.bNewOppCnt = reader.ReadSByte();
        reader.Skip(2);

        s.usAniCode = reader.ReadUInt16();
        s.usAniFrame = reader.ReadUInt16();
        s.sAniDelay = reader.ReadInt16();

        s.bAgility = reader.ReadSByte(nameof(bAgility));
        reader.Skip(1);

        s.sDelayedMovementCauseGridNo = reader.ReadInt16();
        s.sReservedMovementGridNo = reader.ReadInt16();
        s.bStrength = reader.ReadSByte(nameof(bStrength));


        reader.Skip(1);

        s.sTargetGridNo = reader.ReadInt16();
        s.bTargetLevel = reader.ReadSByte();
        s.bTargetCubeLevel = reader.ReadSByte();
        s.sLastTarget = reader.ReadInt16();
        s.bTilesMoved = reader.ReadSByte();
        s.bLeadership = reader.ReadSByte();
        s.dNextBleed = reader.ReadSingle(); // EXTR_FLOAT
        s.fWarnedAboutBleeding = reader.ReadBoolean();
        s.fDyingComment = reader.ReadBoolean();
        s.ubTilesMovedPerRTBreathUpdate = reader.ReadByte();
        reader.Skip(1);
        s.usLastMovementAnimPerRTBreathUpdate = reader.ReadUInt16();
        s.fTurningToShoot = reader.ReadBoolean();


        reader.Skip(1);

        s.fTurningUntilDone = reader.ReadBoolean();
        s.fGettingHit = reader.ReadBoolean();
        s.fInNonintAnim = reader.ReadBoolean();
        s.fFlashLocator = reader.ReadBoolean();
        s.sLocatorFrame = reader.ReadInt16();
        s.fShowLocator = reader.ReadBoolean();
        s.fFlashPortrait = reader.ReadBoolean();
        s.bMechanical = reader.ReadSByte(nameof(bMechanical));
        s.bLifeMax = reader.ReadSByte(nameof(bLifeMax));


        reader.Skip(6);

        s.HeadPal = reader.ReadUtf8(PaletteRepID_LENGTH);
        s.PantsPal = reader.ReadUtf8(PaletteRepID_LENGTH);
        s.VestPal = reader.ReadUtf8(PaletteRepID_LENGTH);
        s.SkinPal = reader.ReadUtf8(PaletteRepID_LENGTH);

        reader.Skip(328);

        s.bMedical = reader.ReadSByte(nameof(bMedical));
        s.fBeginFade = reader.ReadBoolean();
        s.ubFadeLevel = reader.ReadByte();
        s.ubServiceCount = reader.ReadByte();
        s.service_partner = reader.ReadByte(); // EXTR_SOLDIER
        s.bMarksmanship = reader.ReadSByte(nameof(bMarksmanship));
        s.bExplosive = reader.ReadSByte(nameof(bExplosive));


        reader.Skip(1);

        s.pThrowParams = reader.Ptr();

        s.fTurningFromPronePosition = reader.ReadBoolean();
        s.bReverse = reader.ReadSByte();

        reader.Skip(2);

        s.pLevelNode = reader.Ptr();

        reader.Skip(8);

        s.bDesiredDirection = reader.ReadSByte();

        reader.Skip(1);

        s.sDestXPos = reader.ReadInt16();
        s.sDestYPos = reader.ReadInt16();

        reader.Skip(2);

        s.sDestination = reader.ReadInt16();
        s.sFinalDestination = reader.ReadInt16();
        s.bLevel = reader.ReadSByte();

        reader.Skip(3);

        // pathing info takes up 16 bit in the savegame but 8 bit in the engine


        ushort[] usPathingData = reader.ReadUshorts(MAX_PATH_LIST_SIZE);
        ushort usPathDataSize = reader.ReadUInt16();
        ushort usPathIndex = reader.ReadUInt16();


        for (var i = 0; i < usPathDataSize && i < MAX_PATH_LIST_SIZE; i++)
        {
            s.ubPathingData[i] = (byte)usPathingData[i];
        }

        s.ubPathDataSize = (byte)usPathDataSize;
        s.ubPathIndex = (byte)usPathIndex;

        s.sBlackList = reader.ReadInt16();

        s.bAimTime = reader.ReadSByte();
        s.bShownAimTime = reader.ReadSByte();
        s.bPathStored = reader.ReadSByte();
        s.bHasKeys = reader.ReadSByte();

        reader.Skip(18);

        s.ubStrategicInsertionCode = reader.ReadByte();
        reader.Skip(1);
        s.usStrategicInsertionData = reader.ReadUInt16();

        reader.Skip(8);

        s.bMuzFlashCount = reader.ReadSByte();
        reader.Skip(1);

        s.sX = reader.ReadInt16();
        s.sY = reader.ReadInt16();
        s.usOldAniState = reader.ReadUInt16();
        s.sOldAniCode = reader.ReadInt16();
        s.bBulletsLeft = reader.ReadSByte();
        s.ubSuppressionPoints = reader.ReadByte();
        s.uiTimeOfLastRandomAction = reader.ReadUInt32();


        reader.Skip(2);

        s.bOppList = reader.ReadSbytes(MAX_NUM_SOLDIERS); // EXTR_I8A

        s.bLastAction = reader.ReadSByte();
        s.bAction = reader.ReadSByte();
        s.usActionData = reader.ReadUInt16();
        s.bNextAction = reader.ReadSByte();

        reader.Skip(1);

        s.usNextActionData = reader.ReadUInt16();
        s.bActionInProgress = reader.ReadSByte();
        s.bAlertStatus = reader.ReadSByte();
        s.bOppCnt = reader.ReadSByte();
        s.bNeutral = reader.ReadSByte();
        s.bNewSituation = reader.ReadSByte();
        s.bNextTargetLevel = reader.ReadSByte();
        s.bOrders = reader.ReadSByte();
        s.bAttitude = reader.ReadSByte();
        s.bUnderFire = reader.ReadSByte();
        s.bShock = reader.ReadSByte();


        reader.Skip(1);

        s.bBypassToGreen = reader.ReadSByte();
        reader.Skip(1);

        s.bDominantDir = reader.ReadSByte();
        s.bPatrolCnt = reader.ReadSByte();
        s.bNextPatrolPnt = reader.ReadSByte();

        s.usPatrolGrid = reader.ReadShorts(MAXPATROLGRIDS); // EXTR_I16A

        s.sNoiseGridno = reader.ReadInt16();
        s.ubNoiseVolume = reader.ReadByte();
        s.bLastAttackHit = reader.ReadSByte();
        s.xrayed_by = reader.ReadByte(); // EXTR_SOLDIER

        reader.Skip(1);

        s.dHeightAdjustment = reader.ReadSingle(); // EXTR_FLOAT

        if (reader.BaseStream.Position != 1736)
            throw new FormatException($"{nameof(bMorale)} must be at 1736");

        s.bMorale = reader.ReadSByte(nameof(bMorale));
        s.bTeamMoraleMod = reader.ReadSByte();
        s.bTacticalMoraleMod = reader.ReadSByte();
        s.bStrategicMoraleMod = reader.ReadSByte();
        s.bAIMorale = reader.ReadSByte();
        s.ubPendingAction = reader.ReadByte();
        s.ubPendingActionAnimCount = reader.ReadByte();


        reader.Skip(1);

        s.uiPendingActionData1 = reader.ReadUInt32();
        s.sPendingActionData2 = reader.ReadInt16();
        s.bPendingActionData3 = reader.ReadSByte();
        s.ubDoorHandleCode = reader.ReadSByte();
        s.uiPendingActionData4 = reader.ReadUInt32();
        s.bInterruptDuelPts = reader.ReadSByte();
        s.bPassedLastInterrupt = reader.ReadSByte();
        s.bIntStartAPs = reader.ReadSByte();
        s.bMoved = reader.ReadSByte();
        s.bHunting = reader.ReadSByte();

        reader.Skip(1);

        s.ubCaller = reader.ReadByte();

        reader.Skip(1);

        s.sCallerGridNo = reader.ReadInt16();
        s.bCallPriority = reader.ReadByte();
        s.bCallActedUpon = reader.ReadSByte();
        s.bFrenzied = reader.ReadSByte();
        s.bNormalSmell = reader.ReadSByte();
        s.bMonsterSmell = reader.ReadSByte();
        s.bMobility = reader.ReadSByte();


        reader.Skip(1);

        s.fAIFlags = reader.ReadSByte();
        s.fDontChargeReadyAPs = reader.ReadBoolean();

        reader.Skip(1);

        s.usAnimSurface = reader.ReadUInt16();
        s.sZLevel = reader.ReadUInt16();
        s.fPrevInWater = reader.ReadBoolean();
        s.fGoBackToAimAfterHit = reader.ReadBoolean();
        s.sWalkToAttackGridNo = reader.ReadInt16();
        s.sWalkToAttackWalkToCost = reader.ReadInt16();

        reader.Skip(7);

        s.fForceShade = reader.ReadBoolean();

        reader.Skip(2);

        // EXTR_PTR — указатель, можно прочитать как ulong или пропустить
        s.pForcedShade = reader.ReadUInt32(); //reader.Ptr(); // или просто: reader.Skip(8);

        s.bDisplayDamageCount = reader.ReadSByte();
        s.fDisplayDamage = reader.ReadSByte();
        s.sDamage = reader.ReadInt16();
        s.sDamageX = reader.ReadInt16();
        s.sDamageY = reader.ReadInt16();


        reader.Skip(1);

        s.bDoBurst = reader.ReadSByte();
        s.usUIMovementMode = reader.ReadInt16();
        reader.Skip(1);
        s.fUIMovementFast = reader.ReadBoolean();


        reader.Skip(10);

        s.BlinkSelCounter = null;
        s.PortraitFlashCounter = null;
        s.fDeadSoundPlayed = reader.ReadBoolean();
        s.ubProfile = reader.ReadByte();
        s.ubQuoteRecord = reader.ReadByte();
        s.ubQuoteActionID = reader.ReadByte();
        s.ubBattleSoundID = reader.ReadByte();
        s.fClosePanel = reader.ReadBoolean();
        s.fClosePanelToDie = reader.ReadBoolean();
        s.ubClosePanelFrame = reader.ReadByte();
        s.fDeadPanel = reader.ReadBoolean();
        s.ubDeadPanelFrame = reader.ReadByte();

        reader.Skip(2);

        s.sPanelFaceX = reader.ReadInt16();
        s.sPanelFaceY = reader.ReadInt16();
        s.bNumHitsThisTurn = reader.ReadSByte();

        reader.Skip(1);

        s.usQuoteSaidFlags = reader.ReadUInt16();
        s.fCloseCall = reader.ReadSByte();
        s.bLastSkillCheck = reader.ReadSByte();
        s.ubSkillCheckAttempts = reader.ReadSByte();


        reader.Skip(1);

        s.bStartFallDir = reader.ReadSByte();
        s.fTryingToFall = reader.ReadSByte();
        s.ubPendingDirection = reader.ReadByte();

        reader.Skip(1);

        s.uiAnimSubFlags = reader.ReadUInt32();
        s.bAimShotLocation = reader.ReadByte();
        s.ubHitLocation = reader.ReadByte();

        reader.Skip(16);

        s.sSpreadLocations = reader.ReadShorts(6);


        s.fDoSpread = reader.ReadBoolean();
        reader.Skip(1);

        s.sStartGridNo = reader.ReadInt16();
        s.sEndGridNo = reader.ReadInt16();
        s.sForcastGridno = reader.ReadInt16();
        s.sZLevelOverride = reader.ReadInt16();
        s.bMovedPriorToInterrupt = reader.ReadSByte();

        reader.Skip(3);

        s.iEndofContractTime = reader.ReadInt32();
        s.iStartContractTime = reader.ReadInt32();
        s.iTotalContractLength = reader.ReadInt32();
        s.iNextActionSpecialData = reader.ReadInt32();
        s.ubWhatKindOfMercAmI = reader.ReadByte();
        s.bAssignment = reader.ReadSByte();

        reader.Skip(1);

        s.fForcedToStayAwake = reader.ReadBoolean();
        s.bTrainStat = reader.ReadSByte();


        reader.Skip(1);

        if (reader.BaseStream.Position != 1922)
            throw new FormatException("sectorX must be at 1922");

        var sectorX = reader.ReadInt16();
        var sectorY = reader.ReadInt16();
        var sectorZ = reader.ReadSByte();
        s.sSector = new SGPSector(sectorX, sectorY, sectorZ);

        reader.Skip(1);

        s.iVehicleId = reader.ReadInt32();

        // EXTR_PTR — указатель на путь (например, WAYPOINT* или PathSt*)
        s.pMercPath = (PathSt)reader.Ptr();

        s.fHitByGasFlags = reader.ReadByte();

        reader.Skip(1);

        s.usMedicalDeposit = reader.ReadUInt16();
        s.usLifeInsurance = reader.ReadUInt16();

        reader.Skip(26);

        s.iStartOfInsuranceContract = reader.ReadInt32();
        s.uiLastAssignmentChangeMin = reader.ReadUInt32();
        s.iTotalLengthOfInsuranceContract = reader.ReadInt32();
        s.ubSoldierClass = reader.ReadByte();
        s.ubAPsLostToSuppression = reader.ReadByte();
        s.fChangingStanceDueToSuppression = reader.ReadBoolean();
        s.suppressor = reader.ReadByte(); // EXTR_SOLDIER

        reader.Skip(4);

        s.ubCivilianGroup = reader.ReadByte();

        reader.Skip(3);

        s.uiChangeLevelTime = reader.ReadUInt32();
        s.uiChangeHealthTime = reader.ReadUInt32();
        s.uiChangeStrengthTime = reader.ReadUInt32();
        s.uiChangeDexterityTime = reader.ReadUInt32();
        s.uiChangeAgilityTime = reader.ReadUInt32();
        s.uiChangeWisdomTime = reader.ReadUInt32();
        s.uiChangeLeadershipTime = reader.ReadUInt32();
        s.uiChangeMarksmanshipTime = reader.ReadUInt32();
        s.uiChangeExplosivesTime = reader.ReadUInt32();
        s.uiChangeMedicalTime = reader.ReadUInt32();
        s.uiChangeMechanicalTime = reader.ReadUInt32();
        s.uiUniqueSoldierIdValue = reader.ReadUInt32();

        s.bBeingAttackedCount = reader.ReadSByte();

        s.bNewItemCount = reader.ReadSbytes(NUM_INV_SLOTS); // EXTR_I8A
        s.bNewItemCycleCount = reader.ReadSbytes(NUM_INV_SLOTS); // EXTR_I8A

        s.fCheckForNewlyAddedItems = reader.ReadBoolean();
        s.bEndDoorOpenCode = reader.ReadSByte();
        s.ubScheduleID = reader.ReadByte();
        s.sEndDoorOpenCodeData = reader.ReadInt16();

        reader.Skip(4);

        s.NextTileCounter = null;
        s.fBlockedByAnotherMerc = reader.ReadBoolean();
        s.bBlockedByAnotherMercDirection = reader.ReadSByte();
        s.usAttackingWeapon = reader.ReadUInt16();


        s.bWeaponMode = reader.ReadByte();

        s.target = reader.ReadByte();


        s.bAIScheduleProgress = reader.ReadSByte();
        reader.Skip(1);

        s.sOffWorldGridNo = reader.ReadInt16();
        reader.Skip(2);

        // EXTR_PTR — указатель на анимационный тайл (AniTile*), можно просто пропустить
        s.pAniTile = reader.Ptr();

        s.bCamo = reader.ReadSByte();
        reader.Skip(1);

        s.sAbsoluteFinalDestination = reader.ReadInt16();
        s.ubHiResDirection = reader.ReadByte();
        reader.Skip(1);

        s.ubLastFootPrintSound = reader.ReadByte();
        s.bVehicleID = reader.ReadSByte();
        s.fPastXDest = reader.ReadSByte();
        s.fPastYDest = reader.ReadSByte();
        s.bMovementDirection = reader.ReadSByte();
        reader.Skip(1);

        s.sOldGridNo = reader.ReadInt16();
        s.usDontUpdateNewGridNoOnMoveAnimChange = reader.ReadUInt16();
        s.sBoundingBoxWidth = reader.ReadInt16();
        s.sBoundingBoxHeight = reader.ReadInt16();


        s.sBoundingBoxOffsetX = reader.ReadInt16();
        s.sBoundingBoxOffsetY = reader.ReadInt16();
        s.uiTimeSameBattleSndDone = reader.ReadUInt32();
        s.bOldBattleSnd = reader.ReadSByte(); // EXTR_AUTO (sbyte, как EXTR_I8)
        reader.Skip(1);

        s.fContractPriceHasIncreased = reader.ReadBoolean();
        reader.Skip(1);

        s.uiBurstSoundID = reader.ReadUInt32();
        s.fFixingSAMSite = reader.ReadBoolean();
        s.fFixingRobot = reader.ReadBoolean();
        s.bSlotItemTakenFrom = reader.ReadSByte();
        s.fSignedAnotherContract = reader.ReadBoolean();
        s.auto_bandaging_medic = reader.ReadByte(); // EXTR_SOLDIER
        s.fDontChargeTurningAPs = reader.ReadBoolean();
        s.robot_remote_holder = reader.ReadByte(); // EXTR_SOLDIER
        reader.Skip(1);

        s.uiTimeOfLastContractUpdate = reader.ReadUInt32();
        s.bTypeOfLastContract = reader.ReadSByte();
        s.bTurnsCollapsed = reader.ReadSByte();
        s.bSleepDrugCounter = reader.ReadSByte();
        s.ubMilitiaKills = reader.ReadByte();


        s.bFutureDrugEffect = reader.ReadSbytes(2); // EXTR_I8A
        s.bDrugEffectRate = reader.ReadSbytes(2);
        s.bDrugEffect = reader.ReadSbytes(2);
        s.bDrugSideEffectRate = reader.ReadSbytes(2);
        s.bDrugSideEffect = reader.ReadSbytes(2);

        reader.Skip(2);

        s.bBlindedCounter = reader.ReadSByte();
        s.fMercCollapsedFlag = reader.ReadBoolean();
        s.fDoneAssignmentAndNothingToDoFlag = reader.ReadBoolean();
        s.fMercAsleep = reader.ReadBoolean();
        s.fDontChargeAPsForStanceChange = reader.ReadBoolean();

        reader.Skip(2);

        s.ubTurnsUntilCanSayHeardNoise = reader.ReadByte();
        s.usQuoteSaidExtFlags = reader.ReadUInt16();
        s.sContPathLocation = reader.ReadUInt16();
        s.bGoodContPath = reader.ReadSByte();

        reader.Skip(1);

        s.bNoiseLevel = reader.ReadSByte();
        s.bRegenerationCounter = reader.ReadSByte();
        s.bRegenBoostersUsedToday = reader.ReadSByte();
        s.bNumPelletsHitBy = reader.ReadSByte();
        s.sSkillCheckGridNo = reader.ReadInt16();
        s.ubLastEnemyCycledID = reader.ReadByte();
        s.ubPrevSectorID = reader.ReadByte();
        s.ubNumTilesMovesSinceLastForget = reader.ReadByte();
        s.bTurningIncrement = reader.ReadSByte();
        s.uiBattleSoundID = reader.ReadUInt32();
        s.fSoldierWasMoving = reader.ReadBoolean();
        s.fSayAmmoQuotePending = reader.ReadBoolean();
        s.usValueGoneUp = reader.ReadUInt16();
        s.ubNumLocateCycles = reader.ReadByte();
        s.ubDelayedMovementFlags = reader.ReadByte();
        s.fMuzzleFlash = reader.ReadBoolean();
        s.CTGTTarget = reader.ReadByte(); // EXTR_SOLDIER


        reader.Skip(4);

        s.PanelAnimateCounter = null;

        if (reader.BaseStream.Position != 2208)
            throw new FormatException($"{nameof(CheckSum)} must be at 2208");

        s.CheckSum = reader.ReadUInt32(nameof(CheckSum));

        s.bCurrentCivQuote = reader.ReadSByte();
        s.bCurrentCivQuoteDelta = reader.ReadSByte();
        s.ubMiscSoldierFlags = reader.ReadByte();
        s.ubReasonCantFinishMove = reader.ReadByte();
        s.sLocationOfFadeStart = reader.ReadInt16();
        s.bUseExitGridForReentryDirection = reader.ReadByte();

        reader.Skip(1);

        s.uiTimeSinceLastSpoke = reader.ReadUInt32();
        s.ubContractRenewalQuoteCode = reader.ReadByte();

        reader.Skip(1);

        s.sPreTraversalGridNo = reader.ReadInt16();
        s.uiXRayActivatedTime = reader.ReadUInt32();
        s.bTurningFromUI = reader.ReadSByte();
        s.bPendingActionData5 = reader.ReadSByte();
        s.bDelayedStrategicMoraleMod = reader.ReadSByte();
        s.ubDoorOpeningNoise = reader.ReadByte();

        reader.Skip(4);

        s.ubLeaveHistoryCode = reader.ReadByte();
        s.fDontUnsetLastTargetFromTurn = reader.ReadBoolean();
        s.bOverrideMoveSpeed = reader.ReadSByte();


        s.fUseMoverrideMoveSpeed = reader.ReadBoolean();
        s.uiTimeSoldierWillArrive = reader.ReadUInt32();

        reader.Skip(1);

        s.fUseLandingZoneForArrival = reader.ReadBoolean();
        s.fFallClockwise = reader.ReadBoolean();
        s.bVehicleUnderRepairID = reader.ReadSByte();
        s.iTimeCanSignElsewhere = reader.ReadInt32();
        s.bHospitalPriceModifier = reader.ReadSByte();

        reader.Skip(3);

        s.uiStartTimeOfInsuranceContract = reader.ReadUInt32();
        s.fRTInNonintAnim = reader.ReadBoolean();
        s.fDoingExternalDeath = reader.ReadBoolean();
        s.bCorpseQuoteTolerance = reader.ReadSByte();

        reader.Skip(1);

        s.iPositionSndID = reader.ReadInt32();
        s.uiTuringSoundID = reader.ReadUInt32();
        s.ubLastDamageReason = reader.ReadByte();
        s.fComplainedThatTired = reader.ReadBoolean();
        s.sLastTwoLocations = reader.ReadShorts(2);


        reader.Skip(2);

        s.uiTimeSinceLastBleedGrunt = reader.ReadInt32(); // EXTR_I32
        s.next_to_previous_attacker = reader.ReadByte(); // EXTR_SOLDIER


        reader.Skip(39);

        /*
    if (stracLinuxFormat)
        {
            Assert(d.getConsumed() == 2352);
        }
        else
        {
            Assert(d.getConsumed() == 2328);
        }

        */

        if (reader.BaseStream.Position != 2328)
            throw new FormatException($"Must be 2328, but {reader.BaseStream.Position}");

        var checksum = Encryption.MercChecksum(s);
        if (checksum != s.CheckSum)
            throw new Exception($"Wrong checksum. Must be {s.CheckSum}, calculated {checksum}");

        s.Offsets = reader.Offsets;

        return s;
    }

    private static ObjectType ExtractObject(BinaryReader reader, ContentDataManager itemsManager)
    {
        var start = reader.BaseStream.Position;

        var o = new ObjectType
        {
            usItem = reader.ReadUInt16(),
            ubNumberOfObjects = reader.ReadByte()
        };

        o.usItemCheckSum = o.usItem;

        reader.Skip(1);

        o.usItem = itemsManager.ReplaceInvalidItem(o.usItem);
        var item = itemsManager.Get(o.usItem) ?? new Item
        {
            usItemClass = Item.IC_AMMO,
            itemIndex = (ushort)ITEMDEFINE.NONE
        };

        switch (item.GetItemClass())
        {
            case Item.IC_AMMO:
                o.ubShotsLeft = reader.ReadBytes(ObjectType.MAX_OBJECTS_PER_SLOT);
                reader.Skip(4);

                break;

            case Item.IC_GUN:
                o.bGunStatus = reader.ReadSByte();
                o.ubGunAmmoType = reader.ReadByte();
                o.ubGunShotsLeft = reader.ReadByte();

                reader.Skip(1);

                o.usGunAmmoItem = reader.ReadUInt16();
                o.bGunAmmoStatus = reader.ReadSByte();


                reader.Skip(5);

                break;

            case Item.IC_KEY:
                o.bKeyStatus = reader.ReadSbytes(6);
                o.ubKeyID = reader.ReadByte();

                reader.Skip(5);

                break;

            case Item.IC_MONEY:
                o.bMoneyStatus = reader.ReadSByte();

                reader.Skip(3);

                o.uiMoneyAmount = reader.ReadUInt32();

                reader.Skip(4);

                break;

            case Item.IC_MISC:
                switch (o.usItem)
                {
                    case (ushort)ITEMDEFINE.ACTION_ITEM:
                        o.bBombStatus = reader.ReadSByte();
                        o.bDetonatorType = reader.ReadSByte();
                        o.usBombItem = reader.ReadUInt16();
                        o.bDelay = reader.ReadSByte(); // TODO: could also be bDelay depending on context
                        o.ubBombOwner = reader.ReadByte();
                        o.bActionValue = reader.ReadByte();
                        o.ubTolerance = reader.ReadByte();


                        reader.Skip(4);

                        break;

                    case (ushort)ITEMDEFINE.OWNERSHIP:
                        o.ubOwnerProfile = reader.ReadByte();
                        o.ubOwnerCivGroup = reader.ReadByte();


                        reader.Skip(10);

                        break;

                    case (ushort)ITEMDEFINE.SWITCH:
                        o.bBombStatus = reader.ReadSByte();
                        o.bDetonatorType = reader.ReadSByte();
                        o.usBombItem = reader.ReadUInt16();
                        o.bDelay = reader.ReadSByte();
                        o.ubBombOwner = reader.ReadByte();
                        o.bActionValue = reader.ReadByte();
                        o.ubTolerance = reader.ReadByte();


                        reader.Skip(4);

                        break;

                    default:
                        o.bStatus = reader.ReadSbytes(ObjectType.MAX_OBJECTS_PER_SLOT);
                        reader.Skip(4);
                        break;
                }

                break;

            default:

                o.bStatus = reader.ReadSbytes(ObjectType.MAX_OBJECTS_PER_SLOT);
                reader.Skip(4);

                break;
        }

        o.usAttachItem = reader.ReadUshorts(ObjectType.MAX_ATTACHMENTS);
        o.bAttachStatus = reader.ReadSbytes(ObjectType.MAX_ATTACHMENTS);

        o.fFlags = reader.ReadSByte();
        o.ubMission = reader.ReadByte();
        o.bTrap = reader.ReadSByte();
        o.ubImprintID = reader.ReadByte();
        o.ubWeight = reader.ReadByte();
        o.fUsed = reader.ReadByte();

        reader.Skip(2);

        if (reader.BaseStream.Position != start + 36)
            throw new FormatException();

        // Check and remove invalid items in attachment slots

        for (var i = 0; i < o.usAttachItem.Length; i++)
        {
            o.usAttachItem[i] = itemsManager.ReplaceInvalidItem(o.usAttachItem[i]);
        }

        o.Offset = new BinaryReaderOffsetItem
        {
            Start = start,
            End = reader.BaseStream.Position,
            Type = typeof(ObjectType)
        };

        o.Item = item;

        return o;
    }

    public byte[] Serialize()
    {
        CheckSum = Encryption.MercChecksum(this);

        using var stream = new MemoryStream(Data, writable: true);
        using var writer = new BinaryWriter(stream);

        foreach (var offset in Offsets)
        {
            var value = this.GetValue(offset.Key);
            if (value == null) continue;

            writer.BaseStream.Seek(offset.Value.Start, SeekOrigin.Begin);

            switch (offset.Value.Type)
            {
                case var t when t == typeof(byte):
                    writer.Write((byte)value);
                    break;

                case var t when t == typeof(sbyte):
                    writer.Write((sbyte)value);
                    break;

                case var t when t == typeof(short):
                    writer.Write((short)value);
                    break;

                case var t when t == typeof(ushort):
                    writer.Write((ushort)value);
                    break;

                case var t when t == typeof(bool):
                    writer.Write((bool)value);
                    break;

                case var t when t == typeof(uint):
                    writer.Write((uint)value);
                    break;

                default:
                    throw new NotSupportedException($"Unsupported type: {offset.Value.Type}");
            }
        }

        return Data;
    }
}