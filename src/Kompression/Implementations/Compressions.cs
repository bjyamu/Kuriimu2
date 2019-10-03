﻿using Kompression.Configuration;
using Kompression.Huffman;
using Kompression.Implementations.Decoders;
using Kompression.Implementations.Encoders;
using Kompression.IO;
using Kompression.Models;

namespace Kompression.Implementations
{
    public static class Compressions
    {
        public static KompressionConfiguration Lz10
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Lz10Decoder()).EncodeWith((parser, builder, modes) => new Lz10Encoder(parser));
                config.WithMatchOptions(options =>
                    options.WithinLimitations(() => new FindLimitations(0x3, 0x12, 1, 0x1000)));

                return config;
            }
        }

        public static KompressionConfiguration Lz11
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Lz11Decoder()).
                    EncodeWith((parser, builder, modes) => new Lz11Encoder(parser));
                config.WithMatchOptions(options =>
                    options.WithinLimitations(() => new FindLimitations(3, 0x10110, 1, 0x1000)));

                return config;
            }
        }

        public static KompressionConfiguration Lz40
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Lz40Decoder()).
                    EncodeWith((parser, builder, modes) => new Lz40Encoder(parser));
                config.WithMatchOptions(options =>
                    options.WithinLimitations(() => new FindLimitations(0x3, 0x1010F, 1, 0xFFF)));

                return config;
            }
        }

        public static KompressionConfiguration Lz60
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Lz60Decoder()).
                    EncodeWith((parser, builder, modes) => new Lz60Encoder(parser));
                config.WithMatchOptions(options =>
                    options.WithinLimitations(() => new FindLimitations(0x3, 0x1010F, 1, 0xFFF)));

                return config;
            }
        }

        public static KompressionConfiguration Lz77
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Lz77Decoder()).
                    EncodeWith((parser, builder, modes) => new Lz77Encoder(parser));
                config.WithMatchOptions(options =>
                    options.SkipUnitsAfterMatch(1).
                        WithinLimitations(() => new FindLimitations(0x1, 0xFF, 1, 0xFF)));

                return config;
            }
        }

        public static KompressionConfiguration BackwardLz77
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new BackwardLz77Decoder(ByteOrder.LittleEndian)).
                    EncodeWith((parser, builder, modes) => new BackwardLz77Encoder(parser, ByteOrder.LittleEndian));
                config.WithMatchOptions(options =>
                    options.FindInBackwardOrder().
                        WithinLimitations(() => new FindLimitations(3, 0x12, 3, 0x1002)));

                return config;
            }
        }

        public static KompressionConfiguration LzEcd
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new LzEcdDecoder(0x3BE)).
                    EncodeWith((parser, builder, modes) => new LzEcdEncoder(parser));
                config.WithMatchOptions(options =>
                    options.WithPreBufferSize(0x3BE).
                        WithinLimitations(() => new FindLimitations(3, 0x42, 1, 0x400)));

                return config;
            }
        }

        public static KompressionConfiguration Lze
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new LzeDecoder()).
                    EncodeWith((parser, builder, modes) => new LzeEncoder(parser));
                config.WithMatchOptions(options =>
                    options.WithinLimitations(() => new FindLimitations(0x3, 0x12, 5, 0x1004)).
                        WithinLimitations(() => new FindLimitations(0x2, 0x41, 1, 4)));

                return config;
            }
        }

        /* Is more LZSS, described by wikipedia, through the flag denoting if following data is compressed or raw.
           Though the format is denoted as LZ77 with the magic num? (Issue 517) */
        public static KompressionConfiguration Lzss
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new LzssDecoder()).
                    EncodeWith((parser, builder, modes) => new LzssEncoder(parser));
                config.WithMatchOptions(options =>
                    options.WithinLimitations(() => new FindLimitations(0x3, 0x12, 1, 0x1000)));

                return config;
            }
        }

        public static KompressionConfiguration LzssVle
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new LzssVlcDecoder()).
                    EncodeWith((parser, builder, modes) => new LzssVlcEncoder(parser));
                config.WithMatchOptions(options =>
                    options.WithinLimitations(() => new FindLimitations(4, -1, 1, -1)));

                return config;
            }
        }

        public static KompressionConfiguration NintendoHuffman4BitLe
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new NintendoHuffmanDecoder(4, ByteOrder.LittleEndian)).
                    EncodeWith((parser, builder, modes) => new NintendoHuffmanEncoder(4, ByteOrder.LittleEndian, builder));
                config.WithHuffmanOptions(options => options.BuildTreeWith(() => new HuffmanTreeBuilder()));

                return config;
            }
        }

        public static KompressionConfiguration NintendoHuffman4BitBe
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new NintendoHuffmanDecoder(4, ByteOrder.BigEndian)).
                    EncodeWith((parser, builder, modes) => new NintendoHuffmanEncoder(4, ByteOrder.BigEndian, builder));
                config.WithHuffmanOptions(options => options.BuildTreeWith(() => new HuffmanTreeBuilder()));

                return config;
            }
        }

        public static KompressionConfiguration NintendoHuffman8Bit
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new NintendoHuffmanDecoder(8, ByteOrder.LittleEndian)).
                    EncodeWith((parser, builder, modes) => new NintendoHuffmanEncoder(8, ByteOrder.LittleEndian, builder));
                config.WithHuffmanOptions(options => options.BuildTreeWith(() => new HuffmanTreeBuilder()));

                return config;
            }
        }

        public static KompressionConfiguration NintendoRle
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new NintendoRleDecoder()).
                    EncodeWith((parser, builder, modes) => new NintendoRleEncoder(parser));
                config.WithMatchOptions(options => options.WithinLimitations(() => new FindLimitations(0x3, 0x82)));

                return config;
            }
        }

        public static KompressionConfiguration Mio0Le
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Mio0Decoder(ByteOrder.LittleEndian)).
                    EncodeWith((parser, builder, modes) => new Mio0Encoder(ByteOrder.LittleEndian, parser));
                config.WithMatchOptions(options => options.WithinLimitations(() => new FindLimitations(3, 0x12, 1, 0x1000)));

                return config;
            }
        }

        public static KompressionConfiguration Mio0Be
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Mio0Decoder(ByteOrder.BigEndian)).
                    EncodeWith((parser, builder, modes) => new Mio0Encoder(ByteOrder.BigEndian, parser));
                config.WithMatchOptions(options => options.WithinLimitations(() => new FindLimitations(3, 0x12, 1, 0x1000)));

                return config;
            }
        }

        public static KompressionConfiguration Yay0Le
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Yay0Decoder(ByteOrder.LittleEndian)).
                    EncodeWith((parser, builder, modes) => new Yay0Encoder(ByteOrder.LittleEndian, parser));
                config.WithMatchOptions(options => options.WithinLimitations(() => new FindLimitations(3, 0x111, 1, 0x1000)));

                return config;
            }
        }

        public static KompressionConfiguration Yay0Be
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Yay0Decoder(ByteOrder.BigEndian)).
                    EncodeWith((parser, builder, modes) => new Yay0Encoder(ByteOrder.BigEndian, parser));
                config.WithMatchOptions(options => options.WithinLimitations(() => new FindLimitations(3, 0x111, 1, 0x1000)));

                return config;
            }
        }

        public static KompressionConfiguration Yaz0Le
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Yaz0Decoder(ByteOrder.LittleEndian)).
                    EncodeWith((parser, builder, modes) => new Yaz0Encoder(ByteOrder.LittleEndian, parser));
                config.WithMatchOptions(options => options.WithinLimitations(() => new FindLimitations(3, 0x111, 1, 0x1000)));

                return config;
            }
        }

        public static KompressionConfiguration Yaz0Be
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Yaz0Decoder(ByteOrder.BigEndian)).
                    EncodeWith((parser, builder, modes) => new Yaz0Encoder(ByteOrder.BigEndian, parser));
                config.WithMatchOptions(options => options.WithinLimitations(() => new FindLimitations(3, 0x111, 1, 0x1000)));

                return config;
            }
        }

        public static KompressionConfiguration TaikoLz80
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new TaikoLz80Decoder()).
                    EncodeWith((parser, builder, modes) => new TaikoLz80Encoder(parser));
                config.WithMatchOptions(options => options.
                    WithinLimitations(() => new FindLimitations(2, 5, 1, 0x10)).
                    WithinLimitations(() => new FindLimitations(3, 0x12, 1, 0x400)).
                    WithinLimitations(() => new FindLimitations(4, 0x83, 1, 0x8000)));

                return config;
            }
        }

        public static KompressionConfiguration TaikoLz81
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new TaikoLz81Decoder()).
                    EncodeWith((parser, builder, modes) => new TaikoLz81Encoder(parser, builder));
                config.WithMatchOptions(options => options.
                    WithinLimitations(() => new FindLimitations(1, 0x102, 2, 0x8000)));

                return config;
            }
        }

        public static KompressionConfiguration Wp16
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new Wp16Decoder()).
                    EncodeWith((parser, builder, modes) => new Wp16Encoder(parser));
                config.WithMatchOptions(options => options.
                    WithinLimitations(() => new FindLimitations(4, 0x42, 2, 0xFFE)).
                    WithUnitSize(UnitSize.Short));

                return config;
            }
        }

        public static KompressionConfiguration TalesOf01
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new TalesOf01Decoder(0xFEE));
                // TODO: Encoding of TalesOf01

                return config;
            }
        }

        public static KompressionConfiguration TalesOf03
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new TalesOf03Decoder(0xFEF));
                // TODO: Encoding of TalesOf03

                return config;
            }
        }

        public static KompressionConfiguration LzEnc
        {
            get
            {
                var config = new KompressionConfiguration();

                config.DecodeWith(modes => new LzEncDecoder());
                // TODO: Encoding of LzEnc

                return config;
            }
        }
    }
}
