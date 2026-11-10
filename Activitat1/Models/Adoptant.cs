using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Activitat1.Models;

[Index(nameof(DNI), IsUnique = true)]
public class Adoptant
{
    public int AdoptantID { get; set; }
    [Required] [MaxLength(9)] public string DNI { get; set; }
    [Required] [MaxLength(50)] public string Nom { get; set; }
    [Required] [MaxLength(100)] public string Cognoms { get; set; }
    [Required] public bool TeJardi { get; set; }
    [Required] public DateTime DataRegistre { get; set; }

    public virtual ICollection<Animal> Animals { get; set; }
    
    public Adoptant()
    {
        DataRegistre = DateTime.Today;
        Animals = new HashSet<Animal>();
    }

    public override string ToString()
    {
        return $"Adoptant {AdoptantID}: [DNI = {DNI}, Nom = {Nom}, Cognoms = {Cognoms}, Jardí = {TeJardi}, DataRegistre = {DataRegistre}, Animals = [{LlistaAnimals()}] ]";
    }

    public string LlistaAnimals()
    {
        string msg = "";

        foreach (Animal a in Animals)
        {
            msg += $"{a.Nom} (codi {a.Xip.Codi}), ";
        }

        msg = msg.Substring(0, msg.Length - 2);

        return msg;
    }
}