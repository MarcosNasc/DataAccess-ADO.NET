using Microsoft.Data.SqlClient;

namespace BaltaDataAccess
{
    class Program
    {
        const string connectionString = "Server=localhost;Database=crud;Integrated Security=True;TrustServerCertificate=True";
        public static void Main(string[] args)
        {

            Menu();
        }

        public static void Menu()
        {
            Console.WriteLine("Gerenciar Contatos");
            Console.WriteLine("[C] Criar novo Contato");
            Console.WriteLine("[R] Listar todos Contatos");
            Console.WriteLine("[U] Atualizar Contato");
            Console.WriteLine("[D] Deletar Contato");
            Console.WriteLine("[E] Sair");

            var option = Console.ReadLine().ToUpper();
            int id;
            switch (option)
            {
                case "C":
                    Create();
                    break;
                case "R":
                    Read();
                    break;
                case "U":
                    Console.WriteLine("Informe o ID do Contato");
                    id = int.Parse(Console.ReadLine());
                    var contact = UpdateModel();
                    Update(id, contact);
                    break;
                case "D":
                    Console.WriteLine("Informe o ID do Contato");
                    id = int.Parse(Console.ReadLine());
                    Delete(id);
                    break;
                case "E":
                    Environment.Exit(0);
                    break;
            }
        }

        public static void Create()
        {

            var contact = new Contact();

            Console.WriteLine("Cadastro de Contatos");
            Console.WriteLine("Informe o Nome do Contato: ");
            contact.Name = Console.ReadLine();
            Console.WriteLine("Infome o Email do Contato ");
            contact.Email = Console.ReadLine();
            Console.WriteLine("Informe o Telefone do Contato");
            contact.Phone = Console.ReadLine();
            Console.WriteLine("Informe a data de nascimento do Contato");
            var birthDateString = Console.ReadLine();
            contact.Birthdate = DateTime.Parse(birthDateString);
            contact.CreateDate = DateTime.Now;

            Save(contact);
        }

        public static void Save(Contact contact)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@Name", contact.Name);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@Phone", contact.Phone);
                    command.Parameters.AddWithValue("@Birthdate", contact.Birthdate);
                    command.Parameters.AddWithValue("@Createdate", contact.CreateDate);

                    command.CommandText = $@"INSERT INTO 
                                            Contact(
                                                Name,
                                                Email,
                                                Phone,
                                                Birthdate,
                                                CreateDate
                                            ) 
                                            VALUES(
                                                @Name,
                                                @Email,
                                                @Phone,
                                                @Birthdate,
                                                @Createdate
                                            )";

                    var rows = command.ExecuteNonQuery();

                    Console.WriteLine($"{rows} linha Afetada"!);
                }
                connection.Close();
            }

        }

        public static void Read()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM Contact ";

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetInt32(0)} - {reader.GetString(1)}");
                    }
                }
                connection.Close();
            }
        }

        public static void Update(int id, Contact contact)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Conectado!");
                connection.Open();
                using (var command = new SqlCommand())
                {

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Name", contact.Name);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@Phone", contact.Phone);
                    command.CommandText = @"UPDATE Contact
                                                SET 
                                                    Name = @Name,
                                                    Email = @Email,
                                                    Phone = @Phone
                                                WHERE id = @Id
                                            ";

                    var rows = command.ExecuteNonQuery();

                    Console.WriteLine($"{rows} linha afetada!");

                }
                connection.Close();
            }

        }

        public static Contact UpdateModel()
        {
            var contact = new Contact();

            Console.WriteLine("Atualizar dados do Contato");
            Console.WriteLine("Informe o Nome do Contato: ");
            contact.Name = Console.ReadLine();
            Console.WriteLine("Infome o Email do Contato ");
            contact.Email = Console.ReadLine();
            Console.WriteLine("Informe o Telefone do Contato");
            contact.Phone = Console.ReadLine();

            return contact;
        }
        public static void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Conectado!");
                connection.Open();
                using (var command = new SqlCommand())
                {

                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@Id", id);
                    command.CommandText = @"DELETE FROM Contact
                                                WHERE id = @Id
                                            ";

                    var rows = command.ExecuteNonQuery();

                    Console.WriteLine($"{rows} linha afetada!");

                }
                connection.Close();
            }

        }
    }
}