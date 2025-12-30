// Erstelle ein einfaches Personal verwaltungsprogramm 
// Dieses soll uns erlauben eine Kartei über einen oder mehrere Mittarbeiter zu erstellen und auszugeben und auch einzelne Mittarbeiter wieder zu entfernen
// mit folgenden Informationen: Name - Nachname- Funktion des Mittarbeiters und Urlaubstage - Liste(mit Datum von Krankheitstagen)
// Sorge dafür das die Personen gespeichert werden in einer json-datei
// und auch wieder abgerufen werden können automatisch beim start des Programms
// Recherchiere was ein Dictonary ist
// Informiere dich über das Thema serialisierung unter c# 

//Klasse Mitarbeiter mit den Eigenschaften Name, Nachname, Funktion, Urlaubstage (int), Krankheitstage (Liste mit Datum)
//Methode Menü mit den Optionen : Mitarbeiter hinzufügen, Alle Mitarbeiter anzeigen ,Mitarbeiter entfernen, Krankheitstag hinzufügen
//Liste Mitarbeiter um die Mitarbeiter zu speichern
//Methode um neue Mitarbeiter in die Liste hinzuzufügen
//Methode um Mitarbeiter aus der Liste zu entfernen
//Methode um alle Mitarbeiter anzuzeigen
//Methode um Krankheitstage zu einem Mitarbeiter hinzuzufügen
//Methode um die Mitarbeiter aus der Liste einzeln auszulesen und diese in einer json-datei zu speichern
//Methode um die Mitarbeiter aus der json-datei zu laden und in die Liste zu speichern


using Personalverwaltung;
using System.Text.Json;

List<Mitarbeiter> mitarbeiter = new List<Mitarbeiter>();
LoadMitarbeiter();
mitarbeiter = mitarbeiter.OrderBy(x => x.Nachname).ToList();
Menu();
void Menu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Willkommen in der Personalverwaltung!\n\n");
        Console.WriteLine("Bitte wählen Sie eine Option:\n");
        Console.WriteLine("1. Mitarbeiter hinzufügen");
        Console.WriteLine("2. Alle Mitarbeiter anzeigen");
        Console.WriteLine("3. Mitarbeiter entfernen");
        Console.WriteLine("4. Krankheitstag hinzufügen");
        var input = Console.ReadKey();
        Console.Clear();
        switch (input.KeyChar)
        {
            case '1':
                AddMitarbeiter();
                break;
            case '2':
                ShowAllMitarbeiter();
                break;
            case '3':
                RemoveMitarbeiter();
                break;
            case '4':
                AddKrankheitstag();
                break;        
            default:
                Console.WriteLine("\nUngültige Eingabe, bitte versuchen Sie es erneut.\n");
                break;
        }
        SaveMitarbeiter();
        Console.ReadKey();
    }
}

void AddMitarbeiter()
{
    Console.WriteLine("Geben Sie den Namen des Mitarbeiters ein:");
    var name = Console.ReadLine() ?? "";
    Console.WriteLine("Geben Sie den Nachnamen des Mitarbeiters ein:");
    var nachname = Console.ReadLine() ?? "";
    Console.WriteLine("Geben Sie die Funktion des Mitarbeiters ein:");
    var funktion = Console.ReadLine() ?? "";

    if (name == "" || nachname == "" || funktion == "") 
    {
        Console.WriteLine("Fehlerhafte Eingabe, Mitarbeiter wird nicht erstellt!");
        return;
    } 
    
    var neuerMitarbeiter = new Mitarbeiter(name, nachname, funktion);
    mitarbeiter.Add(neuerMitarbeiter);
    mitarbeiter = mitarbeiter.OrderBy(x => x.Nachname).ToList();
    Console.WriteLine($"\nMitarbeiter {name} {nachname} wurde hinzugefügt.\n");
}

void ShowAllMitarbeiter()
{
    Console.Clear();
    Console.WriteLine("Alle Mitarbeiter:\n\n");
       
    if(mitarbeiter.Count() == 0) {         
        Console.WriteLine("Keine Mitarbeiter vorhanden.\n");
        return;
    }

    Console.WriteLine("Nr. |\t Vorname |\t Nachname |\t Funktion |\t Krankheitstage |\t Urlaubstage");
    for (int i = 0; i < mitarbeiter.Count(); i++)
    {
        Console.WriteLine($"{i} \t {mitarbeiter[i].Name} \t {mitarbeiter[i].Nachname} \t  {mitarbeiter[i].Funktion} \t  {mitarbeiter[i].Krankheitstage.Count()} \t\t\t  {mitarbeiter[i].Urlaubstage}");
    }

}

void RemoveMitarbeiter()
{
    Console.Clear();
    Console.WriteLine("Mitarbeiter entfernen:\n\n");
    ShowAllMitarbeiter();

    Console.Write("\nBitte geben Sie die Nummer des Mitarbeiters ein, den Sie entfernen möchten: ");
    var input = Console.ReadLine();

    try
    {
        mitarbeiter.RemoveAt(int.Parse(input ?? ""));
        Console.WriteLine("Mitarbeiter wurde erfolgreich entfernt.\n");
    }
    catch(ArgumentOutOfRangeException)
    {
        Console.WriteLine("Mitarbeiter mit dieser Nummer existiert nicht, bitte versuchen Sie es erneut.\n");
        return;
    }
    catch (FormatException) 
    {
        Console.WriteLine("Fehlerhafte Eingabe, bitte versuchen Sie es erneut.\n");
        return;
    }
    catch(Exception error)
    {
        Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {error.Message}\n");
        return;
    }

}

void AddKrankheitstag()
{
    Console.WriteLine("Krankheitstag hinzufügen:\n\n");
    ShowAllMitarbeiter();
    Console.Write("\nBitte geben Sie die Nummer des Mitarbeiters ein, dem Sie einen Krankheitstag hinzufügen möchten: ");
    var input = Console.ReadLine();
    try
    {
        var mitarb = mitarbeiter[int.Parse(input ?? "")];
        Console.Write("Bitte geben Sie das Datum des Krankheitstages im Format TT.MM.JJJJ ein: ");
        var datumInput = Console.ReadLine() ?? "";
        var datum = DateOnly.ParseExact(datumInput, "dd.MM.yyyy");
        mitarb.Krankheitstage.Add(datum);
        Console.WriteLine("Krankheitstag wurde erfolgreich hinzugefügt.\n");
    }
    catch (ArgumentOutOfRangeException)
    {
        Console.WriteLine("Mitarbeiter mit dieser Nummer existiert nicht, bitte versuchen Sie es erneut.\n");
        return;
    }
    catch (FormatException)
    {
        Console.WriteLine("Fehlerhafte Eingabe, bitte versuchen Sie es erneut.\n");
        return;
    }
    catch (Exception error)
    {
        Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {error.Message}\n");
        return;
    }
}

void SaveMitarbeiter()
{
    var json = JsonSerializer.Serialize(mitarbeiter, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText("mitarbeiter.json", json);
}

void LoadMitarbeiter()
{
    try
    {
        if (File.Exists("mitarbeiter.json"))
        {
            var json = File.ReadAllText("mitarbeiter.json");
            mitarbeiter = JsonSerializer.Deserialize<List<Mitarbeiter>>(json) ?? new List<Mitarbeiter>();
        }
    }
    catch (Exception error)
    {
        Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {error.Message}\n");
        mitarbeiter = new List<Mitarbeiter>();
    }
}