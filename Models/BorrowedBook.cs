﻿namespace LibrarySystem.Models
{
    public class BorrowedBook
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int InventoryId { get; set; }
        public Inventory? Inventory { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string Status { get; set; } = "Borrowed";
    }
}
