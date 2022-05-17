using System;

namespace DIO.Bank
{
    /// <summary>
    /// Classe que armazena os detalhes de uma conta bancaria.
    /// </summary>
    public class Conta
    {
        private TipoConta TipoConta {get; set;}
        private double Saldo {get; set;}
        private double Credito {get; set;}
        public Identificacao Id {get; private set;}
        public string Codigo {get; init;}

        public Conta(TipoConta tipo, double saldo, double credito, Identificacao id, string codigo)
        {
            this.TipoConta = tipo;
            this.Saldo = saldo;
            this.Credito = credito;
            this.Id = id;
            this.Codigo = codigo;
        }

        /// <summary>
        /// Saca o valor da conta
        /// </summary>
        /// <param name="valor">O valor a ser sacado.</param>
        /// <returns> true se o valor foi sacado com sucesso ou false se a conta nao possui
        /// saldo e credito suficientes. </returns>
        public bool Sacar(double valor)
        {
            if (valor > this.Saldo + this.Credito)
            {
                return false;
            }
            this.Saldo -= valor;
            return true;
        }

        /// <summary>
        /// Deposita o valor nessa conta
        /// </summary>
        /// <param name="valor">O valor a ser depositado na conta.</param>
        public void Depositar(double valor)
        {
            this.Saldo += valor;
        }

        /// <summary>
        /// Transfere o valor dessa conta para outra.
        /// </summary>
        /// <param name="valor">O valor a ser transferido dessa conta.</param>
        /// <param name="destino">A conta de destino que recebera o valor.</param>
        /// <returns> true se a transferencia teve exito, ou false se nao ha recursos
        /// suficientes para transferir. </returns>
        public bool Transferir(double valor, Conta destino)
        {
            if (valor > this.Saldo)
            {
                return false;
            }
            this.Saldo -= valor;
            destino.Depositar(valor);
            return true;
        }

        /// <summary>
        /// Retorna os detalhes da conta.
        /// </summary>
        /// <returns> Detalhes dessa conta. </returns>
        public override string ToString()
        {
            return String.Format(
                "{0} | Conta {1} | TipoConta: {2} | Saldo: {3:0.00}$ | Crédito disponível: {4:0.00}$",
                this.Id.Nome, this.Codigo, this.TipoConta, this.Saldo, this.Credito);
        }

    }
}