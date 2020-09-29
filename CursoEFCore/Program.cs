using System;
using System.Collections.Generic;
using System.Linq;
using CursoEFCoreConsole.Data;
using CursoEFCoreConsole.Domain;
using CursoEFCoreConsole.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new Data.ApplicationContext();

            // db.Database.Migrate();
            var existe = db.Database.GetPendingMigrations().Any();
            if (existe)
            {
                // TODO:
            }

            // InserirDados();
            // InserirDadosEmMassa();
            // ConsultarDados();
            // CadastrarPedido();
            // ConsultarPedidoCarregamentoAdiantado();
            // AtualizaDados();
            RemoverRegistro();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            // var cliente = db.Clientes.Find(3);
            var cliente = new Cliente { Id = 4 };
            db.Clientes.Remove(cliente);
            // db.Remove(cliente);
            // db.Entry(cliente).State = EntityState.Deleted;

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de registro(s): {registros}");
        }

        private static void AtualizaDados()
        {
            using var db = new Data.ApplicationContext();
            // var cliente = db.Clientes.Find(2);
            // cliente.Nome = "José Eduardo";
            // db.Clientes.Update(cliente);
            var cliente = new Cliente
            {
                Id = 2,
            };

            var clienteDesconectado = new
            {
                CEP = "89211590"
            };

            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de registro(s): {registros}");
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = db.Pedidos
            .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
            .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0.5m,
                        Quantidade = 1,
                        Valor = 9.5m,
                    }
                }
            };

            db.Pedidos.Add(pedido);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de registro(s): {registros}");
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();

            // var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var consultaPorMetodo = db.Clientes
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Nome)
                .ToList();
            foreach (var cliente in consultaPorMetodo)
            {
                // Console.WriteLine($"Consultando cliente: {cliente.Id}");
                Console.WriteLine(cliente);
                // db.Clientes.Find(cliente.Id);
                // db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste 3",
                CodigoBarras = "87264531395611",
                Valor = 5m,
                TipoProduto = TipoProduto.Servico,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Onon da Silva",
                Email = "onon.silva@email.com",
                CEP = "89200000",
                Cidade = "Joinville",
                Estado = "SC",
                Telefone = "47991153545"
            };

            var lstClientes = new[]
            {
                new Cliente
                {
                    Nome = "Beltrano de Souza",
                    Email = "beltrano.souza@email.com",
                    CEP = "89226330",
                    Cidade = "Joinville",
                    Estado = "SC",
                    Telefone = "47988239874"
                },
                new Cliente
                {
                    Nome = "Ciclano Pereira",
                    Email = "ciclano.pereira@email.com",
                    CEP = "89226001",
                    Cidade = "Joinville",
                    Estado = "SC",
                    Telefone = "47991150325"
                }
            };

            using var db = new ApplicationContext();

            db.AddRange(produto, cliente);
            db.Clientes.AddRange(lstClientes);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de registro(s): {registros}");
        }
       
        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "10956879395611",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new ApplicationContext();

            db.Produtos.Add(produto);
            // db.Set<Produto>().Add(produto);
            // db.Entry(produto).State = EntityState.Added;
            // db.Add(produto);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de registro(s): {registros}");
        }
    }
}
