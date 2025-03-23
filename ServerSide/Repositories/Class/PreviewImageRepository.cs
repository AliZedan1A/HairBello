using Domain.DatabaseModels;
using ServerSide.Database;

namespace ServerSide.Repositories.Class
{
    public class PreviewImageRepository : Repository<PreviewImageModel>
    {
        private readonly ApplicationDbContext _context;
        public PreviewImageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
