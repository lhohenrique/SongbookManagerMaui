using SongbookManagerMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Services
{
    public interface IRepertoireService
    {
        #region Properties
        Repertoire Repertoire { get; set; }
        #endregion

        Task<List<Repertoire>> GetRepertoires();

        Task<List<Repertoire>> GetRepertoiresByUser(string owner);

        Task<List<Repertoire>> GetRepertoiresBySinger(string singer);

        Task<List<Repertoire>> GetRepertoiresByPeriod(string owner, DateTime startDate, DateTime endDate);

        Task<List<Repertoire>> GetRepertoiresBySingerAndPeriod(string singer, DateTime startDate, DateTime endDate);

        Task<bool> InsertRepertoire(Repertoire repertoire);

        Task UpdateRepertoire(Repertoire repertoire);

        Task DeleteRepertoire(Repertoire repertoire);

        Task DeleteAll();

        void SetRepertoire(Repertoire selectedRepertoire);
        Task<List<Repertoire>> GetRepertoiresByDate(string owner, DateTime date);
    }
}
