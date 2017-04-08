using System.Collections.Generic;

namespace ContentEngine.Models
{
    public class CollectionApiResult<T> where T : class
    {
        public Meta meta;
        public ICollection<T> data;
        public int total;
        public int page;
        public int pageSize;

        public CollectionApiResult()
        {
            meta = new Meta()
            {
                success = true,
                type = ApiResultType.Collection
            };
        }
    }
}
