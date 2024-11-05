using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient.Authentication;
using Org.BouncyCastle.Asn1.Misc;
using System;

namespace ConsoleAppMySQL
{
    class Program
    {
        static void crear_tablas(string string_conexion)
        {
            MySqlConnection conn = new MySqlConnection(string_conexion);
            string query = @"
                            create table if not exists usuarios(
                            id int not null auto_increment PRIMARY key,
                            nombre varchar(20) not null,
                            apellido varchar(20) not null,
                            dni varchar(8) not null,
                            telefono varchar(12) not null,
                            email varchar(50) not null,
                            creado_el timestamp default now(),
                            actualizado_el timestamp default now(),
                            estado boolean default 1);

                            create table if not exists generos(id int not null auto_increment PRIMARY KEY,
                                                descripcion varchar(30));

                            create table if not exists libros(
                            id int not null auto_increment PRIMARY KEY,
                            titulo VARCHAR(30) not null,
                            autor VARCHAR(20) not null,
                            genero_id int not null,
                            fecha_lanzamiento date,
                            creado_el timestamp default now(),
                            actualizado_el timestamp default now(),
                            estado boolean default 1,
                            constraint fk_genero foreign key(genero_id) references generos(id));

                            create table if not exists prestamos(
                            id int not null auto_increment PRIMARY KEY,
                            fecha_retiro timestamp default now(),
                            fecha_entrega_estimada timestamp,
                            fecha_entrega_real timestamp,
                            id_libros int not NULL,
                            CONSTRAINT fk_prestamos_libros FOREIGN key (id_libros) REFERENCES libros(id),
                            id_usuarios int not NULL,
                            CONSTRAINT fk_prestamos_usuarios FOREIGN key (id_usuarios) REFERENCES usuarios(id));";

                            try
                            {
                                conn.Open();
                                MySqlCommand cmd = new MySqlCommand(query, conn);
                                
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ha ocurrido un error" + ex.Message);
                            }
                            finally
                            {
                                conn.Close();   
                            }
        }

        static string centrar_texto(string? texto, int ancho)
        {
            int espacio_der = -1, espacio_izq = -1;
            if (texto != null)
            {
                if (texto.Length == ancho) return texto;

                espacio_izq = (ancho - texto.Length) / 2;
                espacio_der = ancho - texto.Length - espacio_izq;
            
            }
            return new string(' ', espacio_izq) + texto + new string(' ', espacio_der);            
        }

        static void listar_tablas(int opcion, string string_conexion)
        {
            Console.Clear();
            MySqlConnection conn = new MySqlConnection(string_conexion);

            switch (opcion) //0 lista usuarios, 1 lista libros, 2 lista prestamos, 3 lista generos, 4 lista todos los libros
            {
                case 0:              
                try
                {
                    conn.Open();

                    string query = "SELECT id, nombre, apellido FROM usuarios where estado = 1";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    Console.WriteLine(centrar_texto("------ Usuarios activos ------",45));
                    Console.WriteLine();
                    Console.WriteLine($"{centrar_texto("ID",5)} | {centrar_texto("Nombre",20)} | {centrar_texto("Apellido",20)} |");

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {  
                        Console.WriteLine($"{centrar_texto(Convert.ToString(reader["id"]),5)} | {centrar_texto(Convert.ToString(reader["nombre"]),20)} | {centrar_texto(Convert.ToString(reader["apellido"]),20)} |");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.WriteLine();
                    conn.Close();
                }
                break;

                case 1: 
                try
                {
                    conn.Open();

                    string query = "SELECT id, titulo from libros where estado = 1";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    Console.WriteLine(centrar_texto("------ Libros disponibles ------",35));
                    Console.WriteLine();
                    Console.WriteLine($"{centrar_texto("ID",5)} | {centrar_texto("Titulo",30)} |");

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"{centrar_texto(Convert.ToString(reader["id"]),5)} | {centrar_texto(Convert.ToString(reader["titulo"]),30)} |");
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.WriteLine();
                    conn.Close();
                }
                break;

                case 2: 
                try
                {
                    conn.Open();

                    string query = "SELECT p.id, u.nombre, u.apellido, l.titulo FROM prestamos as p JOIN libros as l ON p.id_libros = l.id JOIN usuarios as u ON p.id_usuarios = u.id WHERE p.fecha_entrega_real IS NULL";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    Console.WriteLine(centrar_texto("------ Prestamos actuales ------",75));
                    Console.WriteLine();
                    Console.WriteLine($"{centrar_texto("ID",5)} | {centrar_texto("Nombre",20)} | {centrar_texto("Apellido",20)} | {centrar_texto("Titulo",30)} |");
                    MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                 {
                        Console.WriteLine($"{centrar_texto(Convert.ToString(reader["id"]),5)} | {centrar_texto(Convert.ToString(reader["nombre"]),20)} | {centrar_texto(Convert.ToString(reader["apellido"]),20)} | {centrar_texto(Convert.ToString(reader["titulo"]),30)} |");
                 }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.WriteLine();
                    conn.Close();
                }
                break;

                case 3: 

                try
                {
                    conn.Open();

                    string query = "SELECT * FROM generos";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    Console.WriteLine(centrar_texto("------ Generos disponibles ------",35));
                    Console.WriteLine();
                    Console.WriteLine($"{centrar_texto("ID",5)} | {centrar_texto("Genero",30)} |");

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"{centrar_texto(Convert.ToString(reader["id"]),5)} | {centrar_texto(Convert.ToString(reader["descripcion"]),30)} |");
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.WriteLine();
                    conn.Close();
                }
                break;

                case 4: 
                try
                {
                    conn.Open();

                    string query = "SELECT id, titulo from libros";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    Console.WriteLine(centrar_texto("------ Generos disponibles ------",35));
                    Console.WriteLine();
                    Console.WriteLine($"{centrar_texto("ID",5)} | {centrar_texto("Titulo",30)} |");

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"{centrar_texto(Convert.ToString(reader["id"]),5)} | {reader["titulo"]} |");
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.WriteLine();
                    conn.Close();
                }
                break;

                default: break;
            }
        }
        static void crear_usuario(string string_conexion){
                string? nombre = "",apellido = "",dni = "",telefono = "",email = "";
                bool valido = false;

                while(valido == false)
                {
                Console.Write("Ingrese el Nombre: ");
                nombre = Console.ReadLine();
                Console.Write("Ingrese el Apellido: ");
                apellido = Console.ReadLine();
                Console.Write("Ingrese el DNI: ");
                dni = Console.ReadLine();
                Console.Write("Ingrese el Telefono: ");
                telefono = Console.ReadLine();
                Console.Write("Ingrese el Email: ");
                email = Console.ReadLine();

                if(nombre != "" & apellido != "" & dni != "" & telefono != "" & email != "")
                    {
                    valido = true;
                    }
                else
                    {
                    Console.WriteLine("Existen campos vacios, ingrese nuevamente");
                    Console.ReadLine();
                    Console.Clear();
                    }
                }

                MySqlConnection conn = new MySqlConnection(string_conexion);
                try
                {
                    conn.Open();
                    string query = "INSERT INTO usuarios(nombre,apellido,dni,telefono,email) VALUES (@Nombre,@Apellido,@DNI,@Telefono,@Email)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Nombre",nombre);
                    cmd.Parameters.AddWithValue("@Apellido",apellido);
                    cmd.Parameters.AddWithValue("@DNI",dni);
                    cmd.Parameters.AddWithValue("@Telefono",telefono);
                    cmd.Parameters.AddWithValue("@Email",email);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
        }

        static void actualizar_usuario(string string_conexion){
               string? telefono = "", email = "";
                int v_id = -1;
                bool valido = false;

                try
                {
                listar_tablas(0,string_conexion);

                while(valido == false)
                {
                Console.WriteLine("------ Ingresando datos de usuario a cambiar ------");

                Console.Write("Ingrese el id del usuario a cambiar: ");
                v_id = Convert.ToInt16(Console.ReadLine());

                Console.Write("Ingrese el Telefono: ");
                telefono = Console.ReadLine();
                Console.Write("Ingrese el Email: ");
                email = Console.ReadLine();

                if(telefono != "" & email != "" & v_id > 0)
                    {
                    valido = true;
                    }
                else
                    {
                    Console.WriteLine("Existen datos incorrectos, ingrese nuevamente");
                    Console.ReadLine();
                    Console.Clear();
                    }
                }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }

                MySqlConnection conn = new MySqlConnection(string_conexion);
                try
                {
                    conn.Open();
                    string query = $"UPDATE usuarios SET telefono = @Telefono, email = @Email WHERE id = {v_id}";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Telefono",telefono);
                    cmd.Parameters.AddWithValue("@Email",email);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            static void desactivar_usuario(string string_conexion){
                int v_id = -1;
                bool valido = false;
                listar_tablas(0,string_conexion);

                Console.WriteLine("------ Ingresando datos de usuario a borrar ------");

                while(valido == false)
                {
                    try
                    {
                        Console.Write("Ingrese el id del usuario a eliminar: ");
                        v_id = Convert.ToInt16(Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ocurrió un error: " + ex.Message);
                    }

                    if(v_id>0) valido = true;
                }

                MySqlConnection conn = new MySqlConnection(string_conexion);
                try
                {
                    conn.Open();
                    string query = $"UPDATE usuarios SET estado = {0} WHERE id = {v_id}";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            static void ingresar_libro(string string_conexion){
                string? titulo = "",autor = "",fecha_lanzamiento = "";
                int genero_id = -1;
                bool valido = false;

                try
                {
                    while(valido == false)
                {
                Console.WriteLine("------ Ingresando libro ------");
                Console.Write("Ingrese el titulo: ");
                titulo = Console.ReadLine();
                Console.Write("Ingrese el autor: ");
                autor = Console.ReadLine();
                listar_tablas(3,string_conexion);
                
                Console.Write("Ingrese el id del genero: ");
                genero_id = Convert.ToInt16(Console.ReadLine());
                Console.Write("Ingrese la fecha de lanzamiento (yyyy-mm-dd): ");
                fecha_lanzamiento = Console.ReadLine();

                if(titulo != "" & autor != "" & genero_id > 0 & fecha_lanzamiento != "")
                    {
                    valido = true;
                    }
                else
                    {
                    Console.WriteLine("Existen campos vacios o erroneos, ingrese nuevamente");
                    Console.ReadLine();
                    Console.Clear();
                    }
                }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }

                MySqlConnection conn = new MySqlConnection(string_conexion);
                try
                {
                    conn.Open();
                    string query = "INSERT INTO libros(titulo,autor,genero_id,fecha_lanzamiento) VALUES (@Titulo,@Autor,@Genero_id,@Fecha_lanzamiento)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Titulo",titulo);
                    cmd.Parameters.AddWithValue("@Autor",autor);
                    cmd.Parameters.AddWithValue("Genero_id",genero_id);
                    cmd.Parameters.AddWithValue("@Fecha_lanzamiento",fecha_lanzamiento);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            static void actualizar_libro(string string_conexion){
                int id_libro = -1, opcion = -1;
                bool valido = false;
                string query = "";
                MySqlConnection conn = new MySqlConnection(string_conexion);
                listar_tablas(4,string_conexion);
                listar_tablas(1,string_conexion);

                try
                {
                    while(valido == false)
                    {
                        Console.Write("Ingrese el id del libro a actualizar: ");
                        id_libro = Convert.ToInt16(Console.ReadLine());
                        if(id_libro>0) valido = true;
                    }
                    valido = false;
                    while(valido == false)
                    {
                        Console.Write("Ingrese 0 o 1 (desabilitar/habilitar libro): ");
                        opcion = Convert.ToInt16(Console.ReadLine());
                        if(opcion == 0 | opcion == 1)
                        {
                            valido = true;
                        } 
                        else
                        {
                            Console.WriteLine("Valor invalido");
                        }
                    }

                    conn.Open();
                    if(opcion == 0)
                    {
                        query = $"UPDATE libros set estado = 0 WHERE id = {id_libro}";
                    }
                    else
                    {
                        query = $"UPDATE libros set estado = 1 WHERE id = {id_libro}";
                    }
                    

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            static void desactivar_libro(string string_conexion)
            {
                int v_id = -1;
                bool valido = false;
                listar_tablas(1,string_conexion);

                Console.WriteLine("------ Ingresando datos de libro a borrar ------");

                while(valido == false)
                {
                    try
                    {
                        Console.Write("Ingrese el id del libro a eliminar: ");
                        v_id = Convert.ToInt16(Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ocurrió un error: " + ex.Message);
                    }

                    if(v_id>0) valido = true;
                }

                MySqlConnection conn = new MySqlConnection(string_conexion);
                try
                {
                    conn.Open();
                    string query = $"UPDATE libros SET estado = {0} WHERE id = {v_id}";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            static void agregar_genero(string string_conexion){
                string? descripcion = "";
                bool valido = false;

                MySqlConnection conn = new MySqlConnection(string_conexion);
                try
                {
                    while(valido == false)
                    {
                        listar_tablas(3,string_conexion);
                        Console.WriteLine("");
                        Console.WriteLine("------ Ingresando genero ------");
                        Console.Write("Ingrese el nuevo genero: ");
                        descripcion = Console.ReadLine();

                        if(descripcion != "")
                        {
                            valido = true;
                        }
                        else
                        {
                            Console.WriteLine("Campo vacio, ingrese nuevamente");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }   

                    conn.Open();
                    string query = "INSERT INTO generos(descripcion) VALUES (@Descripcion)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Descripcion",descripcion);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            static void actualizar_genero(string string_conexion){
                string? descripcion = "";
                bool valido = false;
                int v_id = -1;

                MySqlConnection conn = new MySqlConnection(string_conexion);
                try
                {
                    while(valido == false)
                    {
                        listar_tablas(3,string_conexion);
                        Console.WriteLine("------ Ingresando genero ------");

                        Console.Write("Ingrese el id del genero a cambiar: ");
                        v_id = Convert.ToInt16(Console.ReadLine());
                        Console.Write("Ingrese el nuevo genero: ");
                        descripcion = Console.ReadLine();

                        if(descripcion != "" & v_id > 0)
                        {
                            valido = true;
                        }
                        else
                        {
                            Console.WriteLine("Existen datos erroneos, ingrese nuevamente");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }

                    conn.Open();
                    string query = $"UPDATE generos SET descripcion = @Descripcion WHERE id = {v_id}";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Descripcion",descripcion);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            static void crear_prestamo(string string_conexion){
                int id_libros = -1, id_usuarios = -1;
                bool valido = false;

                MySqlConnection conn = new MySqlConnection(string_conexion);
                try
                {
                    while(valido == false)
                    {
                        listar_tablas(1,string_conexion);
                        Console.Write("Ingrese el id del libro disponible que sera alquilado: ");
                        id_libros = Convert.ToInt16(Console.ReadLine());
                        listar_tablas(0,string_conexion);
                        Console.Write("Ingrese el id del usuario que lo alquilará: ");
                        id_usuarios = Convert.ToInt16(Console.ReadLine());

                        if(id_libros>0 & id_usuarios>0)
                        {
                            valido = true;
                        }
                        else
                        {
                            Console.WriteLine("Existen campos incorrectos, ingrese nuevamente");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }

                    conn.Open();
                    string query = @$"INSERT INTO prestamos(fecha_entrega_estimada,id_libros,id_usuarios) VALUES (date_add(now(), interval 7 day),@ID_libros,@ID_usuarios)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@ID_libros",id_libros);
                    cmd.Parameters.AddWithValue("@ID_usuarios",id_usuarios);

                    cmd.ExecuteNonQuery();

                    query = $"UPDATE libros SET estado = 0 where id ={id_libros}";
                    cmd = new MySqlCommand(query, conn);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            static void actualizar_prestamo(string string_conexion){
                int id_prestamo = -1, id_usuario = -1, id_libro = -1;
                bool valido = false;
                DateTime fecha_entrega_estimada = DateTime.MinValue;
                DateTime fecha_entrega_real = DateTime.Now;
                MySqlConnection conn = new MySqlConnection(string_conexion);

                listar_tablas(2, string_conexion);

                try
                {
                    while (!valido)
                    {
                        Console.WriteLine("------ Devolución de libro ------ ");
                        Console.Write("Ingrese el id del préstamo a finalizar: ");
                        if (int.TryParse(Console.ReadLine(), out id_prestamo) && id_prestamo > 0)
                            valido = true;
                        else
                            Console.WriteLine("ID incorrecta, ingrese nuevamente");
                    }           

                    conn.Open();
                    string query = $"SELECT fecha_entrega_estimada, id_usuarios, id_libros FROM prestamos WHERE id = {id_prestamo}";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        fecha_entrega_estimada = reader.GetDateTime("fecha_entrega_estimada");
                        id_libro = reader.GetInt16("id_libros");
                        id_usuario = reader.GetInt16("id_usuarios");
                    }
                    reader.Close();

                    Console.WriteLine("------ Se establecerá el libro como entregado en este momento ------");
                    Console.ReadLine();

                    if (fecha_entrega_estimada >= fecha_entrega_real)
                    {
                        Console.WriteLine("------ Entrega correcta ------");
                    }
                    else
                    {
                        Console.WriteLine("------ Entrega fuera de término ------");

                        query = $"UPDATE usuarios SET estado = 0 WHERE id = {id_usuario}";
                        cmd = new MySqlCommand(query, conn);
                        cmd.ExecuteNonQuery();
                    }

                    query = $"UPDATE libros SET estado = 1 WHERE id = {id_libro}";
                    cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    query = $"UPDATE prestamos SET fecha_entrega_real = '{fecha_entrega_real:yyyy-MM-dd HH:mm:ss}' WHERE id = {id_prestamo}";
                    cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error: " + ex.Message);
                }
                finally 
                {
                    Console.Clear();
                    conn.Close();
                    Console.WriteLine("Volviendo al menu, presione enter");
                    Console.ReadKey();
                    Console.Clear();
                }
     }
        static void Main(string[] args)
        {
            string string_conexion = "server=localhost;database=biblioteca;user=root;pwd=123456789";
            int opcion = -1;

            crear_tablas(string_conexion);

                while (opcion != 0){
                    Console.WriteLine("----------Menu de opciones----------");
                    Console.WriteLine("Opcion 1: Crear un usuario");
                    Console.WriteLine("Opcion 2: Actualizar un usuario");
                    Console.WriteLine("Opcion 3: Borrar usuario");
                    Console.WriteLine("Opcion 4: Ingresar un libro");
                    Console.WriteLine("Opcion 5: Actualizar libro");
                    Console.WriteLine("Opcion 6: Borrar libro");
                    Console.WriteLine("Opcion 7: Agregar genero");
                    Console.WriteLine("Opcion 8: Actualizar genero");
                    Console.WriteLine("Opcion 9: Crear prestamo");
                    Console.WriteLine("Opcion 10: Actualizar prestamo");
                    Console.WriteLine("Opcion 0: Salir");
                    opcion = Convert.ToInt16(Console.ReadLine());
                    Console.Clear();

                    switch (opcion){
                    case 0: break;
                    case 1: crear_usuario(string_conexion); break;
                    case 2: actualizar_usuario(string_conexion); break;
                    case 3: desactivar_usuario(string_conexion); break;
                    case 4: ingresar_libro(string_conexion); break;
                    case 5: actualizar_libro(string_conexion); break;
                    case 6: desactivar_libro(string_conexion); break;
                    case 7: agregar_genero(string_conexion); break;
                    case 8: actualizar_genero(string_conexion); break;
                    case 9: crear_prestamo(string_conexion); break;
                    case 10: actualizar_prestamo(string_conexion); break;
                    default: Console.WriteLine($"Error, el valor {opcion} no es valido, trate nuevamente"); break;
                    
                    }
                }
                Console.WriteLine("Programa Finalizado");
        }
    }
}