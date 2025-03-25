using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

class Producto
{
    public string nombreProducto { get; set; }
    public int codigoProducto { get; set; }
    public int cantidadStock { get; set; }
    public double precioProducto { get; set; }

    public Producto(string nombreProducto, int codigoProducto, int cantidadStock, double precioProducto)
    {
        this.nombreProducto = nombreProducto;
        this.codigoProducto = codigoProducto;
        this.cantidadStock = cantidadStock;
        this.precioProducto = precioProducto;
    }
}

class Registro
{
    private string _rutaArchivo;

    public Registro(string rutaArchivo)
    {
        this._rutaArchivo = rutaArchivo;
    }

    public void GuardarProducto(Producto producto)
    {
        List<Producto> productos = new List<Producto>();

        if (File.Exists(_rutaArchivo))
        {
            string jsonString = File.ReadAllText(_rutaArchivo);
            if (!string.IsNullOrEmpty(jsonString))
            {
                productos = JsonSerializer.Deserialize<List<Producto>>(jsonString);
            }
        }

        productos.Add(producto);

        string jsonNuevo = JsonSerializer.Serialize(productos, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_rutaArchivo, jsonNuevo);
    }

    public void MostrarInfo()
    {
        if (File.Exists(_rutaArchivo))
        {
            string jsonString = File.ReadAllText(_rutaArchivo);
            if (!string.IsNullOrEmpty(jsonString))
            {
                List<Producto> productos = JsonSerializer.Deserialize<List<Producto>>(jsonString);

                foreach (var producto in productos)
                {
                    Console.WriteLine($"\nProducto: {producto.nombreProducto}, Codigo: {producto.codigoProducto}, Cantidad en Stock: {producto.cantidadStock}, Precio: {producto.precioProducto}");
                }
            }
            else
            {
                Console.WriteLine("El archivo está vacío.");
            }
        }
        else
        {
            Console.WriteLine("No se ha encontrado el archivo");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Ingrese los datos a guardar (escriba 'salir' en el nombre del producto para poder terminar) ");
        Registro miRegistro = new Registro("inventario.json");

        while (true)
        {
            Console.WriteLine("\nNombre del producto:");
            string nombreProducto = Console.ReadLine();
            if (nombreProducto.ToLower() == "salir") break;

            Console.WriteLine("Codigo del producto:");
            if (!int.TryParse(Console.ReadLine(), out int codigoProducto))
            {
                Console.WriteLine("Código de producto inválido. Intente de nuevo.");
                continue;
            }

            Console.WriteLine("Cantidad en stock:");
            if (!int.TryParse(Console.ReadLine(), out int cantidadStock))
            {
                Console.WriteLine("Cantidad en stock inválida. Intente de nuevo.");
                continue;
            }

            Console.WriteLine("Precio del producto:");
            if (!double.TryParse(Console.ReadLine(), out double precioProducto))
            {
                Console.WriteLine("Precio del producto inválido. Intente de nuevo.");
                continue;
            }

            Producto producto = new Producto(nombreProducto, codigoProducto, cantidadStock, precioProducto);
            miRegistro.GuardarProducto(producto);
        }

        miRegistro.MostrarInfo();
    }
}