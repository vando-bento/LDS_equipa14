using System;

namespace HabitTracker {

    /// 
    /// Componente Controller do padrão MVC (Krasner e Pope, 1988).
    /// Responsável por:
    ///   - Instanciar e vincular todos os componentes (IniciarPrograma)
    ///   - Gerir o ciclo de interação com o utilizador
    ///   - Receber eventos da View e traduzir em comandos ao Model
    ///
    /// REGRA: É o único componente que conhece os outros dois.
    ///        Toda a vinculação de eventos deve ficar em IniciarPrograma()
    ///        para que o acoplamento fique concentrado num único lugar.
    /// 
    class Controller {

        private Model model;
        private View view;

        // ── Eventos do Controller para comandar a View
        /// Evento para ordenar à View que desenhe o menu principal.
        public delegate void ComandoSimples();
        public event ComandoSimples MostrarMenu;

        /// Evento para ordenar à View que peça dados de novo hábito.
        public event ComandoSimples MostrarFormularioNovoHabito;

        /// Evento para ordenar à View que peça seleção de hábito a concluir.
        public event ComandoSimples MostrarSelecaoHabito;

        // ── Inicialização e vinculação (NÃO ALTERAR) 

        /// 
        /// Ponto de entrada da aplicação (chamado pelo Program.cs).
        /// Instancia os componentes, vincula todos os eventos e arranca o ciclo.
        ///
        /// A vinculação de eventos está toda aqui — é a única zona de acoplamento
        /// da aplicação. Seguindo o padrão do professor, nenhuma classe conhece
        /// as outras fora deste método.
        /// 
        public void IniciarPrograma() {

            // 1. Criar os componentes
            // O Controller injeta a implementação concreta no Model (inversão de dependência).
            model = new Model(new HabitRepository());
            view  = new View();

            // 2. Vincular eventos — TODA a vinculação fica aqui (padrão Krasner & Pope)

            // Controller → View (o Controller comanda o que a View apresenta)
            MostrarMenu                 += view.DesenharMenuPrincipal;
            MostrarFormularioNovoHabito += view.PedirDadosNovoHabito;
            MostrarSelecaoHabito        += view.PedirSelecaoHabito;

            // Model → View (observer: o Model notifica sem saber quem recebe)
            model.ListaFoiAlterada += view.AtualizarLista;

            // View → Model (leitura com ref: a View não tem referência ao Model)
            view.PrecisoDaListaDeHabitos += model.SolicitarListaHabitos;

            // View → Controller (input: a View não chama o Controller diretamente)
            view.UtilizadorSubmeteuNovoHabito += AdicionarHabito;
            view.UtilizadorSubmeteuConclusao  += ConcluirHabito;

            // 3. Carregar dados e arrancar
            model.CarregarHabitos();
            CicloDeInteracao();
        }

        // ── Ciclo principal 

        /// 
        /// Ciclo de interação principal da aplicação.
        /// Lê a opção do utilizador e despacha para o evento correto da View.
        /// Termina quando o utilizador escolhe "0".
        /// 
        private void CicloDeInteracao() {
            // TODO: criar variável bool emExecucao = true
            //       ciclo while(emExecucao):
            //         disparar MostrarMenu()
            //         ler opção com Console.ReadLine()
            //         switch na opção:
            //           "1" → disparar MostrarFormularioNovoHabito()
            //           "2" → disparar MostrarSelecaoHabito()
            //           "0" → emExecucao = false
            //           default → view.MostrarErro("Opção inválida.")
            //throw new NotImplementedException();
           
            bool emExecucao = true;
            while (emExecucao)
            {
                MostrarMenu?.Invoke();
                string opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        MostrarFormularioNovoHabito?.Invoke();
                        break;
                    case "2":
                        MostrarSelecaoHabito?.Invoke();
                        break;
                    case "0":
                        emExecucao = false;
                        break;
                    default:
                        view.MostrarErro("Opção inválida.");
                        break;
                }
            }
        }
        

        // ── Handlers dos eventos da View

        /// 
        /// Chamado quando a View dispara UtilizadorSubmeteuNovoHabito.
        /// Traduz o pedido da View num comando ao Model.
        /// 
        /// <param name="nome">Nome do hábito submetido pela View</param>
        /// <param name="descricao">Descrição do hábito submetida pela View</param>
        private void AdicionarHabito(string nome, string descricao) {
            // TODO: chamar model.AdicionarHabito(nome, descricao)
            //       chamar view.MostrarSucesso("Hábito \"{nome}\" adicionado com sucesso.")
            
            try
            {
                model.AdicionarHabito(nome, descricao);
                view.MostrarSucesso($"Hábito \"{nome}\" adicionado com sucesso.");
            }
            catch (HabitTrackerException ex)
            {
                view.MostrarErro($"Erro ao guardar hábito: {ex.Message}");
            }

        }

        /// 
        /// Chamado quando a View dispara UtilizadorSubmeteuConclusao.
        /// Traduz o pedido da View num comando ao Model.
        /// 
        /// <param name="indice">Índice (0-based) do hábito selecionado pela View</param>
        private void ConcluirHabito(int indice) {
            // TODO: chamar model.ConcluirHabito(indice)
            //       chamar view.MostrarSucesso("Hábito marcado como concluído hoje.")
            try
            {
                model.ConcluirHabito(indice);
                view.MostrarSucesso("Hábito marcado como concluído hoje.");
            }
            catch (HabitTrackerException ex)
            {
                view.MostrarErro($"Erro ao concluir hábito: {ex.Message}");
            }

        }
    }
}
