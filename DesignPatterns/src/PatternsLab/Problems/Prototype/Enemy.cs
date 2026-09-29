namespace PatternsLab.Problems.Prototype;

public class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
}

public abstract class Enemy
{
    private string _modelData;

    public string Name { get; set; }
    public int Health { get; set; }
    public Weapon Weapon { get; set; }
    public List<string> Abilities { get; set; } = new();
    public string ModelId => _modelData;

    protected Enemy()
    {
        Console.WriteLine("   ...loading 3D model (slow)...");
        Thread.Sleep(500);
        _modelData = "MODEL_" + Guid.NewGuid().ToString("N")[..6];
    }
    public abstract Enemy Clone();
}

public class Orc : Enemy
{
    public Orc()
    {
        Name = "Orc";
        Health = 100;
        Weapon = new Weapon { Name = "Axe", Damage = 25 };
        Abilities.Add("Rage");
    }
    public override Enemy Clone()
    {
        var orc = (Orc)MemberwiseClone();
        orc.Weapon = new Weapon()
        {
            Name = orc.Weapon.Name,
            Damage = orc.Weapon.Damage
        };
        orc.Abilities = new List<string>(Abilities);
        return orc;
    }
}

public class Elf : Enemy
{
    public Elf()
    {
        Name = "Elf";
        Health = 70;
        Weapon = new Weapon { Name = "Bow", Damage = 18 };
        Abilities.Add("Stealth");
    }
    public override Enemy Clone()
    {
        var elf = (Elf)MemberwiseClone();
        elf.Weapon = new Weapon()
        {
            Name = elf.Weapon.Name,
            Damage = elf.Weapon.Damage
        };
        elf.Abilities = new List<string>(Abilities);
        return elf;
    }
}

public static class EnemyCopyHelper
{
    public static Enemy CopyEnemy(Enemy e)
    {
        return e.Clone();
    }
}
