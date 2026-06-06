using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HabitTracker {

    
    /// Responsável por:
    ///   - Guardar e carregar a lista de hábitos via Json.NET
    ///   - Executar a lógica de negócio (adicionar, concluir)
    ///   - Notificar a View quando os dados mudam (via eventos)
    
    /// REGRA: O Model não conhece a View nem o Controller.
    ///        Comunica apenas através dos eventos declarados abaixo.
    ///        Nunca instanciar View ou Controller aqui.
   
    class Model {

        // ── Estado interno ─────────────────────────────────────────────────

        /// Lista de hábitos em memória.
        private List<Habit> habitos;

        /// Repositório para persistência dos hábitos.
        private HabitRepository repo = new HabitRepository();

        /// Caminho do ficheiro JSON de persistência.
        //private const string FicheiroJSON = "habitos.json";  - Tratado no HabitRepository

        /// Disparado sempre que a lista de hábitos é alterada.
        /// A View subscreve este evento para se atualizar automaticamente.
        /// Vinculação feita no Controller: model.ListaFoiAlterada += view.AtualizarLista
       
        public delegate void NotificarAlteracao();
        public event NotificarAlteracao ListaFoiAlterada;

       
        /// Fornece a lista de hábitos à View quando esta a solicita.
        /// Usa ref para evitar que a View tenha referência direta ao Model.
        /// Vinculação feita no Controller: view.PrecisoDaListaDeHabitos += model.SolicitarListaHabitos
      
        public void SolicitarListaHabitos(ref List<IReadOnlyHabit> lista) {
            // Devolve a lista de hábitos atual para a View.
            // Usar ref para evitar que a View tenha referência direta ao Model.
            lista = new List<IReadOnlyHabit>();

            foreach (Habit h in habitos)
            {
                lista.Add(h);
            }

        }

        /// Inicialização ──────────────────────────────────────────────────

     
        /// Construtor. Inicializa a lista de hábitos vazia.
        /// O carregamento do ficheiro é feito em CarregarHabitos().
       
        public Model() {
            habitos = new List<Habit>();
        }

        /// Carrega os hábitos do ficheiro JSON para memória.
        /// Se o ficheiro não existir, mantém a lista vazia (primeira execução).
    
        public void CarregarHabitos() {
            habitos = repo.LerHabitos();
        }

        /// Funcionalidades 

        /// Cria um novo hábito e adiciona-o à lista.
        /// Guarda os dados no ficheiro JSON.
        /// Notifica a View disparando o evento ListaFoiAlterada.
       
        /// <param name="nome">Nome do hábito</param>
        /// <param name="descricao">Descrição do hábito</param>
        public void AdicionarHabito(string nome, string descricao) {
            Habit novoHabito = new Habit(nome, descricao);
            habitos.Add(novoHabito);
            GuardarHabitos();
            ListaFoiAlterada?.Invoke();
        }

        
        /// Marca um hábito como concluído hoje, dado o seu índice na lista.
        /// Guarda os dados no ficheiro JSON.
        /// Notifica a View disparando o evento ListaFoiAlterada.
     
        /// <param name="indice">Índice do hábito na lista (0-based)</param>
        public void ConcluirHabito(int indice) {
            if (indice >= 0 && indice < habitos.Count) {
                habitos[indice].DataConclusao = DateTime.Now;
                GuardarHabitos();
                ListaFoiAlterada?.Invoke();
            }
        }

        //  Persistência (privado — apenas o Model toca aqui) 

        /// 
        /// Serializa a lista de hábitos e guarda no ficheiro JSON.
        /// Usar JsonConvert.SerializeObject com Formatting.Indented.
        /// 
        private void GuardarHabitos() {
            repo.SalvarHabitos(habitos);
        }
    }
}
