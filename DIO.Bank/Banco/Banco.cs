using System;
using System.Linq;
using System.Collections.Generic;

namespace DIO.Bank
{
    /// <summary>
    /// Classe responsavel pelas operacoes bancarias
    /// </summary>
    public class Banco
    {
        /// <summary>
        /// Listagem de contas
        /// </summary>
        public List<Conta> Contas {get; private set;}

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Banco()
        {
            this.Contas = new List<Conta>();
        }

        /// <summary>
        /// Gera um codigo de identificacao de conta de 4 digitos.
        /// Esse codigo eh gerado aleatoriamente e composto de letras maiusculas e numeros
        /// Nao sera gerado um codigo ja usado nas contas dessa instancia
        /// </summary>
        /// <returns> Um codigo de conta de 4 digitos. </returns>
        private string GerarCodigoConta()
        {
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

        /// <summary>
        /// Cria uma conta usando os parametros.
        /// </summary>
        /// <param name="tipo">O tipo da conta.</param>
        /// <param name="nome">O titular da conta.</param>
        /// <param name="senha">A senha.</param>
        /// <returns> O codigo da conta criada. </returns>
        public string CriarConta(TipoConta tipo, string nome, string senha)
        {
            Identificacao id = new Identificacao(nome, senha);
            string codigo = GerarCodigoConta();
            Conta c = new Conta(tipo, 0.0, 1000.0, id, codigo);
            Contas.Add(c);
            return codigo;
        }

        /// <summary>
        /// Verifica se uma conta com o codigo existe.
        /// </summary>
        /// <param name="codigo">O codigo a ser verificado.</param>
        /// <returns> true se a conta corresponde a alguma conta, senao false. </returns>
        public bool ContaExiste(string codigo) =>
            Contas.Any(c => c.Codigo == codigo);

        /// <summary>
        /// Verifica se a senha de uma conta esta correta.
        /// </summary>
        /// <param name="codigo">O codigo da conta.</param>
        /// <param name="senha">A senha a ser verificada.</param>
        /// <returns> true se a senha esta correta, senao false. </returns>
        public bool Autenticar(string codigo, string senha) =>
            Contas.Any(
                conta => conta.Codigo == codigo && conta.Id.Verificar(senha)
                );

        /// <summary>
        /// Retorna os detalhes da conta.
        /// </summary>
        /// <param name="codigo">O codigo da conta.</param>
        /// <returns> Os detalhes da conta. </returns>
        public string Extrato(string codigo) =>
            Contas.Find(c => c.Codigo == codigo).ToString();

        /// <summary>
        /// Saca o valor de uma conta
        /// </summary>
        /// <param name="codigo">O codigo da conta.</param>
        /// <param name="valor">A quantidade a ser sacada.</param>
        /// <returns> true se o saque ocorreu com sucesso, senao false </returns>
        public bool Sacar(string codigo, double valor) =>
            Contas.Find(c => c.Codigo == codigo).Sacar(valor);
        
        /// <summary>
        /// Deposita o valor em uma conta.
        /// </summary>
        /// <param name="codigo">O codigo da conta.</param>
        /// <param name="valor">A quantidade a ser depositada.</param>
        public void Depositar(string codigo, double valor) =>
            Contas.Find(c => c.Codigo == codigo).Depositar(valor);

        /// <summary>
        /// Transfere o valor entre contas
        /// </summary>
        /// <param name="origem">O codigo da conta da qual o valor sera retirado.</param>
        /// <param name="destino">O codigo da conta na qual o valor sera depositado.</param>
        /// <param name="valor">O valor a ser transferido.</param>
        /// <returns> true se o valor foi transferido, false se nao ha saldo suficiente. </returns>
        public bool Transferir(string origem, string destino, double valor)
        {
            var contaOrigem = Contas.Find(c => c.Codigo == origem);
            var contaDestino = Contas.Find(c => c.Codigo == destino);
            return contaOrigem.Transferir(valor, contaDestino);
        }
    }
}