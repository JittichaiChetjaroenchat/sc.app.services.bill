namespace SC.App.Services.Bill.Database.Constants
{
    public interface Tag
    {
        public const string TableName = "tags";

        public static class Column
        {
            public const string Id = "id";

            public const string ChannelId = "channel_id";

            public const string Color = "color";

            public const string Name = "name";

            public const string Description = "description";

            public const string CreatedBy = "created_by";

            public const string CreatedOn = "created_on";

            public const string UpdatedBy = "updated_by";

            public const string UpdatedOn = "updated_on";
        }
    }
}