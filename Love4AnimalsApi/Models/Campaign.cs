using System;

namespace Love4AnimalsApi.Models;

public class Campaign
{
    public Campaign(int IdCampania, string Titulo, string Descripcion, decimal MetaMonto, decimal MontoRecaudado, DateTime FechaInicio, DateTime FechaFin, string Estado, int UsuarioId)
    {
        this.IdCampania = IdCampania;
        this.Titulo = Titulo;
        this.Descripcion = Descripcion;
        this.MetaMonto = MetaMonto;
        this.MontoRecaudado = MontoRecaudado;
        this.FechaInicio = FechaInicio;
        this.FechaFin = FechaFin;
        this.Estado = Estado;
        this.UsuarioId = UsuarioId;
    }
    public int IdCampania { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public decimal MetaMonto { get; set; }
    public decimal MontoRecaudado { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string Estado { get; set; }
    public int UsuarioId { get; set; }
}