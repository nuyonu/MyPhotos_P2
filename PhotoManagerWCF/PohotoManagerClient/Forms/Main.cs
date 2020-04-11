﻿using MyPhotosDatabase;
using MyPhotosDatabase.Enums;
using MyPhotosDatabase.Models.DTO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PhotoManager.Forms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            PhotoManagerServiceClient photoManagerServiceClient = new PhotoManagerServiceClient();
            List<MediaDTO> media = new List<MediaDTO>(photoManagerServiceClient.GetAll());
            labelTotalMediaFromBegin.Text = "Total media stored from begin: " + media.Count;
            int mediaNow = (from m in media
                            where m.Deleted == false
                            select m).Count();
            labelTotalMediaNow.Text = "Total media avaible now: " + mediaNow;
            int totalVideos = (from m in media
                               where m.Deleted == false && m.Type == MediaType.Video
                               select m).Count();
            labelTotalVideos.Text = "Total videos now: " + totalVideos;
            int totalPhotos = (from m in media
                               where m.Deleted == false && m.Type == MediaType.Photo
                               select m).Count();
            labelTotalPhotos.Text = "Total photos now: " + totalPhotos;
            int mediaDeleted = (from m in media
                                where m.Deleted
                                select m).Count();
            labelTotalMediaDeleted.Text = "Total media deleted: " + mediaDeleted;
        }
    }
}
