using Activitat1.Data;
using Activitat1.Models;

namespace csharp1.Services;

public class VoluntariService
{
    private RefugiContext ctx;

    public VoluntariService(RefugiContext context)
    {
        ctx = context;
    }

    public bool AssignarAnimal(string nomVoluntari, string codiXip)
    {
        AnimalService animalService = new AnimalService(ctx);
        Animal? animal = animalService.CercaPerXip(codiXip);
        if (animal == null)
        {
            Console.WriteLine($"ERROR: No existeix l'animal amb xip {codiXip}");
            return false;
        }

        Voluntari? voluntari = ctx.Voluntaris.FirstOrDefault(v => v.Nom == nomVoluntari);
        if (voluntari == null)
        {
            Console.WriteLine($"ERROR: No existeix cap voluntari amb nom {nomVoluntari}");
            return false;
        }

        if (animal.EstaAdoptat)
        {
            Console.WriteLine($"ERROR: {animal.Nom} ja está adoptat");
            return false;
        }
        
        if (voluntari.Animals.Contains(animal))
        {
            Console.WriteLine($"ERROR: {animal.Nom} ja está asignat a {voluntari.Nom}");
            return false;
        }
        
        if (voluntari.Animals.Count >= 4)
        {
            Console.WriteLine($"ERROR: {voluntari.Nom} no pot tenir més animals assignats");
            return false;
        }
        
        voluntari.Animals.Add(animal);
        animal.Voluntaris.Add(voluntari);
        
        ctx.SaveChanges();
        
        Console.WriteLine($"{animal.Nom} ha sigut assignat a {voluntari.Nom} correctament");
        
        return true;
    }
    
    public bool DesassignarAnimal(string nomVoluntari, string codiXip)
    {
        AnimalService animalService = new AnimalService(ctx);
        Animal? animal = animalService.CercaPerXip(codiXip);
        if (animal == null)
        {
            Console.WriteLine($"ERROR: No existeix l'animal amb xip {codiXip}");
            return false;
        }

        Voluntari? voluntari = ctx.Voluntaris.FirstOrDefault(v => v.Nom == nomVoluntari);
        if (voluntari == null)
        {
            Console.WriteLine($"ERROR: No existeix cap voluntari amb nom {nomVoluntari}");
            return false;
        }
        
        if (!voluntari.Animals.Contains(animal))
        {
            return false;
        }
        
        voluntari.Animals.Remove(animal);
        animal.Voluntaris.Remove(voluntari);
        
        ctx.SaveChanges();
        
        Console.WriteLine($"{animal.Nom} ha sigut desassignat de {voluntari.Nom} correctament");
        
        return true;
    }

    public bool RegistrarPasseig(string nomVoluntari, string codiXip, int minuts)
    {
        AnimalService animalService = new AnimalService(ctx);
        Animal? animal = animalService.CercaPerXip(codiXip);
        if (animal == null)
        {
            Console.WriteLine($"ERROR: No existeix l'animal amb xip {codiXip}");
            return false;
        }

        Voluntari? voluntari = ctx.Voluntaris.FirstOrDefault(v => v.Nom == nomVoluntari);
        if (voluntari == null)
        {
            Console.WriteLine($"ERROR: No existeix cap voluntari amb nom {nomVoluntari}");
            return false;
        }

        if (animal.Especie != Especie.GOS)
        {
            Console.WriteLine($"ERROR: {animal.Nom} no és un gos");
            return false;
        }
        
        if (!voluntari.Animals.Contains(animal))
        {
            Console.WriteLine($"ERROR: {animal.Nom} no está asignat a {voluntari.Nom}");
            return false;
        }

        voluntari.MinutsPasseig += minuts;
        ctx.SaveChanges();
        
        Console.WriteLine($"{voluntari.Nom} ha tret de passeig a {animal.Nom} durant {minuts} minuts");
        
        return true;
    }
    
    public void LlistatVoluntaris(int minAnimals)
    {
        Console.WriteLine($"---LLISTA DE VOLUNTARIS AMB {minAnimals} O MES ANIMALS---");
        
        var voluntaris = ctx.Voluntaris
            .Where(v => v.Animals.Count >= minAnimals)
            .OrderByDescending(v => v.Animals.Count)
            .ToList();
        
        foreach (var v in voluntaris)
        {
            Console.WriteLine($"{v.Nom} - {v.Animals.Count} animals - {v.MinutsPasseig}min");
        }
    }

}