namespace SC.App.Services.Bill.Common.Constants
{
    public class AppSettings
    {
        public class Applications
        {
            public class Buyer
            {
                public const string BaseUrl = "Applications:Buyer:BaseUrl";
            }

            public class Bill
            {
                public const string Name = "Applications:Bill:Name";
            } 
        }

        public class Culture
        {
            public const string Default = "Culture:Default";

            public const string Supports = "Culture:Supports";
        }

        public class Databases
        {
            public class Bill
            {
                public const string ConnectionString = "Databases:Bill:ConnectionString";
            }
        }

        public class Services
        {
            public const string BaseUrl = "Services:BaseUrl";

            public class Area
            {
                public const string BaseUrl = "Services:Area:BaseUrl";
            }

            public class Customer
            {
                public const string BaseUrl = "Services:Customer:BaseUrl";
            }

            public class Courier
            {
                public const string BaseUrl = "Services:Courier:BaseUrl";
            }

            public class Credit
            {
                public const string BaseUrl = "Services:Credit:BaseUrl";
            }

            public class Document
            {
                public const string BaseUrl = "Services:Document:BaseUrl";
            }

            public class Inventory
            {
                public const string BaseUrl = "Services:Inventory:BaseUrl";
            }

            public class Notification
            {
                public const string BaseUrl = "Services:Notification:BaseUrl";
            }

            public class Order
            {
                public const string BaseUrl = "Services:Order:BaseUrl";
            }

            public class Security
            {
                public const string BaseUrl = "Services:Security:BaseUrl";
            }

            public class Setting
            {
                public const string BaseUrl = "Services:Setting:BaseUrl";
            }

            public class Streaming
            {
                public const string BaseUrl = "Services:Streaming:BaseUrl";
            }
        }

        public static class Queues
        {
            public const string HostName = "Queues:HostName";

            public const string UserName = "Queues:UserName";

            public const string Password = "Queues:Password";
        }

        public class ElasticSearch
        {
            public const string Uri = "ElasticSearch:Uri";
        }
    }
}