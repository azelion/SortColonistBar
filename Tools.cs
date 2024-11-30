using RimWorld;
using SortColonistBar.FloatMenus;
using System;
using System.Collections.Generic;
using Verse;

namespace SortColonistBar
{
    public static class Tools
    {
        private const int _noSkill = 999999;
        private const int _oneDigitSignificant = 1000;
        private const int maxSkillLevel = 20;
        public static FloatMenuWithOptions LabelMenu =
            new FloatMenuWithOptions(new List<FloatMenuOption>
            {
                MakeMenuItemForLabel(SortChoice.Manual),
                MakeMenuItemForLabel(SortChoice.Name),
                MakeMenuItemForLabel(SortChoice.Ranged),
                MakeMenuItemForLabel(SortChoice.Melee),
                MakeMenuItemForLabel(SortChoice.Construction),
                MakeMenuItemForLabel(SortChoice.Mining),
                MakeMenuItemForLabel(SortChoice.Cooking),
                MakeMenuItemForLabel(SortChoice.Plants),
                MakeMenuItemForLabel(SortChoice.Animals),
                MakeMenuItemForLabel(SortChoice.Crafting),
                MakeMenuItemForLabel(SortChoice.Art),
                MakeMenuItemForLabel(SortChoice.Medicine),
                MakeMenuItemForLabel(SortChoice.Social),
                MakeMenuItemForLabel(SortChoice.Intellectual),
                MakeMenuItemForLabel(SortChoice.Value),
                MakeMenuItemForLabel(SortChoice.Health),
                MakeMenuItemForLabel(SortChoice.Speed),
                MakeMenuItemForLabel(SortChoice.Age),
            });
        public static FloatMenuWithOptions ActionMenu;

        public static void CloseLabelMenu(bool sound = false)
        {
            if (LabelMenu != null)
            {
                Find.WindowStack.TryRemove(LabelMenu, sound);
            }
        }

        public static bool Reverse { get; set; }

        public enum SortChoice
        {
            Manual,
            Name,
            Ranged,
            Melee,
            Construction,
            Mining,
            Cooking,
            Plants,
            Animals,
            Crafting,
            Art,
            Health,
            Medicine,
            Social,
            Intellectual,
            Speed,
            Value,
            Age,
        }

        public static FloatMenuOption MakeMenuItemForLabel(SortChoice choice)
        {
            return new FloatMenuOptionNoClose(choice.ToString(), new Action(() => Sort = choice));
        }

        private static SortChoice _sort = SortChoice.Manual;
        public static SortChoice Sort
        {
            get => _sort;
            set
            {
                Log.Message("Sort: " + value.ToString());
                Find.ColonistBar.MarkColonistsDirty();
                if (_sort != value)
                {
                    ThingIDNumberGetter = NextThingIDNumberGetter;
                }
                if (value == _sort)
                {
                    Tools.Reverse = !Tools.Reverse;
                }
                else
                {
                    Tools.Reverse = false;
                }
                switch (value)
                {
                    case SortChoice.Name:
                        break;

                    case SortChoice.Manual:
                        DisplayOrderGetter = _defaultDisplayOrderGetter;
                        ThingIDNumberGetter = _defaultThingIDNumberGetter;
                        break;
                    case SortChoice.Speed:
                        DisplayOrderGetter = (Pawn x) => -(int)(x.GetStatValue(StatDefOf.MoveSpeed) * _oneDigitSignificant);
                        NextThingIDNumberGetter = (Pawn x) => _oneDigitSignificant - (int)(x.GetStatValue(StatDefOf.MoveSpeed) * _oneDigitSignificant);
                        break;
                    case SortChoice.Value:
                        DisplayOrderGetter = (Pawn x) => -(int)(x.GetStatValue(StatDefOf.MarketValue));
                        NextThingIDNumberGetter = (Pawn x) => _noSkill - (int)(x.GetStatValue(StatDefOf.MarketValue));
                        break;
                    case SortChoice.Social:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Social) == null || x.skills.GetSkill(SkillDefOf.Social).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Social).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Social) == null || x.skills.GetSkill(SkillDefOf.Social).TotallyDisabled ? _noSkill : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Social).Level;
                        break;
                    case SortChoice.Ranged:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Shooting) == null || x.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Shooting).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Shooting) == null || x.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled ? _noSkill : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Shooting).Level;
                        break;
                    case SortChoice.Medicine:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Medicine) == null || x.skills.GetSkill(SkillDefOf.Medicine).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Medicine).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Medicine) == null || x.skills.GetSkill(SkillDefOf.Medicine).TotallyDisabled ? x.thingIDNumber : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Medicine).Level;
                        break;
                    case SortChoice.Crafting:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Crafting) == null || x.skills.GetSkill(SkillDefOf.Crafting).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Crafting).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Crafting) == null || x.skills.GetSkill(SkillDefOf.Crafting).TotallyDisabled ? x.thingIDNumber : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Crafting).Level;
                        break;
                    case SortChoice.Construction:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Construction) == null || x.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Construction).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Construction) == null || x.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled ? x.thingIDNumber : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Construction).Level;
                        break;
                    case SortChoice.Cooking:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Cooking) == null || x.skills.GetSkill(SkillDefOf.Cooking).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Cooking).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Cooking) == null || x.skills.GetSkill(SkillDefOf.Cooking).TotallyDisabled ? x.thingIDNumber : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Cooking).Level;
                        break;
                    case SortChoice.Mining:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Mining) == null || x.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Mining).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Mining) == null || x.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled ? x.thingIDNumber : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Mining).Level;
                        break;
                    case SortChoice.Intellectual:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Intellectual) == null || x.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Intellectual).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Intellectual) == null || x.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled ? _noSkill : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Intellectual).Level;
                        break;
                    case SortChoice.Plants:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Plants) == null || x.skills.GetSkill(SkillDefOf.Plants).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Plants).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Plants) == null || x.skills.GetSkill(SkillDefOf.Plants).TotallyDisabled ? x.thingIDNumber : 20 - x.skills.GetSkill(SkillDefOf.Plants).Level;
                        break;
                    case SortChoice.Animals:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Animals) == null || x.skills.GetSkill(SkillDefOf.Animals).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Animals).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Animals) == null || x.skills.GetSkill(SkillDefOf.Animals).TotallyDisabled ? x.thingIDNumber : 20 - x.skills.GetSkill(SkillDefOf.Animals).Level;
                        break;
                    case SortChoice.Melee:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Melee) == null || x.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Melee).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Melee) == null || x.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled ? x.thingIDNumber : 20 - x.skills.GetSkill(SkillDefOf.Melee).Level;
                        break;
                    case SortChoice.Art:
                        DisplayOrderGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Artistic) == null || x.skills.GetSkill(SkillDefOf.Artistic).TotallyDisabled ? _noSkill : -x.skills.GetSkill(SkillDefOf.Artistic).Level;
                        NextThingIDNumberGetter = (Pawn x) => x.skills?.GetSkill(SkillDefOf.Artistic) == null || x.skills.GetSkill(SkillDefOf.Artistic).TotallyDisabled ? x.thingIDNumber : 20 - x.skills.GetSkill(SkillDefOf.Artistic).Level;
                        break;
                    case SortChoice.Age:
                        DisplayOrderGetter = (Pawn x) => x.ageTracker?.AgeBiologicalYears ?? _noSkill;
                        NextThingIDNumberGetter = (Pawn x) => (20 - x.ageTracker?.AgeBiologicalYears) ?? x.thingIDNumber;
                        break;
                    default:
                        Log.Warning("Unimplemented sort opstion");
                        _sort = SortChoice.Manual;
                        DisplayOrderGetter = _defaultDisplayOrderGetter;
                        ThingIDNumberGetter = _defaultThingIDNumberGetter;
                        NextThingIDNumberGetter = _defaultThingIDNumberGetter;
                        break;
                }

                _sort = value;
            }
        }

        private static readonly Func<Pawn, int> _defaultDisplayOrderGetter = (Pawn x) => (x.playerSettings == null) ? _noSkill : x.playerSettings.displayOrder;
        private static readonly Func<Pawn, int> _defaultThingIDNumberGetter = (Pawn x) => x.thingIDNumber;

        public static Func<Pawn, int> DisplayOrderGetter { get; private set; } = _defaultDisplayOrderGetter;
        public static Func<Pawn, int> ThingIDNumberGetter { get; private set; } = _defaultThingIDNumberGetter;
        public static Func<Pawn, int> NextThingIDNumberGetter { get; private set; } = _defaultThingIDNumberGetter;
    }
}