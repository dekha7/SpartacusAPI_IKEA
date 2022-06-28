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

namespace SpartacusAPI.Controllers
{
    [ApiController]
    [Route("spartacusapi/UpdateArticleComponents")]
    //[ApiKeyAuth]
    public class UpdateArticleComponentsController : ControllerBase
    {

        /// <summary>
        /// To update Component(spare part) quantity/stock against the Article and StoreID 
        /// </summary>
        [HttpPost]
        public async Task<object> UpdateArticleComponentsDetails([FromBody] UpdateComponentStock componentStock)
        {

            var data = await UpdateArtice(componentStock.businessUnit, componentStock.articleNo, componentStock.compNo, componentStock.quantity);
            //if (data != null)
            return data;


        }
        private async Task<object> UpdateArtice(string businessUnit, string articleNo, string compNo, Int32? quantity)
        {
            await Task.Delay(100).ConfigureAwait(false);
            DataTable ds = new DataTable();
            SqlConnection con = new SqlConnection(Startup.dbConn);

            var myCommand = new SqlCommand("sp_UpdateArticleComponentsDetails", con);
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.AddWithValue("@StoreID", businessUnit);
            myCommand.Parameters.AddWithValue("@ArticleNo", articleNo);
            myCommand.Parameters.AddWithValue("@CompNo", compNo);
            myCommand.Parameters.AddWithValue("@Quantity", quantity);
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            da.Fill(ds);
            var item = new ArticleComponents();
            if (ds != null && ds.Rows.Count > 0)
            {
                DataColumnCollection columns = ds.Columns;
                if (columns.Contains("ErrorNumber"))
                {
                    Error E = new Error();
                    E.ErrorNumber = Convert.IsDBNull(ds.Rows[0]["ErrorNumber"]) ? "" : ds.Rows[0]["ErrorNumber"].ToString();
                    E.ErrorMessage = Convert.IsDBNull(ds.Rows[0]["ErrorMessage"]) ? "" : ds.Rows[0]["ErrorMessage"].ToString();
                    return E;
                }
                else
                {
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
        }
    }
}
