﻿using System.ComponentModel.DataAnnotations;
using Todo.Data.Entities;

namespace Todo.Models.TodoItems
{
    public class TodoItemCreateFields
    {
        public int TodoListId { get; set; }

        public string Title { get; set; }

        public string TodoListTitle { get; set; }

        [Display(Name = "Responsible")]
        public string ResponsiblePartyId { get; set; }

        public Importance Importance { get; set; } = Importance.Medium;

        [Display(Name = "Rank (0-100)")]
        [Range(0, 100)]
        public int Rank { get; set; }

        public TodoItemCreateFields() { }

        public TodoItemCreateFields(int todoListId, string todoListTitle, string responsiblePartyId)
        {
            TodoListId = todoListId;
            TodoListTitle = todoListTitle;
            ResponsiblePartyId = responsiblePartyId;
        }
    }
}