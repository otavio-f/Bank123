using System;

namespace DIO.Bank
{
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

        public bool Sacar(double valor)
        {
            if (valor > this.Saldo + this.Credito)
            {
                return false;
            }
            this.Saldo -= valor;
            return true;
        }

        public void Depositar(double valor)
        {
            this.Saldo += valor;
        }

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

        public override string ToString()
        {
            return String.Format(
                "{0} | Conta {1} | TipoConta: {2} | Saldo: {3:0.00}$ | Crédito disponível: {4:0.00}$",
                this.Id.Nome, this.Codigo, this.TipoConta, this.Saldo, this.Credito);
        }

    }
}