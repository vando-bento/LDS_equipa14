using System;

namespace HabitTracker
{
    // Interface de leitura usada pela View para efeitos de consulta, sem permissão de alteração.
    public interface IReadOnlyHabit
    {
        string Nome { get; }
        string Descricao { get; }
        DateTime? DataConclusao { get; }

        bool ConcluidoHoje();
    }
}
