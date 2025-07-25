﻿using System.ComponentModel.DataAnnotations;

namespace Banking.Domain.Entities
{
    public class Customer
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public List<Account> Accounts { get; set; } = new();

        public Customer Clone()
        {
            return new Customer()
            {
                Id = Id,
                FullName = FullName,
                Accounts = Accounts
            };
        }
    }
}
