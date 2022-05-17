using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace DIO.Bank
{
    public class Identificacao
    {
        public Byte[] SenhaTemperada {get; private set;}
        public Byte[] Sal {get; private set;}
        public string Nome {get; private set;}

        public Identificacao(string nome, string senha)
        {
            this.Nome = nome;
            this.Sal = MexerSaleiro(32);
            this.SenhaTemperada = Salgar(senha, this.Sal);
        }

        public bool Verificar(string senha)
        {
            var outra = Salgar(senha, this.Sal);
            return this.SenhaTemperada.SequenceEqual(outra);
        }

        private static Byte[] MexerSaleiro(int tamanho)
        {
            var randGen = new System.Random();
            var sal = new Byte[tamanho];
            randGen.NextBytes(sal);
            return sal;
        }

        private static Byte[] Salgar(string senha, Byte[] sal)
        {
            using(var hash = SHA256.Create())
            {
                var conteudo = Encoding.UTF8.GetBytes(senha);
                // deve tem outros jeitos mais avancados
                //mas vou simplesmente juntar o sal com a senha
                Byte[] salgado = conteudo.Concat(sal).ToArray();
                return hash.ComputeHash(salgado);
            }
        }
    }
}