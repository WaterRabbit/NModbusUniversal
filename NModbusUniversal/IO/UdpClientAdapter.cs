﻿//namespace Modbus.IO
//{
//    using System;
//    using System.Collections.Generic;
//    using System.IO;
//    using System.Linq;
//    using System.Net;
//    using System.Net.Sockets;
//    using System.Threading;

//    using Unme.Common;

//    /// <summary>
//    ///     Concrete Implementor - http://en.wikipedia.org/wiki/Bridge_Pattern
//    /// </summary>
//    internal class UdpClientAdapter : IStreamResource
//    {
//        // strategy for cross platform r/w 
//        private UdpClient _udpClient;
//        private List<byte> _buffer;

//        public UdpClientAdapter(UdpClient udpClient)
//        {
//            if (udpClient == null)
//            {
//                throw new ArgumentNullException(nameof(udpClient));
//            }

//            _udpClient = udpClient;
//        }

//        public int InfiniteTimeout
//        {
//            get { return Timeout.Infinite; }
//        }

//        public int ReadTimeout
//        {
//            get { return _udpClient.Client.ReceiveTimeout; }
//            set { _udpClient.Client.ReceiveTimeout = value; }
//        }

//        public int WriteTimeout
//        {
//            get { return _udpClient.Client.SendTimeout; }
//            set { _udpClient.Client.SendTimeout = value; }
//        }

//        public void DiscardInBuffer()
//        {
//            // no-op
//        }

//        public int Read(byte[] buffer, int offset, int count)
//        {
//            if (buffer == null)
//            {
//                throw new ArgumentNullException(nameof(buffer));
//            }

//            if (offset < 0)
//            {
//                throw new ArgumentOutOfRangeException(nameof(offset), "Argument offset must be greater than or equal to 0.");
//            }

//            if (offset > buffer.Length)
//            {
//                throw new ArgumentOutOfRangeException(nameof(offset), "Argument offset cannot be greater than the length of buffer.");
//            }

//            if (count < 0)
//            {
//                throw new ArgumentOutOfRangeException(nameof(count), "Argument count must be greater than or equal to 0.");
//            }

//            if (count > buffer.Length - offset)
//            {
//                throw new ArgumentOutOfRangeException(nameof(count), "Argument count cannot be greater than the length of buffer minus offset.");
//            }

//            if (_buffer == null || _buffer.Count == 0)
//            {
//                IPEndPoint remoteIpEndPoint = null;
//                _buffer = _udpClient.Receive(ref remoteIpEndPoint).ToList();
//            }

//            if (_buffer.Count < count)
//            {
//                throw new IOException("Not enough bytes in the datagram.");
//            }

//            _buffer.CopyTo(0, buffer, offset, count);
//            _buffer.RemoveRange(0, count);

//            return count;
//        }

//        public void Write(byte[] buffer, int offset, int count)
//        {
//            if (buffer == null)
//            {
//                throw new ArgumentNullException(nameof(buffer));
//            }

//            if (offset < 0)
//            {
//                throw new ArgumentOutOfRangeException(nameof(offset), "Argument offset must be greater than or equal to 0.");
//            }

//            if (offset > buffer.Length)
//            {
//                throw new ArgumentOutOfRangeException(nameof(offset), "Argument offset cannot be greater than the length of buffer.");
//            }

//            if (count < 0)
//            {
//                throw new ArgumentOutOfRangeException(nameof(count), "Argument count must be greater than or equal to 0.");
//            }

//            if (count > buffer.Length - offset)
//            {
//                throw new ArgumentOutOfRangeException(nameof(count), "Argument count cannot be greater than the length of buffer minus offset.");
//            }

//            _udpClient.Send(buffer.Skip(offset).ToArray(), count);
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                DisposableUtility.Dispose(ref _udpClient);
//            }
//        }
//    }
//}
