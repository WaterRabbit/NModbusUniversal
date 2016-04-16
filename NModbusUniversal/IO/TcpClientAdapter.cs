namespace Modbus.IO
{
    using System;
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Threading;

    using System.Runtime.InteropServices.WindowsRuntime;

    using Unme.Common;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;

    /// <summary>
    ///     Concrete Implementor - http://en.wikipedia.org/wiki/Bridge_Pattern
    /// </summary>
    internal class TcpClientAdapter : IStreamResource
    {
        private StreamSocket _tcpClient;

        public TcpClientAdapter(StreamSocket tcpClient)
        {
            Debug.Assert(tcpClient != null, "Argument tcpClient cannot be null.");

            _tcpClient = tcpClient;
        }

        public int InfiniteTimeout
        {
            get { return Timeout.Infinite; }
        }

        public int ReadTimeout
        {
            get { return 0; } // return _tcpClient.GetStream().ReadTimeout; }
            set { } //            _tcpClient.GetStream().ReadTimeout = value; }
        }

        public int WriteTimeout
        {
            get { return 0; }// return _tcpClient.GetStream().WriteTimeout; }
            set { }// _tcpClient.GetStream().WriteTimeout = value; }
        }

        public async void Write(byte[] buffer, int offset, int size)
        {
            DataWriter wr = new DataWriter(_tcpClient.OutputStream);
            wr.WriteBytes(buffer);
            await wr.StoreAsync().AsTask().ConfigureAwait(false);
            await wr.FlushAsync().AsTask().ConfigureAwait(false);
            //;            await _tcpClient.OutputStream.WriteAsync(buffer);
            //            _tcpClient.GetStream().Write(buffer, offset, size);
            wr.DetachStream();
        }

        public async System.Threading.Tasks.Task<int> Read(byte[] buffer, int offset, int size)
        {
            DataReader dr = new DataReader(_tcpClient.InputStream);

            CancellationTokenSource cts = new CancellationTokenSource(1000); // cancel after 1000ms
            DataReaderLoadOperation op = dr.LoadAsync((uint)size);
            size =(int) await op.AsTask(cts.Token).ConfigureAwait(false);

       //     await wr.LoadAsync((uint)size).AsTask().ConfigureAwait(false);
            IBuffer buff = dr.ReadBuffer((uint)size);
            buff.CopyTo(0, buffer, offset, (int)buff.Length);
            //return _tcpClient.GetStream().Read(buffer, offset, size);
            dr.DetachStream();
            return (int)buff.Length;
        }

        public void DiscardInBuffer()
        {
  //          _tcpClient.GetStream().Flush();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposableUtility.Dispose(ref _tcpClient);
            }
        }
    }
}
