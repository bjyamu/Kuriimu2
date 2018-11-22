﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kryptography
{
    public class SubStream : Stream
    {
        private Stream _baseStream;
        private readonly long _baseOffset;
        private long _length;
        private bool _fixedLength;

        public override long Position { get; set; }

        public override long Length => _length;

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public SubStream(Stream input, long offset, long length)
        {
            ValidateCtor(input, offset, length);

            _baseOffset = offset;

            _length = length;
            _fixedLength = true;

            Position = Math.Max(input.Position - offset, 0);
        }

        public SubStream(byte[] input, long offset, long length) : this(new MemoryStream(input), offset, length)
        {
        }

        #region Overrides
        public override int Read(byte[] buffer, int offset, int count)
        {
            ValidateRead(buffer, offset, count);

            count = Math.Max(0, Math.Min(count, (int)(_length - Position)));

            var origPosition = _baseStream.Position;
            _baseStream.Position = _baseOffset + Position;

            var read = _baseStream.Read(buffer, offset, count);

            _baseStream.Position = origPosition;
            Position += read;

            return read;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ValidateWrite(buffer, offset, count);

            var origPosition = _baseStream.Position;
            _baseStream.Position = _baseOffset + Position;

            _baseStream.Write(buffer, offset, count);

            _baseStream.Position = origPosition;
            Position += count;

            if (Position > _length) _length = Position;
        }

        public override void SetLength(long value)
        {
            ValidateSetLength(value);

            _fixedLength = true;
            _length = value;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!CanSeek) throw new NotSupportedException("Seek is not supported.");

            switch (origin)
            {
                case SeekOrigin.Begin: return Position = offset;
                case SeekOrigin.Current: return Position += offset;
                case SeekOrigin.End: return Position = _length + offset;
            }
            throw new ArgumentException(nameof(origin));
        }

        public override void Flush() => _baseStream.Flush();
        #endregion

        #region Private methods
        private void ValidateCtor(Stream input, long offset, long length)
        {
            if (input == null) throw new ArgumentException("Given Stream is null");

            if (offset < 0) throw new ArgumentException("Offset can't be negative");
            if (length <= 0) throw new ArgumentException("Length can't be negative or 0");
            if (offset >= _baseStream.Length) throw new ArgumentOutOfRangeException("Offset can't be outside the stream");
            if (length > _baseStream.Length) throw new ArgumentOutOfRangeException("Length can't be larger than stream.Length");
            if (offset + length > _baseStream.Length) throw new ArgumentOutOfRangeException("Portion of data is out of range.");
        }

        private void ValidateRead(byte[] buffer, int offset, int count)
        {
            if (!CanRead) throw new NotSupportedException("Read is not supported.");

            ValidateInput(buffer, offset, count);
        }

        private void ValidateWrite(byte[] buffer, int offset, int count)
        {
            if (!CanWrite) throw new NotSupportedException("Write is not supported");
            if (_fixedLength && Position >= _length) throw new ArgumentOutOfRangeException("Stream has fixed length and Position was out of range.");
            if (_fixedLength && _length - Position < count) throw new InvalidOperationException("Stream has fixed length and tries to write too much data.");

            ValidateInput(buffer, offset, count);
        }

        private void ValidateInput(byte[] buffer, int offset, int count)
        {
            if (offset < 0 || count < 0) throw new ArgumentOutOfRangeException("Offset or count can't be negative.");
            if (offset + count > buffer.Length) throw new InvalidDataException("Buffer too short.");
        }

        private void ValidateSetLength(long length)
        {
            if (length <= 0) throw new ArgumentException("Length can't be negative or 0");
        }
        #endregion

        public new void Dispose()
        {
            _baseStream.Dispose();
        }
    }
}
