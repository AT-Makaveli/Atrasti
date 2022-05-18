using System.Collections.Generic;

namespace Atrasti.API.Models.Error
{
    public class BaseError
    {
        public int Status { get; } = 400;
        public IList<ErrorEntry> Errors { get; } = new List<ErrorEntry>();
    }
}