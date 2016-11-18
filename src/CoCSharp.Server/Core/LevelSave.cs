﻿using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Server.API.Core;
using System;
using System.Collections.Generic;

namespace CoCSharp.Server.Core
{
    public class LevelSave : ILevelSave
    {
        #region Constructors
        public LevelSave()
        {
            // Space
        }

        public LevelSave(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            FromLevel(level);
        }
        #endregion

        #region Fields & Properties
        public string VillageJson { get; set; }

        public DateTime LastSave { get; set; }
        public long ID { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public bool IsNamed { get; set; }
        public int Trophies { get; set; }
        public int League { get; set; }
        public int ExpPoints { get; set; }
        public int ExpLevel { get; set; }
        public int Gems { get; set; }
        public int FreeGems { get; set; }
        public int AttacksWon { get; set; }
        public int AttacksLost { get; set; }
        public int DefensesWon { get; set; }
        public int DefensesLost { get; set; }

        public IEnumerable<ResourceCapacitySlot> ResourcesCapacity { get; set; }
        public IEnumerable<ResourceAmountSlot> ResourcesAmount { get; set; }
        public IEnumerable<UnitSlot> Units { get; set; }
        public IEnumerable<SpellSlot> Spells { get; set; }
        public IEnumerable<UnitUpgradeSlot> UnitUpgrades { get; set; }
        public IEnumerable<SpellUpgradeSlot> SpellUpgrades { get; set; }
        public IEnumerable<HeroUpgradeSlot> HeroUpgrades { get; set; }
        public IEnumerable<HeroHealthSlot> HeroHealths { get; set; }
        public IEnumerable<HeroStateSlot> HeroStates { get; set; }
        public IEnumerable<AllianceUnitSlot> AllianceUnits { get; set; }
        public IEnumerable<TutorialProgressSlot> TutorialProgress { get; set; }
        public IEnumerable<AchievementSlot> Achievements { get; set; }
        public IEnumerable<AchievementProgessSlot> AchievementProgress { get; set; }
        public IEnumerable<NpcStarSlot> NpcStars { get; set; }
        public IEnumerable<NpcGoldSlot> NpcGold { get; set; }
        public IEnumerable<NpcElixirSlot> NpcElixir { get; set; }
        #endregion

        #region Methods
        public void Overwrite(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            level.Village = Village.FromJson(VillageJson, level);
            level.LastSaveTime = LastSave;

            level.Token = Token;
            level.Avatar.ID = ID;
            level.Avatar.Name = Name;
            level.Avatar.IsNamed = IsNamed;
            level.Avatar.League = League;
            level.Avatar.Trophies = Trophies;
            level.Avatar.ExpPoints = ExpPoints;
            level.Avatar.ExpLevel = ExpLevel;
            level.Avatar.Gems = Gems;
            level.Avatar.FreeGems = FreeGems;
            level.Avatar.AttacksWon = AttacksWon;
            level.Avatar.AttacksLost = AttacksLost;
            level.Avatar.DefensesWon = DefensesWon;
            level.Avatar.DefensesLost = DefensesLost;

            Replace(level.Avatar.ResourcesCapacity, ResourcesCapacity);
            Replace(level.Avatar.ResourcesAmount, ResourcesAmount);
            Replace(level.Avatar.Units, Units);
            Replace(level.Avatar.Spells, Spells);
            Replace(level.Avatar.UnitUpgrades, UnitUpgrades);
            Replace(level.Avatar.SpellUpgrades, SpellUpgrades);
            Replace(level.Avatar.HeroUpgrades, HeroUpgrades);
            Replace(level.Avatar.HeroHealths, HeroHealths);
            Replace(level.Avatar.HeroStates, HeroStates);
            Replace(level.Avatar.AllianceUnits, AllianceUnits);
            Replace(level.Avatar.TutorialProgess, TutorialProgress);
            Replace(level.Avatar.Achievements, Achievements);
            Replace(level.Avatar.AchievementProgress, AchievementProgress);
            Replace(level.Avatar.NpcStars, NpcStars);
            Replace(level.Avatar.NpcGold, NpcGold);
            Replace(level.Avatar.NpcElixir, NpcElixir);
        }

        public Level ToLevel(AssetManager assets)
        {
            var level = new Level(assets);
            Overwrite(level);
            return level;
        }

        public void FromLevel(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            VillageJson = level.Village.ToJson();
            LastSave = level.LastSaveTime;

            Token = level.Token;
            ID = level.Avatar.ID;
            Name = level.Avatar.Name;
            IsNamed = level.Avatar.IsNamed;
            League = level.Avatar.League;
            Trophies = level.Avatar.Trophies;
            ExpPoints = level.Avatar.ExpPoints;
            ExpLevel = level.Avatar.ExpLevel;
            Gems = level.Avatar.Gems;
            FreeGems = level.Avatar.FreeGems;
            AttacksWon = level.Avatar.AttacksWon;
            AttacksLost = level.Avatar.AttacksLost;
            DefensesWon = level.Avatar.DefensesWon;
            DefensesLost = level.Avatar.DefensesLost;

            ResourcesCapacity = level.Avatar.ResourcesCapacity;
            ResourcesAmount = level.Avatar.ResourcesAmount;
            Units = level.Avatar.Units;
            Spells = level.Avatar.Spells;
            UnitUpgrades = level.Avatar.UnitUpgrades;
            SpellUpgrades = level.Avatar.SpellUpgrades;
            HeroUpgrades = level.Avatar.HeroUpgrades;
            HeroHealths = level.Avatar.HeroHealths;
            HeroStates = level.Avatar.HeroStates;
            AllianceUnits = level.Avatar.AllianceUnits;
            TutorialProgress = level.Avatar.TutorialProgess;
            Achievements = level.Avatar.Achievements;
            AchievementProgress = level.Avatar.AchievementProgress;
            NpcStars = level.Avatar.NpcStars;
            NpcGold = level.Avatar.NpcGold;
            NpcElixir = level.Avatar.NpcElixir;
        }

        private static void Replace<T>(ICollection<T> dst, IEnumerable<T> src)
        {
            if (src == null)
                return;

            dst.Clear();
            foreach (var val in src)
                dst.Add(val);
        }
        #endregion
    }
}