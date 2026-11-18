using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Activitat1.Models;

[Index(nameof(Nom), IsUnique = true)]
public class Voluntari
{
    public int VoluntariID { get; set; }
    [Required] [MaxLength(50)] public string Nom { get; set; }
    [Required] public Torn Torn { get; set; }
    public int MinutsPasseig { get; set; }
    
    public virtual ICollection<Animal> Animals { get; set; }

    public Voluntari()
    {
        Animals = new HashSet<Animal>();
    }
    
    public override string ToString()
    {
        return $"Voluntari {VoluntariID}: [Nom = {Nom}, Torn = {Torn}, MinutsPasseig = {MinutsPasseig}, Animals = [{LlistaAnimals()}] ]";
    }
    
    private string LlistaAnimals()
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