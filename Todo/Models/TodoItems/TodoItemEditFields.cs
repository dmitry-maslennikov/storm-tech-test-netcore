using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemEditFields
    {
        public int TodoListId { get; set; }

        public string Title { get; set; }

        public string TodoListTitle { get; set; }

        public int TodoItemId { get; set; }

        public bool IsDone { get; set; }

        [Display(Name = "Responsible")]
        public string ResponsiblePartyId { get; set; }

        public Importance Importance { get; set; }

        [Display(Name = "Rank (0-100)")]
        [Range(0, 100)]
        public int Rank { get; set; }

        public TodoItemEditFields() { }

        public TodoItemEditFields(int todoListId, string todoListTitle, int todoItemId, string title, bool isDone, string responsiblePartyId, Importance importance, int rank)
        {
            TodoListId = todoListId;
            TodoListTitle = todoListTitle;
            TodoItemId = todoItemId;
            Title = title;
            IsDone = isDone;
            ResponsiblePartyId = responsiblePartyId;
            Importance = importance;
            Rank = rank;
        }
    }
}