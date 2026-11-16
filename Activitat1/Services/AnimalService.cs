using Activitat1.Data;
using Activitat1.Models;

namespace csharp1.Services;

public class AnimalService
{
    private RefugiContext ctx;

    public AnimalService(RefugiContext context)
    {
        ctx = context;
    }
    
    public List<Animal> GetDisponibles(Especie especie)
    {
        return ctx.Animals
            .Where(a => a.Especie == especie && !a.EstaAdoptat)
            .OrderBy(a => a.DataEntrada)
            .ToList();
    }

    public List<Animal> GetTots() => ctx.Animals.ToList();

    public List<Animal>? Descendents(string codiXip)
    {
        Animal? animal = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);
        if (animal == null)
        {
            Console.WriteLine($"ERROR: No existeix animal amb el xip {codiXip}");
            return null;
        }

        List<Animal> descendents = new List<Animal>();

        AfegirDescendents(descendents, animal);
        
        return descendents;
    }

    private void AfegirDescendents(List<Animal> descendents, Animal animal)
    {
        foreach (Animal d in animal.Cries)
        {
            descendents.Add(d);
            AfegirDescendents(descendents, d);
        }
    }
    
    public Animal? CercaPerXip(string codiXip)
    {
        Animal? animal = ctx.Animals
            .FirstOrDefault(a => a.Xip.Codi == codiXip);

        if (animal == null)
        {
            Console.WriteLine("No existeix animal amb aquest xip");
        }

        return animal;
    }

    public bool CanviarMare(string codiXipCria, string? codiXipMare)
    {
        Animal? cria = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXipCria);
        if (cria == null)
        {
            Console.WriteLine($"ERROR: No existeix cria amb el xip {codiXipCria}");
            return false;
        }

        if (codiXipMare == null)
        {
            cria.Mare = null;
            ctx.SaveChanges();
        
            Console.WriteLine($"{cria.Nom} ja no té mare assignada");
            return true;
        }
        
        Animal? mare = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXipMare);
        if (mare == null)
        {
            Console.WriteLine($"ERROR: No existeix mare amb el xip {codiXipMare}");
            return false;
        }

        if (!mare.EsFemella)
        {
            Console.WriteLine($"ERROR: {mare.Nom} no es femella");    
            return false;
        }

        if (cria.MareID != null && cria.MareID == mare.AnimalID)
        {
            Console.WriteLine($"ERROR: {mare.Nom} ja está assignada com a mare de {cria.Nom}");    
            return false;
        }

        if(cria.Mare != null) cria.Mare.Cries.Remove(cria);
        cria.Mare = mare;
        cria.MareID = mare.AnimalID;
        mare.Cries.Add(cria);

        ctx.SaveChanges();
        
        Console.WriteLine($"Ara {mare.Nom} es la mare de {cria.Nom}");
        
        return true;
    }

    public bool RegistrarCria(string codiXipMare, Animal cria)
    {
        Animal? mare = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXipMare);
        if (mare == null)
        {
            Console.WriteLine($"ERROR: No existeix mare amb el xip {codiXipMare}");
            return false;
        }
        if (!mare.EsFemella)
        {
            Console.WriteLine($"ERROR: {mare.Nom} no es femella");    
            return false;
        }

        if (cria.Especie != mare.Especie)
        {
            Console.WriteLine($"ERROR: {cria.Nom} es {cria.Especie} i {mare.Nom} es {mare.Especie}, han de ser de la mateixa especie");
            return false;
        }
        if (cria.DataNaixement <= mare.DataNaixement)
        {
            Console.WriteLine($"ERROR: {cria.Nom} no pot ser major a la mare {mare.Nom}");
            return false;
        }
        
        mare.Cries.Add(cria);
        cria.Mare = mare;
        cria.MareID = mare.AnimalID;

        ctx.SaveChanges();
        
        Console.WriteLine($"{cria.Nom} registrat com cria de {mare.Nom}");
        
        return true;
    }
}