using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SpartacusAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace SpartacusAPI.Controllers
{
    [ApiController]
    //[Route("[controller]")]    
    [Route("spartacusapi/getArticleComponents")]
    //[ApiKeyAuth]
    public class GetArticleComponentsController : ControllerBase
    {
        /// <summary>
        /// To get Component(spare parts) details against the Article and StoreID 
        /// </summary>
        [HttpPost]

        public async Task<ArticleComponents> GetArticleComponents([FromBody] Article paramArticle)
        {
            var data = await GetAuthors(paramArticle.businessUnit, paramArticle.ArticleNo);
            //if (data != null)
            return data;

        }

        private async Task<ArticleComponents> GetAuthors(string businessUnit, string articleNo)
        {
            await Task.Delay(100).ConfigureAwait(false);
            DataTable ds = new DataTable();            
            SqlConnection con = new SqlConnection(Startup.dbConn);

            var myCommand = new SqlCommand("sp_GetArticleComponentsDetails", con);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.AddWithValue("@StoreID", businessUnit);
            myCommand.Parameters.AddWithValue("@ArticleNo", articleNo);
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            da.Fill(ds);
            var item = new ArticleComponents();
            if (ds != null && ds.Rows.Count > 0)
            {
                //return ds.AsEnumerable().GroupBy(a => a.Field<string>("ArticleNo")).Select(c => new ArticleComponents
                //{
                //    ArticleDesc = Convert.IsDBNull(c.FirstOrDefault()["ArticleDesc"]) ? "" : c.FirstOrDefault()["ArticleDesc"].ToString(),
                //    ArticleNo = Convert.IsDBNull(c.FirstOrDefault()["ArticleNo"]) ? "" : c.FirstOrDefault()["ArticleNo"].ToString(),
                //    ArticleType = Convert.IsDBNull(c.FirstOrDefault()["ArticleType"]) ? "" : c.FirstOrDefault()["ArticleType"].ToString(),
                //    businessUnit = Convert.IsDBNull(c.FirstOrDefault()["StoreID"]) ? "" : c.FirstOrDefault()["StoreID"].ToString(),
                //    Components = c.Where(cr => !string.IsNullOrEmpty(Convert.IsDBNull(cr["ComponentNo"]) ? "" : cr["ComponentNo"].ToString())).Select(t => new Component
                //    {
                //        CompNo = Convert.IsDBNull(t["ComponentNo"]) ? "" : t["ComponentNo"].ToString(),
                //        CompDesc = Convert.IsDBNull(t["CompDesc"]) ? "" : t["CompDesc"].ToString(),
                //        Stock = !Convert.IsDBNull(t["Stock"]) ? Convert.ToInt32(t["Stock"]) : (int?)null,
                //        Location = Convert.IsDBNull(t["Location"]) ? null : t["Location"].ToString(),
                //        ImageDesc = Convert.IsDBNull(t["ImageDesc"]) ? "" : t["ImageDesc"].ToString(),
                //        //Image = Convert.IsDBNull(t["Image"]) ? "" : Convert.ToBase64String(((byte[])(t["Image"])), 0, ((byte[])(t["Image"])).Length)
                //        ImageRequestID = Convert.IsDBNull(t["ImageRequestID"]) ? "" : t["ImageRequestID"].ToString()

                //    }).ToList()
                //}).ToList();



                List<Component> comps = new List<Component>();
                for (int i = 0; i < ds.Rows.Count - 1; i++)
                {
                    Component C = new Component();
                    C.CompNo = Convert.IsDBNull(ds.Rows[i]["ComponentNo"]) ? "" : ds.Rows[i]["ComponentNo"].ToString();
                    C.CompDesc = Convert.IsDBNull(ds.Rows[i]["CompDesc"]) ? "" : ds.Rows[i]["CompDesc"].ToString();
                    C.Stock = !Convert.IsDBNull(ds.Rows[i]["Stock"]) ? Convert.ToInt32(ds.Rows[i]["Stock"]) : (int?)null;
                    C.Location = Convert.IsDBNull(ds.Rows[i]["Location"]) ? null : ds.Rows[i]["Location"].ToString();
                    C.ImageDesc = Convert.IsDBNull(ds.Rows[i]["ImageDesc"]) ? "" : ds.Rows[i]["ImageDesc"].ToString();
                    //Image = Convert.IsDBNull(t["Image"]) ? "" : Convert.ToBase64String(((byte[])(t["Image"])), 0, ((byte[])(t["Image"])).Length)
                    C.ImageRequestID = Convert.IsDBNull(ds.Rows[i]["ImageRequestID"]) ? "" : ds.Rows[i]["ImageRequestID"].ToString();
                    comps.Add(C);


                }
                item.ArticleDesc = Convert.IsDBNull(ds.Rows[0]["ArticleDesc"]) ? "" : ds.Rows[0]["ArticleDesc"].ToString();
                item.ArticleNo = Convert.IsDBNull(ds.Rows[0]["ArticleNo"]) ? "" : ds.Rows[0]["ArticleNo"].ToString();
                item.ArticleType = Convert.IsDBNull(ds.Rows[0]["ArticleType"]) ? "" : ds.Rows[0]["ArticleType"].ToString();
                item.businessUnit = Convert.IsDBNull(ds.Rows[0]["StoreID"]) ? "" : ds.Rows[0]["StoreID"].ToString();
                item.Components = comps;


            }

            else
            {
                item.ArticleDesc = null;
                item.ArticleNo = null;
                item.ArticleType = null;
                item.businessUnit = null;
                item.Components = null;
            }
            return item;

            //return ds.AsEnumerable().GroupBy(a => a.Field<string>("ArticleNo")).Select(c => new ArticleComponents
            //{
            //    ArticleDesc = null,
            //    ArticleNo = null,
            //    ArticleType = null,
            //    businessUnit = null,
            //    Components = c.Where(cr => !string.IsNullOrEmpty(Convert.IsDBNull(cr["ComponentNo"]) ? "" : cr["ComponentNo"].ToString())).Select(t => new Component
            //    {
            //        CompNo = null,
            //        CompDesc = null,
            //        Stock = null,
            //        Location = null,
            //        ImageDesc = null,
            //        ImageRequestID = null

            //    }).ToList()
            //}).ToList();
        }

    }


}
