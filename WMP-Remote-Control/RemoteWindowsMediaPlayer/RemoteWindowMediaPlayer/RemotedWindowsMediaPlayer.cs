// Written by Jonathan Dibble, Microsoft Corporation
// CODE IS PROVIDED AS-IS WITH NO WARRIENTIES EXPRESSED OR IMPLIED.

namespace WMPRemote
{
    using System;
    using System.Runtime.InteropServices;
    using WMPLib;


    /// <summary>
    /// This is the actual Windows Media Control.
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class RemotedWindowsMediaPlayer :
       IOleServiceProvider,
       IOleClientSite
    {

        /// <summary>
        /// Used to attach the appropriate interface to Windows Media Player.
        /// In here, we call SetClientSite on the WMP Control, passing it
        /// the dotNet container (this instance.)
        [DllImport("ole32.dll", PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern object CoCreateInstance(
          [In] ref Guid clsid,
          [MarshalAs(UnmanagedType.Interface)] object punkOuter,
          int context,
          [In] ref Guid iid);
        WMPRemote.IOleObject oleObject;
        public void CreateComObject()
        {
            Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");

            var gid = "{6bf52a52-394a-11d3-b153-00c04f79faa6}";
            var clsid = new Guid(gid);

            object instance = CoCreateInstance(ref clsid, (object)null, 1, ref IID_IUnknown);
            oleObject = instance as WMPRemote.IOleObject;
        }


        public void Connect()
        {
            oleObject.SetClientSite(this);

        }

        public WMPCore GetCore()
        {
            return oleObject as WMPCore;
        }

        #region IOleServiceProvider Memebers - Working
        /// <summary>
        /// During SetClientSite, WMP calls this function to get the pointer to <see cref="RemoteHostInfo"/>.
        /// </summary>
        /// <param name="guidService">See MSDN for more information - we do not use this parameter.</param>
        /// <param name="riid">The Guid of the desired service to be returned.  For this application it will always match
        /// the Guid of <see cref="IWMPRemoteMediaServices"/>.</param>
        /// <returns></returns>
        IntPtr IOleServiceProvider.QueryService(ref Guid guidService, ref Guid riid)
        {
            //If we get to here, it means Media Player is requesting our IWMPRemoteMediaServices interface
            if (riid == new Guid("cbb92747-741f-44fe-ab5b-f1a48f3b2a59"))
            {
                IWMPRemoteMediaServices iwmp = new RemoteHostInfo();

                return Marshal.GetComInterfaceForObject(iwmp, typeof(IWMPRemoteMediaServices));
            }

            throw new System.Runtime.InteropServices.COMException("No Interface", (int)HResults.E_NOINTERFACE);
        }
        #endregion

        #region IOleClientSite Members
        /// <summary>
        /// Not in use.  See MSDN for details.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException">E_NOTIMPL</exception>
        void IOleClientSite.SaveObject()
        {
            throw new System.Runtime.InteropServices.COMException("Not Implemented", (int)HResults.E_NOTIMPL);
        }

        /// <summary>
        /// Not in use.  See MSDN for details.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException"></exception>
        object IOleClientSite.GetMoniker(uint dwAssign, uint dwWhichMoniker)
        {
            throw new System.Runtime.InteropServices.COMException("Not Implemented", (int)HResults.E_NOTIMPL);
        }

        /// <summary>
        /// Not in use.  See MSDN for details.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException"></exception>
        object IOleClientSite.GetContainer()
        {
            return (int)HResults.E_NOINTERFACE;
        }

        /// <summary>
        /// Not in use.  See MSDN for details.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException"></exception>
        void IOleClientSite.ShowObject()
        {
            throw new System.Runtime.InteropServices.COMException("Not Implemented", (int)HResults.E_NOTIMPL);
        }

        /// <summary>
        /// Not in use.  See MSDN for details.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException"></exception>
        void IOleClientSite.OnShowWindow(bool fShow)
        {
            throw new System.Runtime.InteropServices.COMException("Not Implemented", (int)HResults.E_NOTIMPL);
        }

        /// <summary>
        /// Not in use.  See MSDN for details.
        /// </summary>
        /// <exception cref="System.Runtime.InteropServices.COMException"></exception>
        void IOleClientSite.RequestNewObjectLayout()
        {
            throw new System.Runtime.InteropServices.COMException("Not Implemented", (int)HResults.E_NOTIMPL);
        }

        #endregion          

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public RemotedWindowsMediaPlayer()//: 
                                          // base("6bf52a52-394a-11d3-b153-00c04f79faa6") 
        {
        }
    }
}

