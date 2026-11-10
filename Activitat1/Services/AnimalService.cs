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
}