﻿namespace SIMCommon.Responses
{
    public sealed class SignIn : Base
    {
        public SignIn(bool success) : base()
        {
            this.Success = success;
        }

        public bool Success { get; private set; }
    }
}
