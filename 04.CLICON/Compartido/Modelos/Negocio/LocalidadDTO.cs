namespace Compartido.Modelos.Negocio
{
    /// <summary>
    /// DTO que representa una localidad de un partido con disponibilidad y precio.
    /// Espejo del DataContract del servidor WCF.
    /// </summary>
    public class LocalidadDTO
    {
        public string CodigoLocalidad { get; set; } = string.Empty;
        public int Disponibilidad { get; set; }
        public decimal Precio { get; set; }

        public override string ToString()
        {
            return $"[{CodigoLocalidad}] Disponibles: {Disponibilidad} — Precio: ${Precio:F2}";
        }
    }
}
