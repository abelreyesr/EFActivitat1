using System.Runtime.InteropServices.JavaScript;
using Activitat1.Data;
using Activitat1.Models;

namespace csharp1.Services;

public class AdoptantService
{
    private RefugiContext ctx;

    public AdoptantService(RefugiContext context)
    {
        ctx = context;
    }

    public bool Adoptar(string dni, string codiXip)
    {
        Adoptant? adoptant = ctx.Adoptants.FirstOrDefault(a => a.DNI == dni);
        
        if (adoptant == null)
        {
            Console.WriteLine($"ERROR: No existeix l'adoptant amb dni {dni}");
            return false;
        }

        AnimalService animalService = new AnimalService(ctx);
        Animal? animal = animalService.CercaPerXip(codiXip);
        
        if (animal == null)
        {
            Console.WriteLine($"ERROR: No existeix l'animal amb xip {codiXip}");
            return false;
        }

        if (animal.EstaAdoptat)
        {
            Console.WriteLine($"ERROR: l'animal ja está adoptat");
            return false;
        }

        if (animal.Especie == Especie.GOS && !adoptant.TeJardi)
        {
            Console.WriteLine($"ERROR: l'adoptant ha de tenir jardí per adoptar un gos.");
            return false;
        }
        
        if(adoptant.Animals.Count >= 3)
        {
            Console.WriteLine($"EROR: l'adoptant només pot tenix com a maxim 3 animals.");
            return false;
        }
        
        adoptant.Animals.Add(animal);
        
        animal.Adoptant = adoptant;
        animal.EstaAdoptat = true;
        animal.DataAdopcio = DateTime.Today;

        ctx.Adoptants.Update(adoptant);
        ctx.Animals.Update(animal);
        ctx.SaveChanges();
        
        Console.WriteLine($"{adoptant.Nom} ha adoptat a {animal.Nom}!");
        
        return true;
    }
    
    public bool Retornar(string codiXip)
    {
        AnimalService animalService = new AnimalService(ctx);
        Animal? animal = animalService.CercaPerXip(codiXip);
        
        if (animal == null)
        {
            Console.WriteLine($"ERROR: No existeix l'animal amb xip {codiXip}");
            return false;
        }

        Adoptant? adoptant = animal.Adoptant;
        
        if (!animal.EstaAdoptat || adoptant == null)
        {
            Console.WriteLine($"ERROR: l'animal no está adoptat");
            return false;
        }

        adoptant.Animals.Remove(animal);
        
        animal.Adoptant = null;
        animal.EstaAdoptat = false;
        animal.DataAdopcio = null;
        animal.DataEntrada = DateTime.Today;

        ctx.Adoptants.Update(adoptant);
        ctx.Animals.Update(animal);
        ctx.SaveChanges();
        
        Console.WriteLine($"{adoptant.Nom} ha retornat a {animal.Nom}!");
        
        return true;
    }

    public void LlistatAdoptants(int minAnimals)
    {
        Console.WriteLine($"Adoptants amb mínim {minAnimals} animals");

        List<Adoptant> adoptants =  ctx.Adoptants
            .Where(a => a.Animals.Count >= minAnimals)
            .OrderByDescending(a => a.Animals.Count)
            .ToList();

        foreach (Adoptant adoptant in adoptants)
        {
            Console.WriteLine($"{adoptant.Nom} {adoptant.Cognoms}: {adoptant.Animals.Count} animals");
        }
    }

}