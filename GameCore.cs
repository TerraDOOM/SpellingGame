using System;
using System.Collections.Generic;
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
    class Game {
        Duel duel;

        public Game() {
            duel = new Duel();
        }

        void TakeTurn() {
            if (duel.player.initiative > duel.opponent.initiative) {
                
            } else if (duel.player.initiative == duel.opponent.initiative) {
                switch (duel.rng.Next(0, 2)) {
                    case 0:
                        duel.player.initiative++;
                        break;
                    case 1:
                        duel.opponent.initiative++;
                        break;
                    default:
                        throw new SystemException("math broke, cosmic rays or something idk");
                }
            }
        }

        static void Main(string[] args)
        {
            bool gameInProgress = false;
            while (true)
            {
                switch (Console.ReadLine().ToLower())
                {
                    case "help":
                        Console.WriteLine("Help: gives a list of commands.");
                        Console.WriteLine("Start: starts a duel");
                        Console.WriteLine("Exit: quits the game.");
                        break;
                    case "start":
                        duel.Initialize();
                        gameInProgress = true;
                        while (gameInProgress)
                        {
                            if (duel.player.initiative > duel.opponent.initiative)
                            {
                                switch (Console.ReadLine().ToLower())
                                {
                                    case "status":
                                        //add code for displaying different visual effects, current mana and initiative, current shields, etc.
                                        break;
                                    case "spell":
                                        Console.WriteLine("Cast your spell:");
                                        duel.player.currentSpell = new Spell(Console.ReadLine().ToLower());
                                        duel.player.currentSpell.ParseWords();
                                        duel.player.currentSpell.ApplyEffects(duel, duel.player, duel.opponent);
                                        //add code for actually shooting the spell at the opponent, as well as applying its effects to the opponent
                                        break;
                                    default:
                                        Console.WriteLine("Invalid command");
                                        break;
                                        /*
                                         * Put in-duel commands here.
                                         * of importance are:
                                         * "Spell" which makes the next command be interpreted as a spell. Typos fizzle it.
                                         * "Status" to see the shields and effects on are on both you and your opponent.
                                         * "Time" to see the turncount. Maybe time left?
                                         * Probably nothing else.
                                        */
                                }
                            }
                            else if (duel.player.initiative == duel.opponent.initiative)
                            {
                                switch (duel.rng.Next(0, 2))
                                {
                                    case 0:
                                        duel.player.initiative++;
                                        break;
                                    case 1:
                                        duel.opponent.initiative++;
                                        break;
                                    default:
                                        Console.WriteLine("Error in iniative tie-breaker code.");
                                        System.Environment.Exit(0);
                                        break;
                                }
                            }
                            else
                            {
                                duel.opponent.TakeTurn();
                            }
                            duel.Uptick();
                            if (duel.player.alive == false || duel.opponent.alive == false)
                            {
                                if (duel.player.alive == false)
                                {
                                    //player dying code here
                                }
                                if (duel.opponent.alive == false)
                                {
                                    Console.WriteLine("You won! Good job.");
                                    gameInProgress = false;
                                }
                            }
                        }
                        break;
                    case "exit":
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid command. Enter \"help\" for a list of commands");
                        break;
                }
            }
        }
    }

    public class Duel
    {
        public Random rng;
        public Character player;
        public Character opponent;
        List<Spell> stack = new List<Spell>();
        public void Initialize()
        {
            rng = new Random();
            player = new Character();
            opponent = new Character();
            player.initiative = 1000; //rng.Next(1, 11);
            player.mana = 10;
            player.alive = true;
            opponent.initiative = 0; //rng.Next(1, 11);
            opponent.mana = 10;
            opponent.alive = true;
        }
        public void Uptick()
        {
            player.UpdateCharacter();
            opponent.UpdateCharacter();
            foreach(Spell s in stack)
            {
                s.ApplyEffects(this, s.stackCaster, s.stackTarget);
                stack.Remove(s);
            }
        }
        public void Stack(Spell s, Character c, Character t)
        {
            s.stackCaster = c;
            s.stackTarget = t;
            s.duration--;
            stack.Add(s);
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
        public bool fizzle = false;

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
            switch (elem)
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

        public void ApplyEffects(Duel handler, Character s, Character t)
        {
            if (!fizzle)
            {
                for (int i = 0; i < echoes + 1; i++)
                {
                    Character targetActive;
                    Character casterActive = s;
                    string descriptiveText;
                    if (target == TargetType.Opponent)
                    {
                        targetActive = t;
                    }
                    else
                    {
                        targetActive = s;
                    }
                    if (duration > 0)
                    {
                        handler.Stack(this, targetActive, casterActive);
                    }
                    switch (type)
                    {
                        case SpellType.Cloud:
                            targetActive.Damage(potency, damageTypes);
                            break;
                        case SpellType.Nova:
                            targetActive.Damage(potency, damageTypes);
                            break;
                        case SpellType.TargetBolt:
                            targetActive.Damage(potency, damageTypes);
                            break;
                        case SpellType.TargetExplode:
                            targetActive.Damage(potency, damageTypes);
                            break;
                        case SpellType.TargetGas:
                            targetActive.Damage(potency, damageTypes);
                            break;
                        case SpellType.TargetKill:
                            damageTypes.Clear();
                            damageTypes.Add(DamageType.Accursed); //reminder to move this to the switch case for verbs later.
                            targetActive.Damage(potency, damageTypes);
                            break;
                        case SpellType.Enhance:
                            switch (aspectTarget)
                            {
                                case TargetedAspect.ManaRegen:
                                    targetActive.manaRegen = Aspect.Enhanced;
                                    break;
                                case TargetedAspect.Speed:
                                    targetActive.speed = Aspect.Enhanced;
                                    break;
                                case TargetedAspect.Senses:
                                    targetActive.senses = Aspect.Enhanced;
                                    break;
                                case TargetedAspect.None:
                                    return;
                            }
                            break;
                        case SpellType.Diminish:
                            switch (aspectTarget)
                            {
                                case TargetedAspect.ManaRegen:
                                    targetActive.manaRegen = Aspect.Diminish;
                                    break;
                                case TargetedAspect.Speed:
                                    targetActive.speed = Aspect.Diminish;
                                    break;
                                case TargetedAspect.Senses:
                                    targetActive.senses = Aspect.Diminish;
                                    break;
                                case TargetedAspect.None:
                                    return;
                            }
                            break;
                        case SpellType.Shield:
                            foreach (DamageType element in damageTypes)
                            {
                                targetActive.shields[element] += (potency / damageTypes.Count);
                            }
                            break;
                        default:
                            throw new SystemException("Wtf");
                    }
                    //Eventually add mechanics for the mind control spells (caster = opponent)
                }
            }
            else
            {
                Console.WriteLine("The spell fizzles, the words lacking in fluency,");
            }
            /* Debug code
            if (damageTypes.Count == 0)
            {
                damageTypes.Add(DamageType.Force);
            }
            Console.WriteLine("Potency: {0}", potency);
            Console.WriteLine("Range: {0}", range);
            Console.WriteLine("Cost: {0}", cost);
            Console.WriteLine("Time: {0}", time);
            Console.WriteLine("Echoes: {0}", echoes);
            Console.WriteLine("Size: {0}", size);
            Console.WriteLine("Silence: {0}", silence);
            Console.WriteLine("Duration: {0}", duration);
            Console.WriteLine("Targeted Aspect: {0}", aspectTarget);
            Console.WriteLine("Type: {0}", type);
            Console.WriteLine("Target: {0}", target);
            Console.WriteLine("Caster: {0}", caster);
            Console.WriteLine("Fizzle: {0}", fizzle);
            foreach(DamageType damageType in damageTypes)
            {
                Console.WriteLine("Damage Type: {0}", damageType);
            }
            */
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

        public Character()
        {
            shields = new Dictionary<DamageType, int>();
            foreach (var elem in Enum.GetValues(typeof(DamageType)).Cast<DamageType>())
            {
                shields[elem] = 10;
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
            subj = new Subject(split[0]);
            verb = new Verb(split[1]);
            obj = new Object(split[2]);
        }
    }
}
