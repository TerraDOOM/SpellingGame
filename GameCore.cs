using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*

    Spelling Game is a 1v1 duel where you cast spells by typing in the language of magic to say what you want to happen
    Language follows SVO word order, with suffixes modifying things
    Subject is nearly constant as first person, though mind influencing uses second person. Third person doesn't exist.
    Verb dictates the effect of the spell, such as an explosion, life drain, heal, shield, etc.
    Object dictatest the target of the spell, generally the opponent or yourself, though modifiers can be added.
    Suffixes modify potency, descriptors or range.

*/

namespace Spelling
{
    public class Language
    {
        Dictionary<Suffix, LangObject> suffixDict;
        Dictionary<VerbEnum, LangObject> verbDict;
        Dictionary<ObjectEnum, LangObject> objDict;
        Dictionary<SubjectEnum, LangObject> subjDict;

        public Language()
        {
            suffixDict = new Dictionary<Suffix, LangObject>();
            verbDict = new Dictionary<VerbEnum, LangObject>();
            objDict = new Dictionary<ObjectEnum, LangObject>();
            subjDict = new Dictionary<SubjectEnum, LangObject>();

            foreach (Suffix suffix) {
                suffixDict[suffix] = new LangObject();
            }  
            foreach (VerbEnum verb) {
                verbDict[verb] = new LangObject();
            }
            foreach (ObjectEnum obj) {
                objDict[obj] = new LangObject();
            }
            foreach (SubjectEnum subj) {
                subjDict[subj] = new LangObject();
            }
        }

        public void InitializeLanguage()
        {
            subjDict[SubjectEnum.so].attributes.caster = TargetType.Self;
            subjDict[SubjectEnum.ya].attributes.caster = TargetType.Opponent;
            subjDict[SubjectEnum.Fizzle].attributes.fizzle = true;

            verbDict[VerbEnum.diestalaminya].attributes.type = SpellType.TargetKill;
            verbDict[VerbEnum.diestalaminya].attributes.potencyMod = 10;
            verbDict[VerbEnum.diestalaminya].attributes.costMod = 3;
            verbDict[VerbEnum.diestalaminya].attributes.damageType = DamageType.Accursed;
            verbDict[VerbEnum.diestalaminya].attributes.descriptor = "accursed power";
            verbDict[VerbEnum.turya].attributes.type = SpellType.Enhance;
            verbDict[VerbEnum.turya].attributes.durationMod = 3;
            verbDict[VerbEnum.turza].attributes.type = SpellType.Diminish;
            verbDict[VerbEnum.turza].attributes.durationMod = 3;
            verbDict[VerbEnum.sadem].attributes.type = SpellType.Shield;
            verbDict[VerbEnum.ilien].attributes.type = SpellType.Cloud;
            verbDict[VerbEnum.ilien].attributes.rangeMod = -999;
            verbDict[VerbEnum.ilien].attributes.durationMod = 1;
            verbDict[VerbEnum.ilien].attributes.sizeMod = 1;
            verbDict[VerbEnum.krak].attributes.type = SpellType.Nova;
            verbDict[VerbEnum.krak].attributes.sizeMod = 2;
            verbDict[VerbEnum.krak].attributes.rangeMod = -999;
            verbDict[VerbEnum.fuilien].attributes.type = SpellType.TargetGas;
            verbDict[VerbEnum.fuilien].attributes.rangeMod = -1;
            verbDict[VerbEnum.fuilien].attributes.sizeMod = 1;
            verbDict[VerbEnum.fulua].attributes.type = SpellType.TargetBolt;
            verbDict[VerbEnum.fulua].attributes.rangeMod = 3;
            verbDict[VerbEnum.fukrak].attributes.type = SpellType.TargetExplode;
            verbDict[VerbEnum.fukrak].attributes.potencyMod = 1;
            verbDict[VerbEnum.fukrak].attributes.rangeMod = 1;
            verbDict[VerbEnum.fukrak].attributes.costMod = 1;
            verbDict[VerbEnum.fukrak].attributes.sizeMod = 2;

            objDict[ObjectEnum.so].attributes.target = TargetType.Self;
            objDict[ObjectEnum.ya].attributes.target = TargetType.Opponent;
            objDict[ObjectEnum.pret].attributes.targetAspect = TargetedAspect.ManaRegen;
            objDict[ObjectEnum.iill].attributes.targetAspect = TargetedAspect.Speed;
            objDict[ObjectEnum.erin].attributes.targetAspect = TargetedAspect.Senses;
            objDict[ObjectEnum.Fizzle].attributes.fizzle = true;

            suffixDict[Suffix.so].attributes.caster = TargetType.Self;
            suffixDict[Suffix.so].attributes.target = TargetType.Self;
            suffixDict[Suffix.ya].attributes.caster = TargetType.Opponent;
            suffixDict[Suffix.ya].attributes.target = TargetType.Opponent;
            suffixDict[Suffix.la].attributes.echoMod = 1;
            suffixDict[Suffix.te].attributes.potencyMod = 1;
            suffixDict[Suffix.gen].attributes.sizeMod = 1;
            suffixDict[Suffix.iaz].attributes.silenceMod = 3;
            suffixDict[Suffix.noth].attributes.damageType = DamageType.Fire;
            suffixDict[Suffix.noth].attributes.descriptor = "blazing fire";
            suffixDict[Suffix.noz].attributes.damageType = DamageType.Cold;
            suffixDict[Suffix.noz].attributes.descriptor = "cold vapour";
            suffixDict[Suffix.fui].attributes.damageType = DamageType.Electricity;
            suffixDict[Suffix.fui].attributes.descriptor = "crackling air";
            suffixDict[Suffix.iai].attributes.damageType = DamageType.Sonic;
            suffixDict[Suffix.iai].attributes.descriptor = "booming noise";
            suffixDict[Suffix.nig].attributes.damageType = DamageType.Profane;
            suffixDict[Suffix.nig].attributes.descriptor = "profane energy";
            suffixDict[Suffix.wis].attributes.damageType = DamageType.Sacred;
            suffixDict[Suffix.wis].attributes.descriptor = "sacred energy";
            suffixDict[Suffix.kyf].attributes.damageType = DamageType.Acid;
            suffixDict[Suffix.kyf].attributes.descriptor = "acidic gas";
            suffixDict[Suffix.cek].attributes.damageType = DamageType.Physical;
            suffixDict[Suffix.cek].attributes.descriptor = "bludgeoning force";
            suffixDict[Suffix.laz].attributes.echoMod = -1;
            suffixDict[Suffix.tez].attributes.potencyMod = -1;
            suffixDict[Suffix.gez].attributes.sizeMod = -1;
        }
    }

    public class LangObject
    {
        public SpellAttributes attributes;

        public LangObject()
        {
            attributes = new SpellAttributes(true);
        }
    }

    public struct SpellAttributes
    {
        public SpellType type;
        public TargetType caster;
        public TargetType target;
        public TargetedAspect targetAspect;

        public int potencyMod;
        public int sizeMod;
        public int rangeMod;
        public int costMod;
        public int timeMod;
        public int echoMod;
        public int silenceMod;
        public int durationMod;
        public bool weak;
        public bool fizzle;
        public string descriptor;
        public DamageType damageType;

        public SpellAttributes(bool goFuckYourself)
        {
            type = SpellType.Shield;
            caster = TargetType.Self;
            target = TargetType.Opponent;
            targetAspect = TargetedAspect.None;

            potencyMod = 0;
            sizeMod = 0;
            rangeMod = 0;
            costMod = 0;
            timeMod = 0;
            echoMod = 0;
            silenceMod = 0;
            durationMod = 0;
            weak = false;
            fizzle = false;
            descriptor = "";
            damageType = DamageType.Force;
        }
        //note to self, add Tileset integration
    }

    public class Game
    {
        Duel duel;
        bool gameInProgress;
		public Tile[,] gameboard;

        public Game()
        {
            duel = new Duel();
            gameInProgress = true;
            gameboard = new Tile[10, 10];
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    gameboard[i, j] = new Tile();
                }
            }
        }

        Character GetPlayer()
        {
            int highestInit = 0;
            Character curPlayer;
            List<Character> highestCharacters = new List<Character>();
            foreach (Character player in duel.players)
            {
                if (player.initiative > highestInit) 
                {
                    highestInit = player.initiative;
                    highestCharacters.Clear();
                    highestCharacters.Add(player);
                }
                else if (player.initiative == highestInit)
                {
                    highestCharacters.Add(player);
                }
            }
            if (highestCharacters.Count > 0)
            {
                curPlayer = duel.players[duel.rng.Next(0, highestCharacters.Count())];
            }
            else
            {
                throw new Exception("No players in duel.players");
            }
            return curPlayer;
        }

        void runGame(Character player, Character[] opponents)
        {
        }
    }
	
	public class Tile
	{
		public bool containsPlayer = false;
		public Character containedPlayer;
		public bool containsCloud = false;
		public Spell containedCloud;
		public bool containsPatch = false;
		public Spell containedPath;
	}

    public class Duel
    {
        public Random rng;
        public Character player;
        public Character[] players;
        List<Spell> stack = new List<Spell>();
        public void InitializeSP(int playerCount, Tile[,] gameboard)
        {
            rng = new Random();
            var coordinates = new HashSet<(int, int)>();
            for (int i = 0; i < playerCount; i++) {
                var coordinate = (rng.Next(10), rng.Next(10));
                if (!coordinates.Contains(coordinate)) coordinates.Add(coordinate);
                else {
                    i--;
                    continue;
                }
            }
            Trace.Assert(coordinates.Count == playerCount);

            player = new Character(rng.Next(10), rng.Next(10));
            players = new Character[playerCount];
            player.initiative = rng.Next(1, 11);
            player.mana = 10;
            player.alive = true;
            players[0] = player;
            for (int i = 1; i < playerCount; i++) {
                Character opponent;
                do {
                    opponent = new Character(rng.Next(10), rng.Next(10));
                } while (players.Any(s => (s != null) ? s.posX != opponent.posX && s.posY != opponent.posY : false));
                opponent.initiative = rng.Next(1, 11);
                opponent.mana = 10;
                opponent.alive = true;
                players[i] = opponent;
            }
            foreach (Character player in players) {
                gameboard[player.posX, player.posY].containedPlayer = player;
                gameboard[player.posX, player.posY].containsPlayer = true;
            }
        }
        public void Uptick()
        {
            foreach (var player in players) player.UpdateCharacter();
            foreach (Spell s in stack)
            {
                s.ApplyEffects(this, s.stackCaster, s.stackTarget);
                stack.Remove(s);
                //Add code for upticking integration with map code
            }
        }
        public void Stack(Spell s, Character c, Character t, int x, int y, bool isCloud, bool inhabitsPlayer)
        {
            s.stackCaster = c;
            s.stackTarget = t;
            s.duration--;
            stack.Add(s);
            //Add code for stack integration working with map code
        }
    }

    public class Spell
    {
        public Character stackTarget;
        public Character stackCaster;

        static readonly HashSet<Suffix> weakSuffixes = new HashSet<Suffix> { Suffix.laz, Suffix.tez, Suffix.gez };
        public List<DamageType> damageTypes = new List<DamageType>();
        public Sentence sentence;
        public SpellType type;
        public TargetType target;
        public TargetType caster;
        public TargetedAspect aspectTarget = TargetedAspect.None;
        public int potency = 1;
        public int range = 3;
        public int cost = 1;
        public int time = 4;
        public int echoes = 0;
        public int size = 1;
        public int silence = 0;
        public int duration = 0;
        public int posX;
        public int posY;
        public bool fizzle = false;

        public float[] GetColour() {
            float[] colour = new float[3];
            colour[0] = 1;
            colour[1] = 0;
            colour[2] = 0;
            return colour;
        }

        public string GenerateDescriptors(List<DamageType> damageTypes)
        {
            string descriptors = "";
            foreach (DamageType elem in damageTypes.Distinct().Skip(1))
            {
                switch (elem)
                {
                    case DamageType.Accursed:
                        descriptors += "accursed power, ";
                        break;
                    case DamageType.Acid:
                        descriptors += "acidic gas, ";
                        break;
                    case DamageType.Cold:
                        descriptors += "cold vapour, ";
                        break;
                    case DamageType.Electricity:
                        descriptors += "crackling air, ";
                        break;
                    case DamageType.Fire:
                        descriptors += "blazing fire, ";
                        break;
                    case DamageType.Force:
                        descriptors += "magical force, ";
                        break;
                    case DamageType.Physical:
                        descriptors += "bludgeoning force, ";
                        break;
                    case DamageType.Profane:
                        descriptors += "profane energy, ";
                        break;
                    case DamageType.Sacred:
                        descriptors += "sacred energy, ";
                        break;
                    case DamageType.Sonic:
                        descriptors += "booming noise, ";
                        break;
                    default:
                        throw new Exception("Wtf");
                }
            }
            switch (damageTypes[0])
            {
                case DamageType.Accursed:
                    descriptors += "and accursed power";
                    break;
                case DamageType.Acid:
                    descriptors += "and acidic gas";
                    break;
                case DamageType.Cold:
                    descriptors += "and cold vapour";
                    break;
                case DamageType.Electricity:
                    descriptors += "and crackling air";
                    break;
                case DamageType.Fire:
                    descriptors += "and blazing fire";
                    break;
                case DamageType.Force:
                    descriptors += "and magical force";
                    break;
                case DamageType.Physical:
                    descriptors += "and bludgeoning force";
                    break;
                case DamageType.Profane:
                    descriptors += "and profane energy";
                    break;
                case DamageType.Sacred:
                    descriptors += "and sacred energy";
                    break;
                case DamageType.Sonic:
                    descriptors += "and booming noise";
                    break;
                default:
                    throw new Exception("Wtf");
            }
            return descriptors;
        }

        public string ApplyEffects(Duel handler, Character s, Character t)
        {
            if (!fizzle)
            {
                for (int i = 0; i < echoes + 1; i++)
                {
                    Character targetActive;
                    Character casterActive = s;
                    var descriptiveText = new StringBuilder();
                    var targetDescriptor = "";
                    if (target == TargetType.Opponent)
                    {
                        targetActive = t;
                        targetDescriptor = " opponent";
                    }
                    else
                    {
                        targetActive = s;
                        targetDescriptor = "self";
                    }
                    if (duration > 0)
                    {
                        handler.Stack(this, targetActive, casterActive, 0, 0, false, false);
                    }
                    switch (type)
                    {
                        case SpellType.Cloud:
                            targetActive.Damage(potency, damageTypes);
                            descriptiveText.AppendFormat(
                                "A cloud of {0} bursts into creation around you, hitting your {1}",
                                this.GenerateDescriptors(damageTypes),
                                targetDescriptor);
                            break;
                        case SpellType.Nova:
                            targetActive.Damage(potency, damageTypes);
                            descriptiveText.AppendFormat("A burst of {0} rushes out from your body, hitting your {1}",
                                this.GenerateDescriptors(damageTypes),
                                targetDescriptor);
                            break;
                        case SpellType.TargetBolt:
                            targetActive.Damage(potency, damageTypes);
                            descriptiveText.AppendFormat("You point at your{0} and a bolt of {1} flies out from your finger, hitting your{0}",
                                targetDescriptor,
                                this.GenerateDescriptors(damageTypes));
                            break;
                        case SpellType.TargetExplode:
                            targetActive.Damage(potency, damageTypes);
                            descriptiveText.AppendFormat(
                                "You point at your{0} and a bead of {1} flies out from your finger, blossoming into an explosion around your{0}",
                                targetDescriptor, this.GenerateDescriptors(damageTypes));
                            break;
                        case SpellType.TargetGas:
                            targetActive.Damage(potency, damageTypes);
                            descriptiveText.AppendFormat(
                                "You point at your{0} and a bead of {1} flies out from your finger, blossoming into a cloud of gas around your{0}",
                                targetDescriptor, this.GenerateDescriptors(damageTypes));
                            break;
                        case SpellType.TargetKill:
                            damageTypes.Clear();
                            damageTypes.Add(DamageType.Accursed); //reminder to move this to the switch case for verbs later.
                            targetActive.Damage(potency, damageTypes);
                            descriptiveText.Append(
                                $"You point at your{targetDescriptor} " +
                                $"and foul vapour mixed with ${this.GenerateDescriptors(damageTypes)} streams from your finger, " +
                                $"reaching and grasping at your${targetDescriptor}");
                            break;
                        case SpellType.Enhance:
                            switch (aspectTarget)
                            {
                                case TargetedAspect.ManaRegen:
                                    targetActive.manaRegen = Aspect.Enhanced;
                                    descriptiveText.AppendFormat(
                                        "You flood your{0} with harmonic mana, enhancing the Corona Potentia, and by extension, the mana regeneration system.",
                                        targetDescriptor);
                                    break;
                                case TargetedAspect.Speed:
                                    targetActive.speed = Aspect.Enhanced;
                                    descriptiveText.AppendFormat(
                                        "You flood your{0} with harmonic mana, enhancing the muscle fibers and innate reflexes.",
                                        targetDescriptor);
                                    break;
                                case TargetedAspect.Senses:
                                    targetActive.senses = Aspect.Enhanced;
                                    descriptiveText.AppendFormat(
                                        "You flood your{0} with harmonic mana, enhancing the five senses.",
                                        targetDescriptor);
                                    break;
                                case TargetedAspect.None:
                                    return "";
                            }
                            break;
                        case SpellType.Diminish:
                            switch (aspectTarget)
                            {
                                case TargetedAspect.ManaRegen:
                                    targetActive.manaRegen = Aspect.Diminish;
                                    descriptiveText.AppendFormat(
                                        "You flood your{0} with discordant mana, inhibiting the Corona Potentia, and by extension, the mana regeneration system.",
                                        targetDescriptor);
                                    break;
                                case TargetedAspect.Speed:
                                    targetActive.speed = Aspect.Diminish;
                                    descriptiveText.AppendFormat(
                                        "You flood your{0} with discordant mana, inhibiting the muscle fibers and innate reflexes.",
                                        targetDescriptor);
                                    break;
                                case TargetedAspect.Senses:
                                    targetActive.senses = Aspect.Diminish;
                                    descriptiveText.AppendFormat(
                                        "You flood your{0} with discordant mana, inhibiting the five senses.",
                                        targetDescriptor);
                                    break;
                                case TargetedAspect.None:
                                    return "";
                            }
                            break;
                        case SpellType.Shield:
                            foreach (DamageType element in damageTypes)
                            {
                                targetActive.shields[element] += (potency / damageTypes.Count);
                            }
                            descriptiveText.AppendFormat(
                                "You build up shields around your{0}, forming them out of {1}.",
                                targetDescriptor, this.GenerateDescriptors(damageTypes));
                            break;
                        default:
                            throw new SystemException("Wtf");
                    }

                    return descriptiveText.ToString();
                    //Eventually add mechanics for the mind control spells (caster = opponent)
                }
                return "lmao what";
            }
            else
            {
                return "The spell fizzles, the words lacking in fluency,";
            }
        }
        public Spell(string s)
        {
            sentence = new Sentence(s);
        }
        public void ParseWords()
        {
            int count = sentence.subj.suffixes.Count + sentence.verb.suffixes.Count + sentence.obj.suffixes.Count;
            foreach (Suffix suffix in sentence.subj.suffixes.FindAll(suffix => weakSuffixes.Contains(suffix)))
            {
                count -= 2;
                time++;
            }
            for (int i = 0; i < count; i++)
            {
                cost = cost + i;
                time++;
            }
            switch (sentence.obj.word)
            {
                case ObjectEnum.so:
                    target = TargetType.Self;
                    break;
                case ObjectEnum.ya:
                    target = TargetType.Opponent;
                    break;
                case ObjectEnum.Fizzle:
                    fizzle = true;
                    break;
                default:
                    break;
            }
            switch (sentence.subj.word)
            {
                case SubjectEnum.so:
                    caster = TargetType.Self;
                    break;
                case SubjectEnum.ya:
                    caster = TargetType.Opponent;
                    break;
                default:
                    fizzle = true;
                    break;
            }
            foreach (Suffix suffix in sentence.verb.suffixes)
            {
                switch (suffix)
                {
                    case Suffix.la:
                        echoes++;
                        break;
                    case Suffix.te:
                        potency++;
                        break;
                    case Suffix.noth:
                        damageTypes.Add(DamageType.Fire);
                        break;
                    case Suffix.noz:
                        damageTypes.Add(DamageType.Cold);
                        break;
                    case Suffix.fui:
                        damageTypes.Add(DamageType.Electricity);
                        break;
                    case Suffix.iai:
                        damageTypes.Add(DamageType.Sonic);
                        break;
                    case Suffix.laz:
                        echoes--;
                        break;
                    case Suffix.tez:
                        potency--;
                        break;
                    case Suffix.gen:
                        size++;
                        break;
                    case Suffix.nig:
                        damageTypes.Add(DamageType.Profane);
                        break;
                    case Suffix.wis:
                        damageTypes.Add(DamageType.Sacred);
                        break;
                    case Suffix.kyf:
                        damageTypes.Add(DamageType.Acid);
                        break;
                    case Suffix.iaz:
                        silence += 3;
                        break;
                    case Suffix.gez:
                        size--;
                        break;
                    default:
                        fizzle = true;
                        break;
                }
            }
            switch (sentence.verb.word)
            {
                case VerbEnum.fukrak:
                    type = SpellType.TargetExplode;
                    size += 2;
                    range++;
                    cost++;
                    potency++;
                    break;
                case VerbEnum.fulua:
                    type = SpellType.TargetBolt;
                    range += 3;
                    break;
                case VerbEnum.fuilien:
                    type = SpellType.TargetGas;
                    range--;
                    size++;
                    break;
                case VerbEnum.krak:
                    type = SpellType.Nova;
                    size += 2;
                    range = 0;
                    break;
                case VerbEnum.ilien:
                    type = SpellType.Cloud;
                    size++;
                    range = 0;
                    duration++;
                    break;
                case VerbEnum.sadem:
                    type = SpellType.Shield;
                    break;
                case VerbEnum.diestalaminya:
                    type = SpellType.TargetKill;
                    cost += 3;
                    potency = 10;
                    break;
                case VerbEnum.turya:
                    type = SpellType.Enhance;
                    duration = 3;
                    break;
                case VerbEnum.turza:
                    type = SpellType.Diminish;
                    duration = 3;
                    break;
                default:
                    fizzle = true;
                    break;
            }
            foreach (Suffix suffix in sentence.obj.suffixes)
            {
                switch (suffix)
                {
                    case Suffix.so:
                        target = TargetType.Self;
                        switch (sentence.obj.word)
                        {
                            case ObjectEnum.erin:
                                aspectTarget = TargetedAspect.Senses;
                                break;
                            case ObjectEnum.iill:
                                aspectTarget = TargetedAspect.Speed;
                                break;
                            case ObjectEnum.pret:
                                aspectTarget = TargetedAspect.ManaRegen;
                                break;
                        }
                        break;
                    case Suffix.ya:
                        target = TargetType.Opponent;
                        switch (sentence.obj.word)
                        {
                            case ObjectEnum.erin:
                                aspectTarget = TargetedAspect.Senses;
                                break;
                            case ObjectEnum.iill:
                                aspectTarget = TargetedAspect.Speed;
                                break;
                            case ObjectEnum.pret:
                                aspectTarget = TargetedAspect.ManaRegen;
                                break;
                        }
                        break;
                    default:
                        fizzle = true;
                        break;

                }
            }
        }
    }

    public class Character
    {
        public int mana;
        public int initiative;
        public Dictionary<DamageType, int> shields;
        public bool alive;
        public Spell currentSpell;
        public Aspect manaRegen = Aspect.Normal;
        public Aspect senses = Aspect.Normal;
        public Aspect speed = Aspect.Normal;
		public int posX;
		public int posY;
        public List<Spell> containedSpells;

        public void Damage(int damage, List<DamageType> damageTypes)
        {
            foreach (DamageType elem in damageTypes)
            {
                shields[elem] -= (damage / damageTypes.Count);
                if (shields[elem] < 0)
                {
                    alive = false;
                }
            }
        }

        public void UpdateCharacter()
        {
            switch (speed)
            {
                case Aspect.Diminish:
                    initiative++;
                    break;
                case Aspect.Normal:
                    initiative += 2;
                    break;
                case Aspect.Enhanced:
                    initiative += 3;
                    break;
            }
            switch (manaRegen)
            {
                case Aspect.Diminish:
                    mana++;
                    break;
                case Aspect.Normal:
                    mana += 2;
                    break;
                case Aspect.Enhanced:
                    mana += 4;
                    break;
            }
            speed = Aspect.Normal;
            manaRegen = Aspect.Normal;
        }

        public Character(int x, int y)
        {
            shields = new Dictionary<DamageType, int>();
            foreach (var elem in Enum.GetValues(typeof(DamageType)).Cast<DamageType>())
            {
                shields[elem] = 10;
                posX = x;
                posY = y;
                
            }
            alive = true;
        }

        public void TakeTurn()
        {

        }
    }

    public class Subject
    {
        public SubjectEnum word;
        public List<Suffix> suffixes;

        public Subject(string s)
        {
            var sArr = s.Split('\'');
            suffixes = new List<Suffix>();
            try
            {
                word = (SubjectEnum)Enum.Parse(typeof(SubjectEnum), sArr[0]);
                foreach (string suffix in sArr.Skip(1))
                {
                    suffixes.Add((Suffix)Enum.Parse(typeof(Suffix), suffix));
                }
            }
            catch (ArgumentException)
            {
                word = SubjectEnum.Fizzle;
            }
        }
    }

    public class Verb
    {
        public VerbEnum word;
        public List<Suffix> suffixes;
        public Verb(string s)
        {
            var sArr = s.Split('\'');
            suffixes = new List<Suffix>();
            try
            {
                word = (VerbEnum)Enum.Parse(typeof(VerbEnum), sArr[0]);
                foreach (string suffix in sArr.Skip(1))
                {
                    suffixes.Add((Suffix)Enum.Parse(typeof(Suffix), suffix));
                }
            }
            catch
            {
                word = VerbEnum.Fizzle;
            }
        }
    }

    public class Object
    {
        public ObjectEnum word;
        public List<Suffix> suffixes;
        public Object(string s)
        {
            var sArr = s.Split('\'');
            suffixes = new List<Suffix>();
            try
            {
                word = (ObjectEnum)Enum.Parse(typeof(ObjectEnum), sArr[0]);
                foreach (string suffix in sArr.Skip(1))
                {
                    suffixes.Add((Suffix)Enum.Parse(typeof(Suffix), suffix));
                }
            }
            catch
            {
                word = ObjectEnum.Fizzle;
            }
        }
    }

    public enum TargetType
    {
        Self,
        Opponent,
    }

    public enum Aspect
    {
        Diminish,
        Normal,
        Enhanced
    }

    public enum TargetedAspect
    {
        Speed,
        Senses,
        ManaRegen,
        None
    }

    public enum SpellType
    {
        TargetExplode,
        TargetBolt,
        TargetGas,
        Nova,
        Cloud,
        Shield,
        TargetKill,
        Enhance,
        Diminish
    }

    public enum DamageType
    {
        Fire,
        Cold,
        Electricity,
        Acid,
        Physical,
        Sacred,
        Profane,
        Force,
        Sonic,
        Accursed
    }

    public enum Suffix
    {
        la,
        te,
        noth,
        noz,
        fui,
        iai,
        laz,
        tez,
        gen,
        nig,
        wis,
        kyf,
        cek,
        iaz,
        gez,
        so,
        ya
    }

    public enum SubjectEnum
    {
        so,
        ya,
        Fizzle
    }

    public enum VerbEnum
    {
        fukrak,
        fulua,
        fuilien,
        krak,
        ilien,
        sadem,
        diestalaminya,
        turya,
        turza,
        Fizzle
    }

    public enum ObjectEnum
    {
        so,
        ya,
        erin,
        iill,
        pret,
        Fizzle
    }

    public class Sentence
    {
        public Subject subj { get; set; }
        public Verb verb { get; set; }
        public Object obj { get; set; }

        public Sentence(string s)
        {
            var split = s.Split(null);
            if (split.Length == 3)
            {
                subj = new Subject(split[0]);
                verb = new Verb(split[1]);
                obj = new Object(split[2]);
            }
            else
            {
                subj = new Subject("ss");
                verb = new Verb("vv");
                obj = new Object("oo");
            }
        }
    }
}
