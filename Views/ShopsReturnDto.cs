using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Zembil.Views
{
    public class ShopReturnDto
    {
        public ShopReturnDto()
        {
            IsApproved = false;
        }

        public int ShopId { get; set; }

        public string ShopName { get; set; }

        public string BuildingName { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public int OwnerId { get; set; }

        public string Description { get; set; }


        public bool IsApproved { get; set; }

    }
}
