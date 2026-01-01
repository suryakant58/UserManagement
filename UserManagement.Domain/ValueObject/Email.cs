using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UserManagement.Domain.Entities;

namespace UserManagement.Domain.ValueObject
{
    public sealed class Email
    {
     
        //If two objects have the same values, they are considered equal, then it’s a Value Object.
        public string Value { get; private set; }

        public Email(string value)
        {
            
            Value = value;
        }

        // Parameterless ctor for EF Core / serializers
        public Email() { }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty");

            if (!value.Contains("@"))
                throw new ArgumentException("Invalid email format");

            return new Email(value.Trim().ToLower());
        }

        public override bool Equals(object? obj)
        {
            return obj is Email other && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value;
    }

}
