using System;
using System.IO;

namespace WebApiTemplate.Controllers
{
    internal class PartialContentFileStream : Stream
    {
        private readonly long _start;
        private readonly long _end;
        private long _position;
        private FileStream _fileStream;

        public override bool CanRead => _fileStream.CanRead;

        public override bool CanSeek => _fileStream.CanSeek;

        public override bool CanWrite => _fileStream.CanWrite;

        public override long Length => _fileStream.Length;

        public override long Position { get => _position; set => _position = value; }

        public override void Flush()
        {
            _fileStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int byteCountToRead = count;
            if(_position + count > _end)
            {
                byteCountToRead = (int)(_end - _position) + 1;
            }
            var result = _fileStream.Read(buffer, offset, byteCountToRead);
            _position += byteCountToRead;
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                _position = _start + offset;
                return _fileStream.Seek(_start + offset, origin);
            }
            else if (origin == SeekOrigin.Current)
            {
                _position += offset;
                return _fileStream.Seek(_position + offset, origin);
            }
            else
            {
                throw new NotImplementedException("SeekOrigin.End未实现");
            }
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }
    }
}