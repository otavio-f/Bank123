using System;

namespace DIO.Bank
{
    class Program
    {
        static Banco banco;
        static string TEXTO_RETORNAR = "Pressione <Enter> para retornar ao menu principal.";

        static void Main(string[] args)
        {
            bool rodando = true;
            banco = new Banco();
            while (rodando) 
            {
                string opcao = ObterOpcao();
                switch(opcao)
                {
                    case "":
                        break;
                    case "1":
                        Extrato();
                        break;
                    case "2":
                        NovaConta();
                        break;
                    case "3":
                        Transferir();
                        break;
                    case "4":
                        Sacar();
                        break;
                    case "5":
                        Depositar();
                        break;
                    case "X":
                        rodando = false;
                        AnuncioTopo(
                            "Obrigado por usar nossos serviços.",
                            "Pressione <Enter> para encerrar."
                        );
                        Textao(true);
                        break;
                    default:
                        Textao(true,
                            "Opção inválida selecionada!",
                            TEXTO_RETORNAR
                        );
                        break;
                }
            }
            Console.Clear();
        }

        public static void Extrato() 
        {
            AnuncioTopo(
                "Extrato da conta",
                "Por favor forneça os dados:");
            
            string conta;

            try
            {
                conta = ObterConta();
                ValidarIdentidade(conta);
            }
            catch (InvalidOperationException)
            {
                return;
            }
            
            AnuncioTopo(
                "Extrato da conta",
                "Dados da conta:");
            Linha(2);
            Textao(true,
                banco.Extrato(conta),
                TEXTO_RETORNAR
            );
        }

        public static void NovaConta()
        {
            AnuncioTopo("Criação de conta.", "Por favor forneça os dados e aperte Enter.");
            Linha();
            string entrada = Perguntar("Digite 1 para Conta Física ou 2 para Conta Jurídica: ");
            
            TipoConta tipo;
            switch (entrada) 
            {
                case "1":
                    tipo = TipoConta.PessoaFisica;
                    break;
                case "2":
                    tipo = TipoConta.PessoaJuridica;
                    break;
                default:
                    Textao(true,
                        "Opção inválida.",
                        "Por favor tente novamente.",
                        TEXTO_RETORNAR
                    );
                    return;
            }

            Linha();
            string nome = Perguntar("Informe o nome: ");

            Linha();
            string senha = Perguntar("Informe a senha: "); //Senha visivel

            var codigo = banco.AdicionarConta(tipo, nome, senha);
            
            AnuncioTopo("Criação de conta.", "Operação concluída com êxito!");
            Linha(2);
            Textao(true,
                "Sua conta foi criada com sucesso!",
                "Dados da sua conta:",
                banco.Extrato(codigo)
            );
        }

        public static void Transferir()
        {
            AnuncioTopo(
                "Transferência de dinheiro",
                "Por favor forneça os dados e aperte Enter.");
            
            string origem;
            string destino;
            double valor;
            
            try
            {
                Textao(false,
                    "Dados da conta de origem"
                );
                origem = ObterConta();
                Textao(false,
                    "Dados da conta de destino"
                );
                destino = ObterConta();
                valor = ObterValor();
                ValidarIdentidade(origem);
            }
            catch (InvalidOperationException)
            {
                return;
            }
            
            if (!banco.Transferir(origem, destino, valor))
            {
                Textao(true,
                    "Saldo não é o suficiente para transferência.",
                    TEXTO_RETORNAR
                );
                return;
            }

            AnuncioTopo(
                "Transferência de dinheiro",
                "Operação concluída com êxito!");
            Linha(2);
            Textao(true,
                "Foi transferido o valor de sua conta para outra.",
                banco.Extrato(origem),
                TEXTO_RETORNAR
            ); 

        }

        public static void Sacar()
        {
            AnuncioTopo(
                "Saque de dinheiro",
                "Por favor forneça os dados:");
            
            string conta;
            double valor;

            try
            {
                conta = ObterConta();
                valor = ObterValor();
                ValidarIdentidade(conta);
            }
            catch (InvalidOperationException)
            {
                return;
            }

            if (!banco.Sacar(conta, valor))
            {
                Textao(true,
                    "Saldo não é suficiente para saque.",
                    TEXTO_RETORNAR
                );
                return;
            }

            AnuncioTopo(
                "Saque de dinheiro",
                "Operação concluída com êxito!");
            Linha(2);
            Textao(true,
                "Foi sacado o valor de sua conta.",
                banco.Extrato(conta),
                TEXTO_RETORNAR
            ); 
        }

        public static void Depositar()
        {
            AnuncioTopo(
                "Depósito de dinheiro",
                "Por favor forneça os dados e aperte Enter.");
            string conta;
            double valor;

            try {
                conta = ObterConta();
                valor = ObterValor();
                ValidarIdentidade(conta);
            } catch (InvalidOperationException) {
                return;
            }
            
            banco.Depositar(conta, valor);
            
            AnuncioTopo(
                "Depósito de dinheiro",
                "Operação concluída com êxito!");
            Linha(2);
            Textao(true,
                "O valor foi depositado na sua conta.",
                banco.Extrato(conta),
                TEXTO_RETORNAR
            );
        }

        private static void AnuncioTopo(string texto, string informe)
        {
            Console.Clear();
            Console.WriteLine(texto);
            Console.WriteLine(informe);
            Console.WriteLine();
        }

        private static string Perguntar(string texto) {
            Console.Write(texto);
            return Console.ReadLine();
        }

        private static void Textao(bool pressioneAoFinal, params string[] linhas)
        {
            foreach(string linha in linhas)
            {
                Console.WriteLine(linha);
            }
            if (pressioneAoFinal)
                Console.ReadLine();
        }

        private static void Linha(int quantidade=1)
        {
            for(int i=0; i<quantidade; i++)
                Console.WriteLine();
        }

        private static string ObterOpcao()
        {
            AnuncioTopo("DIO Bank a seu dispor!!!", "Informe a opção desejada:");

            Textao(false,
                "1 - Extrato",
                "2 - Criar nova conta",
                "3 - Transferência",
                "4 - Saque",
                "5 - Depósito",
                "X - Sair",
                ""
            );

            string opcao = Perguntar("").ToUpper();
            Linha();
            return opcao;
        }

        private static string ObterConta()
        {
            string codigo = Perguntar("Codigo da conta: ");
            if (!banco.ContaExiste(codigo))
            {
                Textao(true,
                    "A conta selecionada não existe!",
                    TEXTO_RETORNAR
                );
                throw new InvalidOperationException();
            }
            return codigo;
        }

        private static double ObterValor()
        {
            double valor;
            if (!Double.TryParse(Perguntar("Valor: "), out valor))
            {
                Textao(true,
                    "Valor incorreto.",
                    TEXTO_RETORNAR
                );
                throw new InvalidOperationException();
            }
            return valor;
        }

        private static void ValidarIdentidade(string conta)
        {
            string senha = Perguntar("Senha: ");
            if (!banco.Logar(conta, senha)) 
            {
                Textao(true,
                    "Senha incorreta.",
                    TEXTO_RETORNAR
                );
                throw new InvalidOperationException();
            }
        }
    }
}
