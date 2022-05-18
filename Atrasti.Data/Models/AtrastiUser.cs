using Microsoft.AspNetCore.Identity;
using System;
using System.Data;

namespace Atrasti.Data.Models
{
    public class AtrastiUser : IdentityUser<int>, ISerializable
    {
        public string? Company { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Company CompanyModel { get; set; }

        public string CompanyLogo { get; set; }

        public bool CompanySetup { get; set; }

        public bool CompanyInfoSetup { get; set; }
        
        public int? Referrer { get; set; }
        
        public UserData UserData { get; set; }

        public void Serialize(IDataReader reader)
        {
            Id = reader.GetData<int>("Id");
            Email = reader.GetDataNullable("Email");
            EmailConfirmed = reader.GetData<bool>("EmailConfirmed");
            PasswordHash = reader.GetData<string>("PasswordHash");
            SecurityStamp = reader.GetData<string>("SecurityStamp");
            PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader.GetData<string>("PhoneNumber") : null;
            PhoneNumberConfirmed = reader.GetData<bool>("PhoneNumberConfirmed");
            TwoFactorEnabled = reader.GetData<bool>("TwoFactorEnabled");
            LockoutEnabled = reader.GetData<bool>("LockoutEnabled");
            LockoutEnd = reader.GetDate("LockoutEndDateUtc");
            AccessFailedCount = reader.GetData<int>("AccessFailedCount");
            UserName = reader.GetData<string>("UserName");
            Company = reader.GetData<string>("Company");
            FirstName = reader.GetData<string>("FirstName");
            LastName = reader.GetData<string>("LastName");
            CompanySetup = reader.GetData<bool>("CompanySetup");
            CompanyInfoSetup = reader.GetData<bool>("CompanyInfoSetup");
            if (reader["CompanyLogo"] != DBNull.Value)
                CompanyLogo = reader.GetData<string>("CompanyLogo");

            if (reader.HasColumn("RefId"))
                if (reader["RefId"] != DBNull.Value)
                {
                    CompanyModel = new Company();
                    CompanyModel.Serialize(reader);
                }

            if (reader["referrer"] != DBNull.Value)
            {
                Referrer = reader.GetData<int?>("referrer");
            }

            UserData = new UserData();
            UserData.Serialize(reader);
        }
    }
}