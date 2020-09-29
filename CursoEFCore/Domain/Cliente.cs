using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoEFCoreConsole.Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }  
        public string Cidade { get; set; }  
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{Nome} ({Email} - {Telefone}) - {CEP} - {Cidade}/{Estado}";
        }
    }
}