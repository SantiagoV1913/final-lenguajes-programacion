using PersonaADO.Models;
using System.Xml.Linq;

namespace PersonaADO.Repositories;

public class ProductoRepository
{
    private readonly string archivoXml;

    public ProductoRepository()
    {
        this.archivoXml = Path.Combine(AppContext.BaseDirectory, "Productos.xml");
    }

    public int Insertar(int idProducto, string nombre, DateTime fechaVencimiento, string descripcion)
    {
        XDocument documento = CargarDocumento();
        XElement productos = documento.Element("productos")!;

        int nuevoId = productos
            .Elements("producto")
            .Select(producto => (int?)producto.Attribute("idProducto") ?? 0)
            .DefaultIfEmpty(0)
            .Max() + 1;

        XElement producto = new XElement("producto",
            new XAttribute("idProducto", nuevoId),
            new XElement("nombre", nombre),
            new XElement("fechaVencimiento", fechaVencimiento.ToString("yyyy-MM-dd")),
            new XElement("descripcion", descripcion));

        productos.Add(producto);
        documento.Save(archivoXml);

        Console.WriteLine("Producto registrado correctamente");
        return nuevoId;
    }

    public Producto? BuscarPorId(int id)
    {
        XDocument documento = CargarDocumento();
        XElement? producto = documento
            .Descendants("producto")
            .FirstOrDefault(elemento => (int?)elemento.Attribute("idProducto") == id);

        if (producto == null)
        {
            return null;
        }

        return ConvertirProducto(producto);
    }

    public void Actualizar(int idProducto, string nombre, DateTime fechaVencimiento, string descripcion)
    {
        XDocument documento = CargarDocumento();
        XElement? producto = documento
            .Descendants("producto")
            .FirstOrDefault(elemento => (int?)elemento.Attribute("idProducto") == idProducto);

        if (producto == null)
        {
            Console.WriteLine("No se encontró el producto o no se realizó ningún cambio");
            return;
        }

        producto.SetElementValue("nombre", nombre);
        producto.SetElementValue("fechaVencimiento", fechaVencimiento.ToString("yyyy-MM-dd"));
        producto.SetElementValue("descripcion", descripcion);
        documento.Save(archivoXml);

        Console.WriteLine("Producto actualizado correctamente");
    }

    public void Eliminar(int idProducto)
    {
        XDocument documento = CargarDocumento();
        XElement? producto = documento
            .Descendants("producto")
            .FirstOrDefault(elemento => (int?)elemento.Attribute("idProducto") == idProducto);

        if (producto == null)
        {
            Console.WriteLine("No se encontró el producto para eliminar");
            return;
        }

        producto.Remove();
        documento.Save(archivoXml);
        Console.WriteLine("Producto eliminado correctamente");
    }

    private XDocument CargarDocumento()
    {
        if (!File.Exists(archivoXml))
        {
            XDocument documentoNuevo = new XDocument(new XElement("productos"));
            documentoNuevo.Save(archivoXml);
        }

        return XDocument.Load(archivoXml);
    }

    private static Producto ConvertirProducto(XElement elemento)
    {
        return new Producto(
            (int)elemento.Attribute("idProducto")!,
            (string)elemento.Element("nombre")!,
            DateTime.Parse((string)elemento.Element("fechaVencimiento")!),
            (string)elemento.Element("descripcion")!);
    }
}
