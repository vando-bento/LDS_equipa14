using System;

namespace HabitTracker {

    /// 
    /// Representa um hábito do utilizador.
    /// Esta classe é usada pelo Model para guardar e carregar dados via Json.NET.
    /// Não deve ter dependências de View nem de Controller.
    /// 
    public class Habit : IReadOnlyHabit
    {

        // ── Propriedades ───────────────────────────────────────────────────

        /// Nome do hábito. Ex: "Beber água"
        public string Nome { get; set; }

        /// Descrição breve do hábito. Ex: "Beber 2L por dia"
        public string Descricao { get; set; }

        /// 
        /// Data e hora em que o hábito foi concluído.
        /// Null se ainda não foi concluído.
        /// 
        public DateTime? DataConclusao { get; set; }

        // ── Construtores ───────────────────────────────────────────────────

        /// 
        /// Construtor vazio obrigatório para o Json.NET conseguir deserializar.
        /// Não remover.
        /// 
        public Habit() { }

        /// 
        /// Construtor principal. Cria um hábito novo sem data de conclusão.
        /// 
        /// <param name="nome">Nome do hábito</param>
        /// <param name="descricao">Descrição do hábito</param>
        public Habit(string nome, string descricao) {
            Nome = nome;
            Descricao = descricao;
            DataConclusao = null;
        }

        // ── Métodos ────────────────────────────────────────────────────────

        /// 
        /// Indica se o hábito foi concluído hoje.
        /// Compara a data de DataConclusao com DateTime.Today.
        /// 
        /// <returns>True se concluído hoje, False caso contrário</returns>
        public bool ConcluidoHoje() {
            if (DataConclusao.HasValue && DataConclusao.Value.Date == DateTime.Today) {
                return true;
            }
            return false;
        }
    }
}
