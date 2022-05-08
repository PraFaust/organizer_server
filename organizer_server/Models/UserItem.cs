using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace organizer_server.Models
{
    public class UserItem
    {
        public int UserItemId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemCount { get; set; }
    }
}
