using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemSummaryViewmodel
    {
        public int TodoItemId { get; }
        public string Title { get; }
        public UserSummaryViewmodel ResponsibleParty { get; }
        public bool IsDone { get; }
        public Importance Importance { get; }
        public int Rank { get; }

        public TodoItemSummaryViewmodel(int todoItemId, string title, bool isDone, UserSummaryViewmodel responsibleParty, Importance importance, int rank)
        {
            TodoItemId = todoItemId;
            Title = title;
            IsDone = isDone;
            ResponsibleParty = responsibleParty;
            Importance = importance;
            Rank = rank;
        }

        public string GetImportanceClass()
        {
            string result;
            switch (Importance)
            {
                case Importance.High:
                    result = "list-group-item-danger";
                    break;
                case Importance.Low:
                    result = "list-group-item-info";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
    }
}