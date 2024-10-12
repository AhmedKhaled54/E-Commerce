
using Core.Entity;
using Services.Dtos.CartsDto;
using Services.Dtos.WishListDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.UserDto
{
    public class UserProfileDto
    {
        public string UserId {   get; set;}
        public string UserName { get; set;}
        public string Email  {   get; set;}
        public string Address {  get;set; }
        public IEnumerable<WishListsDto>? wishLists  { get; set; }



        

    }
}
