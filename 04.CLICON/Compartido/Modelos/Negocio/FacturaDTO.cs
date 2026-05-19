namespace Compartido.Modelos.Negocio
{
    public class FacturaDTO
    {
        public int IdFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal TotalFinal { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Factura #{IdFactura} — {FechaEmision:dd/MM/yyyy HH:mm}\n" +
                   $"  Subtotal: ${Subtotal:F2}\n" +
                   $"  IVA (15%): ${Iva:F2}\n" +
                   $"  Total: ${TotalFinal:F2}\n" +
                   $"  {Mensaje}";
        }
    }
}
