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

            AnimalService animalService = new AnimalService(ctx);
            AdoptantService adoptantService = new AdoptantService(ctx);
            VoluntariService voluntariService = new VoluntariService(ctx);
            VisitaService visitaService = new VisitaService(ctx);

            
            Console.WriteLine("--------------- PROVES ACTIVITAT 1 ---------------");
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

            Console.WriteLine("DADES INSERIDES");

            // Llegir els animals de la base de dades
            List<Animal> llista = ctx.Animals.Include(a => a.Xip).ToList();

            // Mostrar tots els animals amb el codi del seu xip
            Console.WriteLine("LLISTA D'ANIMALS:");

            foreach (Animal a in llista)
            {
                Console.WriteLine(a.ToString());
            }
            
            Console.WriteLine("LLISTA D'AUS DISPONIBLES:");
            foreach (Animal au in animalService.GetDisponibles(Especie.AU))
            {
                Console.WriteLine(au.ToString());
            }
            
            Console.WriteLine("ANIMAL AMB XIP 1:"+ animalService.CercaPerXip("000000000000001"));
            Console.WriteLine("ANIMAL AMB XIP 12:");
            Console.WriteLine(animalService.CercaPerXip("000000000000012"));
                
            
            Console.WriteLine("--------------- PROVES ACTIVITAT 2 ---------------");
            // crear i afegir adoptants
            Adoptant adoptant1 = new Adoptant()
            {
                DNI = "12345678Z",
                Nom = "Marta",
                Cognoms = "Puig Soler",
                TeJardi = true,
            };
            Adoptant adoptant2 = new Adoptant()
            {
                DNI = "87654321X",
                Nom = "Pau",
                Cognoms = "Ibañez",
                TeJardi = false
            };

            ctx.Adoptants.Add(adoptant1);
            ctx.Adoptants.Add(adoptant2);
            ctx.SaveChanges();
            
            // tres bien
            adoptantService.Adoptar(adoptant1.DNI, animal1.Xip.Codi);
            adoptantService.Adoptar(adoptant1.DNI, animal2.Xip.Codi);
            adoptantService.Adoptar(adoptant1.DNI, animal3.Xip.Codi);
            
            // mal
            adoptantService.Adoptar(adoptant2.DNI, animal1.Xip.Codi); // ja esta adoptat
            adoptantService.Adoptar(adoptant2.DNI, animal4.Xip.Codi); // gos ha de tenir jardi
            adoptantService.Adoptar("123456789A", animal5.Xip.Codi); // no existeix dni
            adoptantService.Adoptar(adoptant2.DNI, "000000000000067"); // no existeix chip
            adoptantService.Adoptar(adoptant1.DNI, animal6.Xip.Codi); // maxim d'animals
            
            Console.WriteLine("Abans de retornar");
            Console.WriteLine(animal1.ToString());
            Console.WriteLine(adoptant1.ToString());
            adoptantService.Retornar(animal1.Xip.Codi);
            
            Console.WriteLine("Despres de retornar: "+animal1);
            Console.WriteLine(animal1.ToString());
            Console.WriteLine(adoptant1.ToString());

            foreach (Animal au in animalService.GetDisponibles(Especie.GOS))
            {
                Console.WriteLine(au.ToString());
            }

            adoptantService.LlistatAdoptants(0);
            adoptantService.LlistatAdoptants(2);
            
            Console.WriteLine("--------------- PROVES ACTIVITAT 3 ---------------");
            
            // crear voluntaris
            Voluntari voluntari1 = new Voluntari()
            {
                Nom = "Aina",
                Torn = Torn.MATI
            };
            Voluntari voluntari2 = new Voluntari()
            {
                Nom = "Joan",
                Torn = Torn.TARDA
            };
            Voluntari voluntari3 = new Voluntari()
            {
                Nom = "Nuria",
                Torn = Torn.CAP_DE_SETMANA
            };
            
            ctx.Voluntaris.Add(voluntari1);
            ctx.Voluntaris.Add(voluntari3);
            ctx.Voluntaris.Add(voluntari2);

            ctx.SaveChanges();

            // assignar animals
            voluntariService.AssignarAnimal(voluntari1.Nom, animal1.Xip.Codi);
            voluntariService.AssignarAnimal(voluntari1.Nom, animal4.Xip.Codi);
            voluntariService.AssignarAnimal(voluntari1.Nom, animal3.Xip.Codi);
            voluntariService.AssignarAnimal(voluntari2.Nom, animal5.Xip.Codi);
            
            voluntariService.AssignarAnimal(voluntari1.Nom, animal1.Xip.Codi);
            voluntariService.AssignarAnimal(voluntari1.Nom, animal7.Xip.Codi);
            voluntariService.AssignarAnimal(voluntari1.Nom, animal8.Xip.Codi);
            voluntariService.AssignarAnimal(voluntari1.Nom, animal9.Xip.Codi);
            
            // desassignar animals
            adoptantService.Adoptar(adoptant1.DNI, animal1.Xip.Codi);
            
            // passeig
            voluntariService.RegistrarPasseig(voluntari1.Nom, animal1.Xip.Codi, 15);
            voluntariService.RegistrarPasseig(voluntari1.Nom, animal4.Xip.Codi, 15);
            voluntariService.RegistrarPasseig(voluntari1.Nom, animal4.Xip.Codi, 22);
            Console.WriteLine($"{voluntari1.Nom} total minuts: {voluntari1.MinutsPasseig}");
            voluntariService.RegistrarPasseig(voluntari2.Nom, animal7.Xip.Codi, 22);

            Console.WriteLine("--------------- PROVES ACTIVITAT 4 ---------------");
            // ============ Nivell 1 · Dades ============
            Animal nala = new Animal() { Nom = "Nala", Especie = Especie.GOS, EsFemella = true, Pes = 25,
                DataNaixement = DateTime.Today.AddYears(-9), Xip = new Xip() { Codi = "724098100000011" } };
            Animal coco = new Animal() { Nom = "Coco", Especie = Especie.GOS, EsFemella = true, Pes = 21,
                DataNaixement = DateTime.Today.AddYears(-5), Xip = new Xip() { Codi = "724098100000012" } };
            Animal rocky = new Animal() { Nom = "Rocky", Especie = Especie.GOS, EsFemella = false, Pes = 6.5,
                DataNaixement = DateTime.Today.AddMonths(-4), Xip = new Xip() { Codi = "724098100000013" } };
            Animal bimba = new Animal() { Nom = "Bimba", Especie = Especie.GOS, EsFemella = true, Pes = 5.8,
                DataNaixement = DateTime.Today.AddMonths(-4), Xip = new Xip() { Codi = "724098100000014" } };

            coco.Cries.Add(rocky);
            coco.Cries.Add(bimba);
            nala.Cries.Add(coco);

            ctx.Animals.Add(nala);
            ctx.SaveChanges();

            void MostrarFamilies()
            {
                foreach (Animal an in animalService.GetTots())
                {
                    string mare;
                    if (an.Mare != null) mare = an.Mare.Nom;
                    else mare = "sense mare";
                    Console.WriteLine($"{an.Nom} ({an.Especie}) · mare: {mare} · {an.Cries.Count} cries");
                }
            }

            Console.WriteLine("\n--- Animals amb la seva mare i les seves cries ---");
            MostrarFamilies();

            // ============ Nivell 1 · CanviarMare ============
            Console.WriteLine("\n--- Canviar la mare ---");
            animalService.CanviarMare(animal4.Xip.Codi, animal2.Xip.Codi);      // bé: la Nala (sessió 2) ara és cria de la Luna
            animalService.CanviarMare(animal4.Xip.Codi, animal2.Xip.Codi);      // false: ja té la Luna com a mare
            animalService.CanviarMare(animal4.Xip.Codi, null);                  // bé: la Nala es queda sense mare
            animalService.CanviarMare(animal4.Xip.Codi, animal1.Xip.Codi);      // false: el Rocky és mascle
            animalService.CanviarMare("724098100000999", animal2.Xip.Codi);     // false: no hi ha cap cria amb aquest xip
            animalService.CanviarMare(animal4.Xip.Codi, "724098100000999");     // false: no hi ha cap mare amb aquest xip
            animalService.CanviarMare(animal4.Xip.Codi, null);                  // false: la Nala ja no té mare

            // ============ Nivell 2 · RegistrarCria ============
            Console.WriteLine("\n--- Registrar cries ---");
            Animal toby = new Animal() { Nom = "Toby", Especie = Especie.GOS, EsFemella = false, Pes = 2.1,
                DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000015" } };
            animalService.RegistrarCria("724098100000012", toby);               // bé: el Toby és cria de la Coco

            Animal criaThor = new Animal() { Nom = "Max", Especie = Especie.GOS, EsFemella = false, Pes = 2,
                DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000016" } };
            animalService.RegistrarCria("724098100000013", criaThor);           // false: el Rocky és mascle

            Animal criaGat = new Animal() { Nom = "Misha", Especie = Especie.GAT, EsFemella = true, Pes = 0.8,
                DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000017" } };
            animalService.RegistrarCria("724098100000012", criaGat);            // false: la Coco és un gos i la Misha un gat
            
            Animal criaVella = new Animal() { Nom = "Avi", Especie = Especie.GOS, EsFemella = false, Pes = 20,
                DataNaixement = DateTime.Today.AddYears(-10), Xip = new Xip() { Codi = "724098100000018" } };
            animalService.RegistrarCria("724098100000012", criaVella);          // false: ha nascut abans que la Coco
            animalService.RegistrarCria("724098100000999", criaVella);          // false: no hi ha cap mare amb aquest xip

            Console.WriteLine("\n--- Famílies després de registrar (la Coco té 3 cries, cap animal nou dels dolents) ---");
            MostrarFamilies();

            // ============ Nivell 3 · Descendents ============
            void MostrarDescendents(string codiXip)
            {
                List<Animal>? descendents = animalService.Descendents(codiXip);

                if (descendents == null) return;
                
                List<string> noms = new List<string>();
                foreach (Animal an in descendents)
                {
                    noms.Add(an.Nom);
                }
                Console.WriteLine($"Descendents de {codiXip}: [{string.Join(", ", noms)}]");
            }

            Console.WriteLine("\n--- Descendents ---");
            MostrarDescendents("724098100000011");     // Nala: [Coco, Rocky, Bimba, Toby]
            MostrarDescendents("724098100000012");     // Coco: [Rocky, Bimba, Toby]
            MostrarDescendents("724098100000013");     // Rocky: [] (no té cries)
            MostrarDescendents("724098100000999");     // no existeix: missatge i []
            
            
            Console.WriteLine("--------------- PROVES ACTIVITAT 5 ---------------");
            Adoptant adoptant3 = new Adoptant()
            {
                DNI = "12349678Z",
                Nom = "Tercero",
                Cognoms = "Tercerez",
                TeJardi = true,
            };
            ctx.Adoptants.Add(adoptant3);
            
            animalService.CalcularDespesaMes();
            adoptantService.Adoptar(adoptant3.DNI, coco.Xip.Codi);
            animalService.CalcularDespesaMes();
            
            Console.WriteLine("--------------- PROVES ACTIVITAT 6 ---------------");
            visitaService.GenerarVisites(30);
            animalService.EstadistiquesPerEspecie();
            visitaService.EstadistiquesPerMotiu();
            voluntariService.LlistatVoluntaris(1);
            voluntariService.LlistatVoluntaris(3);
        }
    }
}