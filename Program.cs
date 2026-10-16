// Fonction pour lire un nombre de l'utilisateur et vérifier sa validité
double LireNombre(string message)
{
    Console.Write(message);
    string? saisie = Console.ReadLine();

    double nombre;
    while (!double.TryParse(saisie, out nombre))
    {
        Console.Write("Ce n'est pas un nombre, réessaie : ");
        saisie = Console.ReadLine();
    }

    return nombre;
}

// Fonction pour lire l'opérateur de l'utilisateur et vérifier sa validité
string LireOperateur()
{
    Console.Write("Opérateur (+, -, *, /, %) : ");
    string? operateur = Console.ReadLine()?.Trim();

    while (operateur is not ("+" or "-" or "*" or "/" or "%"))
    {
        Console.Write("Opérateur invalide, réessaie : ");
        operateur = Console.ReadLine()?.Trim();
    }

    return operateur;
}

// Fonction pour demander à l'utilisateur s'il veut recommencer, si oui retourne true, sinon false
bool DemanderRecommencer()
{
    Console.Write("Veux-tu faire un autre calcul ? (o/n) : ");
    string? reponse = Console.ReadLine()?.Trim().ToLower();

    while (reponse is not ("o" or "n"))
    {
        Console.Write("Réponse invalide, tape o ou n : ");
        reponse = Console.ReadLine()?.Trim().ToLower();
    }

    return reponse == "o";
}

// Boucle principale du programme
bool continuer;

var historique = new List<string>();

do
{
    double premier = LireNombre("Entre le premier nombre : ");
    double deuxieme = LireNombre("Entre le deuxième nombre : ");
    string operateur = LireOperateur();

    switch (operateur)
    {
        case "+":
            Console.WriteLine(premier + deuxieme);
            break;
        case "-":
            Console.WriteLine(premier - deuxieme);
            break;
        case "*":
            Console.WriteLine(premier * deuxieme);
            break;
        case "/":
            if (deuxieme == 0)
            {
                Console.WriteLine("Division par zéro impossible");
            }
            else
            {
                Console.WriteLine(premier / deuxieme);
            }
            break;
        case "%":
            if (deuxieme == 0)
            {
                Console.WriteLine("Modulo par zéro impossible");
            }
            else
            {
                Console.WriteLine(premier % deuxieme);
            }
            break;
    }

    historique.Add($"{premier} {operateur} {deuxieme} = {operateur switch
    {
        "+" => premier + deuxieme,
        "-" => premier - deuxieme,
        "*" => premier * deuxieme,
        "/" => deuxieme == 0 ? double.NaN : premier / deuxieme,
        "%" => deuxieme == 0 ? double.NaN : premier % deuxieme,
        _ => double.NaN
    }}");      

    continuer = DemanderRecommencer();   // mise à jour à la fin de chaque tour

} while (continuer);


Console.WriteLine("Historique des calculs :");

foreach (string ligne in historique) {
    Console.WriteLine(ligne);
}










// int a = 5;

// while (a > 0)
// {
//     Console.WriteLine(a);
//     a--;
// }

// int b = 5;
// do
// {
//     Console.WriteLine(b);
//     b++;
// } while (b < 10);