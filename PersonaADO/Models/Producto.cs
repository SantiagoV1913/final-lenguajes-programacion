using System;

namespace PersonaADO.Models
{
    public class Producto
    {
        private int IdProducto;
        private string Nombre;
        private DateTime FechaVencimiento;
        private string Descripcion;

        public Producto(int idProducto, string nombre, DateTime fechaVencimiento, string descripcion)
        {
            this.IdProducto = idProducto;
            this.Nombre = nombre;
            this.FechaVencimiento = fechaVencimiento;
            this.Descripcion = descripcion;
        }

        public int GetIdProducto()
        {
            return IdProducto;
        }

        public string GetNombre()
        {
            return Nombre;
        }

        public DateTime GetFechaVencimiento()
        {
            return FechaVencimiento;
        }

        public string GetDescripcion()
        {
            return Descripcion;
        }

        public void SetIdProducto(int idProducto)
        {
            this.IdProducto = idProducto;
        }

        public void SetNombre(string nombre)
        {
            this.Nombre = nombre;
        }

        public void SetFechaVencimiento(DateTime fechaVencimiento)
        {
            this.FechaVencimiento = fechaVencimiento;
        }

        public void SetDescripcion(string descripcion)
        {
            this.Descripcion = descripcion;
        }
    }
}
