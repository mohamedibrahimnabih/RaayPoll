﻿using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RaayPoll.API.Validations
{
    public class CustomLengthAttribute : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public CustomLengthAttribute(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        public override bool IsValid(object? value)
        {
            if(value is string v)
            {
                if(v.Length >= _minLength && v.Length <= _maxLength)
                {
                    return true;
                }
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be between {_minLength} and {_maxLength} characters long.";
        }
    }
}
