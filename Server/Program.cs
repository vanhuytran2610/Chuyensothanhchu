using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SimpleTcpSrvr
{
    class Program
    {
        private static string Readonly(string so)
        {
            string[] chuso = { "zero ", "one ", "two ", "three ", "four ", "five ", "six ", "seven ", "eight ", "nice " };
            string total = "";
            string chuoi = "";
            for (int i = 0; i < so.Length; i++)
            {
                char a = so[i];
                int b = Convert.ToInt32(new string(a, 1));
                chuoi = chuso[b];
                total += chuoi;
            }
            return total;
        }
        private static string docchay(string so)
        {
            string[] chuso = { "không ", "một ", "hai ", "ba ", "bốn ", "năm ", "sáu ", "bảy ", "tám ", "chín " };
            string total = "";
            string chuoi = "";
            for (int i = 0; i < so.Length; i++)
            {
                char a = so[i];
                int b = Convert.ToInt32(new string(a, 1));
                chuoi = chuso[b];
                total += chuoi;
            }
            return total;
        }


        #region TiengViet

        static string[] numText = "không;một;hai;ba;bốn;năm;sáu;bảy;tám;chín".Split(';');

        //Đọc hàng chục: 123 -> một trăm hai ba
        private static string DocHangChuc(long so, bool daydu)
        {
            string chuoi = "";

            //Hàm lấy chữ số hàng chục
            Int64 chuc = so / 10; //hàm Math.Floor: làm tròn xuống (21/10 = 2.1 -> 2)

            //Hàm lấy chữ số hàng đơn vị bằng phép chia lấy dư 21 % 10 = 1
            Int64 donvi = (Int64)so % 10;

            if (chuc > 1)
            {
                chuoi = " " + numText[chuc] + " mươi";

                if (donvi == 1) chuoi += " mốt";
            }
            else if (chuc == 1)
            {
                chuoi = " mười";

                if (donvi == 1) chuoi += " một";
            }
            else if (daydu && donvi > 0) chuoi = " lẻ";

            if (donvi == 5 && chuc >= 1)
            {
                chuoi += " lăm";
            }
            else if (donvi > 1 || (donvi == 1 && chuc == 0))
            {
                chuoi += " " + numText[donvi];
            }

            return chuoi;
        }

        private static string DocHangTram(long so, bool daydu)
        {
            string chuoi = "";

            Int64 tram = so / 100;

            //Lấy phần còn lại của hàng trăm bằng cách chia lấy dư 100
            so = so % 100;

            if (daydu || tram > 0)
            {
                chuoi = " " + numText[tram] + " trăm";
                chuoi += DocHangChuc(so, true);
            }
            else
            {
                chuoi = DocHangChuc(so, false);
            }

            return chuoi;
        }

        private static string DocHangTrieu(long so, bool daydu)
        {
            string chuoi = "";

            //Lấy số hàng triệu
            Int64 trieu = so / 1000000;

            //Lấy phần dư sau số hàng triệu (2,123,000 -> so: 123,000)
            so = so % 1000000;

            if (trieu > 0)
            {
                chuoi = DocHangTram(trieu, daydu) + " triệu";
                daydu = true;
            }

            //Lấy số hàng nghìn
            Int64 nghin = so / 1000;

            so = so % 1000;

            if (nghin > 0)
            {
                chuoi += DocHangTram(nghin, daydu) + " nghìn";
                daydu = true;
            }

            if (so > 0)
            {
                chuoi += DocHangTram(so, daydu);
            }

            return chuoi;
        }

        private static string ChuyenSangChuoi(long so)
        {
            if (so == 0) return numText[0];

            string chuoi = "", hauto = "";

            if (so < 0)
            {               
                so *= -1;
            }
            
            while (so > 0)
            {
                //Lấy số hàng tỷ
                long ty = so % 1000000000;

                //Lấy phần dư sau số hàng tỷ
                so = so / 1000000000;

                if (so > 0)
                {
                    chuoi = DocHangTrieu(ty, true) + hauto + chuoi;
                }
                else
                {
                    chuoi = DocHangTrieu(ty, false) + hauto + chuoi;
                }

                hauto = " tỷ";
            } 

            return chuoi;

        }

        #endregion


        #region TiengAnh
                
        public static string NumberToWords(long number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        #endregion

        public static byte[] ConvertDoubleToByteArray(double d)
        {
            return BitConverter.GetBytes(d);
        }
        public static double ConvertByteArrayToDouble(byte[] b)
        {
            return BitConverter.ToDouble(b, 0);
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            int recv;

            byte[] data = new byte[1024];

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.31.218"), 9050);

            Socket Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Client.Bind(ipep);

            Client.Listen(1);

            Console.WriteLine("Waiting for a client...");

            Socket client = Client.Accept();

            IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;

            Console.WriteLine("Connected with {0} at port {1}", clientep.Address, clientep.Port);

            string welcome = "Connected";

            data = Encoding.UTF8.GetBytes(welcome);

            client.Send(data, data.Length, SocketFlags.None);

            string input1;
            while (true)
            {

                data = new byte[1024];

                recv = client.Receive(data);

                if (recv == 0)

                    break;

                Console.WriteLine(recv);
                Console.WriteLine("Client: " + Encoding.UTF8.GetString(data, 0, recv));

                string st = Encoding.UTF8.GetString(data, 0, recv);
                string tv, ta;
                for (int i = 0; i < st.Length; i++)
                {
                    string mystr = st.Substring(3, st.Length - 3);
                    char a = st[i];
                    if (a == 'v')
                    {
                        if (st == " ")
                        {
                            Console.WriteLine("Mời bạn nhập lại dãy số cần chuyển đổi");
                        }

                        else if (mystr.Contains("."))
                        {
                            string[] parts = mystr.Split('.');
                            string part1 = parts[0];
                            string part2 = parts[1];
                            
                            long c = Int64.Parse(part2);
                                                        
                            if (c == 0)
                            {
                                tv = ChuyenSangChuoi(Convert.ToInt64(part1));
                                input1 = tv;
                                client.Send(Encoding.UTF8.GetBytes(input1));
                            }
                            else if (part2 == "")
                            {                                
                                tv = ChuyenSangChuoi(Convert.ToInt64(part1));
                                input1 = tv;
                                client.Send(Encoding.UTF8.GetBytes(input1));
                            }
                            else
                            {
                                char[] charsToTrim = { '0' };
                                string[] numbers = part2.Split();
                                foreach (string charac in numbers)
                                {
                                    string part3 = charac.TrimEnd(charsToTrim);

                                    if (part1.Contains("-"))
                                    {
                                        tv = "âm " + ChuyenSangChuoi(Convert.ToInt64(part1)) + " phẩy " + docchay(part3);
                                    }
                                    else
                                    {
                                        tv = ChuyenSangChuoi(Convert.ToInt64(part1)) + " phẩy " + docchay(part3);
                                    }                                   

                                    input1 = tv;
                                    client.Send(Encoding.UTF8.GetBytes(input1));
                                }
                            }
                        }
                        else
                        {
                            tv = ChuyenSangChuoi(Convert.ToInt64(mystr));
                            input1 = tv;
                            client.Send(Encoding.UTF8.GetBytes(input1));
                        }
                    }
                    else if (a == 'a')
                    {
                        if (st == " ")
                        {
                            Console.WriteLine("Mời bạn nhập lại dãy số cần chuyển đổi");
                        }

                        else if (mystr.Contains("."))
                        {
                            string[] parts = mystr.Split('.');
                            string part1 = parts[0];
                            string part2 = parts[1];
                            
                            long c = Int64.Parse(part2);
                                                        
                            if (c == 0)
                            {
                                ta = NumberToWords(Convert.ToInt64(part1));
                                input1 = ta;
                                client.Send(Encoding.UTF8.GetBytes(input1));
                            }
                            else if (part2 == "")
                            {
                                ta = NumberToWords(Convert.ToInt64(part1));
                                input1 = ta;
                                client.Send(Encoding.UTF8.GetBytes(input1));
                            }
                            else
                            {
                                char[] charsToTrim = { '0' };
                                string[] numbers = part2.Split();
                                foreach (string charac in numbers)
                                {
                                    string part3 = charac.TrimEnd(charsToTrim);

                                    if (part1.Contains("-"))
                                    {
                                        ta = "minus " + NumberToWords(Convert.ToInt64(part1)) + " point " + Readonly(part3);
                                    }
                                    else
                                    {
                                        ta = NumberToWords(Convert.ToInt64(part1)) + " point " + Readonly(part3);
                                    }
                                        
                                    input1 = ta;
                                    client.Send(Encoding.UTF8.GetBytes(input1));
                                }
                            }
                        }
                        else
                        {
                            ta = NumberToWords(Convert.ToInt64(mystr));
                            input1 = ta;
                            client.Send(Encoding.UTF8.GetBytes(input1));
                        }
                    }
                }

            }

            Console.WriteLine("Disconnected from {0}", clientep.Address);

            client.Close();

            Client.Close();

            Console.ReadLine();

        }
    }
}