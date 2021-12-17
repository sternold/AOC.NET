namespace AOC21
{
    public class Day16 : BaseDay
    {
        public enum PacketType
        {
            Sum = 0,
            Product = 1,
            Minimum = 2,
            Maximum = 3,
            Literal = 4,
            GreaterThan = 5,
            LessThan = 6,
            EqualTo = 7
        }

        public enum LengthType
        {
            TotalLength = 0,
            NumberOfPackets = 1,
        }

        public abstract record Packet(string Binary, int Version, PacketType Type)
        {
            public abstract long CombinationVersion { get; }
            public abstract long Value { get; }
        }

        public record OperatorPacket(string Binary, int Version, PacketType Type, LengthType LengthType, int Length, IEnumerable<Packet> SubPackets) : Packet(Binary, Version, Type)
        {
            public override long CombinationVersion => Version + SubPackets.Sum(p => p.CombinationVersion);
            public override long Value => Type switch
            {
                PacketType.Sum => SubPackets.Sum(p => p.Value),
                PacketType.Product => SubPackets.Aggregate(1L, (agg, next) => agg * next.Value),
                PacketType.Minimum => SubPackets.Min(p => p.Value),
                PacketType.Maximum => SubPackets.Max(p => p.Value),
                PacketType.Literal => throw new NotSupportedException(),
                PacketType.GreaterThan => SubPackets.First().Value > SubPackets.Last().Value ? 1 : 0,
                PacketType.LessThan => SubPackets.First().Value < SubPackets.Last().Value ? 1 : 0,
                PacketType.EqualTo => SubPackets.First().Value == SubPackets.Last().Value ? 1 : 0,
                _ => throw new NotSupportedException()
            };
        }

        public record LiteralPacket(string Binary, int Version, PacketType Type, long Literal) : Packet(Binary, Version, Type)
        {
            public override long CombinationVersion => Version;
            public override long Value => Literal;
        }

        public string Input { get; }

        public Day16()
        {
            Input = FileReader.ReadAllLines(InputFilePath).Single();
        }

        public override ValueTask<string> Solve_1()
        {
            return new(BinaryToPacket(HexToBinary(Input)).CombinationVersion.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            return new(BinaryToPacket(HexToBinary(Input)).Value.ToString());
        }

        public static string HexToBinary(string hex) => string.Concat(hex.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

        public static Packet BinaryToPacket(string binary)
        {
            var type = (PacketType)Convert.ToInt32(binary.Substring(3, 3), 2);
            return type switch
            {
                PacketType.Literal => BinaryToLiteralPacket(binary),
                _ => BinaryToOperatorPacket(binary)
            };
        }

        public static LiteralPacket BinaryToLiteralPacket(string binary)
        {
            string packetBinary;
            var versionGroup = binary.Substring(0, 3);
            var typeGroup = binary.Substring(3, 3);

            packetBinary = versionGroup + typeGroup;

            var index = 6;
            var result = string.Empty;
            while (true)
            {
                var group = binary.Substring(index, 5);
                result += group.Substring(1, 4);
                packetBinary += group;
                if (group[0] == '0') break;
                index += 5;
            }

            var version = Convert.ToInt32(versionGroup, 2);
            var type = (PacketType)Convert.ToInt32(typeGroup, 2);
            return new(packetBinary, version, type, Convert.ToInt64(result, 2));
        }

        public static OperatorPacket BinaryToOperatorPacket(string binary)
        {
            string packetBinary;
            var versionGroup = binary.Substring(0, 3);
            var typeGroup = binary.Substring(3, 3);
            var lengthTypeGroup = binary.Substring(6, 1);

            packetBinary = versionGroup + typeGroup + lengthTypeGroup;

            var version = Convert.ToInt32(versionGroup, 2);
            var type = (PacketType)Convert.ToInt32(typeGroup, 2);
            var lengthType = (LengthType)Convert.ToInt32(lengthTypeGroup, 2);
            int length = 0;
            List<Packet> subpackets = new();
            switch (lengthType)
            {
                case LengthType.TotalLength:
                    {
                        var lengthGroup = binary.Substring(7, 15);
                        packetBinary += lengthGroup;
                        length = Convert.ToInt32(lengthGroup, 2);
                        var index = 22;
                        string subPacketsGroup = string.Empty;
                        while (subPacketsGroup.Length < length)
                        {
                            var packet = BinaryToPacket(binary.Substring(index));
                            subpackets.Add(packet);
                            subPacketsGroup += packet.Binary;
                            index += packet.Binary.Length;
                        }
                        packetBinary += subPacketsGroup;
                        break;
                    }
                case LengthType.NumberOfPackets:
                    {
                        var lengthGroup = binary.Substring(7, 11);
                        packetBinary += lengthGroup;
                        length = Convert.ToInt32(lengthGroup, 2);
                        var index = 18;
                        string subPacketsGroup = string.Empty;
                        while (subpackets.Count < length)
                        {
                            var packet = BinaryToPacket(binary.Substring(index));
                            subpackets.Add(packet);
                            subPacketsGroup += packet.Binary;
                            index += packet.Binary.Length;
                        }
                        packetBinary += subPacketsGroup;
                        break;
                    }
            }

            return new(packetBinary, version, type, lengthType, length, subpackets);
        }
    }
}
