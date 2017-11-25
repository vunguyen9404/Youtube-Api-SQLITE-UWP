using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karaoke.Models
{
    public class SqlConnection
    {
        public SQLitePCL.SQLiteConnection conn { get; set; }
        public string path { get; set; }
        public void LoadDatabase()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            conn = new SQLitePCL.SQLiteConnection(path);

            string createVideo = @"CREATE TABLE IF NOT EXISTS Video (
                            VideoID VARCHAR(30) PRIMARY KEY NOT NULL,
                            VideoImg VARCHAR(100),
                            VideoTitle VARCHAR(200),
                            IsLike INTEGER,
                            PlaylistID INTEGER,
                            FOREIGN KEY(PlaylistID) REFERENCES Playlist(PlaylistID)
                        );";

            string createPlaylist = @"CREATE TABLE IF NOT EXISTS Playlist (
                            PlaylistID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                            PlaylistName VARCHAR(100) NOT NULL UNIQUE
                        );";
            string createDefaultPlaylist = @"INSERT INTO Playlist (PlaylistName) VALUES (?);";
            using (var statement = conn.Prepare("BEGIN TRANSACTION"))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(createPlaylist))
            {
                statement.Step();
            }
            using (var statement = conn.Prepare(createVideo))
            {
                statement.Step();
            }

            using (var statement = conn.Prepare(createDefaultPlaylist))
            {
                statement.Bind(1, "Default");
                statement.Step();
            }
            using (var statement = conn.Prepare("COMMIT TRANSACTION"))
            {
                statement.Step();
            }
        }

        public void AddVideo(Video video)
        {
            try
            {
                string sql = "INSERT INTO Video (VideoID, VideoImg, VideoTitle, IsLike, PlaylistID) VALUES (?,?,?,?,?);";
                using(var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, video.Id);
                    statement.Bind(2, video.Img);
                    statement.Bind(3, video.Title);
                    statement.Bind(4, video.IsLike ? 1:0);
                    statement.Bind(5, video.playlist != null ? video.playlist.PlaylistID: 1);

                    statement.Step();
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddPlaylist(LocalPlaylist playlist)
        {
            try
            {
                string sql = "INSERT INTO Playlist (PlaylistName) VALUES (?);";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Bind(1, playlist.PlaylistName);
                    statement.Step();
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Video> getVideo()
        {
            try
            {
                List<Video> list = new List<Video>();
                string sql = "SELECT * FROM Video LEFT JOIN Playlist ON Video.PlaylistID = Playlist.PlaylistID;";
                using (var statement = conn.Prepare(sql))
                {
                    while (SQLitePCL.SQLiteResult.ROW == statement.Step())
                    {
                        list.Add(new Video()
                        {
                            Id = statement.GetText(0),
                            Img = statement.GetText(1),
                            Title = statement.GetText(2),
                            IsLike = (Int64)statement[3] == 1 ? true: false,
                            playlist = new LocalPlaylist()
                            {
                                PlaylistID = (Int64)statement.GetInteger(4),
                                PlaylistName = statement.GetText(6)
                            }
                        });
                    };
                }
                list.Reverse();
                return list;
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteVideo(Video video)
        {
            try
            {
                string sql = @"DELETE FROM Video WHERE VideoID = ?;";
                using (var statment = conn.Prepare(sql))
                {
                    statment.Bind(1, video.Id);
                    statment.Step();
                };
            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
