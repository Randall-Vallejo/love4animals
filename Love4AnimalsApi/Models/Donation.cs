namespace Love4AnimalsApi.Models;

public class Donation
{
    public Donation(int idDonation, decimal monto, string metodoPago, string comprobante, DateTime fecha, int usuarioId, int idCampania)
    {
        IdDonation = idDonation;
        Monto = monto;
        MetodoPago = metodoPago;
        Comprobante = comprobante;
        Fecha = fecha;
        UsuarioId = usuarioId;
        IdCampania = idCampania;
    }

    public int IdDonation { get; set; }
    public decimal Monto { get; set; }
    public string MetodoPago { get; set; }
    public string Comprobante { get; set; }
    public DateTime Fecha { get; set; }
    public int UsuarioId { get; set; }
    public int IdCampania { get; set; }
}