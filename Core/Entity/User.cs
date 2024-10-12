using Core.Entity.CartEntity;
using Core.Entity.OrderEntity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    public class User:IdentityUser
    {
        public string Address { get; set; } 
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<WishList>? wishLists { get; set; }
        public IEnumerable<Cart>? Carts { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
        public List<Rate> ?Rate { get; set; }   



        

    }
}
