using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;

namespace Assignment2.App_Start
{
    public class ApartmentController : ApiController
    {
        List<Appartment> Appartments = new List<Appartment>();

        // GET: api/Apatment
        //IEnumerable<Appartment> Get()
        public string Get()
        {
            Appartment P=new Appartment("New","New",2);
            string output = P.GetAp();
            return output;
        }

        // GET: api/Apatment/priceTOpriceCMDAddressCMDnbrofrooms
        public string Get(string Input)
        {
            string[] Data = Input.Split(new string[] { "CMD" }, StringSplitOptions.None);
            string[] Price= Data[0].Split(new string[] { "TO" }, StringSplitOptions.None);
            int Lower= Convert.ToInt32(Price[0]), Higher= Convert.ToInt32(Price[1]);
            Appartment P = new Appartment("New", "New", 2);
            string output;
            if (Higher != Lower)
            {
                output = P.GetAp(Higher, Lower, Convert.ToString(Data[1]), Convert.ToInt32(Data[2]));
                return output;
            }

            return "Apartment Not Found";
        }

        // POST: api/Apatment/TitleCMDAddressCMDNbrofRooms
        // CMD is used for string split
        public string Post(string Input)
        {
            string[] Data = Input.Split(new string[] { "CMD" }, StringSplitOptions.None);
            Appartment P=new Appartment(Data[0], Data[1], Convert.ToInt32(Data[2]));
            P.Add();

            return "New Appartment Id="+P.Id+" Was Created";
        }

        // PUT: api/Apatment/5CMDnewtitle
        public string Put(string Input)
        {
            string[] Data = Input.Split(new string[] { "CMD" }, StringSplitOptions.None);
            Appartment temp = new Appartment("temp", "temp", 0);
            return temp.Update(Convert.ToInt32(Data[0]),Data[1]);
        }

        // DELETE: api/Apatment/5
        public string Delete(string Input)
        {
            Appartment temp = new Appartment("temp", "temp", 0);
            return temp.Delete(Convert.ToInt32(Input));
        }
    }
}
