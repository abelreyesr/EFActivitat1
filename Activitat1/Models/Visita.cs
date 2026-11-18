using System.ComponentModel.DataAnnotations;

namespace Activitat1.Models;

public class Visita
{
    public int VisitaID { get; set; }
    [Required] public DateTime Data { get; set; }
    [Required] public Motiu Motiu { get; set; }
    [Required] public int Cost { get; set; }
    [Required] public virtual Animal Animal { get; set; }

    public Visita()
    {
        Data = DateTime.Today;
    }
    
    public override string ToString()
    {
        return  $"Visita {VisitaID}: [Animal = {Animal.Nom} (Xip {Animal.Xip.Codi}), Data = {Data}, Motiu = {Motiu.ToString()}, Cost = {Cost}]";
    }

}