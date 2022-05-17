using System;
using System.Linq;
using System.Collections.Generic;

namespace DIO.Bank
{

    public class Banco
    {
        public List<Conta> Contas {get; private set;}

        public Banco()
        {
            this.Contas = new List<Conta>();
        }

        private string GerarCodigoConta()
        { //gera um codigo de 4 digitos, letras e numeros
            var random = new Random();
            string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890";
            string codigo = "";
            for (int i=0; i<4; i++)
            {
                var indice = (int) (random.NextDouble() * alfabeto.Length);
                char escolha = alfabeto[indice];
                codigo += escolha;
            }
            if (Contas.Any(conta => conta.Codigo == codigo))
                return GerarCodigoConta(); // Caso o codigo ja exista!
            return codigo;
        }

        public string AdicionarConta(TipoConta tipo, string nome, string senha)
        {
            Identificacao id = new Identificacao(nome, senha);
            string codigo = GerarCodigoConta();
            Conta c = new Conta(tipo, 0.0, 1000.0, id, codigo);
            Contas.Add(c);
            return codigo;
        }

        public bool ContaExiste(string codigo) =>
            Contas.Any(c => c.Codigo == codigo);

        public bool Logar(string codigo, string senha) =>
            Contas.Any(
                conta => conta.Codigo == codigo && conta.Id.Verificar(senha)
                );

        public string Extrato(string codigo) =>
            Contas.Find(c => c.Codigo == codigo).ToString();

        public bool Sacar(string codigo, double valor) =>
            Contas.Find(c => c.Codigo == codigo).Sacar(valor);
        
        public void Depositar(string codigo, double valor) =>
            Contas.Find(c => c.Codigo == codigo).Depositar(valor);

        public bool Transferir(string origem, string destino, double valor)
        {
            var contaOrigem = Contas.Find(c => c.Codigo == origem);
            var contaDestino = Contas.Find(c => c.Codigo == destino);
            return contaOrigem.Transferir(valor, contaDestino);
        }
    }
}