// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.Omino.Infra.Master
{
    public sealed class PartyDataFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::Omino.Infra.Master.PartyData>
    {
        // Id
        private static global::System.ReadOnlySpan<byte> GetSpan_Id() => new byte[1 + 2] { 162, 73, 100 };
        // Formation
        private static global::System.ReadOnlySpan<byte> GetSpan_Formation() => new byte[1 + 9] { 169, 70, 111, 114, 109, 97, 116, 105, 111, 110 };
        // MoveSpeed
        private static global::System.ReadOnlySpan<byte> GetSpan_MoveSpeed() => new byte[1 + 9] { 169, 77, 111, 118, 101, 83, 112, 101, 101, 100 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::Omino.Infra.Master.PartyData value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            var formatterResolver = options.Resolver;
            writer.WriteMapHeader(3);
            writer.WriteRaw(GetSpan_Id());
            writer.Write(value.Id);
            writer.WriteRaw(GetSpan_Formation());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.IEnumerable<uint>>>(formatterResolver).Serialize(ref writer, value.Formation, options);
            writer.WriteRaw(GetSpan_MoveSpeed());
            writer.Write(value.MoveSpeed);
        }

        public global::Omino.Infra.Master.PartyData Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var ____result = new global::Omino.Infra.Master.PartyData();

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 2:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 25673UL) { goto FAIL; }

                        ____result.Id = reader.ReadUInt32();
                        continue;
                    case 9:
                        switch (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey))
                        {
                            default: goto FAIL;
                            case 8028075772561485638UL:
                                if (stringKey[0] != 110) { goto FAIL; }

                                ____result.Formation = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.IEnumerable<uint>>>(formatterResolver).Deserialize(ref reader, options);
                                continue;

                            case 7306369473965354829UL:
                                if (stringKey[0] != 100) { goto FAIL; }

                                ____result.MoveSpeed = reader.ReadSingle();
                                continue;

                        }

                }
            }

            reader.Depth--;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name
