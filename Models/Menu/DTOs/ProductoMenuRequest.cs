namespace MTLCRISTALVK18BACK.Models.Menu.DTOs
{
    public class ProductoMenuRequest
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
        public string? Imagen { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; } = true;
        public bool ControlaInventario { get; set; } = true;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
    }
}
