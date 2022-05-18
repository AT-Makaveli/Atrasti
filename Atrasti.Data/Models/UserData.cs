using System;
using System.Data;

namespace Atrasti.Data.Models
{
    public class UserData : ISerializable
    {
        public int UserId { get; set; }
        public int Referrals { get; set; }
        public string FcmToken { get; set; }
        public string RefreshToken { get; set; }

        public void Serialize(IDataReader reader)
        {
            UserId = reader.GetData<int>("userData_user_id");
            Referrals = reader.GetData<int>("userData_referrals");
            FcmToken = reader.GetDataNullable("userData_fcm_token");
            
            if (reader.HasColumn("userData_refresh_token"))
                RefreshToken = reader.GetDataNullable("userData_refresh_token");
        }
    }
}