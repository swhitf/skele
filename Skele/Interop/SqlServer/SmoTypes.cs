using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Interop.SqlServer
{
    static class SmoTypes
    {
        private static readonly Dictionary<Type, SqlDataType> ClrToSmoMap;

        static SmoTypes()
        {
            ClrToSmoMap = new Dictionary<Type,SqlDataType>
            {
                { typeof(Boolean), SqlDataType.Bit },
                { typeof(String), SqlDataType.NVarChar },
                { typeof(DateTime), SqlDataType.DateTime },
                { typeof(Int16), SqlDataType.Int },
                { typeof(Int32), SqlDataType.Int },
                { typeof(Int64), SqlDataType.Int },
                { typeof(Decimal), SqlDataType.Float },
                { typeof(Double), SqlDataType.Float },
                { typeof(Single), SqlDataType.Float },
            };
        }

        public static Type GetClrType(SqlDataType sqlType)
        {
            switch (sqlType)
            {
                case SqlDataType.BigInt:
                    return typeof(long?);

                case SqlDataType.Binary:
                case SqlDataType.Image:
                case SqlDataType.Timestamp:
                case SqlDataType.VarBinary:
                case SqlDataType.VarBinaryMax:
                    return typeof(byte[]);

                case SqlDataType.Bit:
                    return typeof(bool?);

                case SqlDataType.Char:
                case SqlDataType.NChar:
                case SqlDataType.NText:
                case SqlDataType.NVarChar:
                case SqlDataType.NVarCharMax:
                case SqlDataType.Text:
                case SqlDataType.VarChar:
                case SqlDataType.VarCharMax:
                case SqlDataType.Xml:
                    return typeof(string);

                case SqlDataType.DateTime:
                case SqlDataType.SmallDateTime:
                case SqlDataType.Date:
                case SqlDataType.Time:
                case SqlDataType.DateTime2:
                    return typeof(DateTime?);

                case SqlDataType.Decimal:
                case SqlDataType.Money:
                case SqlDataType.SmallMoney:
                    return typeof(decimal?);

                case SqlDataType.Float:
                    return typeof(double?);

                case SqlDataType.Int:
                    return typeof(int?);

                case SqlDataType.Real:
                    return typeof(float?);

                case SqlDataType.UniqueIdentifier:
                    return typeof(Guid?);

                case SqlDataType.SmallInt:
                    return typeof(short?);

                case SqlDataType.TinyInt:
                    return typeof(byte?);

                case SqlDataType.Variant:
                    return typeof(object);

                case SqlDataType.DateTimeOffset:
                    return typeof(DateTimeOffset?);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }

        public static SqlDataType GetSmoType(Type type)
        {
            return ClrToSmoMap[type];
        }
    }
}
