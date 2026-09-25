using System.Diagnostics;
using PatternsLab.Problems.Builder;
using PatternsLab.Problems.Prototype;
using PatternsLab.Problems.Singleton;

Console.WriteLine("=== SINGLETON: AFTER ===\n");
var db = new DatabaseService();
var ui = new UiService();
Console.WriteLine();
db.Config.Theme = "Dark";
Console.WriteLine("Admin changed theme to Dark.");
ui.Render();
Console.WriteLine($"\nSame config object? {ReferenceEquals(db.Config, ui.Config)}");
Console.WriteLine($"Times config was loaded from disk: {AppConfig.LoadCount}");






Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("\n=== PROTOTYPE: AFTER ===\n");
var army = new List<Enemy>();
var orc = new Orc();
var sw = Stopwatch.StartNew();
for (int i = 1; i <= 5; i++)
{
    var newOrc = orc.Clone();
    newOrc.Name = $"Orc-{i}";
    army.Add(newOrc);
}
sw.Stop();
Console.WriteLine($"Cloned 5 orcs in {sw.ElapsedMilliseconds} ms\n");
Enemy original = new Orc();
original.Name = "Boss Orc";
Enemy copy = EnemyCopyHelper.CopyEnemy(original);
Console.WriteLine($"\nOriginal model id: {original.ModelId}");
Console.WriteLine($"Copy model id:     {copy.ModelId}");
copy.Weapon.Damage = 999;
Console.WriteLine($"\nWe changed the COPY's weapon damage to 999.");
Console.WriteLine($"Original's weapon damage is now: {original.Weapon.Damage}");
copy.Abilities.Add("Fire Breath");
Console.WriteLine($"Original abilities: {string.Join(", ", original.Abilities)}");





Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("=== BUILDER: AFTER ===");
Console.WriteLine(RegistrationCallSites.CreateLiveStudentUgly());
Console.WriteLine(RegistrationCallSites.CreateVideosOnlyUgly());