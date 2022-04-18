using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace hextobin_wpf
{
   
    public class Tlv
    {
        
        public static TagDescription tagDescription { get; set; }
        public string? Tag { get; set; }
        public string? Length { get; set; }
        public string? Value { get; set; }

        public List<Tlv> subTag { get; set; }

        public string? Description { get; set; }
        public Tlv()
        {
            subTag = new List<Tlv>();

        }

        /// <summary>
        /// parse the tlv data
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static List<Tlv> tlvparse(string hex)
        {
            DeserializeData();
            byte[] rawData = StringToByteArray(hex);
            List<Tlv> tlvList = new List<Tlv>();

            for (int i = 0; i < rawData.Length;)
            {
                Tlv tlv = new Tlv();

                //parse tag
                bool constructedtag = (Convert.ToByte(rawData[i]) & 0x20) == 0x20;
                bool morebytes = (Convert.ToByte(rawData[i]) & 0x1F) == 0x1F;
                if (morebytes)
                {
                    tlv.Tag += rawData[i].ToString("x2");
                    i++;
                }
                tlv.Tag += rawData[i].ToString("x2");
                i++;

                tlv.Description = GetTagDescription(tlv.Tag);

                //parse length
                if (rawData[i].ToString("x2").Equals("81"))
                    i++;
                tlv.Length = rawData[i].ToString("x2");
                i++;
                int leng = Int32.Parse(tlv.Length, System.Globalization.NumberStyles.HexNumber);

                //parse value
                for (int j = i; j < leng + i; j++)
                {
                    tlv.Value += rawData[j].ToString("x2");
                }

                i = i + leng;
                if (constructedtag && !string.IsNullOrEmpty(tlv.Value))
                    tlv.subTag.AddRange(tlvparse(tlv.Value));

                tlvList.Add(tlv);

            }

            return tlvList;
        }

        /// <summary>
        /// convert string to byte Array
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToByteArray(string hex)
        {
            if ((hex.Length % 2) != 0)
                hex = "0" + hex;

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Get Tag Description from the TagDescription.File
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private static string GetTagDescription(string tag)
        {
            TagDescription tagDesc = new TagDescription();
            string result = "";

            if (tagDescription != null)
            {

                foreach (var t in tagDescription.Tag)
                {

                    if (string.Compare(t.Tag, tag, true) == 0)
                    {
                        result = t.Description;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Deserialize the data
        /// </summary>
        public static void DeserializeData()
        {
            string filePath = @"D:\BERTLV\hextobin_wpf\hextobin_wpf\ConfigurationFile\TagDescription.xml";
            string fileText = File.ReadAllText(filePath);

            XmlSerializer serializer = new XmlSerializer(typeof(TagDescription));
            StringReader rdr = new StringReader(fileText);
            tagDescription = (TagDescription)serializer.Deserialize(rdr);

        }
    }
}
