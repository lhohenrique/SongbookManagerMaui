using Firebase.Database;
using Firebase.Database.Query;
using SongbookManagerMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Services
{
    public class RepertoireService : IRepertoireService
    {
        #region Fields
        FirebaseClient client;
        #endregion

        #region Properties
        private Repertoire repertoire;
        public Repertoire Repertoire
        {
            get { return repertoire; }
            set { repertoire = value; }
        }
        #endregion

        public RepertoireService()
        {
            client = new FirebaseClient("https://songbookmanagerlite-default-rtdb.firebaseio.com/");
        }

        public async Task<List<Repertoire>> GetRepertoires()
        {
            var repertoires = (await client.Child("Repertoires").OnceAsync<Repertoire>())
                .Select(item => new Repertoire
                {
                    Id = item.Key,
                    ServiceName = item.Object.ServiceName,
                    Date = item.Object.Date,
                    Keys = item.Object.Keys,
                    Musics = item.Object.Musics,
                    Owner = item.Object.Owner,
                    Time = item.Object.Time
                }).ToList();

            return repertoires;
        }

        public async Task<List<Repertoire>> GetRepertoiresByUser(string owner)
        {
            var repertoires = (await client.Child("Repertoires").OnceAsync<Repertoire>()).Select(item => new Repertoire
            {
                Id = item.Key,
                ServiceName = item.Object.ServiceName,
                Date = item.Object.Date,
                Keys = item.Object.Keys,
                Musics = item.Object.Musics,
                Owner = item.Object.Owner,
                Time = item.Object.Time
            }).Where(r => r.Owner.Equals(owner)).OrderByDescending(r => r.Date).ToList();

            return repertoires;
        }

        public async Task<List<Repertoire>> GetRepertoiresBySinger(string singer)
        {
            var repertoires = (await client.Child("Repertoires").OnceAsync<Repertoire>()).Select(item => new Repertoire
            {
                Id = item.Key,
                ServiceName = item.Object.ServiceName,
                Date = item.Object.Date,
                Keys = item.Object.Keys,
                Musics = item.Object.Musics,
                Owner = item.Object.Owner,
                Time = item.Object.Time
            }).Where(r => r.Musics.FirstOrDefault(m => m.SingerName.Equals(singer)) != null).ToList();

            return repertoires;
        }

        public async Task<List<Repertoire>> GetRepertoiresByPeriod(string owner, DateTime startDate, DateTime endDate)
        {
            var repertoires = (await client.Child("Repertoires").OnceAsync<Repertoire>()).Select(item => new Repertoire
            {
                Id = item.Key,
                ServiceName = item.Object.ServiceName,
                Date = item.Object.Date,
                Keys = item.Object.Keys,
                Musics = item.Object.Musics,
                Owner = item.Object.Owner,
                Time = item.Object.Time
            }).Where(r => r.Owner.Equals(owner) &&
                    r.Date >= startDate && r.Date <= endDate).ToList();

            return repertoires;
        }

        public async Task<List<Repertoire>> GetRepertoiresByDate(string owner, DateTime date)
        {
            var repertoires = (await client.Child("Repertoires").OnceAsync<Repertoire>()).Select(item => new Repertoire
            {
                Id = item.Key,
                ServiceName = item.Object.ServiceName,
                Date = item.Object.Date,
                Keys = item.Object.Keys,
                Musics = item.Object.Musics,
                Owner = item.Object.Owner,
                Time = item.Object.Time
            }).Where(r => r.Owner.Equals(owner) && (r.Date.Day == date.Day && r.Date.Month == date.Month && r.Date.Year == date.Year)).ToList();

            return repertoires;
        }

        public async Task<List<Repertoire>> GetRepertoiresBySingerAndPeriod(string singer, DateTime startDate, DateTime endDate)
        {
            var repertoires = (await client.Child("Repertoires").OnceAsync<Repertoire>()).Select(item => new Repertoire
            {
                Id = item.Key,
                ServiceName = item.Object.ServiceName,
                Date = item.Object.Date,
                Keys = item.Object.Keys,
                Musics = item.Object.Musics,
                Owner = item.Object.Owner,
                Time = item.Object.Time
            }).Where(r => r.Musics.FirstOrDefault(m => m.SingerName.Equals(singer)) != null &&
                    r.Date >= startDate && r.Date <= endDate).ToList();

            return repertoires;
        }

        public async Task<bool> InsertRepertoire(Repertoire repertoire)
        {
            await client.Child("Repertoires").PostAsync(repertoire);

            return true;
        }

        public async Task UpdateRepertoire(Repertoire repertoire)
        {
            await client.Child($"Repertoires/{repertoire.Id}").PutAsync(repertoire);
        }

        //public async Task<List<Repertoire>> SearchRepertoire(string searchText, string owner)
        //{
        //    var repertoires = (await client.Child("Repertoires").OnceAsync<Repertoire>()).Select(item => new Repertoire
        //    {
        //        ServiceName = item.Object.ServiceName,
        //        Date = item.Object.Date,
        //        Keys = item.Object.Keys,
        //        Musics = item.Object.Musics,
        //        Owner = item.Object.Owner,
        //        Time = item.Object.Time
        //    }).Where(r => r.Owner.Equals(owner) &&
        //                  (r.Date.ToString("dd MMMM").ToUpper().Contains(searchText.ToUpper()) ||
        //                  r.SingerName.ToString().ToUpper().Contains(searchText.ToUpper()))).ToList();

        //    return repertoires;
        //}

        public async Task DeleteRepertoire(Repertoire repertoire)
        {
            await client.Child($"Repertoires/{repertoire.Id}").DeleteAsync();
        }

        public async Task DeleteAll()
        {
            await client.Child("Repertoires").DeleteAsync();
        }

        public void SetRepertoire(Repertoire selectedRepertoire)
        {
            Repertoire = selectedRepertoire;
        }
    }
}
