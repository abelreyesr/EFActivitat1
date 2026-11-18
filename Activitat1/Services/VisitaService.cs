using System.Runtime.InteropServices.JavaScript;
using Activitat1.Data;
using Activitat1.Models;

namespace csharp1.Services;

public class VisitaService
{
    private RefugiContext ctx;

    public VisitaService(RefugiContext context)
    {
        ctx = context;
    }

    public void GenerarVisites(int quantes)
    {
        Console.WriteLine($"Afegint {quantes} visites...");
        
        Random rnd = new Random();
        List<Animal> animals = ctx.Animals.ToList();
        
        Visita[] visites = new Visita[quantes];
        for (int i = 0; i < quantes; i++)
        {
            // animal random
            Animal animal = animals[rnd.Next(animals.Count)];
            
            // data random
            DateTime dataFinal = (DateTime) (animal.EstaAdoptat ? animal.DataAdopcio : DateTime.Today);
            int dies = (dataFinal - animal.DataEntrada).Days;
            DateTime data = DateTime.Today.AddDays(-rnd.Next(0, dies));
            
            // motiu random
            Motiu[] motius = Enum.GetValues<Motiu>();
            Motiu motiu = motius[rnd.Next(motius.Length)];

            // cost random per motiu
            int minCost, maxCost;
            switch (motiu)
            {
                case Motiu.REVISIO: minCost = 30; maxCost = 60; break;
                case Motiu.VACUNA: minCost = 25; maxCost = 50; break;
                case Motiu.FERIDA: minCost = 60; maxCost = 150; break;
                case Motiu.CIRURGIA: minCost = 250; maxCost = 600; break;
                default: minCost = 0; maxCost = 0; break;
            }
            
            int cost = rnd.Next(minCost, maxCost);

            // crear i afegir visita
            Visita visita = new Visita()
            {
                Animal = animal,
                Cost = cost,
                Motiu = motiu,
                Data = data
            };

            visites[i] = visita;
            ctx.Visites.Add(visita);
        }

        ctx.SaveChanges();

        // el print despues del savechanges pq sino no se guarda la VisitaID y salian todas con id 0
        foreach (Visita visita in visites) Console.WriteLine($"Afegida {visita}");     
        
        Console.WriteLine($"S'han afegit {quantes} visites correctament");
    }
    
    public void EstadistiquesPerMotiu()
    {
        Console.WriteLine("---ESTADÍSTIQUES PER MOTIU DE VISITA---");
        
        var perMotiu = ctx.Visites
            .GroupBy(v => v.Motiu)
            .Select(m => new
            {
                Motiu = m.Key,
                TotalVisites = m.Count(),
                CostTotal = m.Sum(v => v.Cost),
                CostMitja = m.Average(v => v.Cost)
            })
            .OrderByDescending(m => m.CostTotal)
            .ToList();
        
        foreach (var m in perMotiu)
        {
            Console.WriteLine($"{m.Motiu}: {m.TotalVisites} visites amb un cost total de {m.CostTotal} i un cost mitjà de {m.CostMitja.ToString("F2")}");
        }
    }
    
    public void EstadistiquesPerMes()
    {
        Console.WriteLine("---ESTADÍSTIQUES PER MES---");
        
        var perMotiu = ctx.Visites
            .GroupBy(v => new{v.Data.Year, v.Data.Month})
            .Select(m => new
            {
                Any = m.Key.Year,
                Mes = m.Key.Month,
                TotalVisites = m.Count(),
                CostTotal = m.Sum(v => v.Cost),
            })
            .OrderByDescending(m => m.CostTotal)
            .ToList();
        
        foreach (var m in perMotiu)
        {
            Console.WriteLine($"{m.Mes}/{m.Any}: {m.TotalVisites} visites amb un cost total de {m.CostTotal}");
        }
    }
}