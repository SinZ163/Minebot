﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using C_Minebot.Classes;

namespace C_Minebot.Packets
{
    class ChunkData
    {
        public ChunkData(Wrapped.Wrapped socket, Form1 Mainform)
        {
            // int decompressedSize = 0;
            int x = socket.readInt();
            int z = socket.readInt();
            bool groundup = socket.readBool();
            short pbitmap = socket.readShort();
            short abitmap = socket.readShort();
            int size = socket.readInt();
            byte[] data = socket.readByteArray(size);
            byte[] trim = new byte[size - 2];
            byte[] decompressed;

            // Determine what sections are included

            //numBlocks = decompressedSize * 4096;
            //blocks = new byte[numBlocks];

            //if (groundup == false)
            //    decompressedSize = decompressedSize * 12288; // 16 x 16 x 16 column, (4096), x 3 for metadata, light, skylight, and add array
            //else
            //    decompressedSize = decompressedSize * 12544; // + 256 for biome data

            // Remove first two bytes to array
            Array.Copy(data, 2, trim, 0, size - 2);

            // Decompress the data

            Classes.Decompressor DC = new Decompressor(trim);
            decompressed = DC.decompress();

            if (pbitmap == 0) {
                // Unload chunk, save ALL the ram!
                Classes.Chunk thischunk = null;

                foreach (Chunk f in Mainform.Chunks) {
                    if (f.x == x && f.z == z) {
                        thischunk = f;
                        break;
                    }
                }

                if (thischunk != null) 
                    Mainform.Chunks.Remove(thischunk);

                // Unloaded, hopefully.
            }

            Chunk myChunk = new Chunk(x, z, pbitmap, abitmap, true); // Skylight assumed true..
            decompressed = myChunk.getData(decompressed);
            Mainform.Chunks.Add(myChunk);

            // might as well parse it.. why the fuck not.
            myChunk.parseBlocks();

            //// Get all of the blocks, and load them into memory.
            //for (byte i = 0; i < 16; i++)
            //{
            //    if (Convert.ToBoolean(pbitmap & (1 << i)))
            //    {
            //        for (int f= 0; f < 4096; f++)
            //        {
            //            int blockX = (x * 16) + (f & 0x0F);
            //            int blockY = (i * 16) + (blockX >> 8);
            //            int blockZ = (z * 16) + (f & 0xF0) >> 4;
            //            int blockID = decompressed[f];

            //            MapBlock myBlock = new MapBlock(blockID, blockX, blockY, blockZ);

            //            Mainform.blocks.Add(myBlock);
            //            //if (Mainform.blocks == null)
            //            //    Mainform.blocks = new MapBlock[] { myBlock };
            //            //else
            //            //{
            //            //    MapBlock[] temp = Mainform.blocks;
            //            //    Mainform.blocks = new MapBlock[temp.Length + 1];
            //            //    Array.Copy(temp, Mainform.blocks, temp.Length);
            //            //    Mainform.blocks[temp.Length] = myBlock;
            //            //}

            //        }
            //    }
            //}
            
            
        }
    }
}
