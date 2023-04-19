/********************************************************************
	Framework Script
	Class: 	GreyFramework::MemoryPool
	Author:	
	Created:	
	Note:	
*********************************************************************/



namespace GreyFramework
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MemoryPool
    {
        private List<byte[]> pool;
        private int reservesize;
        private int maxBufferSize ;
        private int currentBufferSize;

        public MemoryPool( )
        {
            this.reservesize = 0x10;
            this.pool = new List<byte[]>();
            maxBufferSize = 0;
            currentBufferSize = 0;
        }

        public MemoryPool( int maxsize )
        {
            this.reservesize = 0x10;
            this.pool = new List<byte[]>();
            maxBufferSize = maxsize;
            currentBufferSize = 0;
        }

        public MemoryPool( int reservesize , int reservecount )
        {
            this.reservesize = 0x10;
            this.pool = new List<byte[]>();
            this.reservesize = reservesize;
            maxBufferSize = reservecount* reservesize;
            currentBufferSize = maxBufferSize;
            for (int i = 0 ; i < reservecount ; i++)
            {
                this.pool.Add( new byte[this.reservesize] );
            }
        }

        public byte[] Alloc( int size , bool islock = false )
        {
            if (islock)
            {
                lock (_lock)
                {
                    return OnAlloc( size );
                }
            }
            return OnAlloc( size );
        }

        private byte[] OnAlloc( int size )
        {
            if (this.pool.Count == 0)
            {
                return new byte[size];
            }

            byte[] buffer = null;
            Get( ref buffer , size);
            return buffer;
        }

        void Add( ref byte[] buffer ) {
            bool inserted = false;

            for (int i = 0 ; i < this.pool.Count  ; i++) {
                if (buffer.Length <= this.pool[i].Length) {
                    pool.Insert( i , buffer );
                    currentBufferSize += buffer.Length;
                    inserted = true;
                    break;
                }
            }

            if (!inserted) {
                this.pool.Add( buffer );
                currentBufferSize += buffer.Length;
            }
        }

        void Get( ref byte[] buffer , int size ) {

            for (int i = 0; i < this.pool.Count  ; i++) {
                int val = this.pool[i].Length - size;
                if (val>=0) {
                    buffer = this.pool[i];
                    if (val>0) {
                        System.Array.Resize( ref buffer , size );
                    }

                    //CDebug.Log( "find {0}  dest {1}  {2}" ,this.pool[i].Length , val , i );
                    this.pool.RemoveAt( i );
                    currentBufferSize -= buffer.Length;
                    return;
                }
            }

            buffer = new byte[size];
        }

        public void Free( ref byte[] buf , bool islock = false )
        {
            if (buf == null)
            {
                Debug.LogError("buf is null");
                return;
            }

            if (islock)
            {
                lock (_lock)
                {
                    OnFree( ref buf );
                    return;
                }
            }

            OnFree( ref buf );
        }

        public void OnFree( ref byte[] buf )
        {
            if (buf == null)
            {
                Debug.LogError( "buf is null" );
                return;
            }

            if (maxBufferSize == 0)
            {
                this.Add( ref buf );
            }
            else
            {
                if (currentBufferSize < maxBufferSize)
                {
                    this.Add( ref buf );
                }
                else
                {
                    buf = null;
                }
            }
        }

        public int GetBufCount( )
        {
            return this.pool.Count;
        }

        public int GetBufSize( ) {
            return currentBufferSize;
        }
        
        public void SetMaxBufferSize( int size )
        {
            maxBufferSize = size;
        }

        public void Clear() {
            currentBufferSize = 0;
            this.pool.Clear();
        }
        public void Print() {
            Debug.Log( currentBufferSize );
            for (int i = 0 ; i < this.pool.Count ; i++) {
                Debug.Log( this.pool[i].Length );
            }
        }

        private object _lock = new object();
    }
}