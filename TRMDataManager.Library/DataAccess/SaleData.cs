
using System;
using System.Linq;
using System.Collections.Generic;
using TRMDataManager.Library.Models;
using TRMDataManager.Library.Internal.DataAccess;


namespace TRMDataManager.Library.DataAccess
{
    public class SaleData
    {
        //public List<ProductModel> GetProducts()
        //{
        //    SqlDataAccess sql = new SqlDataAccess();

        //    var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMDATA");

        //    return output;
        //}

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //TODO: Make this SOLID/DRY/Better
            // Start filling int the models we will save to the database

            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData product = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate();

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                // Get the information about this product
                var productInfo = product.GetProductById(detail.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {detail.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * (taxRate / 100));
                }

                details.Add(detail);
            }

            // Fill in the available information
            // Create the Sale model

            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                UserId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            // NEW WAY WITH TRANSACTION:
            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("TRMData");

                    // Save the Sale model
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    // Get the ID from the Sale Model
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup", new { CashierId = sale.UserId, sale.SaleDate }).FirstOrDefault();

                    // Finish filling in the Sale Detail Models
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        // Save the Sale Detail Models
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    sql.CommitTransaction();
                }
                catch
                {
                    sql.RollbackTransaction();
                    throw;
                }
            }

            //// OLD WAY WITHOUT TRANSACTION:
            // Save the Sale model
            //SqlDataAccess sql = new SqlDataAccess();
            //sql.SaveData("dbo.spSale_Insert", sale, "TRMData");

            //// Get the ID from the Sale Model
            //sale.Id = sql.LoadData<int, dynamic>("spSale_Lookup", new { CashierId = sale.UserId, sale.SaleDate }, "TRMData").FirstOrDefault();

            //// Finish filling in the Sale Detail Models
            //foreach (var item in details)
            //{
            //    item.SaleId = sale.Id;
            //    // Save the Sale Detail Models
            //    sql.SaveData("dbo.spSaleDetail_Insert", item, "TRMData");
            //}
        }


        public List<SaleReportModel> GetSaleReport()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");

            return output;
        }


    }
}