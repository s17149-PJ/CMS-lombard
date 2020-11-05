using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lombard_00.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CItem : ControllerBase
    {
        [Route("api/item/add")]
        [HttpPost]
        public bool Add( )
        {
            return false;
        }
        [Route("api/item/edit")]
        [HttpPost]
        public bool Edit( )
        {
            return false;
        }
        [Route("api/item/delete")]
        [HttpPost]
        public bool Delete( )
        {
            return false;
        }
        [Route("api/item/browse")]
        [HttpPost]
        public bool Browse( )
        {
            return false;
        }
        [Route("api/item/bid")]
        [HttpPost]
        public bool Bid( )
        {
            return false;
        }
        [Route("api/item/comment")]
        [HttpPost]
        public bool Comment( )
        {
            return false;
        }
    }
}
