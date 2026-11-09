using Activitat1.Data;
using Activitat1.Models;
using csharp1.Services;
using Microsoft.EntityFrameworkCore;

namespace csharp1;

internal class Program
{
    static void Main(string[] args)
    {
        using (var ctx = new RefugiContext())
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            // Crear els 9 xips
            Xip xip1 = new Xip() { Codi = "000000000000001" };
            Xip xip2 = new Xip() { Codi = "000000000000002" };
            Xip xip3 = new Xip() { Codi = "000000000000003" };
            Xip xip4 = new Xip() { Codi = "000000000000004" };
            Xip xip5 = new Xip() { Codi = "000000000000005" };
            Xip xip6 = new Xip() { Codi = "000000000000006" };
            Xip xip7 = new Xip() { Codi = "000000000000007" };
            Xip xip8 = new Xip() { Codi = "000000000000008" };
            Xip xip9 = new Xip() { Codi = "000000000000009" };

            // Crear els 9 animals
            Animal animal1 = new Animal()
            {
                Nom = "Rocky",
                Especie = Especie.GOS,
                EsFemella = false,
                DataNaixement = new DateTime(2020, 3, 15),
                DataEntrada = new DateTime(2024, 1, 10),
                Pes = 25.5,
                Xip = xip1
            };

            Animal animal2 = new Animal()
            {
                Nom = "Luna",
                Especie = Especie.GOS,
                EsFemella = true,
                DataNaixement = new DateTime(2021, 6, 20),
                DataEntrada = new DateTime(2024, 2, 15),
                Pes = 18.2,
                Xip = xip2
            };

            Animal animal3 = new Animal()
            {
                Nom = "Max",
                Especie = Especie.GOS,
                EsFemella = false,
                DataNaixement = new DateTime(2019, 9, 5),
                DataEntrada = new DateTime(2023, 11, 20),
                Pes = 30.0,
                Xip = xip3
            };

            Animal animal4 = new Animal()
            {
                Nom = "Nala",
                Especie = Especie.GOS,
                EsFemella = true,
                DataNaixement = new DateTime(2022, 1, 12),
                DataEntrada = new DateTime(2024, 3, 5),
                Pes = 12.5,
                Xip = xip4
            };

            Animal animal5 = new Animal()
            {
                Nom = "Mishi",
                Especie = Especie.GAT,
                EsFemella = false,
                DataNaixement = new DateTime(2021, 4, 8),
                DataEntrada = new DateTime(2023, 10, 12),
                Pes = 4.5,
                Xip = xip5
            };

            Animal animal6 = new Animal()
            {
                Nom = "Mia",
                Especie = Especie.GAT,
                EsFemella = true,
                DataNaixement = new DateTime(2020, 8, 25),
                DataEntrada = new DateTime(2024, 1, 25),
                Pes = 3.8,
                Xip = xip6
            };

            Animal animal7 = new Animal()
            {
                Nom = "Simba",
                Especie = Especie.GAT,
                EsFemella = false,
                DataNaixement = new DateTime(2023, 2, 14),
                DataEntrada = new DateTime(2024, 4, 10),
                Pes = 5.2,
                Xip = xip7
            };

            Animal animal8 = new Animal()
            {
                Nom = "Bugs",
                Especie = Especie.CONILL,
                EsFemella = true,
                DataNaixement = new DateTime(2022, 7, 19),
                DataEntrada = new DateTime(2023, 12, 1),
                Pes = 2.1,
                Xip = xip8
            };

            Animal animal9 = new Animal()
            {
                Nom = "Piolín",
                Especie = Especie.AU,
                EsFemella = false,
                DataNaixement = new DateTime(2023, 5, 30),
                DataEntrada = new DateTime(2024, 5, 2),
                Pes = 0.5,
                Xip = xip9
            };

            // Afegir els animals a la base de dades
            ctx.Animals.AddRange(animal1, animal2, animal3, animal4, animal5, animal6, animal7, animal8, animal9);

            ctx.SaveChanges();

            Console.WriteLine("DADES INSERIDES\n");

            // Llegir els animals de la base de dades
            List<Animal> llista = ctx.Animals.Include(a => a.Xip).ToList();

            // Mostrar tots els animals amb el codi del seu xip
            Console.WriteLine("LLISTA D'ANIMALS:\n");

            foreach (Animal a in llista)
            {
                Console.WriteLine(a.ToString());
            }
            
            AnimalService service = new AnimalService(ctx);
            
            Console.WriteLine("LLISTA D'AUS DISPONIBLES:\n");
            foreach (Animal au in service.GetDisponibles(Especie.AU))
            {
                Console.WriteLine(au.ToString());
            }
            
            Console.WriteLine("ANIMAL AMB XIP 1:"+ service.CercaPerXip("000000000000001"));
            Console.WriteLine("ANIMAL AMB XIP 12:"); 
            Console.WriteLine(service.CercaPerXip("000000000000012"));
        }
    }
}