// <auto-generated />
#pragma warning disable CS0105
using MasterMemory.Validation;
using MasterMemory;
using MessagePack;
using Omino.Infra.Master;
using System.Collections.Generic;
using System;

namespace Omino.Infra.Master.Tables
{
   public sealed partial class CharacterDataTable : TableBase<CharacterData>, ITableUniqueValidate
   {
        public Func<CharacterData, uint> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<CharacterData, uint> primaryIndexSelector;


        public CharacterDataTable(CharacterData[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Id;
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CharacterData FindById(uint key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return ThrowKeyNotFound(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindById(uint key, out CharacterData result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public CharacterData FindClosestById(uint key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<uint>.Default, key, selectLower);
        }

        public RangeView<CharacterData> FindRangeById(uint min, uint max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<uint>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
            ValidateUniqueCore(data, primaryIndexSelector, "Id", resultSet);       
        }

        public static MasterMemory.Meta.MetaTable CreateMetaTable()
        {
            return new MasterMemory.Meta.MetaTable(typeof(CharacterData), typeof(CharacterDataTable), "characters",
                new MasterMemory.Meta.MetaProperty[]
                {
                    new MasterMemory.Meta.MetaProperty(typeof(CharacterData).GetProperty("Id")),
                    new MasterMemory.Meta.MetaProperty(typeof(CharacterData).GetProperty("Name")),
                    new MasterMemory.Meta.MetaProperty(typeof(CharacterData).GetProperty("ObjectId")),
                    new MasterMemory.Meta.MetaProperty(typeof(CharacterData).GetProperty("Hp")),
                    new MasterMemory.Meta.MetaProperty(typeof(CharacterData).GetProperty("Atk")),
                },
                new MasterMemory.Meta.MetaIndex[]{
                    new MasterMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(CharacterData).GetProperty("Id"),
                    }, true, true, System.Collections.Generic.Comparer<uint>.Default),
                });
        }

    }
}