using System.ComponentModel.DataAnnotations;
using csharp1.Services;
using Microsoft.EntityFrameworkCore;

namespace Activitat1.Models;

public class Animal
{
    public int AnimalID { get; set; }
    [Required] [MaxLength(50)] public string Nom { get; set; }
    [Required] public Especie Especie { get; set; }
    [Required] public bool EsFemella { get; set; }
    [Required] public DateTime DataNaixement { get; set; }
    [Required] public DateTime DataEntrada { get; set; }
    [Range(0.1, 100)] public double Pes { get; set; }
    public bool EstaAdoptat { get; set; }
    public DateTime? DataAdopcio { get; set; }
    [Precision(8, 2)] public decimal QuotaAdopcio { get; set; }
    
    [Required] public int XipID { get; set; }
    public virtual Xip Xip { get; set; }

    public virtual Adoptant? Adoptant { get; set; }

    public Animal()
    {
        DataEntrada = DateTime.Now;
        EstaAdoptat = false;
        DataAdopcio = null;
        QuotaAdopcio = 0;
    }
    
    public Animal(string nom, Especie especie, bool esFemella, DateTime dataNaixement, DateTime dataEntrada, double pes, bool estaAdoptat, DateTime? dataAdopcio, decimal quotaAdopcio)
    {
        Nom = nom;
        Especie = especie;
        EsFemella = esFemella;
        DataNaixement = dataNaixement;
        DataEntrada = dataEntrada;
        this.Pes = pes;
        EstaAdoptat = estaAdoptat;
        DataAdopcio = dataAdopcio;
        QuotaAdopcio = quotaAdopcio;
    }
    
    public override string ToString()
    {
        string adoptantText = Adoptant == null ? "" : "Adoptant = " + Adoptant.Nom;
        return
            $"Animal {AnimalID}: [Nom = {Nom}, Especie = {Especie}, EsFemella = {EsFemella}, DataNaixement = {DataNaixement}, DataEntrada = {DataEntrada}, Pes = {Pes}, EstaAdoptat = {EstaAdoptat}, DataAdopcio = {DataAdopcio}, QuotaAdopcio = {QuotaAdopcio}, Xip = {Xip.ToString()}, {adoptantText}]";
    }

}