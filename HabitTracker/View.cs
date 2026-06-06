using System;
using System.Collections.Generic;

namespace HabitTracker
{
    /// Componente View do padrão MVC.
    /// Responsável por:
    ///   - Apresentar informação ao utilizador na consola
    ///   - Recolher input do utilizador
    ///   - Notificar o Controller quando o utilizador age
    ///   - Pedir dados ao Model através de eventos, sem o conhecer diretamente
    class View
    {

        // ── Eventos de comunicação com o Model ─────────────────────────────

        public delegate void SolicitacaoListaHabitos(ref List<IReadOnlyHabit> lista);
        public event SolicitacaoListaHabitos PrecisoDaListaDeHabitos;

        // ── Eventos de input para o Controller ─────────────────────────────

        public delegate void PedidoComNomeDescricao(string nome, string descricao);
        public event PedidoComNomeDescricao UtilizadorSubmeteuNovoHabito;

        public delegate void PedidoComIndice(int indice);
        public event PedidoComIndice UtilizadorSubmeteuConclusao;

        public View() { }

        // ── Menu principal ─────────────────────────────────────────────────

        public void DesenharMenuPrincipal()
        {
            Console.Clear();

            Console.WriteLine("=================================");
            Console.WriteLine("        Habit Tracker");
            Console.WriteLine("=================================");
            Console.WriteLine();

            AtualizarLista();

            Console.WriteLine();
            Console.WriteLine("1 - Adicionar hábito");
            Console.WriteLine("2 - Concluir hábito");
            Console.WriteLine("0 - Sair");
            Console.WriteLine();
            Console.Write("Opção: ");
        }

        // ── Lista de hábitos ───────────────────────────────────────────────

        public void AtualizarLista()
        {
            List<IReadOnlyHabit> lista = ObterListaHabitos();

            Console.WriteLine("── Lista de hábitos ──");

            if (lista == null || lista.Count == 0)
            {
                Console.WriteLine("(sem hábitos registados)");
                return;
            }

            for (int i = 0; i < lista.Count; i++)
            {
                IReadOnlyHabit habito = lista[i];

                string estado = habito.ConcluidoHoje() ? "[✓]" : "[ ]";

                Console.WriteLine(
                    $"{i + 1}. {estado} {habito.Nome} - {habito.Descricao}"
                );
            }
        }

        private List<IReadOnlyHabit> ObterListaHabitos()
        {
            List<IReadOnlyHabit> lista = null;
            PrecisoDaListaDeHabitos?.Invoke(ref lista);

            if (lista == null)
            {
                lista = new List<IReadOnlyHabit>();
            }

            return lista;
        }

        // ── Adicionar hábito ───────────────────────────────────────────────

        public void PedirDadosNovoHabito()
        {
            Console.Clear();

            Console.WriteLine("── Adicionar hábito ──");
            Console.WriteLine();

            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Descrição: ");
            string descricao = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nome))
            {
                MostrarErro("O nome do hábito não pode estar vazio.");
                return;
            }

            if (string.IsNullOrWhiteSpace(descricao))
            {
                MostrarErro("A descrição do hábito não pode estar vazia.");
                return;
            }

            UtilizadorSubmeteuNovoHabito?.Invoke(nome.Trim(), descricao.Trim());
        }

        // ── Concluir hábito ────────────────────────────────────────────────

        public void PedirSelecaoHabito()
        {
            Console.Clear();

            Console.WriteLine("── Concluir hábito ──");
            Console.WriteLine();

            List<IReadOnlyHabit> lista = ObterListaHabitos();

            if (lista.Count == 0)
            {
                Console.WriteLine("(sem hábitos registados)");
                MostrarErro("Não existem hábitos para concluir.");
                return;
            }

            for (int i = 0; i < lista.Count; i++)
            {
                IReadOnlyHabit habito = lista[i];
                string estado = habito.ConcluidoHoje() ? "[✓]" : "[ ]";

                Console.WriteLine(
                    $"{i + 1}. {estado} {habito.Nome} - {habito.Descricao}"
                );
            }

            Console.WriteLine();
            Console.Write("Número do hábito a concluir: ");
            string entrada = Console.ReadLine();

            int numeroEscolhido;

            if (!int.TryParse(entrada, out numeroEscolhido))
            {
                MostrarErro("Entrada inválida. Deve introduzir um número.");
                return;
            }

            int indice = numeroEscolhido - 1;

            if (indice < 0 || indice >= lista.Count)
            {
                MostrarErro("O número escolhido não corresponde a nenhum hábito.");
                return;
            }

            UtilizadorSubmeteuConclusao?.Invoke(indice);
        }

        // ── Feedback ao utilizador ─────────────────────────────────────────

        public void MostrarSucesso(string mensagem)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {mensagem}");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Prima qualquer tecla para continuar...");
            Console.ReadKey();
        }

        public void MostrarErro(string mensagem)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ {mensagem}");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Prima qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}