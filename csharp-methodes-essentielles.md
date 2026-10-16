# Référence C# : méthodes et outils indispensables

> Chaque section contient un tableau récapitulatif puis des exemples exécutables.
> Les exemples supposent `using System; using System.Collections.Generic; using System.Linq;` en haut du fichier.

## Sommaire

1. [string et StringBuilder](#1-string-et-stringbuilder)
2. [Types numériques, Math, Convert, Random](#2-types-numériques-math-convert-random)
3. [char](#3-char)
4. [Tableaux (Array)](#4-tableaux-array)
5. [Collections génériques](#5-collections-génériques)
6. [LINQ](#6-linq)
7. [object, réflexion et pattern matching](#7-object-réflexion-et-pattern-matching)
8. [DateTime, TimeSpan, DateOnly, TimeOnly](#8-datetime-timespan-dateonly-timeonly)
9. [Nullable et gestion des null](#9-nullable-et-gestion-des-null)
10. [Fichiers et I/O](#10-fichiers-et-io)
11. [Console](#11-console)
12. [Exceptions](#12-exceptions)
13. [Async, Task et Threads](#13-async-task-et-threads)
14. [Expressions régulières](#14-expressions-régulières)
15. [JSON](#15-json)
16. [Interfaces et délégués](#16-interfaces-et-délégués)
17. [Utilitaires divers](#17-utilitaires-divers)

---

## 1. string et StringBuilder

Les chaînes sont **immuables** : chaque méthode retourne une **nouvelle** chaîne, l'original ne change pas.

### Recherche

| Méthode | Rôle |
|---|---|
| `Contains(x)` | Contient la sous-chaîne ? |
| `StartsWith(x)` / `EndsWith(x)` | Commence / finit par |
| `IndexOf(x)` / `LastIndexOf(x)` | Position (ou -1 si absent) |
| `IndexOfAny(char[])` | Première position d'un des caractères |

```csharp
string s = "Bonjour le monde";

s.Contains("monde");        // true
s.StartsWith("Bon");        // true
s.EndsWith("monde");        // true
s.IndexOf("o");             // 1
s.LastIndexOf("o");         // 13
s.IndexOf("xyz");           // -1
s.Contains("MONDE", StringComparison.OrdinalIgnoreCase); // true
```

### Transformation

| Méthode | Rôle |
|---|---|
| `ToUpper()` / `ToLower()` | Majuscules / minuscules |
| `Trim()` / `TrimStart()` / `TrimEnd()` | Supprime les espaces (ou caractères donnés) |
| `Replace(a, b)` | Remplace toutes les occurrences |
| `Substring(start, len)` | Extrait une portion |
| `Insert(i, x)` / `Remove(i, n)` | Insère / supprime |
| `PadLeft(n, c)` / `PadRight(n, c)` | Complète jusqu'à n caractères |

```csharp
string s = "  Hello World  ";

s.Trim();                    // "Hello World"
s.ToUpper();                 // "  HELLO WORLD  "
s.Replace("World", "C#");    // "  Hello C#  "

string t = "abcdef";
t.Substring(2);              // "cdef"
t.Substring(1, 3);           // "bcd"
t[1..4];                     // "bcd"  (range, C# 8+)
t.Insert(3, "-");            // "abc-def"
t.Remove(2, 2);              // "abef"

"7".PadLeft(3, '0');         // "007"
"7".PadRight(3, '.');        // "7.."
```

### Découpage et assemblage

```csharp
string csv = "pomme,poire,banane";
string[] fruits = csv.Split(',');              // ["pomme", "poire", "banane"]

string.Join(" | ", fruits);                    // "pomme | poire | banane"
string.Concat("a", "b", "c");                  // "abc"

string nom = "Ana"; int age = 25;
$"{nom} a {age} ans";                          // interpolation (recommandé)
string.Format("{0} a {1} ans", nom, age);      // équivalent
$"{3.14159:F2}";                               // "3.14"

// Split avancé
"a, b,, c".Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
// ["a", "b", "c"]
```

### Comparaison et vérification

```csharp
string.Equals("abc", "ABC", StringComparison.OrdinalIgnoreCase); // true
string.Compare("a", "b");          // négatif (a < b)
"abc".CompareTo("abc");            // 0

string.IsNullOrEmpty("");          // true
string.IsNullOrEmpty(null);        // true
string.IsNullOrWhiteSpace("   ");  // true  (le plus utile)
```

### Conversion

```csharp
char[] lettres = "abc".ToCharArray();       // ['a','b','c']
string s2 = new string(lettres);            // "abc"
string inverse = new string("abc".Reverse().ToArray()); // "cba"
```

### StringBuilder (concaténations répétées en boucle)

| Méthode | Rôle |
|---|---|
| `Append(x)` / `AppendLine(x)` | Ajoute |
| `Insert(i, x)` / `Remove(i, n)` / `Replace(a, b)` | Modifie |
| `Clear()` / `ToString()` / `Length` | Vide / convertit / taille |

```csharp
using System.Text;

var sb = new StringBuilder();
for (int i = 1; i <= 3; i++)
    sb.Append(i).Append(", ");
sb.AppendLine("fin");
sb.Replace("fin", "FIN");
Console.WriteLine(sb.ToString());   // "1, 2, 3, FIN"
```

---

## 2. Types numériques, Math, Convert, Random

### Parsing et formatage

| Méthode | Rôle |
|---|---|
| `int.Parse(s)` | Convertit, lève une exception si invalide |
| `int.TryParse(s, out n)` | Convertit sans exception (retourne bool) |
| `int.MaxValue` / `int.MinValue` | Bornes du type |
| `ToString("N2")`, `("C")`, `("P")`, `("D5")` | Formatage |
| `double.IsNaN(x)` / `double.IsInfinity(x)` | Tests spéciaux |

```csharp
int a = int.Parse("42");                 // 42

if (int.TryParse("abc", out int n))
    Console.WriteLine(n);
else
    Console.WriteLine("invalide");       // affiché

double.TryParse("3.14", System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double d); // 3.14

int.MaxValue;                            // 2147483647
(1234567.891).ToString("N2");            // "1,234,567.89" (selon la culture)
(0.256).ToString("P1");                  // "25.6 %"
(42).ToString("D5");                     // "00042"
(255).ToString("X");                     // "FF" (hexadécimal)
```

### Math

| Méthode | Exemple | Résultat |
|---|---|---|
| `Abs` | `Math.Abs(-5)` | `5` |
| `Max` / `Min` | `Math.Max(3, 7)` | `7` |
| `Pow` | `Math.Pow(2, 10)` | `1024` |
| `Sqrt` | `Math.Sqrt(16)` | `4` |
| `Round` | `Math.Round(2.5)` | `2` (arrondi bancaire) |
| `Floor` / `Ceiling` | `Math.Floor(2.9)` / `Math.Ceiling(2.1)` | `2` / `3` |
| `Truncate` | `Math.Truncate(-2.9)` | `-2` |
| `Clamp` | `Math.Clamp(15, 0, 10)` | `10` |
| `Sign` | `Math.Sign(-8)` | `-1` |
| `Log` / `Log10` / `Exp` | `Math.Log10(1000)` | `3` |
| `Sin` / `Cos` / `Tan` | `Math.Sin(Math.PI / 2)` | `1` |

```csharp
Math.Round(2.567, 2);                          // 2.57
Math.Round(2.5, MidpointRounding.AwayFromZero); // 3
Math.PI;                                       // 3.14159...
int.DivRem(17, 5);                             // (3, 2) quotient et reste
```

### Convert

```csharp
Convert.ToInt32("123");           // 123
Convert.ToInt32(3.7);             // 4 (arrondit, contrairement à un cast)
(int)3.7;                         // 3 (tronque)
Convert.ToDouble("2.5");          // dépend de la culture
Convert.ToBoolean("true");        // true
Convert.ToString(255, 2);         // "11111111" (binaire)
Convert.ToBase64String(new byte[] { 1, 2, 3 });   // "AQID"
Convert.FromBase64String("AQID"); // byte[] {1,2,3}
```

### Random

```csharp
var rnd = Random.Shared;          // .NET 6+, thread-safe
rnd.Next();                       // entier >= 0
rnd.Next(10);                     // 0..9
rnd.Next(1, 7);                   // 1..6 (dé)
rnd.NextDouble();                 // 0.0 <= x < 1.0
```

---

## 3. char

```csharp
char c = 'a';

char.IsDigit('7');          // true
char.IsLetter('x');         // true
char.IsLetterOrDigit('_');  // false
char.IsWhiteSpace(' ');     // true
char.IsUpper('A');          // true
char.IsLower('a');          // true
char.IsPunctuation('!');    // true

char.ToUpper('a');          // 'A'
char.ToLower('Z');          // 'z'
char.GetNumericValue('7');  // 7.0
(int)'A';                   // 65 (code ASCII/Unicode)
(char)66;                   // 'B'
c - 'a';                    // 0 (position dans l'alphabet)
```

---

## 4. Tableaux (Array)

```csharp
int[] t = { 5, 2, 9, 1 };
int[] vide = new int[3];            // [0, 0, 0]
int[,] matrice = new int[2, 3];     // tableau 2D
int[][] dentele = new int[3][];     // tableau de tableaux
```

| Méthode | Rôle |
|---|---|
| `Length` / `Rank` | Taille / nombre de dimensions |
| `Array.Sort(t)` | Trie en place |
| `Array.Reverse(t)` | Inverse en place |
| `Array.IndexOf(t, x)` | Position (ou -1) |
| `Array.BinarySearch(t, x)` | Recherche dichotomique (tableau trié) |
| `Array.Copy(src, dst, n)` | Copie |
| `Array.Resize(ref t, n)` | Redimensionne |
| `Array.Fill(t, x)` | Remplit |
| `Array.Clear(t)` | Remet à zéro |
| `Array.Exists` / `Find` / `FindAll` / `FindIndex` | Recherche par condition |
| `Array.TrueForAll` / `ForEach` / `ConvertAll` | Test / action / conversion |
| `Clone()` / `CopyTo()` / `GetLength(dim)` | Copie / taille d'une dimension |

```csharp
int[] t = { 5, 2, 9, 1 };

Array.Sort(t);                          // [1, 2, 5, 9]
Array.Reverse(t);                       // [9, 5, 2, 1]
Array.IndexOf(t, 5);                    // 1

Array.Exists(t, x => x > 8);            // true
Array.Find(t, x => x < 5);              // 2 (premier trouvé)
Array.FindAll(t, x => x % 2 == 1);      // [9, 5, 1]
Array.ForEach(t, x => Console.Write(x + " "));
string[] txt = Array.ConvertAll(t, x => x.ToString());

Array.Resize(ref t, 6);                 // [9,5,2,1,0,0]
Array.Fill(t, 7);                       // [7,7,7,7,7,7]

int[] copie = (int[])t.Clone();         // copie superficielle
int[] copie2 = t[1..4];                 // sous-tableau (range)

int[,] m = { { 1, 2, 3 }, { 4, 5, 6 } };
m.GetLength(0);                         // 2 lignes
m.GetLength(1);                         // 3 colonnes
```

---

## 5. Collections génériques

### List&lt;T&gt;

| Catégorie | Méthodes |
|---|---|
| Ajout | `Add`, `AddRange`, `Insert`, `InsertRange` |
| Suppression | `Remove`, `RemoveAt`, `RemoveAll`, `RemoveRange`, `Clear` |
| Recherche | `Contains`, `IndexOf`, `LastIndexOf`, `Find`, `FindAll`, `FindIndex`, `FindLast`, `Exists`, `TrueForAll` |
| Ordre | `Sort`, `Reverse`, `BinarySearch` |
| Divers | `ToArray`, `GetRange`, `ForEach`, `ConvertAll`, `Count`, `Capacity` |

```csharp
var nums = new List<int> { 4, 8, 15 };

nums.Add(16);                       // [4, 8, 15, 16]
nums.AddRange(new[] { 23, 42 });    // [4, 8, 15, 16, 23, 42]
nums.Insert(0, 1);                  // [1, 4, 8, 15, 16, 23, 42]
nums.Remove(8);                     // retire la valeur 8
nums.RemoveAt(0);                   // retire l'index 0
nums.RemoveAll(x => x > 20);        // retire tout ce qui dépasse 20

nums.Contains(15);                  // true
nums.IndexOf(15);                   // index
nums.Find(x => x > 10);             // 15
nums.FindAll(x => x % 2 == 0);      // liste des pairs
nums.Exists(x => x == 4);           // true

nums.Sort();                        // ordre croissant
nums.Sort((a, b) => b.CompareTo(a)); // ordre décroissant
nums.Reverse();
nums.GetRange(1, 2);                // 2 éléments à partir de l'index 1
nums.Count;                         // nombre d'éléments
```

### Dictionary&lt;K, V&gt;

| Méthode | Rôle |
|---|---|
| `Add(k, v)` | Ajoute (exception si la clé existe) |
| `TryAdd(k, v)` | Ajoute si absente (retourne bool) |
| `d[k] = v` | Ajoute ou remplace |
| `TryGetValue(k, out v)` | Lecture sûre |
| `ContainsKey` / `ContainsValue` | Tests |
| `Remove(k)` / `Clear()` | Suppression |
| `Keys` / `Values` / `Count` | Accès aux clés, valeurs, taille |

```csharp
var ages = new Dictionary<string, int>
{
    ["Ana"] = 25,
    ["Bob"] = 30
};

ages["Chloé"] = 28;                    // ajout
ages["Ana"] = 26;                      // remplacement
ages.TryAdd("Bob", 99);                // false : "Bob" existe déjà

if (ages.TryGetValue("Ana", out int age))
    Console.WriteLine(age);            // 26

ages.ContainsKey("Zoé");               // false
ages.Remove("Bob");

foreach (var (nom, a) in ages)         // déconstruction
    Console.WriteLine($"{nom} : {a}");

// Compter des occurrences
var compteur = new Dictionary<char, int>();
foreach (char c in "banane")
    compteur[c] = compteur.GetValueOrDefault(c) + 1;
// b:1, a:3, n:2
```

### HashSet&lt;T&gt; (éléments uniques, opérations ensemblistes)

```csharp
var a = new HashSet<int> { 1, 2, 3, 4 };
var b = new HashSet<int> { 3, 4, 5 };

a.Add(2);                  // false (déjà présent, pas de doublon)
a.Contains(3);             // true

a.UnionWith(b);            // a = {1,2,3,4,5}
a.IntersectWith(b);        // a = {3,4,5}
a.ExceptWith(new[] { 3 }); // a = {4,5}

a.IsSubsetOf(b);           // true
a.Overlaps(b);             // true
a.SetEquals(b);            // false
```

### Queue&lt;T&gt; (FIFO) et Stack&lt;T&gt; (LIFO)

```csharp
var file = new Queue<string>();
file.Enqueue("A");
file.Enqueue("B");
file.Peek();               // "A" (sans retirer)
file.Dequeue();            // "A"
file.TryDequeue(out var x); // true, x = "B"

var pile = new Stack<int>();
pile.Push(1);
pile.Push(2);
pile.Peek();               // 2
pile.Pop();                // 2
pile.TryPop(out int y);    // true, y = 1
```

### Autres collections utiles

```csharp
// LinkedList : insertions/suppressions rapides aux extrémités
var ll = new LinkedList<int>();
ll.AddLast(2); ll.AddFirst(1); ll.RemoveFirst();

// SortedDictionary / SortedSet / SortedList : triés par clé
var sd = new SortedDictionary<string, int> { ["b"] = 2, ["a"] = 1 };
// itère dans l'ordre : a, b

// PriorityQueue (.NET 6+) : plus petite priorité en premier
var pq = new PriorityQueue<string, int>();
pq.Enqueue("urgent", 1);
pq.Enqueue("normal", 5);
pq.Dequeue();              // "urgent"

// Lecture seule
IReadOnlyList<int> ro = new List<int> { 1, 2 }.AsReadOnly();

// Thread-safe
var cd = new System.Collections.Concurrent.ConcurrentDictionary<string, int>();
cd.AddOrUpdate("k", 1, (key, old) => old + 1);
```

---

## 6. LINQ

Nécessite `using System.Linq;`. Les requêtes sont **paresseuses** (exécutées à l'itération ou au `ToList()`).

Données utilisées dans les exemples :

```csharp
var nums = new List<int> { 5, 3, 8, 1, 9, 2, 8 };

record Personne(string Nom, int Age, string Ville);
var gens = new List<Personne>
{
    new("Ana", 25, "Paris"),
    new("Bob", 30, "Lyon"),
    new("Chloé", 25, "Paris"),
    new("David", 40, "Lyon"),
};
```

### Filtrage et projection

```csharp
nums.Where(x => x > 4);                  // 5, 8, 9, 8
nums.Select(x => x * 2);                 // 10, 6, 16, 2, 18, 4, 16
gens.Select(p => p.Nom);                 // "Ana", "Bob", ...
gens.Where(p => p.Age > 25).Select(p => p.Nom); // "Bob", "David"

var listes = new[] { new[] { 1, 2 }, new[] { 3 } };
listes.SelectMany(l => l);               // 1, 2, 3 (aplatit)

new object[] { 1, "a", 2 }.OfType<int>(); // 1, 2
```

### Tri

```csharp
nums.OrderBy(x => x);                    // 1,2,3,5,8,8,9
nums.OrderByDescending(x => x);          // 9,8,8,5,3,2,1
gens.OrderBy(p => p.Ville).ThenByDescending(p => p.Age);
```

### Agrégation

```csharp
nums.Count();                // 7
nums.Count(x => x > 4);      // 4
nums.Sum();                  // 36
nums.Average();              // 5.14...
nums.Min();                  // 1
nums.Max();                  // 9
gens.MaxBy(p => p.Age);      // David
gens.MinBy(p => p.Age);      // Ana
nums.Aggregate((acc, x) => acc * x);   // produit de tous
```

### Éléments uniques

| Méthode | Si aucun élément | Si plusieurs |
|---|---|---|
| `First()` | exception | retourne le premier |
| `FirstOrDefault()` | `default` (null / 0) | retourne le premier |
| `Single()` | exception | exception |
| `SingleOrDefault()` | `default` | exception |
| `Last()` / `LastOrDefault()` | idem `First` | retourne le dernier |

```csharp
nums.First();                          // 5
nums.First(x => x > 5);                // 8
nums.FirstOrDefault(x => x > 100);     // 0
gens.SingleOrDefault(p => p.Nom == "Ana");
nums.ElementAt(2);                     // 8
nums.ElementAtOrDefault(50);           // 0
```

### Tests

```csharp
nums.Any();                  // true (non vide)
nums.Any(x => x > 8);        // true
nums.All(x => x > 0);        // true
nums.Contains(9);            // true
nums.SequenceEqual(new[] { 5, 3, 8, 1, 9, 2, 8 }); // true
```

### Ensembles

```csharp
nums.Distinct();                         // 5,3,8,1,9,2
gens.DistinctBy(p => p.Ville);           // Ana, Bob
new[] {1,2,3}.Union(new[] {3,4});        // 1,2,3,4
new[] {1,2,3}.Intersect(new[] {2,3,4});  // 2,3
new[] {1,2,3}.Except(new[] {2});         // 1,3
new[] {1,2}.Concat(new[] {3});           // 1,2,3
new[] {1,2}.Zip(new[] {"a","b"});        // (1,"a"), (2,"b")
```

### Découpage et pagination

```csharp
nums.Take(3);                // 5,3,8
nums.Skip(5);                // 2,8
nums.Skip(2).Take(3);        // pagination : 8,1,9
nums.TakeWhile(x => x > 2);  // 5,3,8
nums.SkipWhile(x => x > 2);  // 1,9,2,8
nums.Chunk(3);               // [5,3,8] [1,9,2] [8]
nums.Take(^2..);             // 2 derniers éléments (via range, .NET 6+)
```

### Regroupement et jointure

```csharp
var parVille = gens.GroupBy(p => p.Ville);
foreach (var g in parVille)
    Console.WriteLine($"{g.Key} : {g.Count()} personne(s), âge moyen {g.Average(p => p.Age)}");

// Join
var villes = new[] { new { Nom = "Paris", Pays = "France" }, new { Nom = "Lyon", Pays = "France" } };
var jointure = gens.Join(villes, p => p.Ville, v => v.Nom,
                         (p, v) => new { p.Nom, v.Pays });

// Lookup : dictionnaire clé -> plusieurs valeurs
var lookup = gens.ToLookup(p => p.Ville);
lookup["Paris"];             // Ana, Chloé
```

### Conversion et génération

```csharp
nums.ToList();
nums.ToArray();
nums.ToHashSet();
gens.ToDictionary(p => p.Nom, p => p.Age);   // clé unique obligatoire

Enumerable.Range(1, 5);            // 1,2,3,4,5
Enumerable.Repeat("x", 3);         // "x","x","x"
Enumerable.Empty<int>();           // séquence vide
```

### Syntaxe de requête (alternative)

```csharp
var res = from p in gens
          where p.Age > 25
          orderby p.Nom
          select p.Nom;
```

---

## 7. object, réflexion et pattern matching

### Méthodes héritées par tous les types

```csharp
object o = new Personne("Ana", 25, "Paris");

o.ToString();                    // représentation texte
o.GetType();                     // Type de l'objet
o.GetType().Name;                // "Personne"
o.Equals(autre);                 // égalité de valeur ou de référence
o.GetHashCode();
object.ReferenceEquals(a, b);    // même instance ?
```

### Opérateurs de type

```csharp
object x = "texte";

x is string;                     // true
x is string s;                   // true et s est utilisable
x as string;                     // "texte" ou null (pas d'exception)
(string)x;                       // cast direct (exception si incompatible)
typeof(int);                     // System.Int32
nameof(Personne.Nom);            // "Nom"
default(int);                    // 0
```

### Pattern matching

```csharp
string Decrire(object o) => o switch
{
    null            => "null",
    int n when n < 0 => "entier négatif",
    int n           => $"entier {n}",
    string s        => $"texte de {s.Length} caractères",
    List<int> l     => $"liste de {l.Count}",
    _               => "autre"
};

// Patterns relationnels et logiques
int note = 15;
string mention = note switch
{
    >= 16        => "Très bien",
    >= 14        => "Bien",
    >= 10 and < 14 => "Passable",
    _            => "Insuffisant"
};

// Pattern de propriété
if (o is Personne { Age: >= 18, Ville: "Paris" })
    Console.WriteLine("Adulte parisien");
```

### Réflexion

```csharp
Type t = typeof(Personne);

foreach (var p in t.GetProperties())
    Console.WriteLine($"{p.Name} : {p.PropertyType.Name}");

t.GetMethods();
t.GetFields();
t.GetConstructors();

var obj = Activator.CreateInstance(t, "Bob", 30, "Lyon");
t.GetProperty("Nom")!.GetValue(obj);        // "Bob"
t.GetProperty("Nom")!.SetValue(obj, "Rob"); // si modifiable
typeof(IEnumerable<int>).IsAssignableFrom(typeof(List<int>));  // true
t.GetCustomAttributes(true);
```

---

## 8. DateTime, TimeSpan, DateOnly, TimeOnly

```csharp
DateTime now = DateTime.Now;          // heure locale
DateTime utc = DateTime.UtcNow;       // UTC (à préférer pour stocker)
DateTime today = DateTime.Today;      // aujourd'hui à 00:00

var d = new DateTime(2026, 10, 15, 14, 30, 0);

d.Year; d.Month; d.Day; d.Hour; d.Minute;
d.DayOfWeek;                          // DayOfWeek.Thursday
d.DayOfYear;                          // 288
d.Date;                               // 15/10/2026 00:00
d.TimeOfDay;                          // TimeSpan 14:30:00
```

### Calculs

```csharp
d.AddDays(7);
d.AddMonths(1);
d.AddYears(-1);
d.AddHours(3);
d.AddMinutes(-30);

DateTime.DaysInMonth(2026, 2);        // 28
DateTime.IsLeapYear(2028);            // true

TimeSpan diff = new DateTime(2026, 12, 25) - DateTime.Today;
diff.TotalDays;                       // nombre de jours restants
diff.Days; diff.Hours;                // composantes
```

### Formatage et parsing

```csharp
d.ToString("yyyy-MM-dd");             // "2026-10-15"
d.ToString("dd/MM/yyyy HH:mm");       // "15/10/2026 14:30"
d.ToString("dddd d MMMM yyyy", new System.Globalization.CultureInfo("fr-FR"));
// "jeudi 15 octobre 2026"

DateTime.Parse("2026-10-15");
DateTime.TryParse("pas une date", out var res);   // false
DateTime.ParseExact("15/10/2026", "dd/MM/yyyy",
    System.Globalization.CultureInfo.InvariantCulture);
```

### TimeSpan

```csharp
var ts = TimeSpan.FromMinutes(90);
ts.TotalHours;                        // 1.5
TimeSpan.FromSeconds(30);
ts.Add(TimeSpan.FromMinutes(10));
ts.ToString(@"hh\:mm\:ss");           // "01:40:00"
```

### DateOnly, TimeOnly, DateTimeOffset, fuseaux

```csharp
DateOnly jour = new DateOnly(2026, 10, 15);   // .NET 6+
jour.AddDays(1);
TimeOnly heure = new TimeOnly(14, 30);

DateTimeOffset dto = DateTimeOffset.Now;      // date + décalage UTC
var fuseau = TimeZoneInfo.FindSystemTimeZoneById("Africa/Lagos");
TimeZoneInfo.ConvertTime(DateTime.UtcNow, fuseau);
```

---

## 9. Nullable et gestion des null

```csharp
int? n = null;                  // Nullable<int>
n.HasValue;                     // false
n.GetValueOrDefault();          // 0
n.GetValueOrDefault(-1);        // -1
n = 5;
n.Value;                        // 5 (exception si null)
```

| Opérateur | Rôle | Exemple |
|---|---|---|
| `?.` | Accès conditionnel | `personne?.Nom` |
| `??` | Valeur par défaut | `nom ?? "inconnu"` |
| `??=` | Affecte si null | `cache ??= new List<int>()` |
| `!` | Dit au compilateur « non null » | `valeur!.Length` |

```csharp
string? nom = null;

int? longueur = nom?.Length;            // null (pas d'exception)
string affiche = nom ?? "inconnu";      // "inconnu"
nom ??= "par défaut";                   // nom = "par défaut"

List<int>? liste = null;
int premier = liste?[0] ?? -1;          // -1
int total = liste?.Count ?? 0;          // 0

// Validation d'arguments (.NET 6+)
void Salut(string prenom)
{
    ArgumentNullException.ThrowIfNull(prenom);
    ArgumentException.ThrowIfNullOrWhiteSpace(prenom);
}
```

---

## 10. Fichiers et I/O

Nécessite `using System.IO;`.

### File

```csharp
string chemin = "donnees.txt";

File.WriteAllText(chemin, "Bonjour\nMonde");
File.AppendAllText(chemin, "\nLigne ajoutée");
File.WriteAllLines(chemin, new[] { "a", "b", "c" });

string contenu = File.ReadAllText(chemin);
string[] lignes = File.ReadAllLines(chemin);
byte[] octets = File.ReadAllBytes(chemin);

File.Exists(chemin);                    // true / false
File.Copy(chemin, "copie.txt", overwrite: true);
File.Move("copie.txt", "deplace.txt");
File.Delete("deplace.txt");

foreach (string ligne in File.ReadLines(chemin))   // lecture paresseuse (gros fichiers)
    Console.WriteLine(ligne);
```

### Directory

```csharp
Directory.CreateDirectory("dossier/sous");     // crée aussi les parents
Directory.Exists("dossier");
string[] fichiers = Directory.GetFiles("dossier", "*.txt");
string[] dossiers = Directory.GetDirectories("dossier");
foreach (var f in Directory.EnumerateFiles(".", "*.cs", SearchOption.AllDirectories))
    Console.WriteLine(f);
Directory.Delete("dossier", recursive: true);
```

### Path

```csharp
Path.Combine("dossier", "sous", "fichier.txt");     // "dossier/sous/fichier.txt"
Path.GetFileName("/a/b/rapport.pdf");               // "rapport.pdf"
Path.GetFileNameWithoutExtension("/a/b/rapport.pdf"); // "rapport"
Path.GetExtension("/a/b/rapport.pdf");              // ".pdf"
Path.GetDirectoryName("/a/b/rapport.pdf");          // "/a/b"
Path.GetFullPath("fichier.txt");
Path.GetTempPath();
Path.ChangeExtension("a.txt", ".md");               // "a.md"
```

### Streams (lecture/écriture ligne par ligne)

```csharp
using (var writer = new StreamWriter("log.txt", append: true))
{
    writer.WriteLine("Une ligne");
    writer.Write("Sans retour à la ligne");
}

using var reader = new StreamReader("log.txt");   // using déclaratif (C# 8+)
string? ligne;
while ((ligne = reader.ReadLine()) != null)
    Console.WriteLine(ligne);

string tout = reader.ReadToEnd();

// En mémoire
using var ms = new MemoryStream();
ms.Write(new byte[] { 1, 2, 3 });
```

### Version asynchrone (recommandée)

```csharp
string txt = await File.ReadAllTextAsync("donnees.txt");
await File.WriteAllTextAsync("sortie.txt", txt.ToUpper());
```

---

## 11. Console

```csharp
Console.WriteLine("Avec retour à la ligne");
Console.Write("Sans retour");
Console.WriteLine($"Total : {12.5:F2}");

string? saisie = Console.ReadLine();            // lit une ligne
ConsoleKeyInfo touche = Console.ReadKey();      // lit une touche
Console.ReadKey(intercept: true);               // sans l'afficher

Console.Clear();
Console.Title = "Mon application";
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("En vert");
Console.ResetColor();

// Saisie sécurisée d'un entier
Console.Write("Âge : ");
while (!int.TryParse(Console.ReadLine(), out int age))
    Console.Write("Invalide, recommence : ");

Console.Error.WriteLine("Message d'erreur");     // flux d'erreur
```

---

## 12. Exceptions

```csharp
try
{
    int x = int.Parse("abc");
}
catch (FormatException ex)
{
    Console.WriteLine($"Format invalide : {ex.Message}");
}
catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
{
    Console.WriteLine("Argument ou opération invalide");
}
finally
{
    Console.WriteLine("Toujours exécuté");
}
```

### Lever une exception

```csharp
void Retirer(decimal solde, decimal montant)
{
    if (montant <= 0)
        throw new ArgumentOutOfRangeException(nameof(montant), "Doit être positif");
    if (montant > solde)
        throw new InvalidOperationException("Solde insuffisant");
}

// Relancer en conservant la pile d'appels
try { /* ... */ }
catch (Exception) { throw; }                 // et non "throw ex;"

// Envelopper une exception
catch (Exception ex) { throw new ApplicationException("Échec traitement", ex); }
```

### Propriétés utiles

| Propriété | Contenu |
|---|---|
| `Message` | Description de l'erreur |
| `StackTrace` | Pile d'appels |
| `InnerException` | Exception d'origine |
| `Data` | Dictionnaire d'infos supplémentaires |

### Exceptions courantes

| Exception | Cas typique |
|---|---|
| `ArgumentException` / `ArgumentNullException` | Argument invalide / null |
| `InvalidOperationException` | État de l'objet incorrect |
| `NullReferenceException` | Accès à un membre d'un objet null |
| `IndexOutOfRangeException` | Index hors du tableau |
| `KeyNotFoundException` | Clé absente d'un `Dictionary` |
| `FormatException` | `Parse` sur un texte invalide |
| `DivideByZeroException` | Division entière par zéro |
| `FileNotFoundException` / `IOException` | Problème de fichier |
| `NotImplementedException` | Code pas encore écrit |
| `OperationCanceledException` | Tâche annulée |

### Exception personnalisée

```csharp
public class SoldeInsuffisantException : Exception
{
    public decimal Manque { get; }
    public SoldeInsuffisantException(decimal manque)
        : base($"Il manque {manque} €") => Manque = manque;
}
```

---

## 13. Async, Task et Threads

### async / await

```csharp
async Task<string> TelechargerAsync(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url);
}

async Task Main()
{
    string html = await TelechargerAsync("https://example.com");
    Console.WriteLine(html.Length);
}
```

### Task : méthodes statiques

```csharp
await Task.Delay(1000);                          // attente non bloquante
var t = Task.Run(() => CalculLourd());           // exécute sur un thread du pool
int r = await Task.FromResult(42);               // tâche déjà terminée

// Plusieurs tâches en parallèle
var t1 = TelechargerAsync("https://a.com");
var t2 = TelechargerAsync("https://b.com");
string[] resultats = await Task.WhenAll(t1, t2);  // attend toutes
Task premiere = await Task.WhenAny(t1, t2);      // attend la première

int CalculLourd() => Enumerable.Range(1, 1000).Sum();
```

### Annulation

```csharp
var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(5));

async Task Travail(CancellationToken ct)
{
    for (int i = 0; i < 100; i++)
    {
        ct.ThrowIfCancellationRequested();
        await Task.Delay(100, ct);
    }
}

try { await Travail(cts.Token); }
catch (OperationCanceledException) { Console.WriteLine("Annulé"); }
```

### Parallel

```csharp
Parallel.For(0, 10, i => Console.WriteLine(i));
Parallel.ForEach(new[] { 1, 2, 3 }, x => Console.WriteLine(x * x));
await Parallel.ForEachAsync(urls, async (url, ct) => await TelechargerAsync(url));
```

### Synchronisation

```csharp
private static readonly object verrou = new();
int compteur = 0;

lock (verrou)                       // une seule thread à la fois
{
    compteur++;
}

Interlocked.Increment(ref compteur);        // incrément atomique, sans lock

var sem = new SemaphoreSlim(3);             // max 3 accès simultanés
await sem.WaitAsync();
try { /* section limitée */ }
finally { sem.Release(); }

var th = new Thread(() => Console.WriteLine("Thread manuel"));
th.Start();
th.Join();
```

---

## 14. Expressions régulières

Nécessite `using System.Text.RegularExpressions;`.

```csharp
string texte = "Contact : ana@mail.com, bob@site.org";

// Test simple
Regex.IsMatch("12345", @"^\d+$");                          // true

// Première correspondance
Match m = Regex.Match(texte, @"[\w.]+@[\w.]+");
if (m.Success)
    Console.WriteLine(m.Value);                            // "ana@mail.com"

// Toutes les correspondances
foreach (Match mm in Regex.Matches(texte, @"[\w.]+@[\w.]+"))
    Console.WriteLine(mm.Value);

// Groupes nommés
var match = Regex.Match("2026-10-15", @"(?<an>\d{4})-(?<mois>\d{2})-(?<jour>\d{2})");
match.Groups["an"].Value;                                  // "2026"
match.Groups["mois"].Value;                                // "10"

// Remplacement et découpage
Regex.Replace("a  b   c", @"\s+", " ");                    // "a b c"
Regex.Split("un1deux22trois", @"\d+");                     // ["un","deux","trois"]
Regex.Escape("a.b*c");                                     // échappe les caractères spéciaux

// Regex compilée (performances), .NET 7+ : [GeneratedRegex]
var re = new Regex(@"\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
```

| Motif | Signification |
|---|---|
| `\d` / `\w` / `\s` | chiffre / lettre-chiffre-_ / espace |
| `.` | n'importe quel caractère |
| `*` `+` `?` | 0 ou plus / 1 ou plus / 0 ou 1 |
| `{n,m}` | entre n et m répétitions |
| `^` `$` | début / fin |
| `[abc]` `[^abc]` | l'un de / aucun de |
| `(...)` | groupe de capture |

---

## 15. JSON

### System.Text.Json (intégré, recommandé)

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

public class Produit
{
    public string Nom { get; set; } = "";
    public decimal Prix { get; set; }
    [JsonPropertyName("en_stock")] public bool EnStock { get; set; }
    [JsonIgnore] public string Secret { get; set; } = "";
}

var p = new Produit { Nom = "Stylo", Prix = 1.5m, EnStock = true };

// Objet -> JSON
string json = JsonSerializer.Serialize(p);
// {"Nom":"Stylo","Prix":1.5,"en_stock":true}

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true
};
string joli = JsonSerializer.Serialize(p, options);

// JSON -> objet
Produit? p2 = JsonSerializer.Deserialize<Produit>(json);
List<Produit>? liste = JsonSerializer.Deserialize<List<Produit>>("[{\"Nom\":\"A\"}]");

// Lecture dynamique
using JsonDocument doc = JsonDocument.Parse(json);
string nom = doc.RootElement.GetProperty("Nom").GetString()!;
decimal prix = doc.RootElement.GetProperty("Prix").GetDecimal();

// Fichiers (asynchrone)
await File.WriteAllTextAsync("p.json", joli);
using var fs = File.OpenRead("p.json");
var depuisFichier = await JsonSerializer.DeserializeAsync<Produit>(fs);
```

### Newtonsoft.Json (package NuGet `Newtonsoft.Json`)

```csharp
using Newtonsoft.Json;

string json = JsonConvert.SerializeObject(p, Formatting.Indented);
Produit? p2 = JsonConvert.DeserializeObject<Produit>(json);
```

---

## 16. Interfaces et délégués

### Interfaces essentielles

| Interface | Rôle | Membre clé |
|---|---|---|
| `IEnumerable<T>` | Peut être parcouru avec `foreach` | `GetEnumerator()` |
| `ICollection<T>` | Collection avec `Count`, `Add`, `Remove` | `Count` |
| `IList<T>` | Collection indexée | `this[int]` |
| `IDictionary<K,V>` | Collection clé/valeur | `this[K]` |
| `IComparable<T>` | Définit un ordre naturel | `CompareTo(T)` |
| `IComparer<T>` | Comparateur externe | `Compare(a, b)` |
| `IEquatable<T>` | Égalité typée | `Equals(T)` |
| `IDisposable` | Libération de ressources | `Dispose()` |

```csharp
public class Etudiant : IComparable<Etudiant>
{
    public string Nom { get; set; } = "";
    public int Note { get; set; }
    public int CompareTo(Etudiant? autre) => Note.CompareTo(autre?.Note ?? 0);
}

var liste = new List<Etudiant> { new() { Nom = "A", Note = 12 }, new() { Nom = "B", Note = 15 } };
liste.Sort();   // utilise CompareTo

public class Connexion : IDisposable
{
    public void Dispose() => Console.WriteLine("Connexion fermée");
}
using (var c = new Connexion()) { /* ... */ }   // Dispose() appelé automatiquement
```

### Délégués : Action, Func, Predicate

```csharp
Action<string> afficher = msg => Console.WriteLine(msg);     // aucun retour
afficher("Salut");

Func<int, int, int> additionner = (a, b) => a + b;           // dernier type = retour
additionner(2, 3);                                           // 5

Predicate<int> estPair = x => x % 2 == 0;                    // retourne bool
estPair(4);                                                  // true

Comparison<string> parLongueur = (a, b) => a.Length.CompareTo(b.Length);

// Événements
public class Minuteur
{
    public event EventHandler? Termine;
    public void Demarrer() => Termine?.Invoke(this, EventArgs.Empty);
}
var m = new Minuteur();
m.Termine += (s, e) => Console.WriteLine("Terminé !");
m.Demarrer();
```

### yield return

```csharp
IEnumerable<int> Carres(int n)
{
    for (int i = 1; i <= n; i++)
        yield return i * i;      // produit les valeurs une par une, paresseusement
}
Carres(4);   // 1, 4, 9, 16
```

### Mots-clés de conception à connaître

```csharp
// record : type immuable avec égalité par valeur
public record Point(int X, int Y);
var p1 = new Point(1, 2);
var p2 = p1 with { X = 5 };          // copie modifiée

// init et required
public class Config
{
    public required string Nom { get; init; }   // à fournir, non modifiable ensuite
}

// abstract / virtual / override / sealed
public abstract class Forme { public abstract double Aire(); public virtual string Nom => "Forme"; }
public class Cercle : Forme
{
    public double R { get; init; }
    public override double Aire() => Math.PI * R * R;
    public override string Nom => "Cercle";
}
public sealed class Final { }        // non héritable

// static, readonly, const
public static class Outils { public const double Pi2 = Math.PI * 2; }
```

---

## 17. Utilitaires divers

### Environment, GC

```csharp
Environment.NewLine;
Environment.MachineName;
Environment.UserName;
Environment.CurrentDirectory;
Environment.GetEnvironmentVariable("PATH");
Environment.Exit(0);
Environment.ProcessorCount;
GC.Collect();                         // à éviter en général
GC.GetTotalMemory(false);
```

### Guid

```csharp
Guid id = Guid.NewGuid();             // identifiant unique
id.ToString();                        // "3f2504e0-4f89-11d3-9a0c-0305e82c3301"
Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301");
Guid.TryParse("pas un guid", out _);  // false
Guid.Empty;                           // 000...000
```

### Stopwatch (mesure de performance)

```csharp
using System.Diagnostics;

var sw = Stopwatch.StartNew();
// ... code à mesurer ...
sw.Stop();
Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
sw.Restart();
sw.Elapsed;                           // TimeSpan
```

### Tuples

```csharp
(int Min, int Max) MinMax(int[] t) => (t.Min(), t.Max());

var (min, max) = MinMax(new[] { 4, 1, 9 });   // déconstruction
var r = MinMax(new[] { 4, 1, 9 });
r.Min; r.Max;

(string nom, int age) personne = ("Ana", 25);
personne.nom;
```

### Span, Memory, ArraySegment

```csharp
int[] t = { 1, 2, 3, 4, 5 };
Span<int> tranche = t.AsSpan(1, 3);   // vue sur [2,3,4] sans copie
tranche[0] = 99;                      // modifie t

ReadOnlySpan<char> mot = "Bonjour".AsSpan(0, 3);   // "Bon" sans allocation
```

### HttpClient

```csharp
using var client = new HttpClient();

string txt = await client.GetStringAsync("https://api.example.com/data");

HttpResponseMessage rep = await client.GetAsync("https://api.example.com/data");
rep.EnsureSuccessStatusCode();
string corps = await rep.Content.ReadAsStringAsync();

var contenu = new StringContent("{\"nom\":\"Ana\"}", System.Text.Encoding.UTF8, "application/json");
HttpResponseMessage post = await client.PostAsync("https://api.example.com/users", contenu);

// Avec System.Net.Http.Json
var obj = await client.GetFromJsonAsync<Produit>("https://api.example.com/p/1");
await client.PostAsJsonAsync("https://api.example.com/p", obj);
```

> Réutilise une seule instance de `HttpClient` (ou `IHttpClientFactory`) plutôt que d'en créer une à chaque requête.

### Encodage, octets, BitConverter

```csharp
using System.Text;

byte[] octets = Encoding.UTF8.GetBytes("Héllo");
string texte = Encoding.UTF8.GetString(octets);
BitConverter.GetBytes(258);                   // {2, 1, 0, 0}
BitConverter.ToInt32(new byte[] { 2, 1, 0, 0 }); // 258
Convert.ToHexString(octets);                  // "48C3A96C6C6F"
```

### Enum

```csharp
enum Jour { Lundi, Mardi, Mercredi }

Jour j = Enum.Parse<Jour>("Mardi");
Enum.TryParse("Samedi", out Jour autre);                 // false
Enum.GetValues<Jour>();                                  // Lundi, Mardi, Mercredi
Enum.GetNames<Jour>();                                   // "Lundi", ...
Enum.IsDefined(typeof(Jour), 1);                         // true
j.ToString();                                            // "Mardi"
(int)j;                                                  // 1

[Flags] enum Droits { Aucun = 0, Lire = 1, Ecrire = 2, Tout = Lire | Ecrire }
Droits d = Droits.Lire | Droits.Ecrire;
d.HasFlag(Droits.Lire);                                  // true
```

### Autres

```csharp
// Lazy : initialisation à la première utilisation
var lazy = new Lazy<List<int>>(() => new List<int> { 1, 2, 3 });
lazy.Value;

// Uri
var uri = new Uri("https://exemple.com:8080/chemin?x=1#frag");
uri.Host; uri.Port; uri.AbsolutePath; uri.Query;

// Version
Version.Parse("1.2.3").Major;                            // 1

// Opérateurs d'index et de plage (C# 8+)
var arr = new[] { 10, 20, 30, 40, 50 };
arr[^1];                                                 // 50 (dernier)
arr[1..3];                                               // [20, 30]
arr[..2];                                                // [10, 20]

// Expressions switch, using déclaratif, chaînes brutes (C# 11)
string brut = """
    Texte sur plusieurs lignes
    avec des "guillemets" sans échappement
    """;
```

---

## Pour aller plus loin

- Documentation officielle : <https://learn.microsoft.com/dotnet/api>
- Guide du langage : <https://learn.microsoft.com/dotnet/csharp>
- Astuce : dans Visual Studio / Rider, tape le nom d'un objet suivi de `.` pour voir toutes ses méthodes disponibles.
