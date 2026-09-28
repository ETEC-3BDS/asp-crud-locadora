using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetoLocadora.Models;

namespace ProjetoLocadora.Controllers
{
    public class VeiculosController : Controller
    {
        
        private readonly string _connectionString = "server=localhost;port=3305;database=db_locadora_veiculos;user=root;password=12345678;";

        
        public IActionResult Index()
        {
            List<Veiculo> listaVeiculos = new List<Veiculo>();

            using (var conexao = new MySqlConnection(_connectionString))
            {
                conexao.Open();
                string query = "SELECT Id, Modelo, Placa, Ano FROM carros";
                using (var cmd = new MySqlCommand(query, conexao))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaVeiculos.Add(new Veiculo
                            {
                                Id = reader.GetInt32("Id"),
                                Modelo = reader.GetString("Modelo"),
                                Placa = reader.GetString("Placa"),
                                Ano = reader.GetInt32("Ano")
                            });
                        }
                    }
                }
            }

            return View(listaVeiculos);
        }

        
        public IActionResult CadastrarVeiculo()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult CadastrarVeiculo(Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                using (var conexao = new MySqlConnection(_connectionString))
                {
                    conexao.Open();
                    string query = "INSERT INTO carros (Modelo, Placa, Ano) VALUES (@Modelo, @Placa, @Ano)";
                    using (var cmd = new MySqlCommand(query, conexao))
                    {
                        cmd.Parameters.AddWithValue("@Modelo", veiculo.Modelo);
                        cmd.Parameters.AddWithValue("@Placa", veiculo.Placa);
                        cmd.Parameters.AddWithValue("@Ano", veiculo.Ano);
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["Mensagem"] = "Veículo cadastrado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(veiculo);
        }

        
        public IActionResult Edit(int id)
        {
            Veiculo veiculo = null;

            using (var conexao = new MySqlConnection(_connectionString))
            {
                conexao.Open();
                string query = "SELECT Id, Modelo, Placa, Ano FROM carros WHERE Id = @Id";
                using (var cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            veiculo = new Veiculo
                            {
                                Id = reader.GetInt32("Id"),
                                Modelo = reader.GetString("Modelo"),
                                Placa = reader.GetString("Placa"),
                                Ano = reader.GetInt32("Ano")
                            };
                        }
                    }
                }
            }

            if (veiculo == null) return NotFound();

            return View(veiculo);
        }

        
        [HttpPost]
        public IActionResult Edit(Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                using (var conexao = new MySqlConnection(_connectionString))
                {
                    conexao.Open();
                    string query = "UPDATE carros SET Modelo=@Modelo, Placa=@Placa, Ano=@Ano WHERE Id=@Id";
                    using (var cmd = new MySqlCommand(query, conexao))
                    {
                        cmd.Parameters.AddWithValue("@Id", veiculo.Id);
                        cmd.Parameters.AddWithValue("@Modelo", veiculo.Modelo);
                        cmd.Parameters.AddWithValue("@Placa", veiculo.Placa);
                        cmd.Parameters.AddWithValue("@Ano", veiculo.Ano);
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["Mensagem"] = "Veículo atualizado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(veiculo);
        }

        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            using (var conexao = new MySqlConnection(_connectionString))
            {
                conexao.Open();
                string query = "DELETE FROM carros WHERE Id = @Id";
                using (var cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Mensagem"] = "Veículo excluído com sucesso!";
            return RedirectToAction("Index");
        }
    }
}
