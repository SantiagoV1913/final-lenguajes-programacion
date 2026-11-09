using PersonaADO.Models;
using PersonaADO.Repositories;

ProductoRepository repository = new ProductoRepository();

int opcion;

do
{
    Console.WriteLine("==================================");
    Console.WriteLine("       CRUD DE PRODUCTOS");
    Console.WriteLine("       XML");
    Console.WriteLine("==================================");
    Console.WriteLine("1. Registrar producto");
    Console.WriteLine("2. Buscar producto");
    Console.WriteLine("3. Actualizar producto");
    Console.WriteLine("4. Eliminar producto");
    Console.WriteLine("5. Agregar y eliminar productos de prueba");
    Console.WriteLine("0. Salir");
    Console.WriteLine();
    Console.Write("Seleccione una opción: ");

    if (!int.TryParse(Console.ReadLine(), out opcion))
    {
        Console.WriteLine("Opción inválida.");
        Console.ReadKey();
        continue;
    }

    try
    {
        switch (opcion)
        {
            case 1:
                RegistrarProducto(repository);
                break;
            case 2:
                BuscarProducto(repository);
                break;
            case 3:
                ActualizarProducto(repository);
                break;
            case 4:
                EliminarProducto(repository);
                break;
            case 5:
                AgregarYEliminarProductos(repository);
                break;
            case 0:
                Console.WriteLine("Programa finalizado.");
                break;
            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Ocurrió un error:");
        Console.WriteLine(ex.Message);
    }

    if (opcion != 0)
    {
        Console.WriteLine();
        Console.WriteLine("Presione una tecla para continuar...");
        Console.ReadKey();
    }
} while (opcion != 0);

static void RegistrarProducto(ProductoRepository repository)
{
    Console.Clear();
    Console.WriteLine("===== REGISTRAR PRODUCTO =====");
    Console.WriteLine();

    Console.Write("Nombre: ");
    string? nombre = Console.ReadLine();

    Console.Write("Descripción: ");
    string? descripcion = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(descripcion))
    {
        Console.WriteLine("El nombre y la descripción son obligatorios.");
        return;
    }

    Console.Write("Fecha de vencimiento (yyyy-MM-dd): ");
    string? fechaTexto = Console.ReadLine();

    if (!DateTime.TryParse(fechaTexto, out DateTime fechaVencimiento))
    {
        Console.WriteLine("Fecha inválida.");
        return;
    }

    Producto producto = new Producto(0, nombre, fechaVencimiento, descripcion);
    repository.Insertar(producto.GetIdProducto(), producto.GetNombre(),
        producto.GetFechaVencimiento(), producto.GetDescripcion());
}

static void BuscarProducto(ProductoRepository repository)
{
    Console.Clear();
    Console.WriteLine("===== BUSCAR PRODUCTO =====");
    Console.WriteLine();
    Console.Write("Ingrese el ID: ");

    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    Producto? producto = repository.BuscarPorId(id);

    if (producto == null)
    {
        Console.WriteLine("No se encontró el producto.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine($"ID:                 {producto.GetIdProducto()}");
    Console.WriteLine($"Nombre:             {producto.GetNombre()}");
    Console.WriteLine($"Fecha vencimiento:  {producto.GetFechaVencimiento():yyyy-MM-dd}");
    Console.WriteLine($"Descripción:        {producto.GetDescripcion()}");
}

static void ActualizarProducto(ProductoRepository repository)
{
    Console.Clear();
    Console.WriteLine("===== ACTUALIZAR PRODUCTO =====");
    Console.WriteLine();
    Console.Write("Ingrese el ID del producto a actualizar: ");

    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    Producto? actual = repository.BuscarPorId(id);

    if (actual == null)
    {
        Console.WriteLine("No se encontró el producto.");
        return;
    }

    Console.Write("Nuevo nombre (dejar vacío para no cambiar): ");
    string? nombre = Console.ReadLine();
    Console.Write("Nueva descripción (dejar vacío para no cambiar): ");
    string? descripcion = Console.ReadLine();
    Console.Write("Nueva fecha de vencimiento (yyyy-MM-dd) (dejar vacío para no cambiar): ");
    string? fechaTexto = Console.ReadLine();

    DateTime fechaVencimiento = actual.GetFechaVencimiento();

    if (!string.IsNullOrWhiteSpace(fechaTexto) &&
        !DateTime.TryParse(fechaTexto, out fechaVencimiento))
    {
        Console.WriteLine("Fecha inválida.");
        return;
    }

    repository.Actualizar(
        id,
        string.IsNullOrWhiteSpace(nombre) ? actual.GetNombre() : nombre,
        fechaVencimiento,
        string.IsNullOrWhiteSpace(descripcion) ? actual.GetDescripcion() : descripcion);
}

static void EliminarProducto(ProductoRepository repository)
{
    Console.Clear();
    Console.WriteLine("===== ELIMINAR PRODUCTO =====");
    Console.WriteLine();
    Console.Write("Ingrese el ID del producto a eliminar: ");

    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    Console.Write("¿Confirma eliminar el producto? (s/N): ");
    string? confirmar = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(confirmar) ||
        !(confirmar.Equals("s", StringComparison.OrdinalIgnoreCase) ||
          confirmar.Equals("si", StringComparison.OrdinalIgnoreCase)))
    {
        Console.WriteLine("Operación cancelada.");
        return;
    }

    repository.Eliminar(id);
}

static void AgregarYEliminarProductos(ProductoRepository repository)
{
    Console.Clear();
    Console.WriteLine("===== PRODUCTOS DE PRUEBA =====");
    Console.WriteLine();

    int[] productos = new int[5];
    productos[0] = repository.Insertar(0, "Cuaderno", new DateTime(2028, 1, 15), "Cuaderno universitario");
    productos[1] = repository.Insertar(0, "Lapicero azul", new DateTime(2027, 6, 20), "Lapicero de tinta azul");
    productos[2] = repository.Insertar(0, "Carpeta", new DateTime(2029, 3, 10), "Carpeta plastificada");
    productos[3] = repository.Insertar(0, "Borrador", new DateTime(2027, 11, 5), "Borrador blanco");
    productos[4] = repository.Insertar(0, "Regla", new DateTime(2030, 8, 30), "Regla de treinta centímetros");

    Console.WriteLine();
    Console.WriteLine("Se agregaron cinco productos.");
    Console.WriteLine("Ahora se eliminarán los tres productos intermedios.");

    for (int i = 1; i < productos.Length - 1; i++)
    {
        if (productos[i] > 0)
        {
            repository.Eliminar(productos[i]);
        }
    }

    Console.WriteLine("Se conservaron el primer y el último producto.");
}
