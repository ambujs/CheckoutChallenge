﻿using System;

namespace Checkout.AcquiringBank.Mock.Models
{
    public class PaymentStatus
    {
        public bool Successful { get; set; }
        public string ErrorCode { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}