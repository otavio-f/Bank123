using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace DIO.Bank
{
    /// <summary>
    /// Classe responsavel pela validacao da identidade
    /// </summary>
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

        /// <summary>
        /// Verifica se a senha corresponde a senha cadastrada nessa Identidade
        /// </summary>
        /// <param name="senha">A senha a ser verificada.</param>
        /// <returns> true se a senha corresponde, senao false. </returns>
        public bool Verificar(string senha)
        {
            var outra = Salgar(senha, this.Sal);
            return this.SenhaTemperada.SequenceEqual(outra);
        }

        /// <summary>
        /// Cria um array de bytes aleatorios (sal)
        /// </summary>
        /// <param name="tamanho">O tamanho do sal.</param>
        /// <returns> O conjunto de bytes. </returns>
        private static Byte[] MexerSaleiro(int tamanho)
        {
            var randGen = new System.Random();
            var sal = new Byte[tamanho];
            randGen.NextBytes(sal);
            return sal;
        }

        /// <summary>
        /// Faz o procedimento de \"salt\" em uma senha.
        /// O sal eh adicionado a senha, e entao eh criado um hash
        /// </summary>
        /// <param name="sal">Um array de bytes para ser adicionados no hash.</param>
        /// <param name="senha">A senha a ser salgada.</param>
        /// <returns> O hash da senha. </returns>
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