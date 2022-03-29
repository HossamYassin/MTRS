using MTRS.Core.DTOs;
using MTRS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MTRS.DAL.Repositories
{
    public class PLRepository : IPLData
    {
        private readonly string _connection;

        public PLRepository(string connection)
        {
            _connection = connection;
        }

        public List<PLDataDto> GetByOpportunityID(string opportunityId)
        {
            var data = new List<PLDataDto>();

            using (SqlConnection con = new SqlConnection(_connection))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM ICT_COMMERCIAL_PORTAL_VIEW_FOR_TIMESHEET where OPPORTUNITY_ID = '" + opportunityId + "'", con))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var plEntity = new PLDataDto();
                        plEntity.OpportunityId = reader["OPPORTUNITY_ID"].ToString();
                        plEntity.AccountName = reader["Account_Name"].ToString();
                        plEntity.StageName = reader["StageName"].ToString();
                        plEntity.ItemName = reader["ITEM_TYPE"].ToString();
                        plEntity.Grad = reader["GRAD"].ToString();
                        plEntity.TotalAmount = float.Parse(reader["TOTAL_AMOUNT"].ToString());
                        plEntity.Percentage = float.Parse(reader["PERCENTAGE"].ToString());
                        plEntity.StartDate = DateTime.Parse(reader["START_DATE"].ToString());
                        plEntity.EndDate = DateTime.Parse(reader["END_DATE"].ToString());

                        data.Add(plEntity);
                    }
                }
            }
            return data;
        }

        public bool IsOpenOpportunity(string opportunityId)
        {
            using (SqlConnection con = new SqlConnection(_connection))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM ICT_COMMERCIAL_PORTAL_VIEW_FOR_TIMESHEET where OPPORTUNITY_ID = '" 
                    + opportunityId + "'", con))
                {
                    var reader = command.ExecuteReader();

                    bool isExist = false;
                    bool isClosed = false;

                    while (reader.Read())
                    {
                        isExist = true;

                        if (isClosed == false)
                        {
                            isClosed = reader["StageName"].ToString().Contains("Closed");
                        }
                    }

                    if (isExist == true && isClosed == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
