﻿namespace ShoppingAppAPI.Models
{
    public class Enums
    {
        public enum UserRole
        {
            Customer,
            Seller,
            Both,
            Admin
        }

        public enum CartStatus
        {
            Open,
            Closed,
            Empty,
            Saved,
            Expired
        }

        public enum OrderStatus
        {
            Pending,
            Processing,
            Shipped,
            Delivered,
            Canceled,
            Refunded,
            Failed
        }

        public enum PaymentStatus
        {
            Pending,
            Authorized,
            Captured,
            Refunded,
            Failed
        }

        public enum RefundStatus
        {
            Pending,
            Approved,
            Processed,
            Rejected,
            Failed
        }


    }
}
