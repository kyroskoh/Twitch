﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Twitch.Model;
using Twitch.Services;

namespace Twitch.ViewModel
{
    public class PlayerViewModel : ViewModelBase
    {
        public PlayerViewModel( INavigationService aNavService, ITwitchQueryService aTwitchQueryService )
        {
            mNavService = aNavService;
            mTwitchQueryService = aTwitchQueryService;
        }

        public async Task Init( Stream aStream )
        {
            if( aStream == null )
            {
                mNavService.GoBack();
                return;
            }

            mStreamLocationList.Clear();

            var theM3uStreams = M3uStream.ParseM3uStreams( await mTwitchQueryService.GetChannel( aStream.Channel.Name ) );
            foreach( var theM3uStream in theM3uStreams )
            {
                mStreamLocationList.Add( theM3uStream );
            }
            SelectedStreamLocation = StreamLocationList.FirstOrDefault();
        }

        public override void Cleanup()
        {
            mSelectedStreamLocation = null;
            mStreamLocationList.Clear();
            base.Cleanup();
        }

        public IEnumerable<M3uStream> StreamLocationList
        {
            get
            {
                return mStreamLocationList;
            }
        }
        private readonly ObservableCollection<M3uStream> mStreamLocationList = new ObservableCollection<M3uStream>();

        public M3uStream SelectedStreamLocation
        {
            get
            {
                return mSelectedStreamLocation;
            }
            set
            {
                Set( nameof( SelectedStreamLocation ), ref mSelectedStreamLocation, value );
            }
        }
        private M3uStream mSelectedStreamLocation;


        private readonly INavigationService mNavService;
        private readonly ITwitchQueryService mTwitchQueryService;
    }
}