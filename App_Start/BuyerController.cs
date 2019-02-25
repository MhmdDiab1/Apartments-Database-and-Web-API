using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Assignment2.App_Start
{
    public class BuyerController : ApiController
    {
        // GET: api/Buyer
        public string Get()
        {
            Buyer B = new Buyer(0, "temp", 0);
            return B.GetB();
        }

        // GET: api/Buyer/5
        public string Get(string input)
        {
            Buyer B = new Buyer(0, "temp", 0);
            return B.Apartment(Convert.ToInt32(input));

        }

        // POST: api/Buyer/IdCMDNameCMDCredit
        public string Post(string Input)
        {
            string[] Data = Input.Split(new string[] { "CMD" }, StringSplitOptions.None);
            Buyer B = new Buyer(Convert.ToInt32(Data[0]), Data[1], Convert.ToInt32(Data[2]));
            B.Add();

            return "New Buyer Id="+B.Id+" Was Created";
        }

        // PUT: api/Buyer/buyeridCMDapartmentid
        public string Put(string Input)
        {
            string[] Data = Input.Split(new string[] { "CMD" }, StringSplitOptions.None);
            int Bid = Convert.ToInt32(Data[0]), Aid = Convert.ToInt32(Data[1]);
            Buyer B = new Buyer(0, "temp", 0);
            string output=B.Buy(Bid, Aid);

            return output;
        }

        // DELETE: api/Buyer/5
        public void Delete(int id)
        {
        }
    }
}
